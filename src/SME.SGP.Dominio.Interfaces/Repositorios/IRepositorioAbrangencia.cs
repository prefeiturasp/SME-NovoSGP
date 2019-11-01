using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAbrangencia
    {
        Task<bool> JaExisteAbrangencia(string login, Guid perfil);

        Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorFiltro(string texto, string login, Guid perfil);

        Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(string login, Guid perfil, Modalidade? modalidade = null);

        Task<IEnumerable<int>> ObterModalidades(string login, Guid perfil);

        Task<IEnumerable<int>> ObterSemestres(string login, Guid perfil, Modalidade modalidade);

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, string login, Guid perfil, Modalidade modalidade);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, string login, Guid perfil, Modalidade? modalidade = null);

        Task RemoverAbrangencias(string login);

        Task<long> SalvarDre(AbrangenciaDreRetornoEolDto abrangenciaDre, string login, Guid perfil);

        Task<long> SalvarTurma(AbrangenciaTurmaRetornoEolDto abrangenciaTurma, long idAbragenciaUe);

        Task<long> SalvarUe(AbrangenciaUeRetornoEolDto abrangenciaUe, long idAbragenciaDre);
    }
}