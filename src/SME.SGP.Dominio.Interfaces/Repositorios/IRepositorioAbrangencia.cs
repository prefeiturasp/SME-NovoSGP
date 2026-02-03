using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAbrangencia
    {
        Task AtualizaAbrangenciaHistorica(IEnumerable<long> paraAtualizar);

        Task AtualizaAbrangenciaHistoricaAnosAnteriores(IEnumerable<long> paraAtualizar, int anoLetivo);

        Task ExcluirAbrangencias(IEnumerable<long> ids);

        Task ExcluirAbrangenciasHistoricas(IEnumerable<long> ids);

        Task InserirAbrangencias(IEnumerable<Abrangencia> abrangencias, string login);

        Task<bool> JaExisteAbrangencia(string login, Guid perfil);

        Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorFiltro(string texto, string login, Guid perfil, bool consideraHistorico, string[] anosInfantilDesconsiderar = null);

        Task<IEnumerable<AbrangenciaSinteticaDto>> ObterAbrangenciaSintetica(string login, Guid perfil, string turmaId = "", bool consideraHistorico = false);
        Task<bool> ObterUsuarioPossuiAbrangenciaAcessoSondagemTiposEscola(string usuarioRF, Guid UsuarioPerfil);
        Task<IEnumerable<AbrangenciaHistoricaDto>> ObterAbrangenciaHistoricaPorLogin(string login);

        Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, string login, Guid perfil, bool consideraHistorico = false, bool abrangenciaPermitida = false);

        Task<IEnumerable<int>> ObterAnosLetivos(string login, Guid perfil, bool consideraHistorico, int anoMinimo);

        Task<IEnumerable<string>> ObterAnosTurmasPorCodigoUeModalidade(string login, Guid perfil, string codigoUe, Modalidade modalidade, bool consideraHistorico,int? anoLetivo);

        Task<AbrangenciaDreRetornoDto> ObterDre(string dreCodigo, string ueCodigo, string login, Guid perfil);

        Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "", bool filtroEhCodigo = false);

        Task<IEnumerable<int>> ObterModalidades(string login, Guid perfil, int anoLetivo, bool consideraHistorico, IEnumerable<Modalidade> modadlidadesQueSeraoIgnoradas);

        Task<IEnumerable<int>> ObterSemestres(string login, Guid perfil, Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0, string dreCodigo = null, string ueCodigo = null);

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, string login, Guid perfil, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0);
        Task<IEnumerable<Modalidade>> ObterModalidadesPorUeAbrangencia(string codigoUe, string login, Guid perfilAtual, IEnumerable<Modalidade> modadlidadesQueSeraoIgnoradas, bool consideraHistorico = false, int anoLetivo = 0);
        Task<AbrangenciaUeRetorno> ObterUe(string codigo, string login, Guid perfil);
        Task<bool> UsuarioPossuiAbrangenciaAdm(long usuarioId);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, int[] ignorarTiposUE = null, string filtro = "", bool filtroEhCodigo = false);

        bool PossuiAbrangenciaTurmaAtivaPorLogin(string login, bool cj = false);

        bool PossuiAbrangenciaTurmaInfantilAtivaPorLogin(string login, bool cj = false);

        Task RemoverAbrangenciasForaEscopo(string login, Guid perfil, TipoAbrangenciaSincronizacao escopo);

        Task<bool> UsuarioPossuiAbrangenciaDeUmDosTipos(Guid perfil, IEnumerable<TipoPerfil> tipos);

        Task<IEnumerable<Modalidade>> ObterModalidadesPorUe(string codigoUe);
        Task<IEnumerable<Modalidade>> ObterModalidadesPorCodigosUe(string[] codigosUe);

        Task<IEnumerable<OpcaoDropdownDto>> ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestre(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre, string[] anosInfantilDesconsiderar=null);

        Task<IEnumerable<OpcaoDropdownDto>> ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestreAnos(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre, IList<string> anos);
        Task<IEnumerable<Abrangencia>> ObterAbrangenciaGeralPorUsuarioId(long usuarioId);
        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasPorTipos(string codigoUe, string login, Guid perfil, Modalidade modalidade, int[] tipos, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string[] anosInfantilDesconsiderar = null);
        Task<IEnumerable<DropdownTurmaRetornoDto>> ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolares(int anoLetivo, string codigoUe, int[] modalidades, int semestre, string[] anos, bool historico);
        Task<IEnumerable<string>> ObterLoginsAbrangenciaUePorPerfil(long ueId, Guid perfil, bool historica = false);
        Task<IEnumerable<string>> ObterProfessoresTurmaPorAbrangencia(string turmaCodigo);
        Task<IEnumerable<Turma>> ObterTurmasPorAbrangenciaCPParaCopiaAvaliacao(int anoLetivo, string codigoRf, int modalidadeTurma, string anoTurma, long turmaIdReferencia);
        Task<bool> VerificarUsuarioLogadoPertenceMesmaUE(string codigoUe, string login, Guid perfil,Modalidade modalidade, int anoLetivo, int periodo, bool consideraHistorico = false);
        Task<bool> VerificaSeUsuarioPossuiAbrangencia(string usuarioRf, Guid perfil);
    }
}