using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioTesteLog  
    {
        Task Gravar(string mensagem);
    }
}
