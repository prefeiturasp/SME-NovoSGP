using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasSupervisor
    {
        Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDre(string dreId);

        Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDreESupervisor(string supervisorId, string dreId);

        IEnumerable<ResponsavelEscolasDto> ObterPorDreESupervisores(string[] supervisoresId, string dreId);

        Task<IEnumerable<ResponsavelEscolasDto>> ObterAtribuicaoResponsavel(FiltroObterSupervisorEscolasDto filtro);
    }
}