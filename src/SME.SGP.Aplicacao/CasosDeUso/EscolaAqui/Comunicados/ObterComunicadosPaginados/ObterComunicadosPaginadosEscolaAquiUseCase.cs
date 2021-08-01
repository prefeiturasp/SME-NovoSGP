using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosEscolaAquiUseCase : AbstractUseCase, IObterComunicadosPaginadosEscolaAquiUseCase
    {
        public ObterComunicadosPaginadosEscolaAquiUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<ComunicadoDto>> Executar(FiltroComunicadoDto filtro)
        {
            return await mediator.Send(new ObterComunicadosPaginadosQuery(filtro.DataEnvio,
                                                                          filtro.DataExpiracao,
                                                                          filtro.Titulo,
                                                                          filtro.AnoLetivo,
                                                                          filtro.CodigoDre,
                                                                          filtro.CodigoUe,
                                                                          filtro.Modalidades,
                                                                          filtro.Semestre,
                                                                          filtro.Turmas,
                                                                          filtro.EventoId));
        }
    }
}
