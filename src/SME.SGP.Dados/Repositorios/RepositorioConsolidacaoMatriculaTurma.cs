using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
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

        public async Task<IEnumerable<InformacoesEscolaresPorDreEAnoDto>> ObterGraficoMatriculasAsync(int anoLetivo, long dreId, long ueId, string ano, Modalidade modalidade, int? semestre)
        {
            var sql = dreId > 0 ? QueryConsolidacaoPorAno(dreId, ueId, semestre) : QueryConsolidacaoPorDre(dreId, ueId, ano, semestre);
            return await database
                .Conexao
                .QueryAsync<InformacoesEscolaresPorDreEAnoDto>(sql, new { modalidade, dreId, ueId, anoLetivo, semestre, ano });
        }

        private string QueryConsolidacaoPorAno( long dreId, long ueId, int? semestre)
        {
            var query = new StringBuilder(@"select * from (
                                                (select t.ano || 'º' as AnoDescricao,
                                                        sum(cfm.quantidade) as quantidade
                                                   from consolidacao_matricula_turma cfm
                                                  inner join turma t on t.id = cfm.turma_id
                                                  inner join ue on ue.id = t.ue_id
                                                  inner join dre on dre.id = ue.dre_id
                                                  where t.tipo_turma not in (2, 7)
                                                    and t.ano_letivo = @anoLetivo 
                                                    and t.modalidade_codigo = @modalidade");
            if (semestre > 0) query.AppendLine(@"   and t.semestre = @semestre");
            if (dreId > 0) query.AppendLine(@"      and dre.id = @dreId");
            if (ueId > 0) query.AppendLine(@"       and ue.id = @ueId");
            query.AppendLine(@"                   group by t.ano
                                                  order by t.ano desc) 
                                                union");
            query.AppendLine(@"                 select 'Turmas de programa' as AnoDescricao,
                                                         sum(cfm.quantidade) as quantidade
                                                  from consolidacao_matricula_turma cfm
                                                 inner join turma t on t.id = cfm.turma_id
                                                 inner join ue on ue.id = t.ue_id
                                                 inner join dre on dre.id = ue.dre_id
                                                 where t.tipo_turma in (2, 7)
                                                   and t.ano_letivo = @anoLetivo 
                                                   and t.modalidade_codigo = @modalidade");
            if (semestre > 0) query.AppendLine(@"  and t.semestre = @semestre");
            if (dreId > 0) query.AppendLine(@"     and dre.id = @dreId");
            if (ueId > 0) query.AppendLine(@"      and ue.id = @ueId");
            query.AppendLine(@"              ) as x
                                             where x.quantidade > 0
                                             order by x.AnoDescricao");
            return query.ToString();
        }

        private string QueryConsolidacaoPorDre(long dreId, long ueId, string ano, int? semestre)
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
            if (ueId > 0) query.AppendLine(@"  and ue.id = @ueId");
            if (!string.IsNullOrEmpty(ano)) query.AppendLine(@"  and t.ano = @ano");
            query.AppendLine(@" group by dre.abreviacao 
                         order by dre.abreviacao desc");

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
    }
}
