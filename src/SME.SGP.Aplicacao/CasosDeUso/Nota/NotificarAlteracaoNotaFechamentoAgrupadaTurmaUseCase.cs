using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase : AbstractUseCase, INotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase
    {
        private readonly IServicoEol servicoEol;

        public NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase(IMediator mediator, IServicoEol servicoEol) : base(mediator)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dados = param.ObterObjetoMensagem<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>();

            var fechamentoTurmaDisciplinaId = dados.FirstOrDefault().FechamentoTurmaDisciplinaId;
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(dados.FirstOrDefault().TurmaId));
            var lancaNota = !dados.FirstOrDefault().WfAprovacao.ConceitoId.HasValue;
            var notaConceitoMensagem = lancaNota ? "nota(s)" : "conceito(s)";
            var mensagem = await MontaMensagemWfAprovacao(dados, lancaNota, turma);

            var wfAprovacao = new WorkflowAprovacaoDto
            {
                Ano = DateTime.Today.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = fechamentoTurmaDisciplinaId,
                Tipo = WorkflowAprovacaoTipo.AlteracaoNotaFechamento,
                TurmaId = turma.CodigoTurma,
                UeId = turma.Ue.CodigoUe,
                DreId = turma.Ue.Dre.CodigoDre,
                NotificacaoTitulo = $"Alteração em {notaConceitoMensagem} final - {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) - {turma.NomeComModalidade()} (ano anterior)",
                NotificacaoTipo = NotificacaoTipo.Notas,
                NotificacaoMensagem = mensagem
            };

            int? bimestre = dados.FirstOrDefault().Bimestre;

            if (bimestre != null)
                wfAprovacao.AdicionarNivel(Cargo.CP);
            else
            {
                wfAprovacao.AdicionarNivel(Cargo.CP);
                wfAprovacao.AdicionarNivel(Cargo.Supervisor);
            }

            var idWorkflow = await mediator.Send(new SalvarWorkflowAprovacaoCommand(wfAprovacao));

            var workflowAprovacaoNotaFechamentoIds = dados.Select(d => d.WfAprovacao.Id);

            await mediator.Send(new AlterarWFAprovacaoNotaFechamentoPorWfAprovacaoIdCommand() { WorkflowAprovacaoId = idWorkflow, WorkflowAprovacaoFechamentoNotaIds = workflowAprovacaoNotaFechamentoIds.ToArray() });

            return true;
        }

        private async Task<string> MontaMensagemWfAprovacao(IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasAprovacao, bool lancaNota, Turma turma)
        {
            int? bimestreNota = notasAprovacao.FirstOrDefault().Bimestre;
            bool ehRegencia = notasAprovacao.FirstOrDefault().ComponenteCurricularEhRegencia;
            var notaConceitoMensagem = lancaNota ? "A(s) nota(s)" : "O(s) conceito(s)";
            var mensagem = new StringBuilder();
            var bimestre = (bimestreNota ?? 0) == 0 ? "bimestre final" : $"{bimestreNota}º bimestre";

            mensagem.Append($"<p>{notaConceitoMensagem} do {bimestre} de {turma.AnoLetivo} da turma {turma.ModalidadeCodigo.ObterNomeCurto()}-{turma.Nome} da ");
            mensagem.Append($"{turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) ");
            mensagem.Append($"foram alteradas");

            var alunosTurmas = await mediator.Send(new ObterAlunosEolPorTurmaQuery(turma.CodigoTurma, true));
            var alunosTurma = alunosTurmas.OrderBy(c => c.NomeAluno);

            mensagem.AppendLine(ehRegencia ?
                await MontarTabelaNotasRegencia(alunosTurma, notasAprovacao, lancaNota) :
                MontarTabelaNotas(alunosTurma, notasAprovacao));

            return mensagem.ToString();
        }

        private async Task<string> MontarTabelaNotasRegencia(IEnumerable<AlunoPorTurmaResposta> alunosTurma, IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasAprovacao, bool lancaNota)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesQuery());

            var mensagem = new StringBuilder();
            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Componente Curricular</b></td>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Estudante</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Valor anterior</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Novo valor</b></td>");
            mensagem.AppendLine("<td style='padding: 10px; text-align:left;'><b>Usuário que alterou</b></td>");
            mensagem.AppendLine("<td style='padding: 10px; text-align:left;'><b>Data da alteração</b></td>");
            mensagem.AppendLine("</tr>");

            notasAprovacao = notasAprovacao
                .OrderBy(n => n.WfAprovacao.AlteradoEm)
                .ThenBy(n => n.WfAprovacao.CriadoEm);

            foreach (var aluno in alunosTurma)
            {
                foreach (var notaAprovacao in notasAprovacao)
                {
                    if ((notasAprovacao == null) || (notaAprovacao.CodigoAluno != aluno.CodigoAluno))
                        continue;

                    string nomeUsuarioAlterou = notaAprovacao.WfAprovacao.AlteradoPor ?? notaAprovacao.WfAprovacao.CriadoPor;
                    string rfUsuarioAlterou = notaAprovacao.WfAprovacao.AlteradoRF ?? notaAprovacao.WfAprovacao.CriadoRF;

                    var (dataNotificacao, horaNotificacao) = RetornarDataHoraNotificacao(notaAprovacao.WfAprovacao.AlteradoEm, notaAprovacao.WfAprovacao.CriadoEm);

                    mensagem.AppendLine("<tr>");

                    if (!notaAprovacao.WfAprovacao.ConceitoId.HasValue && lancaNota)
                    {
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{notaAprovacao.ComponenteCurricularDescricao}</td>");
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.CodigoAluno})</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.NotaAnterior.Value)}</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.WfAprovacao.Nota.Value)}</td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'>{nomeUsuarioAlterou} ({rfUsuarioAlterou}) </td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'>{dataNotificacao} ({horaNotificacao}) </td>");
                    }
                    else
                    {
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{notaAprovacao.ComponenteCurricularDescricao}</td>");
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.CodigoAluno})</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.ConceitoAnteriorId)}</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.WfAprovacao.ConceitoId)}</td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'>{nomeUsuarioAlterou} ({rfUsuarioAlterou}) </td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'>{dataNotificacao} ({horaNotificacao}) </td>");
                    }

                    mensagem.AppendLine("</tr>");
                }
            }
            mensagem.AppendLine("</table>");
            mensagem.AppendLine("<p>Você precisa aceitar esta notificação para que a alteração seja considerada válida.</p>");

            return mensagem.ToString();
        }

        private static string MontarTabelaNotas(IEnumerable<AlunoPorTurmaResposta> alunosTurma, IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasAprovacao)
        {
            var mensagem = new StringBuilder();

            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Componente Curricular</b></td>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Estudante</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Valor anterior</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Novo valor</b></td>");
            mensagem.AppendLine("<td style='padding: 10px; text-align:left;'><b>Usuário que alterou</b></td>");
            mensagem.AppendLine("<td style='padding: 10px; text-align:left;'><b>Data da alteração</b></td>");
            mensagem.AppendLine("</tr>");

            notasAprovacao = notasAprovacao
                .OrderBy(n => n.WfAprovacao.AlteradoEm)
                .ThenBy(n => n.WfAprovacao.CriadoEm);

            foreach (var aluno in alunosTurma)
            {
                foreach (var notaAprovacao in notasAprovacao)
                {
                    if ((notasAprovacao == null) || (notaAprovacao.CodigoAluno != aluno.CodigoAluno))
                        continue;

                    string nomeUsuarioAlterou = notaAprovacao.WfAprovacao.AlteradoPor ?? notaAprovacao.WfAprovacao.CriadoPor;
                    string rfUsuarioAlterou = notaAprovacao.WfAprovacao.AlteradoRF ?? notaAprovacao.WfAprovacao.CriadoRF;
                    var (dataNotificacao, horaNotificacao) = RetornarDataHoraNotificacao(notaAprovacao.WfAprovacao.AlteradoEm, notaAprovacao.WfAprovacao.CriadoEm);

                    mensagem.AppendLine("<tr>");

                    if (!notaAprovacao.WfAprovacao.ConceitoId.HasValue)
                    {
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{notaAprovacao.ComponenteCurricularDescricao}</td>");
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.CodigoAluno})</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.NotaAnterior)}</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.WfAprovacao.Nota)}</td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'> {nomeUsuarioAlterou} ({rfUsuarioAlterou}) </td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'>{dataNotificacao} ({horaNotificacao}) </td>");
                    }
                    else
                    {
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{notaAprovacao.ComponenteCurricularDescricao}</td>");
                        mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.CodigoAluno})</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.ConceitoAnteriorId)}</td>");
                        mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.WfAprovacao.ConceitoId)}</td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'> {nomeUsuarioAlterou} ({rfUsuarioAlterou}) </td>");
                        mensagem.Append($"<td style='padding: 10px; text-align:right;'>{dataNotificacao} ({horaNotificacao}) </td>");
                    }

                    mensagem.AppendLine("</tr>");
                }
            }
            mensagem.AppendLine("</table>");
            mensagem.AppendLine("<p>Você precisa aceitar esta notificação para que a alteração seja considerada válida.</p>");

            return mensagem.ToString();
        }

        private static (string dataNotificacao, string horaNotificacao) RetornarDataHoraNotificacao(DateTime? alteradoEm, DateTime criadoEm)
        {
            var dataFormatada = criadoEm.ToString("dd/MM/yyyy");
            var horaFormatada = criadoEm.ToString("HH:mm:ss");

            if (alteradoEm.HasValue)
            {
                dataFormatada = alteradoEm.Value.ToString("dd/MM/yyyy");
                horaFormatada = alteradoEm.Value.ToString("HH:mm:ss");
            }

            return (dataFormatada, horaFormatada);
        }

        private static string ObterNota(double? nota)
        {
            if (!nota.HasValue)
                return string.Empty;

            return nota.ToString();
        }

        private static string ObterConceito(long? conceitoId)
        {
            if (!conceitoId.HasValue)
                return string.Empty;

            if (conceitoId == (int)ConceitoValores.P)
                return ConceitoValores.P.ToString();
            else if (conceitoId == (int)ConceitoValores.S)
                return ConceitoValores.S.ToString();
            else
                return ConceitoValores.NS.ToString();
        }
    }
}
