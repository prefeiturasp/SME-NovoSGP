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
    public class NotificarAlteracaoNotaPosConselhoAgrupadaTurmaCommandHandler : AprovacaoNotaConselhoCommandBase<NotificarAlteracaoNotaPosConselhoAgrupadaTurmaCommand, bool>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho;

        public NotificarAlteracaoNotaPosConselhoAgrupadaTurmaCommandHandler(IMediator mediator, IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho) 
                    : base(mediator)
        {
            this.repositorioWFAprovacaoNotaConselho = repositorioWFAprovacaoNotaConselho ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoNotaConselho));
        }

        public override async Task<bool> Handle(NotificarAlteracaoNotaPosConselhoAgrupadaTurmaCommand request, CancellationToken cancellationToken)
        {
            await CarregarInformacoesParaNotificacao(await repositorioWFAprovacaoNotaConselho.ObterNotasAguardandoAprovacaoSemWorkflow());

            if (WFAprovacoes.EhNulo() || !WFAprovacoes.Any()) return false;
            var agrupamentoPorTurma = WFAprovacoes.GroupBy(wf => new { wf.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.Id, 
                                                                       wf.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar?.Bimestre } );

            foreach (var grupoTurma in agrupamentoPorTurma)
            {
                var idAprovacao = await EnviarNotificacao(grupoTurma.ToList());

                await ExecuteAlteracoesDasAprovacoes(grupoTurma.ToList(), idAprovacao);
            }

            return true;
        }

        protected override string ObterTexto(Ue ue, Turma turma, PeriodoEscolar periodoEscolar)
        {
            return $@"A alteração de notas/conceitos pós-conselho do bimestre { (periodoEscolar.NaoEhNulo() ? periodoEscolar.Bimestre : "final") } 
                      de { turma.AnoLetivo } da turma { turma.NomeFiltro } da { ue.Nome } ({ ue.Dre.Abreviacao }) foram alteradas.";
        }

        private async Task ExecuteAlteracoesDasAprovacoes(List<WFAprovacaoNotaConselho> aprovacoesPorTurma, long idAprovacao)
        {
            foreach(var aprovacao in aprovacoesPorTurma)
            {
                aprovacao.WfAprovacaoId = idAprovacao;

                await repositorioWFAprovacaoNotaConselho.SalvarAsync(aprovacao);
            }
        }

        private async Task<long> EnviarNotificacao(List<WFAprovacaoNotaConselho> aprovacoesPorTurma)
        {
            var turma = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
            var ue = Ues.Find(ue => ue.Id == turma.UeId);
            var titulo = ObterTitulo(ue, turma);
            var mensagem = ObterMensagem(ue, turma, aprovacoesPorTurma);
            var conselhoClasseId = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasseId;
            var periodo = aprovacoesPorTurma.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;

            Cargo[] cargosAprovacao  = (periodo.NaoEhNulo()) ? new Cargo[] { Cargo.CP } : new Cargo[] { Cargo.CP, Cargo.Supervisor };
            return await mediator.Send(new EnviarNotificacaoCommand(
                                                                    titulo,
                                                                    mensagem,
                                                                    NotificacaoCategoria.Workflow_Aprovacao,
                                                                    NotificacaoTipo.Fechamento,
                                                                    cargosAprovacao,
                                                                    ue.Dre.CodigoDre,
                                                                    ue.CodigoUe,
                                                                    turma.CodigoTurma,
                                                                    WorkflowAprovacaoTipo.AlteracaoNotaConselho,
                                                                    conselhoClasseId)); 
        }

        protected override string ObterTabelaDosAlunos(List<WFAprovacaoNotaConselho> aprovacoesPorTurma, Turma turma)
        {
            return MENSAGEM_DINAMICA_TABELA_POR_ALUNO; 
        }
    }
}
