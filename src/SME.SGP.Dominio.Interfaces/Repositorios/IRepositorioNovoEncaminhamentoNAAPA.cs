using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioNovoEncaminhamentoNAAPA : IRepositorioBase<EncaminhamentoEscolar>
    {
        Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>> ListarPaginado(
            int anoLetivo,
            long dreId,
            string codigoUe,
            string codigoNomeAluno,
            DateTime? dataAberturaQueixaInicio,
            DateTime? dataAberturaQueixaFim,
            int situacao,
            long prioridade,
            long[] turmasIds,
            Paginacao paginacao,
            bool exibirEncerrados,
            OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao);

        Task<EncaminhamentoEscolar> ObterEncaminhamentoPorId(long id);

        Task<EncaminhamentoEscolar> ObterEncaminhamentoComTurmaPorId(long requestEncaminhamentoId);
        Task<bool> VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(long encaminhamentoId);
        Task<bool> ExisteEncaminhamentoNAAPAAtivoParaAluno(string codigoAluno);
    }
}