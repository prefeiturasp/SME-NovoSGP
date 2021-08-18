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
    public class RepositorioPeriodoEscolar : RepositorioBase<PeriodoEscolar>, IRepositorioPeriodoEscolar
    {
        public RepositorioPeriodoEscolar(ISgpContext conexao) : base(conexao) { }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendario(long tipoCalendarioId)
        {
            string query = "select * from periodo_escolar where tipo_calendario_id = @tipoCalendarioId";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query, new { tipoCalendarioId });
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendarioAsync(long tipoCalendarioId)
        {
            string query = "select * from periodo_escolar where tipo_calendario_id = @tipoCalendarioId";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query, new { tipoCalendarioId }, commandTimeout: 20);
        }

        public async Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime data)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryComTipoCalendario(query);
            query.AppendLine("where pe.tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and pe.periodo_inicio::date <= date(@dataPeriodo)");
            query.AppendLine("and pe.periodo_fim::date >= date(@dataPeriodo)");

            return (await database.Conexao.QueryAsync<PeriodoEscolar, TipoCalendario, PeriodoEscolar>(query.ToString(), (pe, tipoCalendario) =>
            {
                pe.AdicionarTipoCalendario(tipoCalendario);
                return pe;
            }, new { tipoCalendarioId, dataPeriodo = data.Date }, splitOn: "id")).FirstOrDefault();
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEmAbertoPorTipoCalendarioData(long tipoCalendarioId, DateTime data)
        {
            StringBuilder query = new StringBuilder();
            MontaQuery(query);
            query.AppendLine("where tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and periodo_fim::date >= date(@dataPeriodo)");

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query.ToString(), new { tipoCalendarioId, dataPeriodo = data.Date });
        }

        public async Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime dataInicio, DateTime dataFim)
        {
            StringBuilder query = new StringBuilder();
            MontaQuery(query);
            query.AppendLine("where tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and periodo_inicio <= @dataInicio");
            query.AppendLine("and periodo_fim >= @dataFim");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query.ToString(), new { tipoCalendarioId, dataInicio, dataFim });
        }

        private static void MontaQuery(StringBuilder query)
        {
            query.AppendLine("select ");
            query.AppendLine("id,");
            query.AppendLine("bimestre,");
            query.AppendLine("periodo_inicio,");
            query.AppendLine("periodo_fim,");
            query.AppendLine("alterado_por,");
            query.AppendLine("alterado_rf,");
            query.AppendLine("alterado_em,");
            query.AppendLine("criado_por,");
            query.AppendLine("criado_rf,");
            query.AppendLine("criado_em,");
            query.AppendLine("tipo_calendario_id");
            query.AppendLine("from periodo_escolar");
        }

        private static void MontaQueryComTipoCalendario(StringBuilder query)
        {
            query.AppendLine("select ");
            query.AppendLine("pe.*,");
            query.AppendLine("tc.*");
            query.AppendLine("from periodo_escolar pe");
            query.AppendLine("inner join tipo_calendario tc on pe.tipo_calendario_id = tc.id");
        }

        public async Task<PeriodoEscolar> ObterUltimoBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {
            var query = new StringBuilder(@"select p.* 
                            from tipo_calendario t
                         inner join periodo_escolar p on p.tipo_calendario_id = t.id
                          where t.excluido = false and t.situacao
                            and t.ano_letivo = @anoLetivo
                            and t.modalidade = @modalidade ");

            DateTime dataReferencia = DateTime.MinValue;
            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                var periodoReferencia = semestre == 1 ? "periodo_inicio < @dataReferencia" : "periodo_fim > @dataReferencia";
                query.AppendLine($"and exists(select 0 from periodo_escolar p where tipo_calendario_id = t.id and {periodoReferencia})");

                // 1/6/ano ou 1/7/ano dependendo do semestre
                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 8, 1);
            }
            query.AppendLine("order by bimestre desc ");
            query.AppendLine("limit 1");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query.ToString(), new { anoLetivo, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<int> ObterBimestrePorTurma(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            var query = new StringBuilder(@"select pe.bimestre
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and turma_id = @codigoTurma
                                              where tc.modalidade = @modalidade
                                              and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                              and not tc.excluido ");


            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { codigoTurma, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<int> ObterBimestreAtualPorTurmaIdAsync(long turmaId, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            var query = new StringBuilder(@"select pe.bimestre
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and t.id = @turmaId
                                             where tc.modalidade = @modalidade
                                               and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                               and not tc.excluido  ");


            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { turmaId, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<PeriodoEscolar> ObterPeriodoEscolarAtualPorTurmaIdAsync(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            const string sql = @"select pe.*
                                from periodo_escolar pe
                                inner join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                inner join turma t on t.ano_letivo = tc.ano_letivo and turma_id = @codigoTurma
                                where tc.modalidade = @modalidade
                                and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                and not tc.excluido ";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(sql, new { codigoTurma, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<bool> PeriodoEmAbertoAsync(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false)
        {
            var query = new StringBuilder(@"select count(pe.Id)
                          from periodo_escolar pe 
                         where pe.tipo_calendario_id = @tipoCalendarioId
                           and periodo_fim >= @dataReferencia ");

            if (!ehAnoLetivo)
                query.AppendLine("and periodo_inicio <= @dataReferencia");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre");

            return (await database.Conexao.QueryFirstAsync<int>(query.ToString(), new { tipoCalendarioId, dataReferencia, bimestre })) > 0;
        }

        public async Task<PeriodoEscolar> ObterPorTipoCalendarioEBimestreAsync(long tipoCalendarioId, int bimestre)
        {
            var query = $@"select p.* 
                            from periodo_escolar p
                          where p.tipo_calendario_id = @tipoCalendarioId
                            and p.bimestre = @bimestre";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query.ToString(), new { tipoCalendarioId, bimestre });
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPorAnoLetivoEModalidadeTurma(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, int semestre = 1)
        {
            var query = new StringBuilder($@"select
	                            distinct pe.*
                            from
	                            periodo_escolar pe
                            inner join tipo_calendario tc on
	                            pe.tipo_calendario_id = tc.id
                            where
	                            tc.modalidade = @modalidadeTipoCalendario
	                            and tc.ano_letivo = @anoLetivo
	                            and tc.situacao
	                            and not tc.excluido");

            var dataReferencia = DateTime.MinValue;
            if (modalidadeTipoCalendario == ModalidadeTipoCalendario.EJA)
            {
                var periodoReferencia = semestre == 1 ? "pe.periodo_inicio < @dataReferencia" : "pe.periodo_fim > @dataReferencia";
                query.AppendLine($" and exists(select 0 from periodo_escolar p where tipo_calendario_id = tc.id and {periodoReferencia})");

                // 1/6/ano ou 1/7/ano dependendo do semestre
                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 8, 1);
            }

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query.ToString(), new { modalidadeTipoCalendario, anoLetivo, dataReferencia });
        }

        public async Task<long> ObterPeriodoEscolarIdPorTurma(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            var query = new StringBuilder(@"select pe.id
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and turma_id = @codigoTurma
                                              where tc.modalidade = @modalidade
                                              and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                              and not tc.excluido ");

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query.ToString(), new { codigoTurma, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<int> ObterBimestreAtualAsync(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            var query = new StringBuilder(@"select pe.bimestre
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and turma_id = @codigoTurma
                                              where tc.modalidade = @modalidade
                                              and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                              and not tc.excluido ");

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { codigoTurma, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPorModalidadeDataFechamento(int modalidadeTipoCalendario, DateTime dataFechamento)
        {
            string query = @"select pe.* from periodo_escolar pe inner join tipo_calendario tc on

                                pe.tipo_calendario_id = tc.id
                            where
                                tc.modalidade = @modalidadeTipoCalendario and pe.periodo_fim = @dataFechamento";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query, new { modalidadeTipoCalendario, dataFechamento = dataFechamento.Date });
        }

        public async Task<long> ObterPeriodoEscolarIdPorTurmaBimestre(string turmaCodigo, ModalidadeTipoCalendario modalidadeTipoCalendario, int bimestre)
        {
            var query = new StringBuilder(@"select pe.id
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and turma_id = @turmaCodigo
                                              where tc.modalidade = @modalidade
                                              and pe.bimestre = @bimestre
                                              and not tc.excluido ");

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query.ToString(), new { turmaCodigo, modalidade = (int)modalidadeTipoCalendario, bimestre });
        }

        public async Task<PeriodoEscolar> ObterPeriodoEscolarPorTurmaBimestre(string turmaCodigo, ModalidadeTipoCalendario modalidadeTipoCalendario, int bimestre)
        {
            const string sql = @"select pe.*
                                from periodo_escolar pe
                                inner join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                inner join turma t on t.ano_letivo = tc.ano_letivo and turma_id = @turmaCodigo
                                where tc.modalidade = @modalidade
                                and pe.bimestre = @bimestre
                                and not tc.excluido";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(sql, new { turmaCodigo, modalidade = (int)modalidadeTipoCalendario, bimestre });
        }

        public async Task<long> ObterPeriodoEscolarIdPorTurmaId(long turmaId, ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataReferencia)
        {
            var query = new StringBuilder(@"select pe.id
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and id = @turmaId
                                              where tc.modalidade = @modalidade
                                              and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                              and not tc.excluido ");

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query.ToString(), new { turmaId, modalidade = (int)modalidadeTipoCalendario, dataReferencia });
        }

        public async Task<PeriodoEscolar> ObterPorModalidadeAnoEDataFinal(ModalidadeTipoCalendario modalidade, int ano, DateTime dataFim)
        {
            var query = @"select p.* 
                    from periodo_escolar p
                   inner join tipo_calendario t on t.id = p.tipo_calendario_id
                   where not t.excluido
                     and t.modalidade = @modalidade
                     and t.ano_letivo = @ano
                     and p.periodo_fim = @dataFim";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query, new { modalidade = (int)modalidade, ano, dataFim });
        }

        public async Task<PeriodoEscolar> ObterUltimoPeriodoEscolarPorData(int anoLetivo, ModalidadeTipoCalendario modalidade, DateTime dataAtual)
        {
            var query = @"select p.* 
                            from tipo_calendario t
                         inner join periodo_escolar p on p.tipo_calendario_id = t.id
                          where t.excluido = false and t.situacao
                            and t.ano_letivo = @anoLetivo
                            and t.modalidade = @modalidade 
                            and periodo_inicio <= @dataAtual 
                            order by bimestre desc 
                            limit 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query, new { anoLetivo, modalidade = (int)modalidade, dataAtual });
        }

        public async Task<int> ObterBimestreAtualComAberturaPorTurmaAsync(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, long ueId, DateTime dataReferencia)
        {
            var query = @"select
	                        pe.bimestre 
                        from
	                        periodo_fechamento pf
                        inner join ue u on
	                        pf.ue_id = u.id
                        inner join periodo_fechamento_bimestre pfb on
	                        pfb.periodo_fechamento_id = pf.id
                        inner join periodo_escolar pe on
	                        pfb.periodo_escolar_id = pe.id
                        inner join tipo_calendario tc on
	                        pe.tipo_calendario_id = tc.id
                        where
	                        tc.modalidade = @modalidadeTipoCalendario
	                        and u.id = @ueId
	                        and tc.ano_letivo = @anoLetivo
	                        and not excluido
	                        and @dataReferencia between pfb.inicio_fechamento and pfb.final_fechamento ";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { anoLetivo, modalidadeTipoCalendario, dataReferencia, ueId });
        }

        public async Task<IEnumerable<PeriodoEscolarModalidadeDto>> ObterPeriodosPassadosNoAno(DateTime data)
        {
            var query = @"select tc.modalidade
 	                    , pe.bimestre
 	                    , pe.periodo_inicio as DataInicio
 	                    , pe.periodo_fim as DataFim
                       from tipo_calendario tc 
                      inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id
                      where not tc.excluido 
                        and tc.situacao 
                        and tc.ano_letivo = @ano
                        and pe.periodo_inicio <= @data ";

            return await database.Conexao.QueryAsync<PeriodoEscolarModalidadeDto>(query, new { data, ano = data.Year });
        }
    }
}