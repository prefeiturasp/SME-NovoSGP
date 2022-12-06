using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommandHandler : AsyncRequestHandler<NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand>
    {
        protected readonly IMediator mediator;
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacao;

        //separar
        protected List<TurmasDoAlunoDto> Alunos;
        protected List<Usuario> Usuarios;
        protected List<Conceito> Conceitos;

        public NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommandHandler(IMediator mediator, IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioWFAprovacao = repositorioWFAprovacao ?? throw new ArgumentNullException(nameof(repositorioWFAprovacao));
        }

        protected override async Task Handle(NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand request, CancellationToken cancellationToken)
        {
            var WFAprovacoes = await repositorioWFAprovacao.ObterPareceresAguardandoAprovacaoSemWorkflow();
            await CarregarInformacoesParaNotificacao(WFAprovacoes);
            if (WFAprovacoes == null || !WFAprovacoes.Any()) return;

            var agrupamentoPorTurma = WFAprovacoes.GroupBy(wf => wf.TurmaId);
            foreach (var grupoTurma in agrupamentoPorTurma)
            {
                var idAprovacao = await EnviarNotificacao(grupoTurma.ToList());
                await ExecuteAlteracoesDasAprovacoes(grupoTurma.ToList(), idAprovacao);
            }
        }
       
        private async Task ExecuteAlteracoesDasAprovacoes(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma, long idAprovacao)
        {
            foreach (var aprovacao in aprovacoesPorTurma)
            {
                var wfAprovacao = repositorioWFAprovacao.ObterPorId(aprovacao.Id);
                wfAprovacao.WfAprovacaoId = idAprovacao;
                await repositorioWFAprovacao.SalvarAsync(wfAprovacao);
            }
        }

        private async Task<long> EnviarNotificacao(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma)
        {
            var turma = await ObterTurma(aprovacoesPorTurma.FirstOrDefault().TurmaId);
            var titulo = $@"Alteração de parecer conclusivo - {turma.Ue.Nome} (ano anterior)";
            var mensagem = ObterMensagem(turma.Ue, turma, aprovacoesPorTurma);
            var conselhoClasseAlunoId = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseAlunoId;

            return await mediator.Send(new EnviarNotificacaoCommand(
                                                                    titulo,
                                                                    mensagem,
                                                                    NotificacaoCategoria.Workflow_Aprovacao,
                                                                    NotificacaoTipo.Fechamento,
                                                                    new Cargo[] { Cargo.CP, Cargo.Supervisor },
                                                                    turma.Ue.Dre.CodigoDre,
                                                                    turma.Ue.CodigoUe,
                                                                    turma.CodigoTurma,
                                                                    WorkflowAprovacaoTipo.AlteracaoParecerConclusivo,
                                                                    conselhoClasseAlunoId));
        }

        private async Task<Turma> ObterTurma(long turmaId)
            => await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

        private async Task CarregarInformacoesParaNotificacao(IEnumerable<WFAprovacaoParecerConclusivoDto> wfAprovacoes)
        {
            await CarregarTodosAlunos(wfAprovacoes);
            await CarregarTodosUsuarios(wfAprovacoes);
        }

        protected string ObterMensagem(Ue ue, Turma turma, List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma)
        {
            var msg = new StringBuilder();
            msg.Append($"O parecer conclusivo dos estudantes abaixo da turma {turma.Nome} da {ue.Nome} ({ue.Dre.Abreviacao}) de {turma.AnoLetivo} foram alterados.");
            msg.Append(ObterTabelaPareceresAlterados(aprovacoesPorTurma));
            msg.Append("Você precisa aceitar esta notificação para que a alteração seja considerada válida.");
            return msg.ToString();
        }

        private string ObterTabelaPareceresAlterados(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma)
        {
            var msg = new StringBuilder();
            msg.AppendLine("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            msg.AppendLine("<tbody>");
            msg.AppendLine("<tr>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Estudante</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Parecer anterior</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Novo Parecer</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Usuário que alterou</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Data da alteração</strong></td>");
            msg.AppendLine("</tr>");

            foreach (var aprovaco in aprovacoesPorTurma)
            {
                msg.AppendLine(ObterLinhaParecerAlterado(aprovaco));
            }
            msg.AppendLine("<tbody>");
            msg.AppendLine("</table>");
            return msg.ToString();
        }

        private string ObterLinhaParecerAlterado(WFAprovacaoParecerConclusivoDto aprovacao)
        {
            var aluno = Alunos.Find(aluno => aluno.CodigoAluno.ToString() == aprovacao.AlunoCodigo);
            var usuario = Usuarios.Find(usuario => usuario.Id == aprovacao.UsuarioSolicitanteId);

            return $@"<tr>
                           <td style='padding: 3px;'>{aluno.NumeroAlunoChamada} - {aluno.NomeAluno} ({aluno.CodigoAluno})</td>
                           <td style='padding: 3px;'>{aprovacao.NomeParecerAnterior}</td>
                           <td style='padding: 3px;'>{aprovacao.NomeParecerNovo}</td>
                           <td style='padding: 3px;'>{usuario.Nome} ({usuario.CodigoRf})</td>
                           <td style='padding: 3px;'>{aprovacao.CriadoEm.ToString("dd/MM/yyy HH:mm")}</td>
                      </tr>";
        }

        private async Task CarregarTodosAlunos(IEnumerable<WFAprovacaoParecerConclusivoDto> WFAprovacoes)
        {
            var codigos = WFAprovacoes.Select(wf => long.Parse(wf.AlunoCodigo)).ToArray();

            Alunos = (await ObterAlunos(codigos)).ToList();
        }
        private async Task CarregarTodosUsuarios(IEnumerable<WFAprovacaoParecerConclusivoDto> WFAprovacoes)
        {
            var ids = WFAprovacoes.Select(wf => wf.UsuarioSolicitanteId).Distinct().ToArray();

            Usuarios = (await ObterUsuarios(ids)).ToList();
        }
        
    private async Task<IEnumerable<TurmasDoAlunoDto>> ObterAlunos(long[] codigos)
            => await mediator.Send(new ObterAlunosEolPorCodigosQuery(codigos));
        private async Task<IEnumerable<Usuario>> ObterUsuarios(long[] ids)
            => await mediator.Send(new ObterUsuarioPorIdsSemPerfilQuery(ids));

    }
}
