using SME.SGP.Dominio.Enumerados;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarLogUseCase
    {
        Task SalvarInformacao(string mensagem, LogContexto logContexto);
    }
}
