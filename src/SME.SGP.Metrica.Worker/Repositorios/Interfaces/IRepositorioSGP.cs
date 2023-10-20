using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios.Interfaces
{
    public interface IRepositorioSGP
    {
        Task AtualizarConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId);
        Task ExcluirConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId);
    }
}
