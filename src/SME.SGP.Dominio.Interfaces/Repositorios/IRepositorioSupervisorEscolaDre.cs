using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSupervisorEscolaDre : IRepositorioBase<SupervisorEscolaDre>
    {
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemPorDreESupervisor(string dreId, string supervisorId, bool excluidos = false);
        Task<IEnumerable<UnidadeEscolarResponsavelDto>> ObterUesAtribuidasAoResponsavelPorSupervisorIdeDre(string dreId, string supervisoresId, int tipoResponsavel);
        Task<List<SupervisorEscolasDreDto>> ObterTodosAtribuicaoResponsavelPorDreCodigo(string dreCodigo);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorUe(string ueId);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorUeAsync(string codigoUe);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorDreAsync(string codigoDre, TipoResponsavelAtribuicao? tipoResponsavelAtribuicao);
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsavelAtribuidoUePorUeTipo(string codigoUe, TipoResponsavelAtribuicao tipoResponsavelAtribuicao);
        Task<IEnumerable<DadosAbrangenciaSupervisorDto>> ObterDadosAbrangenciaSupervisor(string rfSupervisor, bool consideraHistorico, int anoLetivo, string codigoUe = null);
        Task<IEnumerable<UnidadeEscolarSemAtribuicaolDto>> ObterListaUEsParaNovaAtribuicaoPorCodigoDre(string dreCodigo);
        Task<int> VerificarSeJaExisteAtribuicaoAtivaComOutroResponsavelParaAqueleTipoUe(int tipo, string ueCodigo, string dreCodigo, string responsavelCodigo);
        Task<IEnumerable<ListaUesConsultaAtribuicaoResponsavelDto>> ObterListaDeUesFiltroPrincipal(string dreCodigo);
        Task<IEnumerable<ExisteAtribuicaoExcluidaDto>> VerificarSeJaExisteAtribuicaoExcluida(string dreCodigo, string[] uesCodigos, int tipoAtribuicao);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObterResponsaveisPorDreUeTiposAtribuicaoAsync(string codigoDre, string codigoUe, TipoResponsavelAtribuicao[] tiposResponsavelAtribuicao);
        Task<IEnumerable<SupervisorEscolasDreDto>> ObterSupervisoresPorUeTipo(string ueId, TipoResponsavelAtribuicao tipoResponsavelAtribuicao);
        Task RemoverAtribuicoesEmLote(IEnumerable<long> atribuicoesIds);
    }
}