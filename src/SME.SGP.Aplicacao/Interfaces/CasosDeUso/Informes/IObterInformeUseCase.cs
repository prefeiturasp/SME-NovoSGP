using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterInformeUseCase
    {
        Task<InformesRespostaDto> Executar(long informeId);
    }
}
