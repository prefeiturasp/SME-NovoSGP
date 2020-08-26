using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAnualTerritorioSaber : RepositorioBase<PlanoAnualTerritorioSaber>, IRepositorioPlanoAnualTerritorioSaber
    {
        public RepositorioPlanoAnualTerritorioSaber(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PlanoAnualTerritorioSaberCompletoDto> ObterPlanoAnualTerritorioSaberCompletoPorAnoEscolaBimestreETurma(int ano, string escolaId, string turmaId, int bimestre, long territorioExperienciaId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("	pa.ano as AnoLetivo, pa.* ");
            query.AppendLine("from");
            query.AppendLine("	plano_anual_territorio_saber pa");
            query.AppendLine("where");
            query.AppendLine("	pa.ano = @ano");
            query.AppendLine("	and pa.bimestre = @bimestre");
            query.AppendLine("	and pa.escola_id = @escolaId");
            query.AppendLine("	and pa.turma_id = @turmaId");
            query.AppendLine("	and pa.territorio_experiencia_id = @territorioExperienciaId");
            query.AppendLine("group by");
            query.AppendLine("	pa.id");

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAnualTerritorioSaberCompletoDto>(query.ToString(), new { ano, escolaId, turmaId = Convert.ToInt32(turmaId), bimestre, territorioExperienciaId });
        }

        public async Task<IEnumerable<PlanoAnualTerritorioSaberCompletoDto>> ObterPlanoAnualTerritorioSaberCompletoPorAnoUEETurma(int ano, string ueId, string turmaId, long territorioExperienciaId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("	pa.ano as AnoLetivo, pa.* ");
            query.AppendLine("from");
            query.AppendLine("	plano_anual_territorio_saber pa");
            query.AppendLine("where");
            query.AppendLine("	pa.ano = @ano");
            query.AppendLine("	and pa.escola_id = @ueId");
            query.AppendLine("	and pa.turma_id = @turmaId");
            query.AppendLine("	and pa.territorio_experiencia_id = @territorioExperienciaId");
            query.AppendLine("group by");
            query.AppendLine("	pa.id");

            return await database.Conexao.QueryAsync<PlanoAnualTerritorioSaberCompletoDto>(query.ToString(), new { ano, ueId, turmaId = int.Parse(turmaId), territorioExperienciaId });
        }

        public async Task<PlanoAnualTerritorioSaber> ObterPlanoAnualTerritorioSaberSimplificadoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long territorioExperienciaId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("id, escola_id, turma_id, ano, bimestre, territorio_experiencia_id, desenvolvimento, reflexao,");
            query.AppendLine("criado_em, alterado_em, criado_por, alterado_por, criado_rf, alterado_rf");
            query.AppendLine("from plano_anual_territorio_saber");
            query.AppendLine("where");
            query.AppendLine("ano = @ano and");
            query.AppendLine("escola_id = @escolaId and");
            query.AppendLine("bimestre = @bimestre and");
            query.AppendLine("turma_id = @turmaId and");
            query.AppendLine("territorio_experiencia_id = @territorioExperienciaId");

            return (await database.Conexao.QueryAsync<PlanoAnualTerritorioSaber>(query.ToString(),
                new
                {
                    ano,
                    escolaId,
                    turmaId,
                    bimestre,
                    territorioExperienciaId
                })).SingleOrDefault();
        }
    }
}
