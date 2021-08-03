using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosEscolaAquiUseCase : AbstractUseCase, IObterComunicadosPaginadosEscolaAquiUseCase
    {
        public ObterComunicadosPaginadosEscolaAquiUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>> Executar(FiltroComunicadoDto filtro)
            => await mediator.Send(new ObterComunicadosPaginadosQuery(filtro.AnoLetivo,
                                                                      filtro.DreCodigo,
                                                                      filtro.UeCodigo,
                                                                      filtro.Modalidades,
                                                                      filtro.Semestre,
                                                                      filtro.DataEnvioInicio,
                                                                      filtro.DataEnvioFim,
                                                                      filtro.DataExpiracaoInicio,
                                                                      filtro.DataExpiracaoFim,
                                                                      filtro.Titulo,
                                                                      filtro.TurmasCodigo,
                                                                      filtro.AnosEscolares,
                                                                      filtro.TiposEscolas));
    }
}
