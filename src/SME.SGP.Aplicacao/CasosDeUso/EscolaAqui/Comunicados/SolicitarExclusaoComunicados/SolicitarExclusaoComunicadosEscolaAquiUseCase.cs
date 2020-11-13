using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarExclusaoComunicadosEscolaAquiUseCase : ISolicitarExclusaoComunicadosEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public SolicitarExclusaoComunicadosEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Executar(long[] ids)
        {
            return await mediator.Send(new SolicitarExclusaoComunicadosEscolaAquiCommand(ids));
        }
    }
}
