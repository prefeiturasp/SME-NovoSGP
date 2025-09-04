using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFrequenciaAlunoConsulta : RepositorioBase<AnotacaoFrequenciaAluno>, IRepositorioAnotacaoFrequenciaAlunoConsulta
    {
        public RepositorioAnotacaoFrequenciaAlunoConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }         

        public async Task<IEnumerable<string>> ListarAlunosComAnotacaoFrequenciaNaAula(long aulaId)
        {
            var query = "select codigo_aluno from anotacao_frequencia_aluno where not excluido and aula_id = @aulaId";

            return await database.Conexao.QueryAsync<string>(query, new { aulaId });
        }

        public async Task<AnotacaoFrequenciaAluno> ObterPorAlunoAula(string codigoAluno, long aulaId)
        {
            var query = "select * from anotacao_frequencia_aluno where not excluido and codigo_aluno = @codigoAluno and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<AnotacaoFrequenciaAluno>(query, new { codigoAluno, aulaId });
        }
        public async Task<IEnumerable<AnotacaoFrequenciaAluno>> ObterPorAulaIdRegistroExcluido(long aulaId)
        {
            var query = "select * from anotacao_frequencia_aluno where aula_id = @aulaId";

            return await database.Conexao.QueryAsync<AnotacaoFrequenciaAluno>(query, new {aulaId });
        }

        public async Task<IEnumerable<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricular(long turmaId, long codigoAluno, long componenteCurricularId)
        {
            var query = @"select n.* from (
                            (
                            select an.id,case when ma.descricao is not null then ma.descricao else an.anotacao end as Motivo, a.data_aula DataAnotacao 
                            from anotacao_frequencia_aluno an
                            left join motivo_ausencia ma on an.motivo_ausencia_id = ma.id  
                            inner join aula a on a.id = an.aula_id 
                            inner join turma t on t.turma_id = a.turma_id
                            where not an.excluido 
                            and t.id = @turmaId 
                            and an.codigo_aluno = @codigoAluno
                            and a.disciplina_id = @componenteCurricularId
                            )
                            union
                            (
                            select 0 as id, '' as Motivo, a.data_aula as DataAnotacao 
                            from registro_ausencia_aluno raa 
                            inner join registro_frequencia rf on rf.id = raa.registro_frequencia_id 
                            inner join aula a on a.id = rf.aula_id 
                            left join anotacao_frequencia_aluno an on an.aula_id = a.id and an.codigo_aluno = @codigoAluno
                            left join turma t on t.turma_id = a.turma_id 
                            where t.id = @turmaId
                            and raa.codigo_aluno = @codigoAluno
                            and a.disciplina_id = @componenteCurricularId
                            and not a.excluido 
                            and not raa.excluido
                            and an.id is null
                            )
                            ) n
                            order by n.DataAnotacao desc";

            return await database.Conexao.QueryAsync<JustificativaAlunoDto>(query, new
            {
                turmaId,
                codigoAluno = codigoAluno.ToString(),
                componenteCurricularId = componenteCurricularId.ToString()
            });
        }

        public async Task<IEnumerable<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricularBimestre(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre)
        {
            var query = @"select n.* from (
                            (
                            select 
                                an.id,
                                case when ma.descricao is not null then ma.descricao else an.anotacao end as Motivo, 
                                a.data_aula DataAnotacao,
                                an.criado_por as RegistradoPor
                            from anotacao_frequencia_aluno an
                            left join motivo_ausencia ma on an.motivo_ausencia_id = ma.id  
                            inner join aula a on a.id = an.aula_id 
                            inner join turma t on t.turma_id = a.turma_id ";
            if(bimestre > 0)
            { 
                query += " inner join periodo_escolar pe on a.tipo_calendario_id = pe.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim and pe.bimestre = @bimestre";
            }
            query += @" where not an.excluido 
                            and t.id = @turmaId 
                            and an.codigo_aluno = @codigoAluno ";
            if(componenteCurricularId > 0)
                query += " and a.disciplina_id = @componenteCurricularId ";

            query += @")
                            union
                            (
                            select 0 as id, '' as Motivo, a.data_aula as DataAnotacao, '' as RegistradoPor 
                            from registro_ausencia_aluno raa 
                            inner join registro_frequencia rf on rf.id = raa.registro_frequencia_id 
                            inner join aula a on a.id = rf.aula_id 
                            left join anotacao_frequencia_aluno an on an.aula_id = a.id and an.codigo_aluno = @codigoAluno
                            left join turma t on t.turma_id = a.turma_id ";
            if(bimestre > 0)
            { 
                query += " inner join periodo_escolar pe on a.tipo_calendario_id = pe.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim and pe.bimestre = @bimestre";
            }
            query += @" where t.id = @turmaId
                            and raa.codigo_aluno = @codigoAluno ";
            if (componenteCurricularId > 0)
                query += " and a.disciplina_id = @componenteCurricularId ";

            query += @" and not a.excluido 
                            and not raa.excluido
                            and an.id is null
                            )
                            ) n
                            order by n.DataAnotacao desc";

            return await database.Conexao.QueryAsync<JustificativaAlunoDto>(query, new
            {
                turmaId,
                codigoAluno = alunoCodigo.ToString(),
                componenteCurricularId = componenteCurricularId.ToString(),
                bimestre
            });
        }

        public async Task<PaginacaoResultadoDto<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricularBimestrePaginado(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre, Paginacao paginacao, int? semestre)
        {
            StringBuilder sql = new StringBuilder();

            int[] bimestreSemestre = null;

            if (semestre.HasValue)
                bimestreSemestre = semestre.Value <= 2 ? new int[] { 1, 2 } : new int[] { 3, 4 };

            var bimestres = bimestre > 0 ? new int[] { bimestre } : bimestreSemestre;

            MontaQueryObterPorTurmaAlunoComponenteCurricularBimestrePaginado(paginacao, sql, false, componenteCurricularId, bimestres);

            sql.AppendLine(";");

            MontaQueryObterPorTurmaAlunoComponenteCurricularBimestrePaginado(paginacao, sql, true, componenteCurricularId, bimestres);

            var parametros = new { turmaId, alunoCodigo = alunoCodigo.ToString(), componenteCurricularId = componenteCurricularId.ToString(), bimestres };

            var retorno = new PaginacaoResultadoDto<JustificativaAlunoDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), parametros))
            {
                retorno.Items = multi.Read<JustificativaAlunoDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private void MontaQueryObterPorTurmaAlunoComponenteCurricularBimestrePaginado(Paginacao paginacao, StringBuilder sql, bool contador, long componenteCurricularId, int[] bimestres)
        {
            sql.AppendLine(contador ? " select count(n.id) " : " select n.* ");

            sql.AppendLine(@" from (
                            select 
                                an.id,
                                case when ma.descricao is not null then ma.descricao else an.anotacao end as Motivo, 
                                a.data_aula DataAusencia,
                                an.criado_por as RegistradoPor,
                                an.criado_rf as RegistradoRF
                            from anotacao_frequencia_aluno an
                            left join motivo_ausencia ma on an.motivo_ausencia_id = ma.id  
                            inner join aula a on a.id = an.aula_id 
                            inner join turma t on t.turma_id = a.turma_id ");


            if (bimestres.NaoEhNulo())
                sql.AppendLine(" inner join periodo_escolar pe on a.tipo_calendario_id = pe.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim and pe.bimestre = any(@bimestres)");

            sql.AppendLine(@" where not an.excluido 
                            and t.id = @turmaId 
                            and an.codigo_aluno = @alunoCodigo ");
            if (componenteCurricularId > 0)
                sql.AppendLine(" and a.disciplina_id = @componenteCurricularId ");

            sql.AppendLine(@") n");
            if (!contador)
                sql.AppendLine(" order by n.DataAusencia desc ");


            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        public async Task<IEnumerable<AnotacaoAlunoAulaDto>> ListarAlunosComAnotacaoFrequenciaPorPeriodo(string turmaCodigo, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"select afa.codigo_aluno as alunoCodigo
                          , afa.aula_id as aulaId
                      from anotacao_frequencia_aluno afa
                     inner join aula a on a.id = afa.aula_id
                     where not afa.excluido
                       and not a.excluido
                       and a.turma_id = @turmaCodigo
                       and a.data_aula between @dataInicio and @dataFim";

            return await database.Conexao.QueryAsync<AnotacaoAlunoAulaDto>(query, new { turmaCodigo, dataInicio, dataFim });
        }

        public async Task<IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>> ObterPorAlunoPorPeriodo(string codigoAluno, DateTime dataInicio, DateTime dataFim)
        {
            var query = @" SELECT afa.codigo_aluno as CodigoAluno
                               , afa.aula_id as aulaId
                               , afa.motivo_ausencia_id 
                               , afa.anotacao
                               , a.data_aula as DataAula
                               , ma.descricao as DescricaoMotivoAusencia
                               , afa.id
                           FROM anotacao_frequencia_aluno afa
                           INNER JOIN aula a on a.id = afa.aula_id
                           LEFT JOIN motivo_ausencia ma on afa.motivo_ausencia_id = ma.id 
                           WHERE not afa.excluido
                            AND not a.excluido
                            AND afa.codigo_aluno = @codigoAluno
                            AND a.data_aula BETWEEN @dataInicio AND @dataFim
                            ORDER BY a.data_aula DESC, afa.id DESC;";

            return await database.Conexao.QueryAsync<AnotacaoAlunoAulaPorPeriodoDto>(query, new { codigoAluno, dataInicio, dataFim });
        }
    }
}