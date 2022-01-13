using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUeConsulta
    {
        IEnumerable<Ue> ListarPorCodigos(string[] codigos);

        Tuple<List<Ue>, string[]> MaterializarCodigosUe(string[] idUes);

        Task<IEnumerable<Modalidade>> ObterModalidades(string ueCodigo, int ano, IEnumerable<Modalidade> modalidadesQueSeraoIgnoradas);

        Ue ObterPorCodigo(string ueId);

        Task<Ue> ObterUeComDrePorCodigo(string ueCodigo);

        Ue ObterPorId(long id);

        IEnumerable<Ue> ObterPorDre(long dreId);

        Task<IEnumerable<Ue>> ObterUesComDrePorDreEModalidade(string dreCodigo, Modalidade modalidade);
        Task<IEnumerable<string>> ObterCodigosUEs();
        IEnumerable<Ue> ObterTodas();

        Task<IEnumerable<Turma>> ObterTurmas(string ueCodigo, Modalidade modalidade, int ano, bool ehHistorico);
        Task<TipoEscola> ObterTipoEscolaPorCodigo(string ueCodigo);
        Task<int> ObterQuantidadeTurmasSeriadas(long ueId, int ano);
        Ue ObterUEPorTurma(string turmaId);

        Task<IEnumerable<Ue>> ObterUEsSemPeriodoFechamento(long periodoEscolarId, int ano, int[] modalidades, DateTime dataReferencia);
        Task<bool> ValidarUeEducacaoInfantil(long ueId);

        Task<IEnumerable<Ue>> ObterUesPorModalidade(int[] modalidades, int anoLetivo = 0);
        Task<IEnumerable<Ue>> ObterUesPorIds(long[] ids);

        Task<Ue> ObterUePorId(long id);
        Task<IEnumerable<Ue>> ObterUEsComDREsPorIds(long[] ids);
        Task<Ue> ObterUEPorTurmaId(long turmaId);
        Task<Ue> ObterUeComDrePorId(long ueId);
        Task<IEnumerable<string>> ObterUesCodigosPorDreAsync(long dreId);
        Task<int> ObterQuantidadeUesPorAnoLetivoAsync(int anoLetivo);
        Task<IEnumerable<string>> ObterUesCodigosPorModalidadeEAnoLetivo(Modalidade modalidade, int anoLetivo);
        Task<DreUeDto> ObterCodigosDreUePorId(long ueId);
        Task<IEnumerable<long>> ObterTodosIds();
        Task<IEnumerable<Ue>> ObterUEsComDREsPorModalidadeTipoCalendarioQuery(int[] modalidades, int anoLetivo = 0);
    }
}