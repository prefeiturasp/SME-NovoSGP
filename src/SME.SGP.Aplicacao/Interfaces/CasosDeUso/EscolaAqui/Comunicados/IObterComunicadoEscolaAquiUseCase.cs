using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface IObterComunicadoEscolaAquiUseCase
    {
        Task<ComunicadoCompletoDto> Executar(long id);
    }    
}
