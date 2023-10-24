using SME.SGP.Metrica.Worker.Entidade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios.Interfaces
{
    public interface IRepositorioSGPConsulta
    {
        Task<IEnumerable<long>> ObterUesIds();
        Task<IEnumerable<long>> ObterTurmasIds(int[] modalidades);
        Task<int> ObterQuantidadeAcessosDia(DateTime data);
        Task<IEnumerable<ConselhoClasseDuplicado>> ObterConselhosClasseDuplicados();
        Task<IEnumerable<ConselhoClasseAlunoDuplicado>> ObterConselhosClasseAlunoDuplicados(long ueId);
        Task<IEnumerable<ConselhoClasseNotaDuplicado>> ObterConselhosClasseNotaDuplicados();
        Task<IEnumerable<FechamentoTurmaDuplicado>> ObterFechamentosTurmaDuplicados();
        Task<IEnumerable<FechamentoTurmaDisciplinaDuplicado>> ObterFechamentosTurmaDisciplinaDuplicados();
        Task<IEnumerable<FechamentoAlunoDuplicado>> ObterFechamentosAlunoDuplicados(long ueId);
        Task<IEnumerable<FechamentoNotaDuplicado>> ObterFechamentosNotaDuplicados(long turmaId);
        Task<IEnumerable<ConsolidacaoConselhoClasseNotaNulos>> ObterConsolidacaoCCNotasNulos();
        Task<IEnumerable<ConsolidacaoConselhoClasseAlunoTurmaDuplicado>> ObterConsolidacaoCCAlunoTurmaDuplicados(long ueId);
        Task<IEnumerable<ConsolidacaoCCNotaDuplicado>> ObterConsolidacaoCCNotasDuplicados();

    }
}
