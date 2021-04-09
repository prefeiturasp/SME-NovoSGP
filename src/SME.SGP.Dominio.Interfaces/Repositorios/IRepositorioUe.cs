using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUe
    {
        IEnumerable<Ue> ListarPorCodigos(string[] codigos);

        IEnumerable<Ue> MaterializarCodigosUe(string[] idUes, out string[] codigosNaoEncontrados);

        Task<IEnumerable<Modalidade>> ObterModalidades(string ueCodigo, int ano);

        Ue ObterPorCodigo(string ueId);

        Task<Ue> ObterUeComDrePorCodigo(string ueCodigo);

        Ue ObterPorId(long id);

        IEnumerable<Ue> ObterPorDre(long dreId);

        Task<IEnumerable<Ue>> ObterUesComDrePorDreEModalidade(string dreCodigo, Modalidade modalidade);

        IEnumerable<Ue> ObterTodas();

        Task<IEnumerable<Turma>> ObterTurmas(string ueCodigo, Modalidade modalidade, int ano);
        Task<int> ObterQuantidadeTurmasSeriadas(long ueId, int ano);
        Ue ObterUEPorTurma(string turmaId);

        Task<IEnumerable<Ue>> SincronizarAsync(IEnumerable<Ue> entidades, IEnumerable<Dre> dres);
        Task<IEnumerable<Ue>> ObterUEsSemPeriodoFechamento(long periodoEscolarId, int ano, int[] modalidades);
        Task<bool> ValidarUeEducacaoInfantil(long ueId);

        Task<IEnumerable<Ue>> ObterUesPorModalidade(int[] modalidades, int anoLetivo = 0);
        Task<IEnumerable<Ue>> ObterUesPorIds(long[] ids);
        Task<Ue> ObterUEPorTurmaId(long turmaId);
        Task<long> IncluirAsync(Ue ueParaIncluir);
        Task AtualizarAsync(Ue ueParaAtualizar);
    }
}