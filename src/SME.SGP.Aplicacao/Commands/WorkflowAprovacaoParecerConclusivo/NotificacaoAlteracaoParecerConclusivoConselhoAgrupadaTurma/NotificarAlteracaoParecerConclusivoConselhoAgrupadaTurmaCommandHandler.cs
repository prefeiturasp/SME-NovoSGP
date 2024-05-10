using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommandHandler : NotificacaoParecerConclusivoConselhoClasseCommandBase<NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand, bool>  
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacao;

        public NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommandHandler(IMediator mediator, IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacao) : base(mediator)
    {
            this.repositorioWFAprovacao = repositorioWFAprovacao ?? throw new ArgumentNullException(nameof(repositorioWFAprovacao));
        }

        protected override string ObterTabelaPareceresAlterados(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma, Turma turma)
        {
            return MENSAGEM_DINAMICA_TABELA_POR_ALUNO;
        }

        public override async Task<bool> Handle(NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand request,
            CancellationToken cancellationToken)
        {
            var wfAprovacoes = await repositorioWFAprovacao.ObterPareceresAguardandoAprovacaoSemWorkflow();
            
            if (wfAprovacoes.EhNulo() || !wfAprovacoes.Any())
                return false;

            await CarregarInformacoesParaNotificacao(wfAprovacoes);
            
            var agrupamentoPorTurma = wfAprovacoes.GroupBy(wf => wf.TurmaId);
            
            foreach (var grupoTurma in agrupamentoPorTurma)
            {
                var idAprovacao = await EnviarNotificacao(grupoTurma.ToList());
                await ExecuteAlteracoesDasAprovacoes(grupoTurma.ToList(), idAprovacao);
            }

            return true;
        }
        
        private async Task ExecuteAlteracoesDasAprovacoes(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma, long idAprovacao)
        {
            foreach (var aprovacao in aprovacoesPorTurma)
            {
                var wfAprovacao = await repositorioWFAprovacao.ObterPorIdAsync(aprovacao.Id);
                wfAprovacao.WfAprovacaoId = idAprovacao;

                await repositorioWFAprovacao.SalvarAsync(wfAprovacao);
            }
        }

        private async Task<long> EnviarNotificacao(List<WFAprovacaoParecerConclusivoDto> aprovacoesPorTurma)
        {
            var aprovacaoParecerConclusivo = aprovacoesPorTurma.FirstOrDefault();

            if (aprovacaoParecerConclusivo.EhNulo())
                return 0;
            
            var turma = await ObterTurma(aprovacaoParecerConclusivo.TurmaId);
            
            var mensagem = ObterMensagem(turma, aprovacoesPorTurma);
            var conselhoClasseAlunoId = aprovacaoParecerConclusivo.ConselhoClasseAlunoId;

            return await mediator.Send(new EnviarNotificacaoCommand(ObterTitulo(turma),
                mensagem,
                NotificacaoCategoria.Workflow_Aprovacao,
                NotificacaoTipo.Fechamento,
                new[] { Cargo.CP, Cargo.Supervisor },
                turma.Ue.Dre.CodigoDre,
                turma.Ue.CodigoUe,
                turma.CodigoTurma,
                WorkflowAprovacaoTipo.AlteracaoParecerConclusivo,
                conselhoClasseAlunoId));
        }

        protected override string ObterTextoCabecalho(Turma turma)
            => $"O parecer conclusivo dos estudantes abaixo da turma {turma.Nome} da {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) de {turma.AnoLetivo} foram alterados.";

        protected override string ObterTextoRodape()
            => "Você precisa aceitar esta notificação para que a alteração seja considerada válida.";
    }
}
