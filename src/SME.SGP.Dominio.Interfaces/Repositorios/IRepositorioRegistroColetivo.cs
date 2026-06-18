using SME.SGP.Infra;
using System.Threading.Tasks;
using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroColetivo : IRepositorioBase<RegistroColetivo>
    {
        Task<PaginacaoResultadoDto<RegistroColetivoListagemDto>> ListarPaginado(long dreId, long? ueId,
                                                                                DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId,
                                                                                Paginacao paginacao);
        Task<RegistroColetivoCompletoDto> ObterRegistroColetivoCompletoPorId(long id);
    }
}
