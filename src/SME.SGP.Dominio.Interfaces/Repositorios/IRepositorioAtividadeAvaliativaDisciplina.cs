using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativaDisciplina : IRepositorioBase<AtividadeAvaliativaDisciplina>
    {
        Task<IEnumerable<AtividadeAvaliativaDisciplina>> ListarPorIdAtividade(long atividadeAvaliativaId);
        bool PossuiDisciplinas(long atividadeAvaliativaId, long disciplinaId);
        Task<IEnumerable<AtividadeAvaliativaDisciplina>> ObterAvaliacoesBimestrais(long tipoCalendarioId, string turmaId, long disciplinaId, int bimestre);
    }
}
