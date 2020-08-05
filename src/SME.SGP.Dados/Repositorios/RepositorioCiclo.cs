using System;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCiclo : RepositorioBase<Ciclo>, IRepositorioCiclo
    {
        public RepositorioCiclo(ISgpContext conexao) : base(conexao)
        {
        }

        public CicloDto ObterCicloPorAno(int ano)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("	tc.id,");
            query.AppendLine("	tc.descricao");
            query.AppendLine("from");
            query.AppendLine("	tipo_ciclo tc");
            query.AppendLine("inner join tipo_ciclo_ano tca on");
            query.AppendLine("  tc.id = tca.tipo_ciclo_id");
            query.AppendLine("where");
            query.AppendLine("  tca.ano = @ano");
            return database.Conexao.Query<CicloDto>(query.ToString(), new { ano }).SingleOrDefault();
        }

        public CicloDto ObterCicloPorAnoModalidade(string ano, Modalidade modalidade)
        {
            var sql = @"select tc.id, tc.descricao from tipo_ciclo tc
                        inner join tipo_ciclo_ano tca on tc.id = tca.tipo_ciclo_id
                        where tca.ano = @ano and tca.modalidade = @modalidade";

            var parametros = new { ano, modalidade };

            return database.QueryFirstOrDefault<CicloDto>(sql, parametros);
        }


        public IEnumerable<CicloDto> ObterCiclosPorAnoModalidade(FiltroCicloDto filtroCicloDto)
        {
            var anos = "'" + string.Join("','", filtroCicloDto.Anos) + "'";

            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT");
            query.AppendLine("	tc.id,");
            query.AppendLine("	tc.descricao,");
            query.AppendLine("	1 AS Selecionado");
            query.AppendLine("from ");
            query.AppendLine("	tipo_ciclo tc inner join tipo_ciclo_ano tca on tc.id = tca.tipo_ciclo_id ");
            query.AppendLine("WHERE ");
            query.AppendLine($"  tca.Ano = '{filtroCicloDto.AnoSelecionado}'");
            query.AppendLine($"  AND modalidade = {filtroCicloDto.Modalidade}");
            query.AppendLine(" UNION ");
            query.AppendLine("SELECT");
            query.AppendLine("	tc.id,");
            query.AppendLine("	tc.descricao,");
            query.AppendLine("	0 AS Selecionado");
            query.AppendLine("from ");
            query.AppendLine("	tipo_ciclo tc inner join tipo_ciclo_ano tca on tc.id = tca.tipo_ciclo_id ");
            query.AppendLine("WHERE ");
            query.AppendLine($"  tca.Ano IN ({anos})");
            query.AppendLine($"  AND modalidade = {filtroCicloDto.Modalidade} ");
            query.AppendLine($"ORDER BY Selecionado DESC");
            return database.Conexao.Query<CicloDto>(query.ToString()).ToList();
        }

        public async Task<IEnumerable<RetornoCicloDto>> ObterCiclosPorAnoModalidadeECodigoUe(FiltroCicloPorModalidadeECodigoUeDto filtro)
        {
                StringBuilder query = new StringBuilder();
                query.AppendLine("select distinct tc.id, tc.descricao from tipo_ciclo tc ");
                query.AppendLine("inner join tipo_ciclo_ano tca on tca.tipo_ciclo_id = tc.id ");
                query.AppendLine("inner join turma t on t.ano = tca.ano ");
                query.AppendLine("inner join ue ue on t.ue_id = ue.id ");
                query.AppendLine("where tc.descricao is not null ");

                if(filtro.Modalidade > 0)
                    query.AppendLine("and tca.modalidade = @modalidade ");

                if (!string.IsNullOrEmpty(filtro.CodigoUe) && !filtro.CodigoUe.Equals("-99"))
                    query.AppendLine("and ue.ue_id = @codigoUe ");

                var parametros = new { filtro.Modalidade, filtro.CodigoUe };
                return await database.Conexao.QueryAsync<RetornoCicloDto>(query.ToString(), parametros);
        }
    }
}