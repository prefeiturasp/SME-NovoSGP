using System.Threading.Tasks;

namespace SME.SGP.Auditoria.Worker.Interfaces
{
    public interface IRegistrarAuditoriaUseCase
    {
        Task Executar(MensagemRabbit mensagem);
    }
}
