using Elastic.Apm.Api;
using SME.SGP.Dominio.Enumerados;
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
        Task<RegistroColetivoResumidoDto> ObterRegistroColetivoPorId(long id);
    }
}
