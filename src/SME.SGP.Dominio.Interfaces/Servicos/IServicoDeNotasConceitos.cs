using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoDeNotasConceitos
    {
        Task Salvar(IEnumerable<NotaConceito> notasConceitos, string professorRf, string turmaId, string disciplinaId);

        NotaTipoValor TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa);
    }
}