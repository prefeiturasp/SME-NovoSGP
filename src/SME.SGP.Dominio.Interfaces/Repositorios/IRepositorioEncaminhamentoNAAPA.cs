using System;
using System.Collections.Generic;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoNAAPA : IRepositorioBase<EncaminhamentoNAAPA>
    {
        Task<PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>> ListarPaginado(int anoLetivo, long dreId, 
            string codigoUe, string codigoNomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, 
            int situacao, long prioridade, long[] turmasIds, Paginacao paginacao, bool exibirEncerrados, OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] ordenacao);

        Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorId(long id);
        Task<EncaminhamentoNAAPA> ObterCabecalhoEncaminhamentoPorId(long id);
        Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorIdESecao(long id, long secaoId);
        Task<EncaminhamentoNAAPA> ObterEncaminhamentoComTurmaPorId(long requestEncaminhamentoId);

        Task<IEnumerable<EncaminhamentoNAAPACodigoArquivoDto>> ObterCodigoArquivoPorEncaminhamentoNAAPAId(long encaminhamentoId);

        Task<SituacaoDto> ObterSituacao(long id);
        Task<IEnumerable<AtendimentoNAAPADto>> ObterEncaminhamentosComSituacaoDiferenteDeEncerrado();

        Task<bool> VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(long encaminhamentoId);
        Task<bool> EncaminhamentoContemAtendimentosItinerancia(long encaminhamentoId);
        Task<IEnumerable<EncaminhamentosNAAPAConsolidadoDto>> ObterQuantidadeSituacaoEncaminhamentosPorUeAnoLetivo(long ueId, int anoLetivo);
        Task<bool> ExisteEncaminhamentoNAAPAAtivoParaAluno(string codigoAluno);
        Task<IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> ObterInformacoesDeNotificacaoDeInatividadeDeAtendimento(long ueId);
    }
}
