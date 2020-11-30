using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoDeNotasConceitos
    {
        Task Salvar(IEnumerable<NotaConceito> notasConceitos, string professorRf, string turmaId, string disciplinaId);

        Task<NotaTipoValor> TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa, bool consideraHistorico = false);
        Task<NotaTipoValor> ObterNotaTipo(string turmaCodigo, DateTime data, bool consideraHistorico = false);
    }
}