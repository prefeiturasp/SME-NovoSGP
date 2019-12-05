using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAula
    {
        Task<string> Excluir(Aula aula, RecorrenciaAula recorrencia, Usuario usuario);

        Task GravarRecorrencia(bool inclusao, Aula aula, Usuario usuario, RecorrenciaAula recorrencia);

        Task<string> Salvar(Aula aula, Usuario usuario, RecorrenciaAula recorrencia);
    }
}