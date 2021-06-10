using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConsolidacaoMatriculaTurma : IRepositorioConsolidacaoMatriculaTurma
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoMatriculaTurma(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<InformacoesEscolaresPorDreEAnoDto>> ObterGraficoMatriculasAsync(int anoLetivo, long dreId, long ueId, AnoItinerarioPrograma[] anos, Modalidade modalidade, int? semestre)
        {
            var tiposTurma = new List<int>();
            var anosCondicao = new List<string>();
            var sql = "";
            if (anos != null)
            {
                foreach (var ano in anos)
                {
                    if (ano == AnoItinerarioPrograma.Programa || ano == AnoItinerarioPrograma.Itinerario || ano == AnoItinerarioPrograma.EducacaoFisica)
                        tiposTurma.Add(ano.ObterTipoTurma());
                    else anosCondicao.Add(ano.ShortName());
                }
            }
            if (ueId > 0 && anos != null && anos.Count() == 1)
                sql = QueryConsolidacaoPorTurma(tiposTurma, anosCondicao, semestre);
            else if (dreId > 0)
                sql = QueryConsolidacaoPorAno(dreId, ueId, tiposTurma, anosCondicao, semestre);
            else sql = QueryConsolidacaoPorDre(dreId, ueId, tiposTurma, anosCondicao, semestre);
            return await database
                .Conexao
                .QueryAsync<InformacoesEscolaresPorDreEAnoDto>(sql, new { modalidade, dreId, ueId, anoLetivo, semestre, anosCondicao, tiposTurma });
        }

        private string QueryConsolidacaoPorTurma(IEnumerable<int> tiposTurma, IEnumerable<string> anosCondicao, int? semestre)
        {
            var query = new StringBuilder();
            query.AppendLine(@"SELECT t.nome AS TurmaDescricao,
                                       Sum(cfm.quantidade) AS quantidade
                                FROM   consolidacao_matricula_turma cfm
                                       INNER JOIN turma t
                                               ON t.id = cfm.turma_id
                                       INNER JOIN ue
                                               ON ue.id = t.ue_id
                                       INNER JOIN dre
                                               ON dre.id = ue.dre_id 
                                     WHERE t.ano_letivo = @anoLetivo
                                       AND t.modalidade_codigo = @modalidade
                                       AND dre.id = @dreId
                                       AND ue.id = @ueId
                                       ");
            if (tiposTurma.Any())
                query.AppendLine(@"    AND t.tipo_turma IN ( 2, 3, 7 )
                                       AND t.tipo_turma = ANY(@tiposTurma) ");
            else if (anosCondicao.Any())
                query.AppendLine(@"    AND t.tipo_turma NOT IN ( 2, 3, 7 )
                                       AND t.ano = ANY(@anosCondicao) ");

            query.AppendLine(@"      GROUP BY t.nome
                                     ORDER BY t.nome");
            return query.ToString();
        }

        private string QueryConsolidacaoPorAno(long dreId, long ueId, IEnumerable<int> tiposTurma, IEnumerable<string> anosCondicao, int? semestre)
        {
            var query = new StringBuilder();
            if (anosCondicao.Any() && !tiposTurma.Any())
            {
                query.AppendLine(QueryAnos(dreId, ueId, anosCondicao, semestre));
            }
            else if (tiposTurma.Any() && !anosCondicao.Any())
            {
                query.AppendLine(QueryTiposTurma(dreId, ueId, tiposTurma, semestre));
            }
            else
            {
                query.AppendLine($@"select* from(
                                          ({QueryAnos(dreId, ueId, anosCondicao, semestre)}) 
                                    union");
                query.AppendLine($@"       {QueryTiposTurma(dreId, ueId, tiposTurma, semestre)}) as x
                                             where x.quantidade > 0
                                             order by x.AnoDescricao");
            }
            return query.ToString();
        }

        private string CondicaoQueryPorAno(long dreId, long ueId, int? semestre)
        {
            var query = new StringBuilder();
            if (semestre > 0) query.AppendLine(@"   and t.semestre = @semestre");
            if (dreId > 0) query.AppendLine(@"      and dre.id = @dreId");
            if (ueId > 0) query.AppendLine(@"       and ue.id = @ueId");
            return query.ToString();
        }

        private string QueryAnos(long dreId, long ueId, IEnumerable<string> anosCondicao, int? semestre)
        {
            var query = new StringBuilder();
            query.AppendLine(@"                  select t.ano || 'º' as AnoDescricao,
                                                        sum(cfm.quantidade) as quantidade
                                                   from consolidacao_matricula_turma cfm
                                                  inner
                                                   join turma t on t.id = cfm.turma_id

                                             inner
                                                   join ue on ue.id = t.ue_id

                                             inner
                                                   join dre on dre.id = ue.dre_id
                                                  where t.tipo_turma not in (2, 3, 7)
                                                    and t.ano_letivo = @anoLetivo
                                                    and t.modalidade_codigo = @modalidade");
            if (anosCondicao != null && anosCondicao.Any())
            {
                query.AppendLine("and t.ano = ANY(@anosCondicao) ");
            }
            query.AppendLine(CondicaoQueryPorAno(dreId, ueId, semestre));
            query.AppendLine(@"                   group by t.ano
                                                  order by t.ano ");
            return query.ToString();
        }

        private string QueryTiposTurma(long dreId, long ueId, IEnumerable<int> tiposTurma, int? semestre)
        {
            var query = new StringBuilder();
            query.AppendLine(@"                 select case 
 													   when t.tipo_turma = 3 then 'Turmas de programa' 
 													   when t.tipo_turma = 2 then 'Ed. Física'
 													   when t.tipo_turma = 7 then 'Itinerário'end as AnoDescricao,
                                                         sum(cfm.quantidade) as quantidade
                                                  from consolidacao_matricula_turma cfm
                                                 inner join turma t on t.id = cfm.turma_id
                                                 inner join ue on ue.id = t.ue_id
                                                 inner join dre on dre.id = ue.dre_id
                                                 where t.ano_letivo = @anoLetivo 
                                                   and t.modalidade_codigo = @modalidade");
            query.AppendLine(CondicaoQueryPorAno(dreId, ueId, semestre));
            if (tiposTurma != null && tiposTurma.Any())
                query.AppendLine(@"and t.tipo_turma = ANY(@tiposTurma) ");
            else
                query.AppendLine(@"and t.tipo_turma in (2,3,7) ");

            query.AppendLine(@"group by t.tipo_turma");
            return query.ToString();
        }

        private string QueryConsolidacaoPorDre(long dreId, long ueId, IEnumerable<int> tiposTurma, IEnumerable<string> anosCondicao, int? semestre)
        {
            var query = new StringBuilder(@"select dre.abreviacao as DreDescricao,
		                                           sum(cfm.quantidade) as quantidade 
	                                          from consolidacao_matricula_turma cfm
	                                         inner join turma t on t.id = cfm.turma_id
                                             inner join ue on ue.id = t.ue_id 
                                             inner join dre on dre.id = ue.dre_id 
                                             where t.ano_letivo = @anoLetivo 
                                               and t.modalidade_codigo = @modalidade");
            if (semestre > 0) query.AppendLine(@"  and t.semestre = @semestre");
            if (dreId > 0) query.AppendLine(@" and dre.id = @dreId");
            if (ueId > 0) query.AppendLine(@"  and ue.id = @ueId ");
            if (anosCondicao != null && anosCondicao.Any() && tiposTurma != null && tiposTurma.Any())
            {
                query.AppendLine(@"  and (t.ano = ANY(@anosCondicao) or t.tipo_turma = ANY(@tiposTurma)) ");
            }
            else
            {
                if (anosCondicao != null && anosCondicao.Any()) query.AppendLine(@"  and t.ano = ANY(@anosCondicao)");
                if (tiposTurma != null && tiposTurma.Any()) query.AppendLine(@"  and t.tipo_turma = ANY(@tiposTurma)");
            }
            query.AppendLine(@" group by dre.abreviacao 
                         order by dre.abreviacao ");

            return query.ToString();
        }

        public async Task<long> Inserir(ConsolidacaoMatriculaTurma consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoMatriculasTurmasPorAnoLetivo(int anoLetivo)
        {
            var query = @"delete from consolidacao_matricula_turma
                        where turma_id in (
                            select id from turma where ano_letivo = @anoLetivo)";

            await database.Conexao.ExecuteScalarAsync(query, new { anoLetivo });
        }

        public async Task<bool> ExisteConsolidacaoMatriculaTurmaPorAno(int ano)
        {
            var query = @"select 1 
                          from consolidacao_matricula_turma c
                         inner join turma t on t.id = c.turma_id
                         where t.ano_letivo = @ano";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { ano });
        }

        public async Task<IEnumerable<ModalidadesPorAnoItineranciaProgramaDto>> ObterModalidadesPorAnos(int anoLetivo, long dreId, long ueId, int modalidade, int semestre)
        {
            var query = new StringBuilder($@"select 
                            distinct modalidade_codigo as modalidade, 
                            case when tipo_turma = 2 then '{(int)AnoItinerarioPrograma.EducacaoFisica}'
                                 when tipo_turma = 7 then '{(int)AnoItinerarioPrograma.Itinerario}'
                                 when tipo_turma = 3 then '{(int)AnoItinerarioPrograma.Programa}'
                            else ano end as ano
                         from turma t
                         inner join ue u on t.ue_id = u.id
                         inner join dre d on d.id = u.dre_id
                         where ano in ('0','1', '2', '3', '4', '5', '6', '7', '8', '9') and
                         ano_letivo = @anoLetivo
                         ");

            if (dreId > 0)
                query.AppendLine(" and d.id = @dreId ");

            if (ueId > 0)
                query.AppendLine(" and u.id  = @ueId ");

            if (modalidade > 0)
                query.AppendLine(" and t.modalidade_codigo = @modalidade ");

            if (semestre > 0)
                query.AppendLine(" and t.semestre = @semestre ");

            query.AppendLine("order by ano");

            return await database.Conexao.QueryAsync<ModalidadesPorAnoItineranciaProgramaDto>(query.ToString(), new { anoLetivo, dreId, ueId, modalidade, semestre });
        }
    }
}
