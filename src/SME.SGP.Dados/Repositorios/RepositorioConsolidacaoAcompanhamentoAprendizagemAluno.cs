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
            var agrupamento = ueId > 0 ? "nome" : "ano";
            var filtro = ueId > 0 ? "and ue.id = @ueId" :
                            dreId > 0 ? "and dre.id = @dreId" :
                            "";

            var query = $@"select t.{agrupamento} as Turma
	                        , sum(c.quantidade_com_acompanhamento) as QuantidadeComAcompanhamento
	                        , sum(c.quantidade_sem_acompanhamento) as QuantidadeSemAcompanhamento
                          from consolidacao_acompanhamento_aprendizagem_aluno c
                         inner join turma t on t.id = c.turma_id
                         inner join ue on ue.id = t.ue_id 
                        where t.ano_letivo = @anoLetivo
                           and c.semestre = @semestre
                           {filtro}
                         group by t.{agrupamento}";

            return await database.Conexao.QueryAsync<DashboardAcompanhamentoAprendizagemDto>(query, new { anoLetivo, semestre, dreId, ueId });
        }
    }
}
