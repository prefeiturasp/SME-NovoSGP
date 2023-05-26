using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RecusarAprovacaoNotaConselhoCommandHandler : AsyncRequestHandler<RecusarAprovacaoNotaConselhoCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho;

        public RecusarAprovacaoNotaConselhoCommandHandler(
                                                          IMediator mediator,
                                                          IRepositorioWFAprovacaoNotaConselho repositorioWFAprovacaoNotaConselho)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioWFAprovacaoNotaConselho = repositorioWFAprovacaoNotaConselho ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoNotaConselho));
        }

        protected override async Task Handle(RecusarAprovacaoNotaConselhoCommand request, CancellationToken cancellationToken)
        {
            var notasEmAprovacao = await ObterNotaEmAprovacaoPosConselho(request.WorkflowId);

            foreach(var notaEmProvacao in notasEmAprovacao)
            {
                await mediator.Send(new ExcluirWfAprovacaoNotaConselhoClasseCommand(notaEmProvacao.Id));

                if (notaEmProvacao.ConselhoClasseNota.Nota == null && notaEmProvacao.ConselhoClasseNota.Conceito == null) //Conselho de classe nota gerado automaticamente pelo WF Conselho Nota
                {
                    await mediator.Send(new ExcluirConselhoClasseNotaCommand(notaEmProvacao.ConselhoClasseNotaId??0));
                }
            }

            await mediator.Send(new NotificarAprovacaoNotasConselhoCommand(notasEmAprovacao,
                                                                          request.CodigoDaNotificacao,
                                                                          request.TurmaCodigo,
                                                                          request.WorkflowId,
                                                                          false,
                                                                          request.Justificativa));

            var periodoEscolar = notasEmAprovacao.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;
            var bimestre = periodoEscolar != null ? periodoEscolar.Bimestre : (int)Bimestre.Final;
            await RemoverCache(string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE, request.TurmaCodigo, bimestre), cancellationToken);
        }

        private async Task RemoverCache(string nomeChave, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave), cancellationToken);
        }

        private async Task<IEnumerable<WFAprovacaoNotaConselho>> ObterNotaEmAprovacaoPosConselho(long workflowId)
                => await repositorioWFAprovacaoNotaConselho.ObterNotasEmAprovacaoPorWorkflow(workflowId);
    }
}

