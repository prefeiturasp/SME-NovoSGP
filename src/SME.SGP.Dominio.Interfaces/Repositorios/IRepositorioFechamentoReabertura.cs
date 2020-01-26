using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoReabertura : IRepositorioBase<FechamentoReabertura>
    {
        void ExcluirBimestres(long id);

        Task<IEnumerable<FechamentoReabertura>> Listar(long tipoCalendarioId, long? dreId, long? ueId);

        Task<PaginacaoResultadoDto<FechamentoReabertura>> ListarPaginado(long tipoCalendarioId, string dreCodigo, string ueCodigo, Paginacao paginacao);

        FechamentoReabertura ObterCompleto(long idFechamentoReabertura = 0, long workflowId = 0);

        Task SalvarBimestre(FechamentoReaberturaBimestre fechamentoReabertura);
    }
}