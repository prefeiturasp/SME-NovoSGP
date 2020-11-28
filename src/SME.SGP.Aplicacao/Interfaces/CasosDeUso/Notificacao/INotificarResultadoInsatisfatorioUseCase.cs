using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface INotificarResultadoInsatisfatorioUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}