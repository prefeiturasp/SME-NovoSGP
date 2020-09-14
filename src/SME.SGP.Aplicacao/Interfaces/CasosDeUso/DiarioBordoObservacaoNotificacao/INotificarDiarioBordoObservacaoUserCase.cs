using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface INotificarDiarioBordoObservacaoUserCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}