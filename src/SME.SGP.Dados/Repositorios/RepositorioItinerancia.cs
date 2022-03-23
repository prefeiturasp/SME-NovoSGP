using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItinerancia : RepositorioBase<Itinerancia>, IRepositorioItinerancia
    {
        public RepositorioItinerancia(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<ItineranciaObjetivosBaseDto>> ObterObjetivosBase()
        {
            var query = @"select id,
	                             nome,
	                             tem_descricao as TemDescricao
                            from itinerancia_objetivo_base iob  
                           where not excluido 
                           order by ordem  ";

            return await database.Conexao.QueryAsync<ItineranciaObjetivosBaseDto>(query);
        }

        public async Task<IEnumerable<ItineranciaAlunoQuestaoDto>> ObterQuestoesItineranciaAluno(long id)
        {
            var query = @"select iaq.id,
                                 iaq.questao_id as QuestaoId,       
                                 iaq.resposta as Resposta
                            from itinerancia_aluno_questao iaq 
                           where iaq.itinerancia_aluno_id = @id
                             and not iaq.excluido ";

            return await database.Conexao.QueryAsync<ItineranciaAlunoQuestaoDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaQuestaoBaseDto>> ObterItineranciaQuestaoBase(long[] tiposQuestionario)
        {
            var query = @"select q.id, q.nome, q.ordem, q1.tipo, q.obrigatorio 
                            from questao q
                           inner join questionario q1 on q1.id = q.questionario_id 
                           where q1.tipo = ANY(@tiposQuestionario)
                             and not q.excluido";

            return await database.Conexao.QueryAsync<ItineranciaQuestaoBaseDto>(query, new { tiposQuestionario });
        }

        public async Task<ItineranciaDto> ObterItineranciaPorId(long id)
        {
            var query = @"select id, 
                                 data_visita as DataVisita, 
                                 data_retorno_verificacao as DataRetornoVerificacao 
                            from itinerancia i 
                           where id = @id
                             and not excluido";

            return await database.Conexao.QueryFirstOrDefaultAsync<ItineranciaDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaAlunoDto>> ObterItineranciaAlunoPorId(long id)
        {
            var query = @" select id,
 		                          codigo_aluno as CodigoAluno,
                                  turma_id as TurmaId
                             from itinerancia_aluno ia 
                            where itinerancia_id = @id
                              and not excluido ";

            return await database.Conexao.QueryAsync<ItineranciaAlunoDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaObjetivoDto>> ObterObjetivosItineranciaPorId(long id)
        {
            var query = @"select iob.id,
                                 iob.nome,
                                 iob.tem_descricao,
                                 io.descricao 
                            from itinerancia_objetivo_base iob 
                           inner join itinerancia_objetivo io on io.itinerancia_base_id = iob.id 
                           where io.itinerancia_id = @id
                             and not io.excluido 
                           order by iob.ordem";

            return await database.Conexao.QueryAsync<ItineranciaObjetivoDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaQuestaoDto>> ObterQuestoesItineranciaPorId(long id, long tipoQuestionario)
        {
            var query = @"select iq.id,
                                 iq.questao_id as QuestaoId, 
                                 q.nome as Descricao,
                                 iq.resposta,
                                 iq.itinerancia_id as ItineranciaId,
                                 q.obrigatorio
                            from questao q
                           inner join questionario q1 on q1.id = q.questionario_id
                           inner join itinerancia_questao iq on iq.questao_id = q.id 
                           where iq.itinerancia_id = @Id
                             and q1.tipo = @tipoQuestionario
                             and not q.excluido
                           order by q.ordem";

            return await database.Conexao.QueryAsync<ItineranciaQuestaoDto>(query, new { id, tipoQuestionario });
        }

        public async Task<IEnumerable<int>> ObterAnosLetivosItinerancia()
        {
            var query = @"select distinct ano_letivo 
                            from itinerancia i 
                           where not excluido
                           order by ano_letivo desc";

            return await database.Conexao.QueryAsync<int>(query);
        }
        public async Task<Itinerancia> ObterEntidadeCompleta(long id)
        {
            var query = @"select i.*, ia.*, iaq.*, iq.* , io.*, iob.*
                            from itinerancia i 
                           left join itinerancia_aluno ia on ia.itinerancia_id = i.id
                           left join itinerancia_aluno_questao iaq on iaq.itinerancia_aluno_id = ia.id    
                           left join itinerancia_questao iq on iq.itinerancia_id = i.id 
                           left join itinerancia_objetivo io on io.itinerancia_id = i.id   
                           inner join itinerancia_objetivo_base iob on iob.id = io.itinerancia_base_id
                           where i.id = @id
                             and not i.excluido";

            var lookup = new Dictionary<long, Itinerancia>();

            await database.Conexao.QueryAsync<Itinerancia, ItineranciaAluno, ItineranciaAlunoQuestao, ItineranciaQuestao, ItineranciaObjetivo, ItineranciaObjetivoBase, Itinerancia>(query,
                 (registroItinerancia, itineranciaAluno, itineranciaAlunoquestao, itineranciaQuestao, itineranciaObjetivo, itineranciaObjetivoBase) =>
                 {
                     Itinerancia itinerancia;
                     if (!lookup.TryGetValue(registroItinerancia.Id, out itinerancia))
                     {
                         itinerancia = registroItinerancia;
                         lookup.Add(registroItinerancia.Id, itinerancia);
                     }
                     if (itineranciaAluno != null)
                         itinerancia.AdicionarAluno(itineranciaAluno);

                     if (itineranciaAlunoquestao != null)
                         itinerancia.AdicionarQuestaoAluno(itineranciaAluno.Id, itineranciaAlunoquestao);

                     if (itineranciaQuestao != null)
                         itinerancia.AdicionarQuestao(itineranciaQuestao);

                     if (itineranciaObjetivo != null)
                         itinerancia.AdicionarObjetivo(itineranciaObjetivo);

                     if (itineranciaObjetivoBase != null)
                         itinerancia.AdicionarObjetivoBase(itineranciaObjetivoBase);

                     return itinerancia;
                 }, param: new { id });

            return lookup.Values.FirstOrDefault();
        }

        public async Task<PaginacaoResultadoDto<ItineranciaRetornoQueryDto>> ObterItineranciasPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, int anoLetivo, DateTime? dataInicio, DateTime? dataFim, string criadoRf, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, dreId, ueId, turmaId, alunoCodigo, situacao, anoLetivo, dataInicio, dataFim, criadoRf);

            var parametros = new { dreId, ueId, turmaId, alunoCodigo, situacao, anoLetivo, dataInicio, dataFim, criadoRf };
            var retorno = new PaginacaoResultadoDto<ItineranciaRetornoQueryDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<ItineranciaRetornoQueryDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, int anoLetivo, DateTime? dataInicio, DateTime? dataFim, string criadoRf)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, dreId, ueId, turmaId, alunoCodigo, situacao, anoLetivo, dataInicio, dataFim, criadoRf);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, dreId, ueId, turmaId, alunoCodigo, situacao, anoLetivo, dataInicio, dataFim, criadoRf);

            return sql.ToString();
        }
        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, int anoLetivo, DateTime? dataInicio, DateTime? dataFim, string criadoRf)
        {
            ObtenhaCabecalho(sql, contador, dreId, ueId, turmaId, alunoCodigo);

            ObtenhaFiltro(sql, dreId, ueId, turmaId, alunoCodigo, situacao, anoLetivo, dataInicio, dataFim, criadoRf);

            if (!contador)
                sql.AppendLine(" order by i.data_visita desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador, long dreId, long ueId, long turmaId, string alunoCodigo)
        {
            sql.AppendLine("select distinct ");
            if (contador)
                sql.AppendLine(" count(i.id) ");
            else
            {
                sql.AppendLine(" i.id ");
                sql.AppendLine(", i.data_visita as DataVisita ");
                sql.AppendLine(", i.ue_id as UeId ");
                sql.AppendLine(", i.situacao ");
                sql.AppendLine(", i.criado_por||'('||i.criado_rf||')' as criado_por");
                sql.AppendLine($", (select count(*) from itinerancia_aluno ia where ia.itinerancia_id = i.id ) as alunos ");

            }

            sql.AppendLine(" from itinerancia i ");

            if (dreId > 0 || ueId > 0)
            {
                sql.AppendLine(@" inner join ue  on i.ue_id  = ue.id 
	                              inner join dre on ue.dre_id = dre.id ");
            }

            if (turmaId > 0 || !string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine(@" inner join itinerancia_aluno ia on ia.itinerancia_id = i.id ");


        }

        private static void ObtenhaFiltro(StringBuilder sql, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, int anoLetivo, DateTime? dataInicio, DateTime? dataFim, string criadoRf)
        {
            sql.AppendLine(" where ue.dre_id = @dreId and not i.excluido ");
            sql.AppendLine(" and i.ano_letivo = @anoLetivo ");


            if (ueId > 0)
                sql.AppendLine(" and ue.id = @ueId ");

            if (turmaId > 0)
                sql.AppendLine(" and ia.turma_id = @turmaId ");

            if (!string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine(" and ia.codigo_aluno = @alunoCodigo ");

            if (situacao.HasValue && situacao > 0)
                sql.AppendLine(" and i.situacao = @situacao ");

            if (dataInicio != null && dataFim != null)
                sql.AppendLine("and i.data_visita::date between @dataInicio and @dataFim");

            if (turmaId > 0)
                sql.AppendLine("and ia.turma_id = @turmaId");

            if (!string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine("and ia.codigo_aluno = @alunoCodigo");

            if (!string.IsNullOrEmpty(criadoRf))
                sql.AppendLine("and i.criado_rf = @criadoRf");
        }

        public async Task<IEnumerable<ItineranciaCodigoAlunoDto>> ObterCodigoAlunosPorItineranciasIds(long[] itineranciasIds)
        {
            var query = @"select codigo_aluno as alunoCodigo, itinerancia_id as ItineranciaId, turma_id as turmaId from itinerancia_aluno ia 
                            where ia.itinerancia_id = ANY(@itineranciasIds)";

            return await database.Conexao.QueryAsync<ItineranciaCodigoAlunoDto>(query, new { itineranciasIds });
        }

        public async Task<IEnumerable<ItineranciaIdUeInfosDto>> ObterUesItineranciaPorIds(long[] itineranciaIds)
        {
            var query = @"select ue.nome as ueNome, ue.tipo_escola as tipoEscola, iu.itinerancia_id as itineranciaId from itinerancia_ue iu
                            inner join ue on ue.id = iu.ue_id 
                            where iu.itinerancia_id  = ANY(@itineranciaIds)";

            return await database.Conexao.QueryAsync<ItineranciaIdUeInfosDto>(query, new { itineranciaIds });
        }

        public async Task<IEnumerable<ItineranciaNomeRfCriadorRetornoDto>> ObterRfsCriadoresPorNome(string nomeParaBusca)
        {
            var query = $@"select distinct criado_rf as Rf, criado_por as Nome from itinerancia i 
                                        where lower(f_unaccent(i.criado_por)) LIKE lower(f_unaccent('%{nomeParaBusca}%')) order by criado_por limit 10";

            return await database.Conexao.QueryAsync<ItineranciaNomeRfCriadorRetornoDto>(query);
        }

        public async Task<IEnumerable<DashboardItineranciaDto>> ObterQuantidadeVisitasPAAI(int ano, long dreId, long ueId, int mes)
        {
            var sql = new StringBuilder("");
            var where = new StringBuilder("where i.ano_letivo = @ano and not i.excluido ");

            if (dreId > 0)
                sql.AppendLine(@"select i.criado_por || ' (' || i.criado_rf || ')' as Descricao, count(i.id ) as Quantidade");
            else
                sql.AppendLine(@"select dre.abreviacao as Descricao, count(i.id ) as Quantidade");

            sql.AppendLine(@" from itinerancia i
                            inner join ue on i.ue_id = ue.id 
                            inner join dre on ue.dre_id = dre.id ");

            if (dreId > 0)
                where.AppendLine(" and dre.id = @dreId ");

            if (ueId > 0)
                where.AppendLine(" and ue.id = @ueId ");

            if (mes > 0)
                where.AppendLine(" and extract(month from i.data_visita) = @mes");

            sql.AppendLine(where.ToString());

            if (dreId > 0)
                sql.AppendLine(" group by i.criado_por, i.criado_rf order by 1;");
            else
                sql.AppendLine(" group by dre.abreviacao order by 1;");


            return await database.Conexao.QueryAsync<DashboardItineranciaDto>(sql.ToString(), new { ano, dreId, ueId, mes });
        }

        public async Task<IEnumerable<DashboardItineranciaDto>> ObterQuantidadeObjetivos(int ano, long dreId, long ueId, int mes, string codigoRF)
        {
            var sql = new StringBuilder("");
            var where = new StringBuilder("where i.ano_letivo = @ano and not i.excluido ");


            sql.AppendLine(@"select iob.nome as Descricao, count(i.id) as Quantidade from itinerancia_objetivo io 
                            inner join itinerancia i on io.itinerancia_id = i.id
                            inner join itinerancia_objetivo_base iob on io.itinerancia_base_id = iob.id 
                            inner join ue on i.ue_id = ue.id 
                            inner join dre on ue.dre_id = dre.id ");

            if (dreId > 0)
                where.AppendLine(" and dre.id = @dreId ");

            if (ueId > 0)
                where.AppendLine(" and ue.id = @ueId ");

            if (mes > 0)
                where.AppendLine(" and extract(month from i.data_visita) = @mes");

            if(codigoRF != null)
                where.AppendLine(" and i.criado_rf = @codigoRF");

            sql.AppendLine(where.ToString());


            sql.AppendLine(" group by iob.nome order by 1;");


            return await database.Conexao.QueryAsync<DashboardItineranciaDto>(sql.ToString(), new { ano, dreId, ueId, mes, codigoRF });
        }

        public async Task<Itinerancia> ObterComUesPorId(long id)
        {
            var query = @"select i.*, ue.*, dre.*
                  from itinerancia i
                 inner join ue on ue.id = i.ue_id
                 inner join dre on dre.id = ue.dre_id
                 where i.id = @id ";

            Itinerancia registroItinerancia = null;

            await database.Conexao.QueryAsync<Itinerancia, Ue, Dre, Itinerancia>(query,
                (itinerancia, ue, dre) =>
                {
                    if (registroItinerancia == null)
                        registroItinerancia = itinerancia;
                    
                    registroItinerancia.Ue = ue;
                    registroItinerancia.Dre = dre;
                    return registroItinerancia;
                }, new { id });

            return registroItinerancia;
        }

        public async Task<IEnumerable<ItineranciaObjetivoDescricaoDto>> ObterDecricaoObjetivosPorId(long itineranciaId)
        {
            var query = @"select iob.nome, io.descricao
                          from itinerancia_objetivo io 
                         inner join itinerancia_objetivo_base iob on iob.id = io.itinerancia_base_id
                         where io.itinerancia_id = @itineranciaId";

            return await database.Conexao.QueryAsync<ItineranciaObjetivoDescricaoDto>(query, new { itineranciaId });
        }

        public async Task<int> AtualizarStatusItinerancia(long itineranciaId, int situacao)
        {
            var query = @"update itinerancia set situacao = @situacao where id = @itineranciaId ";

            return await database.Conexao.ExecuteAsync(query, new { itineranciaId, situacao });
        }

    }
}
