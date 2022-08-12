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
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT distinct");
            query.AppendLine("	tc.id,");
            query.AppendLine("	tc.descricao,");
            query.AppendLine("	1 AS Selecionado");
            query.AppendLine("from ");
            query.AppendLine("	tipo_ciclo tc inner join tipo_ciclo_ano tca on tc.id = tca.tipo_ciclo_id ");
            query.AppendLine("WHERE ");
            query.AppendLine($"  tca.Ano = @anoSelecionado");
            query.AppendLine($"  AND modalidade = @modalidade");

            var cicloSelecionado = database.Conexao
                .QueryFirstOrDefault<CicloDto>(query.ToString(), new
                {
                    anoSelecionado = filtroCicloDto.AnoSelecionado?.ToUpper().Replace('S', '1'),
                    modalidade = filtroCicloDto.Modalidade,
                });

            query.Clear();
            query.AppendLine("SELECT distinct");
            query.AppendLine("	tc.id,");
            query.AppendLine("	tc.descricao,");
            query.AppendLine("	0 AS Selecionado");
            query.AppendLine("from ");
            query.AppendLine("	tipo_ciclo tc inner join tipo_ciclo_ano tca on tc.id = tca.tipo_ciclo_id ");
            query.AppendLine("WHERE ");
            query.AppendLine($"  tca.Ano = ANY(@anos)");
            query.AppendLine($"  AND modalidade = @modalidade");
            if (cicloSelecionado != null)
                query.AppendLine($"  AND tc.id <> @cicloSelecionadoId");

            var ciclosNaoSelecionados = database.Conexao
                .Query<CicloDto>(query.ToString(), new
                {
                    anos = filtroCicloDto.Anos?.Select(a => a.ToUpper().Replace('S', '1')).ToArray(),
                    modalidade = filtroCicloDto.Modalidade,
                    cicloSelecionadoId = cicloSelecionado?.Id
                });

            var ciclosRetorno = new List<CicloDto>();

            if (cicloSelecionado != null)
                ciclosRetorno.Add(cicloSelecionado);

            if (ciclosNaoSelecionados.Any())
                ciclosRetorno.AddRange(ciclosNaoSelecionados.ToList());

            return ciclosRetorno.AsEnumerable();
        }

        public async Task<IEnumerable<RetornoCicloDto>> ObterCiclosPorAnoModalidadeECodigoUe(FiltroCicloPorModalidadeECodigoUeDto filtro)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select distinct tc.id, tc.descricao from tipo_ciclo tc ");
            query.AppendLine("inner join tipo_ciclo_ano tca on tca.tipo_ciclo_id = tc.id ");
            query.AppendLine("inner join turma t on t.ano = tca.ano ");
            query.AppendLine("inner join ue ue on t.ue_id = ue.id ");
            query.AppendLine("where tc.descricao is not null ");

            if (filtro.Modalidade > 0)
                query.AppendLine("and tca.modalidade = @modalidade ");

            if (!string.IsNullOrEmpty(filtro.CodigoUe) && !filtro.CodigoUe.Equals("-99"))
                query.AppendLine("and ue.ue_id = @codigoUe ");

            var parametros = new { filtro.Modalidade, filtro.CodigoUe };
            return await database.Conexao.QueryAsync<RetornoCicloDto>(query.ToString(), parametros);
        }

        public async Task<IEnumerable<RetornoCicloDto>> ObterCiclosPorAnoModalidadeECodigoUeAbrangencia(FiltroCicloPorModalidadeECodigoUeDto filtro, long usuarioId, Guid perfil)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select distinct tc.id, tc.descricao from tipo_ciclo tc ");
            query.AppendLine("inner join tipo_ciclo_ano tca on tca.tipo_ciclo_id = tc.id ");
            query.AppendLine("inner join turma t on t.ano = tca.ano ");
            query.AppendLine("inner join ue ue on t.ue_id = ue.id ");
            query.AppendLine("inner join v_abrangencia_usuario vau");
            query.AppendLine("on vau.turma_id = t.turma_id");
            query.AppendLine("where tc.descricao is not null and vau.usuario_id = @usuarioId and vau.usuario_perfil = @perfil and tc.id <> 4");

            if (filtro.Modalidade > 0)
                query.AppendLine("and tca.modalidade = @modalidade ");

            if (!string.IsNullOrEmpty(filtro.CodigoUe) && !filtro.CodigoUe.Equals("-99"))
                query.AppendLine("and ue.ue_id = @codigoUe ");

            var parametros = new { filtro.Modalidade, filtro.CodigoUe, usuarioId, perfil };

            return await database.Conexao.QueryAsync<RetornoCicloDto>(query.ToString(), parametros);
        }

        public async Task<CicloEnsino> ObterCicloPorCodigoEol(long codigoEol)
        {
            var sql = @"select *
                          from ciclo_ensino ce 
                         where cod_ciclo_ensino_eol = @codigoEol";

            var parametros = new { codigoEol };

            return await database.QueryFirstOrDefaultAsync<CicloEnsino>(sql, parametros);
        }
    }
}