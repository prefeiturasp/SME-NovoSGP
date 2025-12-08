using System;
using System.Collections.Generic;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtendimentoNAAPA : IRepositorioBase<EncaminhamentoNAAPA>
    {
        Task<PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>> ListarPaginado(int anoLetivo, long dreId, 
            string codigoUe, string codigoNomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, 
            int situacao, long[] turmasIds, Paginacao paginacao, bool exibirEncerrados, OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao);

        Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorId(long id);
        Task<EncaminhamentoNAAPA> ObterCabecalhoEncaminhamentoPorId(long id);
        Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorIdESecao(long id, long secaoId);
        Task<EncaminhamentoNAAPA> ObterEncaminhamentoComTurmaPorId(long requestEncaminhamentoId);

        Task<IEnumerable<AtendimentoNAAPACodigoArquivoDto>> ObterCodigoArquivoPorEncaminhamentoNAAPAId(long encaminhamentoId);

        Task<SituacaoDto> ObterSituacao(long id);
        Task<IEnumerable<AtendimentoNAAPADto>> ObterEncaminhamentosComSituacaoDiferenteDeEncerrado();

        Task<bool> VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(long encaminhamentoId);
        Task<bool> EncaminhamentoContemAtendimentosItinerancia(long encaminhamentoId);
        Task<IEnumerable<AtendimentosNAAPAConsolidadoDto>> ObterQuantidadeSituacaoEncaminhamentosPorUeAnoLetivo(long ueId, int anoLetivo);
        Task<bool> ExisteEncaminhamentoNAAPAAtivoParaAluno(string codigoAluno);
        Task<IEnumerable<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> ObterInformacoesDeNotificacaoDeInatividadeDeAtendimento(long ueId);
    }
}
