using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoGrupo
    {
        Task ExcluirPorIdComunicado(long id);

        Task<long> SalvarAsync(ComunicadoGrupo comunicadoGrupo);
    }
}