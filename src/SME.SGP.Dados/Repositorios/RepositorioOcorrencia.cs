using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrencia : RepositorioBase<Ocorrencia>, IRepositorioOcorrencia
    {
        public RepositorioOcorrencia(ISgpContext conexao) : base(conexao) { }

        public async Task<PaginacaoResultadoDto<Ocorrencia>> ListarPaginado(long turmaId, string titulo, string alunoNome, DateTime? dataOcorrenciaInicio, DateTime? dataOcorrenciaFim, long[] codigosAluno, Paginacao paginacao)
        {
            var tabelas = @" ocorrencia o
						inner join ocorrencia_tipo ot on ot.id = o.ocorrencia_tipo_id 
						inner join ocorrencia_aluno oa on oa.ocorrencia_id = o.id ";

            var condicao = new StringBuilder();
            condicao.AppendLine(" where not o.excluido and turma_id = @turmaId ");

            if (!string.IsNullOrEmpty(titulo))
                condicao.AppendLine("and lower(f_unaccent(o.titulo)) LIKE ('%' || lower(f_unaccent(@titulo)) || '%')");

            if (dataOcorrenciaInicio.HasValue)
                condicao.AppendLine("and data_ocorrencia::date >= @dataOcorrenciaInicio  ");

            if (dataOcorrenciaFim.HasValue)
                condicao.AppendLine("and data_ocorrencia::date <= @dataOcorrenciaFim");

            if (codigosAluno != null)
                condicao.AppendLine("and oa.codigo_aluno = ANY(@codigosAluno)");

            var orderBy = "order by o.data_ocorrencia desc";

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var query = $"select count(distinct o.id) from {tabelas} {condicao}";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
               new { titulo, alunoNome, dataOcorrenciaInicio, dataOcorrenciaFim, codigosAluno, turmaId });

            var offSet = "offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            query = $@" drop table if exists tempOcorrenciasSelecionadas;

                        select
                            distinct o.id, o.data_ocorrencia
                        into temp tempOcorrenciasSelecionadas
                        from {tabelas}
                        {condicao} {orderBy} {offSet};

                        select
							o.id,
							o.turma_id,
							o.titulo,
							o.data_ocorrencia,
							o.hora_ocorrencia,
							o.descricao,
							o.ocorrencia_tipo_id,
							o.excluido,
							o.criado_rf,
							o.criado_em,
							o.alterado_em,
							o.alterado_por,
							o.alterado_rf,
							ot.id,
							ot.descricao,
							oa.id,
							oa.codigo_aluno
                        from tempOcorrenciasSelecionadas tos
                        inner join ocorrencia o on tos.id = o.id
						inner join ocorrencia_tipo ot on ot.id = o.ocorrencia_tipo_id 
						inner join ocorrencia_aluno oa on oa.ocorrencia_id = o.id;";

            var lstOcorrencias = new Dictionary<long, Ocorrencia>();

            await database.Conexao.QueryAsync<Ocorrencia, OcorrenciaTipo, OcorrenciaAluno, Ocorrencia>(query.ToString(), (ocorrencia, tipo, aluno) =>
            {
                if (!lstOcorrencias.TryGetValue(ocorrencia.Id, out Ocorrencia ocorrenciaEntrada))
                {
                    ocorrenciaEntrada = ocorrencia;
                    ocorrenciaEntrada.OcorrenciaTipo = tipo;
                    lstOcorrencias.Add(ocorrenciaEntrada.Id, ocorrenciaEntrada);
                }

                ocorrenciaEntrada.Alunos = ocorrenciaEntrada.Alunos ?? new List<OcorrenciaAluno>();
                ocorrenciaEntrada.Alunos.Add(aluno);
                return ocorrenciaEntrada;
            }, new
            {
                titulo,
                alunoNome,
                dataOcorrenciaInicio,
                dataOcorrenciaFim,
                codigosAluno,
                turmaId,
                qtdeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                qtdeRegistros = paginacao.QuantidadeRegistros
            }, splitOn: "id, id");

            return new PaginacaoResultadoDto<Ocorrencia>()
            {
                Items = lstOcorrencias.Values.ToList(),
                TotalRegistros = totalRegistrosDaQuery,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistrosDaQuery / paginacao.QuantidadeRegistros)
            };
        }

        public override async Task<Ocorrencia> ObterPorIdAsync(long id)
        {
            const string sql = @"select
									o.id,
									o.alterado_em as AlteradoEm,
									o.alterado_por as AlteradoPor,
									o.alterado_rf as AlteradoRf,
									o.criado_em as CriadoEm,
									o.criado_por as CriadoPor,
									o.criado_rf as CriadoRf,
									o.data_ocorrencia as DataOcorrencia,
									o.hora_ocorrencia as HoraOcorrencia,
									o.ocorrencia_tipo_id as OcorrenciaTipoId,
									o.turma_id as TurmaId,
									o.titulo as Titulo,
									o.descricao as Descricao,
									o.excluido as Excluido,
									oa.id,
									oa.codigo_aluno as CodigoAluno,
									oa.ocorrencia_id as OcorrenciaId
								FROM 
									public.ocorrencia o
								INNER JOIN
									public.ocorrencia_aluno oa
									ON o.id = oa.ocorrencia_id
								WHERE
									o.id = @id
									AND not o.excluido;";

            Ocorrencia resultado = null;
            await database.Conexao.QueryAsync<Ocorrencia, OcorrenciaAluno, Ocorrencia>(sql,
                (ocorrencia, ocorrenciaAluno) =>
                {
                    if (resultado is null)
                    {
                        resultado = ocorrencia;
                    }

                    resultado.Alunos = resultado.Alunos ?? new List<OcorrenciaAluno>();
                    resultado.Alunos.Add(ocorrenciaAluno);
                    return resultado;
                },
                new { id });

            return resultado;
        }

        public async Task<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>> ObterOcorrenciasPorTurmaAlunoEPeriodoPaginadas(long turmaId, long codigoAluno, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao)
        {
            try
            {
                var query = MontaQueryCompleta(paginacao, turmaId, codigoAluno, periodoInicio, periodoFim);

                var parametros = new { turmaId, codigoAluno, periodoInicio, periodoFim };
                var retorno = new PaginacaoResultadoDto<OcorrenciasPorAlunoDto>();

                using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
                {
                    retorno.Items = multi.Read<OcorrenciasPorAlunoDto>();
                    retorno.TotalRegistros = multi.ReadFirst<int>();
                }

                retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long turmaId, long alunoCodigo, DateTime periodoIncio, DateTime periodoFim)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, turmaId, alunoCodigo, periodoIncio, periodoFim);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, turmaId, alunoCodigo, periodoIncio, periodoFim);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long turmaId, long alunoCodigo, DateTime periodoIncio, DateTime periodoFim)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, turmaId, alunoCodigo, periodoIncio, periodoFim);

            if (!contador)
                sql.AppendLine(" order by o.data_ocorrencia desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine(" count(o.id) ");
            else
            {
                sql.AppendLine(" o.data_ocorrencia data_ocorrencia ");
                sql.AppendLine(", CONCAT(o.criado_por, ' (', o.criado_rf, ')') registrado_por ");
                sql.AppendLine(", o.titulo ");
            }

            sql.AppendLine(" from ocorrencia o ");
            sql.AppendLine(" inner join ocorrencia_aluno oa on oa.ocorrencia_id = o.id ");
            sql.AppendLine(" inner join turma t on t.id = o.turma_id");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long turmaId, long alunoCodigo, DateTime periodoIncio, DateTime periodoFim)
        {
            sql.AppendLine(" where ");
            sql.AppendLine(" o.turma_id = @turmaId and ");
            sql.AppendLine(" oa.codigo_aluno = @codigoAluno and ");
            sql.AppendLine(" o.criado_em::date between @periodoInicio and @periodoFim ");
        }
    }
}
