using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotasFechamentoParaAprovacaoCommandHandler : AsyncRequestHandler<EnviarNotasFechamentoParaAprovacaoCommand>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;

        public EnviarNotasFechamentoParaAprovacaoCommandHandler(IMediator mediator,
                                                                IServicoEol servicoEol,
                                                                IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.repositorioWfAprovacaoNotaFechamento = repositorioWfAprovacaoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoNotaFechamento));
        }

        protected override async Task Handle(EnviarNotasFechamentoParaAprovacaoCommand request, CancellationToken cancellationToken)
        {
            var lancaNota = !request.NotasAprovacao.First().ConceitoId.HasValue;
            var notaConceitoMensagem = lancaNota ? "nota(s)" : "conceito(s)";

            var componenteSgp = request.ComponenteCurricular;

            var mensagem = await MontaMensagemWfAprovacao(request.NotasAprovacao, lancaNota, request.PeriodoEscolar, request.UsuarioLogado, componenteSgp, request.Turma);

            var wfAprovacaoNota = new WorkflowAprovacaoDto()
            {
                Ano = DateTime.Today.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = request.FechamentoTurmaDisciplinaId,
                Tipo = WorkflowAprovacaoTipo.AlteracaoNotaFechamento,
                TurmaId = request.Turma.CodigoTurma,
                UeId = request.Turma.Ue.CodigoUe,
                DreId = request.Turma.Ue.Dre.CodigoDre,
                NotificacaoTitulo = $"Alteração em {notaConceitoMensagem} final - {componenteSgp.Nome} - {request.Turma.Nome} ({request.Turma.AnoLetivo})",
                NotificacaoTipo = NotificacaoTipo.Notas,
                NotificacaoMensagem = mensagem
            };

            wfAprovacaoNota.AdicionarNivel(Cargo.CP);
            wfAprovacaoNota.AdicionarNivel(Cargo.Supervisor);

            var idWorkflow = await mediator.Send(new SalvarWorkflowAprovacaoCommand(wfAprovacaoNota));
            foreach (var notaFechamento in request.NotasAprovacao)
            {
                await mediator.Send(new ExcluirWFAprovacaoNotaFechamentoPorNotaCommand(notaFechamento.Id));
                await repositorioWfAprovacaoNotaFechamento.SalvarAsync(new WfAprovacaoNotaFechamento()
                {
                    WfAprovacaoId = idWorkflow,
                    FechamentoNotaId = notaFechamento.Id,
                    Nota = notaFechamento.Nota,
                    ConceitoId = notaFechamento.ConceitoId
                });
            }
        }

        private async Task<string> MontaMensagemWfAprovacao(List<FechamentoNotaDto> notasAprovacao, bool lancaNota, PeriodoEscolar periodoEscolar, Usuario usuarioLogado, DisciplinaDto componenteSgp, Turma turma)
        {
            var notaConceitoMensagem = lancaNota ? "A(s) nota(s)" : "O(s) conceito(s)";

            var mensagem = new StringBuilder();
            var bimestre = (periodoEscolar?.Bimestre ?? 0) == 0 ? "bimestre final" : $"{periodoEscolar.Bimestre}º bimestre";
            mensagem.Append($"<p>{notaConceitoMensagem} do {bimestre} do componente curricular {componenteSgp.Nome} da turma {turma.Nome} da ");
            mensagem.Append($"{turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) ");
            mensagem.Append($"de {turma.AnoLetivo} foram alterados pelo Professor {usuarioLogado.Nome} ");
            mensagem.Append($"({usuarioLogado.CodigoRf}) em {DateTime.Now.ToString("dd/MM/yyyy")} às {DateTime.Now.ToString("HH:mm")} para o(s) seguinte(s) estudantes(s):</p>");

            var alunosTurma = await servicoEol.ObterAlunosPorTurma(turma.CodigoTurma);

            mensagem.AppendLine(componenteSgp.Regencia ?
                await MontarTabelaNotasRegencia(alunosTurma, notasAprovacao) :
                MontarTabelaNotas(alunosTurma, notasAprovacao));
            return mensagem.ToString();
        }


        private async Task<string> MontarTabelaNotasRegencia(IEnumerable<AlunoPorTurmaResposta> alunosTurma, List<FechamentoNotaDto> notasAprovacao)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesQuery());

            var mensagem = new StringBuilder();
            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Estudante</b></td>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Componente Curricular</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Valor anterior</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Novo valor</b></td>");
            mensagem.AppendLine("</tr>");

            foreach (var notaAprovacao in notasAprovacao)
            {
                var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == notaAprovacao.CodigoAluno);

                mensagem.AppendLine("<tr>");
                mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.CodigoAluno})</td>");

                if (!notaAprovacao.ConceitoId.HasValue)
                {
                    mensagem.Append($"<td style='padding: 5px; text-align:left;'>{ObterNomeComponente(componentes, notaAprovacao.DisciplinaId)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.NotaAnterior.Value)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.Nota.Value)}</td>");
                }
                else
                {
                    mensagem.Append($"<td style='padding: 5px; text-align:left;'>{ObterNomeComponente(componentes, notaAprovacao.DisciplinaId)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.ConceitoIdAnterior)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.ConceitoId)}</td>");
                }

                mensagem.AppendLine("</tr>");
            }
            mensagem.AppendLine("</table>");
            mensagem.AppendLine("<p>Você precisa aceitar esta notificação para que a alteração seja considerada válida.</p>");

            return mensagem.ToString();
        }

        private string MontarTabelaNotas(IEnumerable<AlunoPorTurmaResposta> alunosTurma, List<FechamentoNotaDto> notasAprovacao)
        {
            var mensagem = new StringBuilder();
            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Estudante</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Valor anterior</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Novo valor</b></td>");
            mensagem.AppendLine("</tr>");

            foreach (var notaAprovacao in notasAprovacao)
            {
                var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == notaAprovacao.CodigoAluno);

                mensagem.AppendLine("<tr>");
                mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.CodigoAluno})</td>");

                if (!notaAprovacao.ConceitoId.HasValue)
                {
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.NotaAnterior)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.Nota)}</td>");
                }
                else
                {
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.ConceitoIdAnterior)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.ConceitoId)}</td>");
                }

                mensagem.AppendLine("</tr>");
            }
            mensagem.AppendLine("</table>");
            mensagem.AppendLine("<p>Você precisa aceitar esta notificação para que a alteração seja considerada válida.</p>");

            return mensagem.ToString();
        }

        private string ObterNomeComponente(IEnumerable<ComponenteCurricularDto> componentes, long disciplinaId)
            => componentes.FirstOrDefault(a => a.Codigo == disciplinaId.ToString()).Descricao;

        private string ObterNota(double? nota)
        {
            if (!nota.HasValue)
                return string.Empty;

            return nota.ToString();
        }

        private string ObterConceito(long? conceitoId)
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
