using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObservacaoEncaminhamentoNAAPA : RepositorioBase<EncaminhamentoNAAPAObservacao>, IRepositorioObservacaoEncaminhamentoNAAPA
    {
        public RepositorioObservacaoEncaminhamentoNAAPA(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {

        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId, long usuarioLogadoId, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>();
            var sql = @$"select 
                             id as IdObservacao,
                             observacao as Observacao,
                             CASE
                                WHEN Criado_RF = @usuarioId THEN true
                                ELSE false
                             end Proprietario,
                             alterado_em as AlteradoEm,
                             Alterado_Por as AlteradoPor,
                             Alterado_RF as AlteradoRF,
                             Criado_Em as CriadoEm,
                             Criado_Por as CriadoPor,
                             Criado_RF as CriadoRF
                            from encaminhamento_naapa_observacao 
                        where encaminhamento_naapa_id = @encaminhamentoNAAPAId ";

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);
            
            IEnumerable<EncaminhamentoNAAPAObservacoesDto> observacoes = await database.Conexao.QueryAsync<EncaminhamentoNAAPAObservacoesDto>(sql, new { encaminhamentoNAAPAId, usuarioId = usuarioLogadoId.ToString() });

            retorno.Items = observacoes;
            retorno.TotalRegistros = observacoes.Count();
            retorno.TotalPaginas = observacoes.Count() > 0 ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros) : 0;
            return retorno;
        }
    }
}
