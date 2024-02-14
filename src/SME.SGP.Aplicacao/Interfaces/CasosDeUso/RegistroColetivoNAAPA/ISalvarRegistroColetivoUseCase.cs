using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarRegistroColetivoUseCase
    {
        Task<ResultadoRegistroColetivoDto> Executar(RegistroColetivoDto registroColetivo);
    }
}
