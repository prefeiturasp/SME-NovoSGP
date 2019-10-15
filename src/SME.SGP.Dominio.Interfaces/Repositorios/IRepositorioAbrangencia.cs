using SME.SGP.Dto;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAbrangencia
    {
        Task<bool> JaExisteAbrangencia(string login, Guid perfil);

        Task RemoverAbrangencias(string login);

        Task<long> SalvarDre(AbrangenciaDreRetornoEolDto abrangenciaDre, string login, Guid perfil);

        Task<long> SalvarTurma(AbrangenciaTurmaRetornoEolDto abrangenciaTurma, long idAbragenciaUe);

        Task<long> SalvarUe(AbrangenciaUeRetornoEolDto abrangenciaUe, long idAbragenciaDre);
    }
}