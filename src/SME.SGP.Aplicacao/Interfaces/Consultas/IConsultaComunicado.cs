using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaComunicado
    {
        Task<ComunicadoCompletoDto> BuscarPorIdAsync(long id);
    }
}