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
    public class NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommandHandler : NotificacaoParecerConclusivoConselhoClasseCommandBase<NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand>  
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacao;

        public NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommandHandler(IMediator mediator, IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacao) : base(mediator)
    {
            this.repositorioWFAprovacao = repositorioWFAprovacao ?? throw new ArgumentNullException(nameof(repositorioWFAprovacao));
        }

        protected override async Task Handle(NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand request, CancellationToken cancellationToken)
        {
            var WFAprovacoes = await repositorioWFAprovacao.ObterPareceresAguardandoAprovacaoSemWorkflow();
            if (WFAprovacoes == null || !WFAprovacoes.Any()) return;

            await CarregarInformacoesParaNotificacao(WFAprovacoes);
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
            
            var mensagem = ObterMensagem(turma, aprovacoesPorTurma);
            var conselhoClasseAlunoId = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseAlunoId;
            return await mediator.Send(new EnviarNotificacaoCommand(
                                                                    ObterTitulo(turma),
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

        protected override string ObterTextoCabecalho(Turma turma)
        {
            return $"O parecer conclusivo dos estudantes abaixo da turma {turma.Nome} da {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) de {turma.AnoLetivo} foram alterados.";
        }

        protected override string ObterTextoRodape()
        { return "Você precisa aceitar esta notificação para que a alteração seja considerada válida."; }

    }
}
