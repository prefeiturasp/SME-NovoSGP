using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObservacaoEncaminhamentoNAAPA : RepositorioBase<EncaminhamentoNAAPAObservacao>, IRepositorioObservacaoEncaminhamentoNAAPA
    {
        public RepositorioObservacaoEncaminhamentoNAAPA(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
            
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId,long usuarioLogadoId, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>();
            var sql = @$"select 
                             id as IdObservacao,
                             observacao,
                             CASE
                                WHEN Criado_RF = @usuarioLogadoId THEN true
                                ELSE false
                             end Proprietario,
                             alterado_em as AlteradoEm,
                             Alterado_Por as AlteradoPor,
                             Alterado_RF as AlteradoRF,
                             Criado_Em as CriadoEm,
                             Criado_Por as CriadoPor,
                             Criado_RF as CriadoRF
                            from encaminhamento_naapa_observacao 
                        where encaminhamento_naapa_id = @encaminhamentoNAAPAId";

            var observacoes = await database.Conexao
                                            .QueryAsync<EncaminhamentoNAAPAObservacoesDto, AuditoriaDto, EncaminhamentoNAAPAObservacoesDto>(
                                                        sql, (encaminhamentoObservacao, auditoria) => 
                                                        {
                                                            encaminhamentoObservacao.Auditoria = auditoria;
                                                            return encaminhamentoObservacao;
                                                        } , new { encaminhamentoNAAPAId,usuarioId = usuarioLogadoId.ToString() });


            retorno.Items = observacoes;
            retorno.TotalRegistros = observacoes.Count();
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);
            return retorno;
        }
    }
}
