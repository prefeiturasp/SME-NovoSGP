using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativaDisciplina : IRepositorioBase<AtividadeAvaliativaDisciplina>
    {
        Task<IEnumerable<AtividadeAvaliativaDisciplina>> ListarPorIdAtividade(long atividadeAvaliativaId);
        bool PossuiDisciplinas(long atividadeAvaliativaId, string disciplinaId);
        Task<IEnumerable<AtividadeAvaliativaDisciplina>> ObterAvaliacoesBimestrais(long tipoCalendarioId, string turmaId, string disciplinaId, int bimestre);
        Task<IEnumerable<AtividadeAvaliativaDisciplina>> ObterDisciplinasAtividadeAvaliativa(long atividadeAvaliativaId);
    }
}
