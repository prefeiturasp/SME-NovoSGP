using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioAtribuicaoEsporadica : RepositorioBase<AtribuicaoEsporadica>, IRepositorioAtribuicaoEsporadica
    {
        public RepositorioAtribuicaoEsporadica(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadica>> ListarPaginada(Paginacao paginacao, int anoLetivo, string dreId, string ueId, string codigoRF)
        {
            var retorno = new PaginacaoResultadoDto<AtribuicaoEsporadica>();

            var sql = MontaQueryCompleta(paginacao, codigoRF);

            var parametros = new { anoLetivo, dreId, ueId, codigoRF };

            using (var multi = await database.Conexao.QueryMultipleAsync(sql, parametros))
            {
                retorno.Items = multi.Read<AtribuicaoEsporadica>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        public IEnumerable<AtribuicaoEsporadica> ObterAtribuicoesDatasConflitantes(DateTime dataInicio, DateTime dataFim, string professorRF, string dreCodigo, string ueCodigo, long id = 0)
        {
            var sql = ObterSqlConflitante(id > 0);

            var parametros = new { professorRF, dataInicio, dataFim, dreCodigo, ueCodigo, id };

            return database.Conexao.Query<AtribuicaoEsporadica>(sql, parametros);
        }

        private string ObterSqlConflitante(bool temId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(@"select * from atribuicao_esporadica
                    where not excluido and professor_rf = @professorRF and dre_id = @dreCodigo and ue_id = @ueCodigo and
                    ((@dataInicio >= data_inicio and @dataInicio <= data_fim) or
                    (@dataFim  >= data_inicio and @dataFim <= data_fim) or
                    (data_inicio >= @dataInicio and data_inicio <= @dataFim) or
                    (data_fim  >= @dataInicio and data_fim <= @dataFim))");

            if (temId)
                sql.AppendLine("and id <> @id");

            return sql.ToString();
        }

        public AtribuicaoEsporadica ObterUltimaPorRF(string codigoRF)
        {
            var sql = @"select * from atribuicao_esporadica
                        where professor_rf = @professorRF
                        order by data_fim desc";

            return database.Conexao.QueryFirstOrDefault<AtribuicaoEsporadica>(sql, new { professorRF = codigoRF });
        }

        private static string MontaQueryCompleta(Paginacao paginacao, string codigoRF)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, codigoRF, contador: false);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, codigoRF, contador: true);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, string codigoRF, bool contador = false)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, codigoRF);

            if (!contador)
                sql.AppendLine("order by id desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine($"select {(contador ? "count(*)" : "id,professor_rf,ue_id,dre_id,data_inicio, data_fim, ano_letivo")} from atribuicao_esporadica where excluido = false");
        }

        private static void ObtenhaFiltro(StringBuilder sql, string codigoRF)
        {
            if (!string.IsNullOrWhiteSpace(codigoRF))
                sql.AppendLine("and professor_rf = @codigoRF");

            sql.AppendLine("and dre_id = @dreId");
            sql.AppendLine("and ue_id = @ueId");
            sql.AppendLine("and ano_letivo = @anoLetivo");
        }

        public AtribuicaoEsporadica ObterUltimaPorRF(string codigoRF, bool somenteInfantil)
        {
            var sql = $@"select
                            ae.*
                        from
                            atribuicao_esporadica ae
                        inner join ue u on
                            u.ue_id = ae.ue_id
                        inner join turma t on
                            t.ue_id = u.id
                        where
                            not ae.excluido and 
                            ae.professor_rf = @codigoRF
                            {(somenteInfantil ? "and t.modalidade_codigo = @infantil " : string.Empty)}
                        order by
                            ae.data_fim desc";

            var infantil = Modalidade.EducacaoInfantil;
            return database.Conexao.QueryFirstOrDefault<AtribuicaoEsporadica>(sql, new { codigoRF, infantil });
        }

        public async Task<IEnumerable<AtribuicaoEsporadica>> ObterAtribuicoesPorRFEAno(string codigoRF, bool somenteInfantil, int anoLetivo, string dreCodigo, string ueCodigo, bool historico)
        {
            var sql = $@"select
                            distinct ae.*
                        from
                            atribuicao_esporadica ae
                        inner join ue u on
                            u.ue_id = ae.ue_id
                        inner join turma t on
                            t.ue_id = u.id
                        where
                            not ae.excluido and
                            ae.professor_rf = @codigoRF and
                            t.historica = @historico
                            {(somenteInfantil ? " and t.modalidade_codigo = @infantil " : string.Empty)}";

            if (anoLetivo > 0)
                sql += " and ae.ano_letivo = @anoLetivo";

            if (!string.IsNullOrEmpty(dreCodigo))
                sql += " and ae.dre_id = @dreCodigo ";

            if (!string.IsNullOrEmpty(ueCodigo))
                sql += " and ae.ue_id = @ueCodigo ";

            sql += " order by ae.data_fim desc";

            var infantil = Modalidade.EducacaoInfantil;

            return await database.Conexao
                .QueryAsync<AtribuicaoEsporadica>(sql, new { codigoRF, infantil, anoLetivo, dreCodigo, ueCodigo, historico });
        }

        public async Task<bool> PossuiAtribuicaoPorAnoData(int? anoLetivo, string dreCodigo, string ueCodigo, string codigoRF, DateTime? data)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"select 1 from atribuicao_esporadica where not excluido ");

            if (anoLetivo.HasValue && anoLetivo.Value > 0)
                sql.AppendLine(" and ano_letivo = @anoLetivo ");

            if (!string.IsNullOrEmpty(dreCodigo))
                sql.AppendLine(" and dre_id = @dreCodigo ");

            if (!string.IsNullOrEmpty(ueCodigo))
                sql.AppendLine(" and ue_id = @ueCodigo ");

            if (!string.IsNullOrEmpty(codigoRF))
                sql.AppendLine(" and professor_rf = @codigoRF ");

            if (data.HasValue)
                sql.AppendLine(" and data_inicio <= @data and data_fim >= @data ");

            return await database.Conexao.QuerySingleOrDefaultAsync<bool>(sql.ToString(), new { anoLetivo, dreCodigo, ueCodigo, codigoRF, data = data.HasValue ? data.Value.Date : DateTime.MinValue });
        }

        public async Task<IEnumerable<AtribuicaoEsporadicaVigenteProfDto>> ObterAtribuicoesPorAno(int anoLetivo, DateTime? data)
        {
            var sql = $@"select
                            distinct ae.professor_rf as UsuarioRf, ae.ue_id as ue, ae.dre_id as dre
                        from
                            atribuicao_esporadica ae
                        where
                            not ae.excluido 
                            and ae.ano_letivo = @anoLetivo ";
            if (data.HasValue)
                sql += " and @data between ae.data_inicio and ae.data_fim ";
            return await database.Conexao
                .QueryAsync<AtribuicaoEsporadicaVigenteProfDto>(sql, new { anoLetivo, data });
        }
    }
}