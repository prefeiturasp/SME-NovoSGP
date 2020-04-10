using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaComunicado
    {
        Task<ComunicadoCompletoDto> BuscarPorIdAsync(long id);
    }
}