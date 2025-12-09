using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioSecaoAtendimentoNAAPA : RepositorioBase<SecaoEncaminhamentoNAAPA>, IRepositorioSecaoAtendimentoNAAPA
    {

        private const int SECAO_ITINERANCIA_NAAPA = 3;
        private const int PRIMEIRA_ETAPA_NAAPA = 1;
        private const string NOME_COMPONENTE_QUESTAO_ANEXOS = "ANEXO_ITINERANCIA";
        public RepositorioSecaoAtendimentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(int modalidade, long? encaminhamentoNAAPAId = null)
        {
            var query = $@"SELECT sea.id
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
                         WHERE not sea.excluido and sea.nome_componente <> '{AtendimentoNAAPAConstants.SECAO_ITINERANCIA}'
                            AND ((senm.modalidade_codigo = @modalidade) or (senm.modalidade_codigo is null)) 
                            AND q.tipo = @tipoQuestionario
                         ORDER BY sea.etapa, sea.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { modalidade, tipoQuestionario = (int)TipoQuestionario.EncaminhamentoNAAPA, encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0 });
        }

        public async Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorModalidade(int? modalidade, long? encaminhamentoNAAPAId = null)
        {
            var query = new StringBuilder($@"SELECT sea.*, eas.*, q.*, ee.*
                                            FROM secao_encaminhamento_naapa sea 
                                                join questionario q on q.id = sea.questionario_id 
                                                left join encaminhamento_naapa_secao eas on eas.encaminhamento_escolar_id = @encaminhamentoNAAPAId
                                                                                        and eas.secao_encaminhamento_id = sea.id
                                                                                        and not eas.excluido  
                                                                                        and sea.nome_componente <> '{AtendimentoNAAPAConstants.SECAO_ITINERANCIA}'
                                                left join encaminhamento_escolar ee on ee.id = eas.encaminhamento_escolar_id
                                                left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = sea.id 
                                            WHERE not sea.excluido 
                                                  AND ((senm.modalidade_codigo = @modalidade) or (senm.modalidade_codigo is null)) 
                                            ORDER BY sea.etapa, sea.ordem; ");



            return await database.Conexao
                .QueryAsync<SecaoEncaminhamentoNAAPA,
                            EncaminhamentoNAAPASecao,
                            Questionario,
                            EncaminhamentoEscolar,
                            SecaoEncaminhamentoNAAPA>(
                    query.ToString(),
                    (secaoEncaminhamento, encaminhamentoSecao, questionario, encaminhamentoEscolar) =>
                    {
                        secaoEncaminhamento.EncaminhamentoNAAPASecao = encaminhamentoSecao;
                        secaoEncaminhamento.Questionario = questionario;

                        if (encaminhamentoSecao.NaoEhNulo())
                        {
                            encaminhamentoSecao.EncaminhamentoEscolar = encaminhamentoEscolar;
                            encaminhamentoSecao.EncaminhamentoEscolarId = encaminhamentoEscolar?.Id;
                        }

                        return secaoEncaminhamento;
                    },
                    new { encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0, modalidade },
                    splitOn: "id"
                );
        }


       

        public async Task<IEnumerable<AtendimentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaDto(long encaminhamentoNAAPAId)
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
                .QueryAsync<AtendimentoNAAPASecaoItineranciaDto, AuditoriaDto, AtendimentoNAAPASecaoItineranciaDto>(
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

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaDtoPaginado(long encaminhamentoNAAPAId, Paginacao paginacao)
        {
            var sql = new StringBuilder();
            MontaQueryConsulta(paginacao, sql, contador: false);
            
            var retorno = new PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>();

            var secoes = (await database.Conexao
                                    .QueryAsync<AtendimentoNAAPASecaoItineranciaDto, AuditoriaDto, AtendimentoNAAPASecaoItineranciaDto>(
                                        sql.ToString(), (encaminhamentoSecao, auditoria) =>
                                        {
                                            encaminhamentoSecao.Auditoria = auditoria;
                                            return encaminhamentoSecao;
                                        }, new { encaminhamentoNAAPAId })).ToList();

            sql.Clear();
            MontaQueryConsulta(paginacao, sql, contador: true);

            var qdadeSecoes = await database.Conexao
                                    .QueryFirstAsync<int>(
                                        sql.ToString(), new { encaminhamentoNAAPAId });

            if (qdadeSecoes > 0)
            {
                sql.Clear();
                MontaQueryConsultaArquivos(sql);
                var arquivosSecoes = await database.Conexao
                                        .QueryAsync<AnexoSecaoItineranciaAtendimentoNAAPADto>(
                                            sql.ToString(), new { encaminhamentoNAAPAId });
                if (arquivosSecoes.PossuiRegistros())
                    secoes.ForEach(secao =>
                    {
                        secao.Arquivos = arquivosSecoes.Where(arq => arq.SecaoItineranciaId.Equals(secao.Auditoria.Id))
                                                       .Select(arq => new ArquivoResumidoDto() { Codigo = arq.CodigoArquivo, Nome = arq.NomeArquivo });
                    });
            }

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
                            left join vw_resposta_tipo_atendimento questaoTipoAtendimento on questaoTipoAtendimento.encaminhamento_naapa_secao_id = ens.id
                            where en.id = @encaminhamentoNAAPAId and not ens.excluido ");

            if (!contador)
                sql.AppendLine("order by questaoDataAtendimento.DataAtendimento desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private void MontaQueryConsultaArquivos(StringBuilder sql)
        {
            sql.AppendLine($@"with vw_resposta_arquivos as (
                                  select ens.id encaminhamento_naapa_secao_id, 
                                         a.codigo as CodigoArquivo, 
                                         a.nome as NomeArquivo
                                  from encaminhamento_naapa_secao ens   
                                  join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                                  join questao q on enq.questao_id = q.id 
                                  join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                                  inner join arquivo a on a.id = enr.arquivo_id 
                                  where q.nome_componente  = '{NOME_COMPONENTE_QUESTAO_ANEXOS}' and q.tipo = {(int)TipoQuestao.Upload}
                                        and not enq.excluido and not ens.excluido 
                        )
                        select ens.id SecaoItineranciaId, 
                               arquivo.CodigoArquivo, arquivo.NomeArquivo  ");
        
            sql.AppendLine(@"from encaminhamento_naapa en
                            inner join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = en.id
                            inner join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id 
                            inner join vw_resposta_arquivos arquivo on arquivo.encaminhamento_naapa_secao_id = ens.id
                            where en.id = @encaminhamentoNAAPAId and not ens.excluido ");

            sql.AppendLine("order by ens.id");
        }

        public async Task<AtendimentoNAAPAItineranciaAtendimentoDto> ObterAtendimentoSecaoItinerancia(long secaoId)
        {
            var query = @"select ens.encaminhamento_naapa_id EncaminhamentoId, 
                                ens.secao_encaminhamento_id SecaoEncaminhamentoNAAPAId, 
                                to_date(enr.texto,'yyyy-mm-dd') DataAtendimento    
                        from encaminhamento_naapa_secao ens   
                        join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                        join questao q on enq.questao_id = q.id 
                        join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                        where q.nome_componente = 'DATA_DO_ATENDIMENTO' and ens.id = @secaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<AtendimentoNAAPAItineranciaAtendimentoDto>(query, new { secaoId });
        }

        public async Task<IEnumerable<AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto>> ObterQuantidadeAtendimentosProfissionalPorUeAnoLetivoMes(long ueId, int mes, int anoLetivo)
        {
            var query = @$"select ens.criado_por as Nome, ens.criado_rf as Rf, 
                                  count(ens.id) as Quantidade, t.modalidade_codigo as Modalidade
                        from encaminhamento_naapa_secao ens
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
                        group by ens.criado_por, ens.criado_rf, t.modalidade_codigo; ";

            var retorno = await database.Conexao.QueryAsync<AtendimentosPorProfissionalAtendimentoNAAPADto>(query, new { ueId, mes, anoLetivo });
            if (retorno.Any())
                return retorno.Select(atendimento => new AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto(ueId, anoLetivo, mes, atendimento.Nome, atendimento.Rf, atendimento.Quantidade, atendimento.Modalidade));

            return Enumerable.Empty<AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto>();

        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesEncaminhamentoPorModalidades(TipoQuestionario tipoQuestionario, int[] modalidades = null)
        {
            var query = new StringBuilder($@"SELECT sea.id, sea.nome, 
	                                          q.id as QuestionarioId,
	                                          sea.etapa, 
	                                          sea.nome_componente as NomeComponente,
	                                          sea.ordem,
	                                          q.tipo as TipoQuestionario,
	                                          COALESCE(array_agg(senm.modalidade_codigo) FILTER (WHERE senm.modalidade_codigo IS NOT NULL), '{{}}'::int[]) as ModalidadesCodigo  
	                                        FROM secao_encaminhamento_naapa sea 
    	                                    join questionario q on q.id = sea.questionario_id 
    	                                    left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = sea.id 
	                                        WHERE not sea.excluido 
	  	                                        and q.tipo = @tipoQuestionario
                                                {(modalidades.PossuiRegistros() ?
                                                 $@" and exists (select seaAux.id from secao_encaminhamento_naapa seaAux 
    	                                                         left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = seaAux.id 
    	                                                         where seaAux.id = sea.id and
                  	                                                  ((senm.modalidade_codigo = any(@modalidades)) or (senm.modalidade_codigo is null)))" : string.Empty) }  
	                                        GROUP BY
    	                                        sea.id, sea.nome, q.id, sea.etapa, sea.nome_componente, sea.ordem, q.tipo
	                                        ORDER BY 
    	                                        sea.etapa, sea.ordem; ");

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(
                    query.ToString(), new
                    {
                        tipoQuestionario = (int)tipoQuestionario,
                        modalidades
                    });           
        }

        public async Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoIndividual(long? encaminhamentoNAAPAId = null)
        {

            int tipoQuestionario = (int)TipoQuestionario.EncaminhamentoNAAPAIndividual;

            var query = new StringBuilder($@"SELECT sea.*, eas.*, q.*
                                            FROM secao_encaminhamento_naapa sea 
                                                join questionario q on q.id = sea.questionario_id 
                                                left join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = @encaminhamentoNAAPAId
                                                                                        and eas.secao_encaminhamento_id = sea.id
                                                                                        and not eas.excluido  
                                              --  left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = sea.id 
                                            WHERE not sea.excluido 
                                              AND q.tipo = @tipoQuestionario
                                            ORDER BY sea.etapa, sea.ordem; ");


            return await database.Conexao
                .QueryAsync<SecaoEncaminhamentoNAAPA, EncaminhamentoNAAPASecao, Questionario, SecaoEncaminhamentoNAAPA>(
                    query.ToString(), (secaoEncaminhamento, encaminhamentoSecao, questionario) =>
                    {
                        secaoEncaminhamento.EncaminhamentoNAAPASecao = encaminhamentoSecao;
                        secaoEncaminhamento.Questionario = questionario;
                        return secaoEncaminhamento;
                    }, new { encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0, tipoQuestionario }, splitOn: "id");
        }
    }
}
