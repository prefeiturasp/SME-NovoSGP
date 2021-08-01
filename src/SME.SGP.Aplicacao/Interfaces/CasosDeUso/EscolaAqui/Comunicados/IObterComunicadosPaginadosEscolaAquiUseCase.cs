using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterComunicadosPaginadosEscolaAquiUseCase : IUseCase<FiltroComunicadoDto, PaginacaoResultadoDto<ComunicadoDto>>
    {
    }
}
