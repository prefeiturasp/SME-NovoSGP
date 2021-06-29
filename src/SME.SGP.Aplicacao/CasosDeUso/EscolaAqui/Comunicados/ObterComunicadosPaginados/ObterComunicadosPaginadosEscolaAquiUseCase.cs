using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosEscolaAquiUseCase : IObterComunicadosPaginadosEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public ObterComunicadosPaginadosEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<ComunicadoDto>> Executar(FiltroComunicadoDto filtro)
        {
            return await mediator.Send(new ObterComunicadosPaginadosQuery(filtro.DataEnvio,
                                                                          filtro.DataExpiracao,
                                                                          filtro.GruposId,
                                                                          filtro.Titulo,
                                                                          filtro.AnoLetivo,
                                                                          filtro.CodigoDre,
                                                                          filtro.CodigoUe,
                                                                          filtro.Modalidades,
                                                                          filtro.Semestre,
                                                                          filtro.Turmas));
        }
    }
}
