using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioDinamicoNAAPA
    {
        Task<RelatorioDinamicoNAAPADto> ObterRelatorioDinamicoNAAPA(FiltroRelatorioDinamicoNAAPADto filtro, Paginacao paginacao, IEnumerable<Questao> questoesParaTotalizadores);
    }
}
