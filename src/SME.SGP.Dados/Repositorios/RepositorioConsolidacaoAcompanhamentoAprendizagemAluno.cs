using Dapper;
using Dommel;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class RepositorioConsolidacaoAcompanhamentoAprendizagemAluno : IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoAcompanhamentoAprendizagemAluno(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(ConsolidacaoAcompanhamentoAprendizagemAluno consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task Limpar(int anoLetivo)
        {
            var query = @" delete from consolidacao_acompanhamento_aprendizagem_aluno c
                            using turma t 
                            where t.id = c.turma_id
                              and t.ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteScalarAsync(query, new { anoLetivo });
        }

        public async Task<IEnumerable<DashboardAcompanhamentoAprendizagemDto>> ObterConsolidacao(int anoLetivo, long dreId, long ueId, int semestre)
        {
            var campo = ueId > 0 ? "t.nome" : "concat(t.ano, 'º ano')";
            var agrupamento = ueId > 0 ? "nome" : "ano";

            var filtro = ueId > 0 ? "and ue.id = @ueId" :
                            dreId > 0 ? "and ue.dre_id = @dreId" :
                            "";

            var query = $@"select {campo} as Turma
	                        , sum(c.quantidade_com_acompanhamento) as QuantidadeComAcompanhamento
	                        , sum(c.quantidade_sem_acompanhamento) as QuantidadeSemAcompanhamento
                          from consolidacao_acompanhamento_aprendizagem_aluno c
                         inner join turma t on t.id = c.turma_id
                         inner join ue on ue.id = t.ue_id 
                        where t.ano_letivo = @anoLetivo
                           and c.semestre = @semestre
                           {filtro}
                         group by t.{agrupamento}
                        order by 1";

            return await database.Conexao.QueryAsync<DashboardAcompanhamentoAprendizagemDto>(query, new { anoLetivo, semestre, dreId, ueId });
        }

        public async Task<IEnumerable<DashboardAcompanhamentoAprendizagemPorDreDto>> ObterConsolidacaoPorDre(int anoLetivo, int? semestre)
        {
            var filtroSemestre = semestre.HasValue ? "and c.semestre = @semestre" : "";

            var query = $@"select 
	                        dre.abreviacao as Dre
                            , sum(c.quantidade_com_acompanhamento) as QuantidadeComAcompanhamento
                            , sum(c.quantidade_sem_acompanhamento) as QuantidadeSemAcompanhamento
                          from consolidacao_acompanhamento_aprendizagem_aluno c
                         inner join turma t on t.id = c.turma_id
                         inner join ue on ue.id = t.ue_id 
                         inner join dre on dre.id = ue.dre_id 
                        where t.ano_letivo = @anoLetivo
                           {filtroSemestre}
                        group by dre.abreviacao, dre.dre_id 
                        order by dre.dre_id";

            return await database.Conexao.QueryAsync<DashboardAcompanhamentoAprendizagemPorDreDto>(query, new { anoLetivo, semestre });
        }
    }
}
