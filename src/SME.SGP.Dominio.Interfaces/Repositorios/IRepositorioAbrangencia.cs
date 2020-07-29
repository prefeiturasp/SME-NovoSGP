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
        void AtualizaAbrangenciaHistorica(IEnumerable<long> paraAtualizar);

        void ExcluirAbrangencias(IEnumerable<long> ids);

        void InserirAbrangencias(IEnumerable<Abrangencia> abrangencias, string login);

        Task<bool> JaExisteAbrangencia(string login, Guid perfil);

        Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorFiltro(string texto, string login, Guid perfil, bool consideraHistorico);

        Task<IEnumerable<AbrangenciaSinteticaDto>> ObterAbrangenciaSintetica(string login, Guid perfil, string turmaId = "", bool consideraHistorico = false);

        Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, string login, Guid perfil, bool consideraHistorico = false);

        Task<IEnumerable<int>> ObterAnosLetivos(string login, Guid perfil, bool consideraHistorico);

        Task<AbrangenciaDreRetorno> ObterDre(string dreCodigo, string ueCodigo, string login, Guid perfil);

        Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0);

        Task<IEnumerable<int>> ObterModalidades(string login, Guid perfil, int anoLetivo, bool consideraHistorico);

        Task<IEnumerable<int>> ObterSemestres(string login, Guid perfil, Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0);

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, string login, Guid perfil, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0);

        Task<AbrangenciaUeRetorno> ObterUe(string codigo, string login, Guid perfil);

        Task<bool> UsuarioPossuiAbrangenciaAdm(long usuarioId);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0);

        bool PossuiAbrangenciaTurmaAtivaPorLogin(string login);

        void RemoverAbrangenciasForaEscopo(string login, Guid perfil, TipoAbrangenciaSincronizacao escopo);

        Task<bool> UsuarioPossuiAbrangenciaDeUmDosTipos(Guid perfil, IEnumerable<TipoPerfil> tipos);

        Task<IEnumerable<Modalidade>> ObterModalidadesPorUe(string codigoUe);

        Task<IEnumerable<OpcaoDropdownDto>> ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestre(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre);
    }
}