using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordo: IRepositorioBase<DiarioBordo>
    {
        Task<DiarioBordo> ObterPorAulaId(long aulaId);
        Task<bool> ExisteDiarioParaAula(long aulaId);
        Task ExcluirDiarioBordoDaAula(long aulaId);
        Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorPeriodoPaginado(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao);
        Task<IEnumerable<long>> ObterIdsPorDevolutiva(long devolutivaId);
    }
}
