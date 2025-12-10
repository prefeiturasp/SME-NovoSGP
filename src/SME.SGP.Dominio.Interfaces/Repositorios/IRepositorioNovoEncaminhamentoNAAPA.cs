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

        Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorId(long id);
    }
}