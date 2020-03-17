﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}