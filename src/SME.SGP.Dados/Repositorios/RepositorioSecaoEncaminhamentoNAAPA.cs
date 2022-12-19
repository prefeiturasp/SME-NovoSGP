using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoEncaminhamentoNAAPA : RepositorioBase<SecaoEncaminhamentoNAAPA>, IRepositorioSecaoEncaminhamentoNAAPA
    {
        public RepositorioSecaoEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoDtoPorEtapa(List<int> etapas, long? encaminhamentoNAAPAId)
        {
            var query = @"SELECT sea.id
	                            , sea.nome
	                            , sea.questionario_id as questionarioId
	                            , eas.concluido
	                            , sea.etapa
                                , sea.ordem
                                , sea.nome_componente as nomeComponente
                         FROM secao_encaminhamento_naapa sea
                        left join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = @encaminhamentoNAAPAId and eas.secao_encaminhamento_id = sea.id
                         WHERE not sea.excluido 
                           AND sea.etapa = ANY(@etapas)
                         ORDER BY sea.etapa, sea.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { etapas, encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0 });
        }

        public async Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorEtapaModalidade(List<int> etapas, int modalidade, long? encaminhamentoNAAPAId)
        {
            var query = new StringBuilder(@"SELECT sea.*, eas.*
                                            FROM secao_encaminhamento_naapa sea 
                                                left join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = @encaminhamentoNAAPAId
                                                                                        and eas.secao_encaminhamento_id = sea.id
                                                left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = sea.id 
                                            WHERE not sea.excluido AND sea.etapa = ANY(@etapas) 
                                                  AND ((senm.modalidade_codigo = @modalidade) or (senm.modalidade_codigo is null)) 
                                            ORDER BY sea.etapa, sea.ordem; ");

            return await database.Conexao
                .QueryAsync<SecaoEncaminhamentoNAAPA, EncaminhamentoNAAPASecao, SecaoEncaminhamentoNAAPA>(
                    query.ToString(), (secaoEncaminhamento, encaminhamentoSecao) =>
                    {
                        secaoEncaminhamento.EncaminhamentoNAAPASecao = encaminhamentoSecao;
                        return secaoEncaminhamento;
                    }, new { etapas, encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0, modalidade });
        }

        public async Task<IEnumerable<EncaminhamentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaEncaminhamentoDto(long encaminhamentoNAAPAId)
        {
            var query = new StringBuilder(@"with vw_resposta_data as (
                                            select ens.id encaminhamento_naapa_secao_id, 
                                                   to_date(enr.texto,'yyyy-mm-dd') DataAtendimento    
                                            from encaminhamento_naapa_secao ens   
                                            join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                                            join questao q on enq.questao_id = q.id 
                                            join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                                            left join opcao_resposta opr on opr.id = enr.resposta_id
                                            where q.nome_componente  = 'DATA_DO_ATENDIMENTO' 
                                            ),
                                            vw_resposta_tipo_atendimento as (
                                            select ens.id encaminhamento_naapa_secao_id,
                                                    opr.nome as TipoAtendimento,
                                                    enr.resposta_id  as TipoAtendimentoId
                                            from encaminhamento_naapa_secao ens   
                                            join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                                            join questao q on enq.questao_id = q.id 
                                            join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                                            left join opcao_resposta opr on opr.id = enr.resposta_id
                                            where q.nome_componente = 'TIPO_DO_ATENDIMENTO' 
                                            )
                                            select 
                                            secao.Nome,
                                            secao.questionario_id as QuestionarioId,
                                            secao.Etapa,
                                            secao.Ordem,
                                            questaoDataAtendimento.DataAtendimento,
                                            questaoTipoAtendimento.TipoAtendimento,
                                            ens.id,
                                            ens.Alterado_Em as AlteradoEm,
                                            ens.Alterado_Por as AlteradoPor,
                                            ens.Alterado_RF as AlteradoRF,
                                            ens.Criado_Em as CriadoEm,
                                            ens.Criado_Por as CriadoPor,
                                            ens.Criado_RF as CriadoRF
                                            from encaminhamento_naapa en
                                            inner join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = en.id
                                            inner join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id 
                                            inner join vw_resposta_data questaoDataAtendimento on questaoDataAtendimento.encaminhamento_naapa_secao_id = ens.id
                                            inner join vw_resposta_tipo_atendimento questaoTipoAtendimento on questaoTipoAtendimento.encaminhamento_naapa_secao_id = ens.id
                                            where en.id = @encaminhamentoNAAPAId");

            return await database.Conexao
                .QueryAsync<EncaminhamentoNAAPASecaoItineranciaDto, AuditoriaDto, EncaminhamentoNAAPASecaoItineranciaDto>(
                    query.ToString(), (encaminhamentoSecao, auditoria) =>
                    {
                        encaminhamentoSecao.Auditoria = auditoria;
                        return encaminhamentoSecao;
                    }, new { encaminhamentoNAAPAId });
        }
    }
}
