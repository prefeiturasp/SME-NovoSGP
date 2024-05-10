using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosParaFiltroDaDashboardUseCase : AbstractUseCase, IObterComunicadosParaFiltroDaDashboardUseCase
    {
        public ObterComunicadosParaFiltroDaDashboardUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> Executar(ObterComunicadosParaFiltroDaDashboardDto obterComunicadosFiltroDto)
            => await mediator.Send(new ObterComunicadosParaFiltroDaDashboardQuery(obterComunicadosFiltroDto));
    }
}
