using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAbrangencia
    {
        Task RemoverAbrangencias(long idUsuario);

        Task<long> SalvarDre(AbrangenciaDreRetornoEolDto abrangenciaDre, long usuarioId, string perfil);

        Task<long> SalvarTurma(AbrangenciaTurmaRetornoEolDto abrangenciaTurma, long idAbragenciaUe);

        Task<long> SalvarUe(AbrangenciaUeRetornoEolDto abrangenciaUe, long idAbragenciaDre);
    }
}