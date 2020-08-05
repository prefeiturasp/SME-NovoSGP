using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoCalendario : RepositorioBase<TipoCalendario>, IRepositorioTipoCalendario
    {
        public RepositorioTipoCalendario(ISgpContext conexao) : base(conexao)
        {
        }
        public async Task<PeriodoEscolar> ObterPeriodoEscolarPorCalendarioEData(long tipoCalendarioId, DateTime dataParaVerificar)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select pe.* from periodo_escolar pe");
            query.AppendLine("inner join tipo_calendario tc");
            query.AppendLine("on tc.id = pe.tipo_calendario_id");
            query.AppendLine("where tc.id = @tipoCalendarioId");
            query.AppendLine("and @dataParaVerificar between symmetric pe.periodo_inicio::date and pe.periodo_fim ::date");
            
            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query.ToString(), new { tipoCalendarioId, dataParaVerificar });
        }
        public async Task<IEnumerable<TipoCalendario>> BuscarPorAnoLetivo(int anoLetivo)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select *");
            query.AppendLine("from tipo_calendario");
            query.AppendLine("where excluido = false");
            query.AppendLine("and ano_letivo = @anoLetivo");

            return await database.Conexao.QueryAsync<TipoCalendario>(query.ToString(), new { anoLetivo });
        }

        public async Task<TipoCalendario> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select *");
            query.AppendLine("from tipo_calendario t");
            query.AppendLine("where t.excluido = false");
            query.AppendLine("and t.ano_letivo = @anoLetivo");
            query.AppendLine("and t.modalidade = @modalidade");

            DateTime dataReferencia = DateTime.MinValue;
            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                var periodoReferencia = semestre == 1 ? "periodo_inicio < @dataReferencia" : "periodo_fim > @dataReferencia";
                query.AppendLine($"and exists(select 0 from periodo_escolar p where tipo_calendario_id = t.id and {periodoReferencia})");

                // 1/6/ano ou 1/7/ano dependendo do semestre
                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 7, 1);
            }

            return await database.Conexao.QueryFirstOrDefaultAsync<TipoCalendario>(query.ToString(), new { anoLetivo, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<IEnumerable<TipoCalendario>> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select *");
            query.AppendLine("from tipo_calendario t");
            query.AppendLine("where t.excluido = false");
            query.AppendLine("and t.ano_letivo = @anoLetivo");
            query.AppendLine("and t.modalidade = @modalidade");

            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                query.AppendLine($"and exists(select 0 from periodo_escolar p where tipo_calendario_id = t.id and @dataReferencia BETWEEN p.periodo_inicio and p.periodo_fim)");
            }

            return await database.Conexao.QueryAsync<TipoCalendario>(query.ToString(), new { anoLetivo, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<IEnumerable<TipoCalendario>> ListarPorAnoLetivo(int anoLetivo)
        {
            StringBuilder query = ObterQueryListarPorAnoLetivo();

            return await database.Conexao.QueryAsync<TipoCalendario>(query.ToString(), new { anoLetivo });
        }

        public override TipoCalendario ObterPorId(long id)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select * ");
            query.AppendLine("from tipo_calendario ");
            query.AppendLine("where excluido = false ");
            query.AppendLine("and id = @id ");

            return database.Conexao.QueryFirstOrDefault<TipoCalendario>(query.ToString(), new { id });
        }

        public async Task<IEnumerable<TipoCalendario>> ObterTiposCalendario()
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("id,");
            query.AppendLine("nome,");
            query.AppendLine("ano_letivo,");
            query.AppendLine("modalidade,");
            query.AppendLine("periodo");
            query.AppendLine("from tipo_calendario");
            query.AppendLine("where excluido = false");

            return await database.Conexao.QueryAsync<TipoCalendario>(query.ToString());
        }

        public async Task<bool> VerificarRegistroExistente(long id, string nome)
        {
            StringBuilder query = new StringBuilder();

            var nomeMaiusculo = nome.ToUpper().Trim();
            query.AppendLine("select count(*) ");
            query.AppendLine("from tipo_calendario ");
            query.AppendLine("where upper(nome) = @nomeMaiusculo ");
            query.AppendLine("and excluido = false");

            if (id > 0)
                query.AppendLine("and id <> @id");

            int quantidadeRegistrosExistentes = await database.Conexao.QueryFirstAsync<int>(query.ToString(), new { id, nomeMaiusculo });

            return quantidadeRegistrosExistentes > 0;
        }

        private static StringBuilder ObterQueryListarPorAnoLetivo()
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select *");
            query.AppendLine("from tipo_calendario");
            query.AppendLine("where excluido = false");
            query.AppendLine("and ano_letivo = @anoLetivo");
            return query;
        }

        public async Task<bool> PeriodoEmAberto(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false)
        {
            var query = new StringBuilder(@"select count(pe.Id)
                          from periodo_escolar pe 
                         where pe.tipo_calendario_id = @tipoCalendarioId
                           and periodo_fim >= @dataReferencia ");

            if (!ehAnoLetivo)
                query.AppendLine("and periodo_inicio <= @dataReferencia");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre");

            return await database.Conexao.QueryFirstAsync<int>(query.ToString(), new { tipoCalendarioId, dataReferencia, bimestre }) > 0;
        }

        public async Task<long> ObterIdPorAnoLetivoEModalidadeAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select id");
            query.AppendLine("from tipo_calendario t");
            query.AppendLine("where t.excluido = false");
            query.AppendLine("and t.ano_letivo = @anoLetivo");
            query.AppendLine("and t.modalidade = @modalidade");

            DateTime dataReferencia = DateTime.MinValue;
            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                var periodoReferencia = semestre == 1 ? "periodo_inicio < @dataReferencia" : "periodo_fim > @dataReferencia";
                query.AppendLine($"and exists(select 0 from periodo_escolar p where tipo_calendario_id = t.id and {periodoReferencia})");

                // 1/6/ano ou 1/7/ano dependendo do semestre
                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 7, 1);
            }

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query.ToString(), new { anoLetivo, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<IEnumerable<TipoCalendario>> ListarPorAnoLetivoEModalidades(int anoLetivo, int[] modalidades)
        {
            StringBuilder query = ObterQueryListarPorAnoLetivo();
            query.AppendLine("and modalidade = any(@modalidades)");

             return await database.Conexao.QueryAsync<TipoCalendario>(query.ToString(), new { anoLetivo, modalidades });
        }
    }
}