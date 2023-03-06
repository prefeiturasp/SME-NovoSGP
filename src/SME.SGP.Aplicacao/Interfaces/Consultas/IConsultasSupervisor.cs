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

        Task<IEnumerable<UnidadeEscolarResponsavelDto>> ObterUesAtribuidasAoResponsavelPorSupervisorIdeDre(string supervisoresId, string dreId, int tipoResponsavel);

        Task<IEnumerable<ResponsavelEscolasDto>> ObterAtribuicaoResponsavel(FiltroObterSupervisorEscolasDto filtro);
        Task<IEnumerable<ListaUesConsultaAtribuicaoResponsavelDto>> ObterListaDeUesFiltroPrincipal(string dreCodigo);

        IEnumerable<TipoReponsavelRetornoDto> ObterTiposResponsaveis();
    }
}