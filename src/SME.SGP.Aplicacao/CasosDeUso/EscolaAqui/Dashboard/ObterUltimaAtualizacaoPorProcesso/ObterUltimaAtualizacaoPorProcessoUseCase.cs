using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard
{
    public class ObterUltimaAtualizacaoPorProcessoUseCase : IObterUltimaAtualizacaoPorProcessoUseCase
    {
        private readonly IMediator mediator;

        public ObterUltimaAtualizacaoPorProcessoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<UltimaAtualizaoWorkerPorProcessoResultado> Executar(string nomeProcesso)
        {
            return await mediator.Send(new ObterUltimaAtualizacaoPorProcessoQuery(nomeProcesso));
        }


    }
}
