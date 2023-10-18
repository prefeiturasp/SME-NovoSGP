using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioDinamicoNAAPA
    {
        Task<RelatorioDinamicoNAAPADto> ObterRelatorioDinamicoNAAPA(FiltroRelatorioDinamicoNAAPADto filtro, Paginacao paginacao);
    }
}
