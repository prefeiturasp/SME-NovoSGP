﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoEncaminhamentoNAAPA : RepositorioBase<SecaoEncaminhamentoNAAPA>, IRepositorioSecaoEncaminhamentoNAAPA
    {

        private const int SECAO_ITINERANCIA_NAAPA = 3;
        private const int PRIMEIRA_ETAPA_NAAPA = 1;
        public RepositorioSecaoEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(int modalidade, long? encaminhamentoNAAPAId)
        {
            var query = @"SELECT sea.id
	                            , sea.nome
	                            , sea.questionario_id as questionarioId
	                            , eas.concluido
	                            , sea.etapa
                                , sea.ordem
                                , sea.nome_componente as nomeComponente
                         FROM secao_encaminhamento_naapa sea
                         inner join questionario q on q.id = sea.questionario_id 
                         left join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = @encaminhamentoNAAPAId 
                                                                 and eas.secao_encaminhamento_id = sea.id
                                                                 and not eas.excluido   
                         left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = sea.id 
                         WHERE not sea.excluido and sea.nome_componente <> 'QUESTOES_ITINERACIA'
                            AND ((senm.modalidade_codigo = @modalidade) or (senm.modalidade_codigo is null)) 
                            AND q.tipo = @tipoQuestionario
                         ORDER BY sea.etapa, sea.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { modalidade, tipoQuestionario = (int)TipoQuestionario.EncaminhamentoNAAPA, encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0 });
        }

        public async Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorModalidade(int? modalidade, long? encaminhamentoNAAPAId)
        {
            var query = new StringBuilder(@"SELECT sea.*, eas.*, q.*
                                            FROM secao_encaminhamento_naapa sea 
                                                join questionario q on q.id = sea.questionario_id 
                                                left join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = @encaminhamentoNAAPAId
                                                                                        and eas.secao_encaminhamento_id = sea.id
                                                                                        and not eas.excluido  
                                                                                        and sea.nome_componente <> 'QUESTOES_ITINERACIA'
                                                left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = sea.id 
                                            WHERE not sea.excluido 
                                                  AND ((senm.modalidade_codigo = @modalidade) or (senm.modalidade_codigo is null)) 
                                            ORDER BY sea.etapa, sea.ordem; ");

            return await database.Conexao
                .QueryAsync<SecaoEncaminhamentoNAAPA, EncaminhamentoNAAPASecao, Questionario, SecaoEncaminhamentoNAAPA>(
                    query.ToString(), (secaoEncaminhamento, encaminhamentoSecao, questionario) =>
                    {
                        secaoEncaminhamento.EncaminhamentoNAAPASecao = encaminhamentoSecao;
                        secaoEncaminhamento.Questionario = questionario;
                        return secaoEncaminhamento;
                    }, new { encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0, modalidade },splitOn: "id");
        }

        public async Task<IEnumerable<EncaminhamentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaDto(long encaminhamentoNAAPAId)
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
                                            where en.id = @encaminhamentoNAAPAId and not ens.excluido ");

            return await database.Conexao
                .QueryAsync<EncaminhamentoNAAPASecaoItineranciaDto, AuditoriaDto, EncaminhamentoNAAPASecaoItineranciaDto>(
                    query.ToString(), (encaminhamentoSecao, auditoria) =>
                    {
                        encaminhamentoSecao.Auditoria = auditoria;
                        return encaminhamentoSecao;
                    }, new { encaminhamentoNAAPAId });
        }

        public async Task<SecaoQuestionarioDto> ObterSecaoQuestionarioDtoPorId(long secaoId)
        {
            var query = @"SELECT sea.id
	                            , sea.nome
	                            , sea.questionario_id as questionarioId
	                            , sea.etapa
                                , sea.ordem
                                , sea.nome_componente as nomeComponente
                         FROM secao_encaminhamento_naapa sea
                         WHERE sea.id = @secaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<SecaoQuestionarioDto>(query, new { secaoId });
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaDtoPaginado(long encaminhamentoNAAPAId, Paginacao paginacao)
        {
            var sql = new StringBuilder();
            MontaQueryConsulta(paginacao, sql, contador: false);
            
            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPASecaoItineranciaDto>();

            var secoes = await database.Conexao
                                    .QueryAsync<EncaminhamentoNAAPASecaoItineranciaDto, AuditoriaDto, EncaminhamentoNAAPASecaoItineranciaDto>(
                                        sql.ToString(), (encaminhamentoSecao, auditoria) =>
                                        {
                                            encaminhamentoSecao.Auditoria = auditoria;
                                            return encaminhamentoSecao;
                                        }, new { encaminhamentoNAAPAId });

            sql.Clear();
            MontaQueryConsulta(paginacao, sql, contador: true);

            var qdadeSecoes = await database.Conexao
                                    .QueryFirstAsync<int>(
                                        sql.ToString(), new { encaminhamentoNAAPAId });

            retorno.Items = secoes;
            retorno.TotalRegistros = qdadeSecoes;
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador)
        {
            var sqlSelect = @"with vw_resposta_data as (
                        select ens.id encaminhamento_naapa_secao_id, 
                                to_date(enr.texto,'yyyy-mm-dd') DataAtendimento    
                        from encaminhamento_naapa_secao ens   
                        join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                        join questao q on enq.questao_id = q.id 
                        join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
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
                        join opcao_resposta opr on opr.id = enr.resposta_id
                        where q.nome_componente = 'TIPO_DO_ATENDIMENTO' 
                        )
                        select ";

            sql.AppendLine(sqlSelect);
            if (contador)
                sql.AppendLine("count(ens.id) as qdade ");
            else
            {
                sql.AppendLine(@"secao.Nome,
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
                ");
            }

            sql.AppendLine(@"from encaminhamento_naapa en
                            inner join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = en.id
                            inner join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id 
                            inner join vw_resposta_data questaoDataAtendimento on questaoDataAtendimento.encaminhamento_naapa_secao_id = ens.id
                            inner join vw_resposta_tipo_atendimento questaoTipoAtendimento on questaoTipoAtendimento.encaminhamento_naapa_secao_id = ens.id
                            where en.id = @encaminhamentoNAAPAId and not ens.excluido ");

            if (!contador)
                sql.AppendLine("order by questaoDataAtendimento.DataAtendimento desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        public async Task<EncaminhamentoNAAPAItineranciaAtendimentoDto> ObterAtendimentoSecaoItinerancia(long secaoId)
        {
            var query = @"select ens.encaminhamento_naapa_id EncaminhamentoId, 
                        		ens.secao_encaminhamento_id SecaoEncaminhamentoNAAPAId, 
                                to_date(enr.texto,'yyyy-mm-dd') DataAtendimento    
                        from encaminhamento_naapa_secao ens   
                        join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                        join questao q on enq.questao_id = q.id 
                        join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                        where q.nome_componente = 'DATA_DO_ATENDIMENTO' and ens.id = @secaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<EncaminhamentoNAAPAItineranciaAtendimentoDto>(query, new { secaoId });
        }

        public async Task<IEnumerable<AtendimentosProfissionalEncaminhamentoNAAPAConsolidadoDto>> ObterQuantidadeAtendimentosProfissionalPorUeAnoLetivoMes(long ueId, int mes, int anoLetivo)
        {
            var query = @$"select ens.criado_por as Nome, ens.criado_rf as Rf, count(ens.id) as Quantidade from encaminhamento_naapa_secao ens 
                        inner join encaminhamento_naapa_questao enq on enq.encaminhamento_naapa_secao_id = ens.id
                        inner join questao q on q.id = enq.questao_id 
                        inner join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                        inner join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id 
                        inner join encaminhamento_naapa en on en.id = ens.encaminhamento_naapa_id 
                        inner join turma t on t.id = en.turma_id 
                        where q.nome_componente = 'DATA_DO_ATENDIMENTO' and sen.etapa = {PRIMEIRA_ETAPA_NAAPA} and sen.ordem = {SECAO_ITINERANCIA_NAAPA} 
                        and not ens.excluido 
                        and not en.excluido 
                        and not enr.excluido 
                        and not enq.excluido 
                        and enr.texto is not null and enr.texto <> ''
                        and EXTRACT('Year' FROM to_date(enr.texto,'yyyy-mm-dd')) = @anoLetivo
                        and EXTRACT('Month' FROM to_date(enr.texto,'yyyy-mm-dd')) = @mes
                        and t.ue_id = @ueId
                        group by ens.criado_por, ens.criado_rf; ";

            var retorno = await database.Conexao.QueryAsync<AtendimentosPorProfissionalEncaminhamentoNAAPADto>(query, new { ueId, mes, anoLetivo });
            if (retorno.Any())
                return retorno.Select(atendimento => new AtendimentosProfissionalEncaminhamentoNAAPAConsolidadoDto(ueId, anoLetivo, mes, atendimento.Nome, atendimento.Rf, atendimento.Quantidade));

            return Enumerable.Empty<AtendimentosProfissionalEncaminhamentoNAAPAConsolidadoDto>();

        }
       
    }
}
