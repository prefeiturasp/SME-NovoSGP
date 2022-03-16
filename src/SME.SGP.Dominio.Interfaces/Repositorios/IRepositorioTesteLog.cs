using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTesteLog  
    {
        Task Gravar(string mensagem);
    }
}
