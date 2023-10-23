using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios.Interfaces
{
    public interface IRepositorioSGP
    {
        Task AtualizarConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId);
        Task ExcluirConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId);
        Task AtualizarNotasConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task AtualizarRecomendacoesConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task ExcluirTurmasComplementaresConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task AtualizarWfAprovacaoConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task AtualizarParecerConclusivoConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task ExcluirConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);

    }
}
