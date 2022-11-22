using System;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoNAAPA : IRepositorioBase<EncaminhamentoNAAPA>
    {
        Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> ListarPaginado(bool exibirHistorico, int anoLetivo, long dreId, 
            string codigoUe, string nomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, 
            int situacao, long prioridade, long[] turmasIds, Paginacao paginacao);

        Task<bool> VerificaSeExisteEncaminhamentoPorAluno(string requestCodigoEstudante, long requestUeId);
        Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorId(long requestId);
        Task<EncaminhamentoNAAPA> ObterEncaminhamentoComTurmaPorId(long requestEncaminhamentoId);
    }
}
