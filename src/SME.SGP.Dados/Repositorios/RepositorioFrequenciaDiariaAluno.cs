using Dapper;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaDiariaAluno : IRepositorioFrequenciaDiariaAluno
    {
        private readonly ISgpContext database;
        public RepositorioFrequenciaDiariaAluno(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<PaginacaoResultadoDto<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>> ObterQuantidadeAulasDiasTipoFrequenciaPorBimestreAlunoCodigoTurmaDisciplina(Paginacao paginacao, int[] bimestres, string codigoAluno, long turmaId, string[] aulaDisciplinasIds, string professor = null)
        {
            var query = new StringBuilder();
            var considerarProfessor = !string.IsNullOrEmpty(professor);

            MontarQueryConsulta(paginacao, query, false, considerarProfessor);
            query.AppendLine(";");
            MontarQueryConsulta(paginacao, query, true, considerarProfessor);

            var parametros = new
            {
                bimestres,
                codigoAluno,
                turmaId,
                aulaDisciplinasIds,
                professor
            };

            var retorno = new PaginacaoResultadoDto<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>();
            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), parametros))
            {
                retorno.Items = multi.Read<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private void MontarQueryConsulta(Paginacao paginacao, StringBuilder query, bool contador, bool considerarProfessor)
        {
            query.AppendLine(contador ? " select count(n.TotalAulasNoDia) " : " select n.*  ");


            query.AppendLine(@$"
                FROM(SELECT count(rfa.id) AS TotalAulasNoDia,
                            a.data_aula AS DataAula,
                            a.id AS AulasId,
                            rfa.codigo_aluno AS AlunoCodigo,
                            an.id AS AnotacaoId,
                            count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (WHERE rfa.valor = 1) AS TotalPresencas,
                            count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (WHERE rfa.valor = 2) AS TotalAusencias,
                            count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (WHERE rfa.valor = 3) AS TotalRemotos,
                            coalesce(ma.descricao, an.anotacao) as MotivoAusencia
                        FROM registro_frequencia_aluno rfa 
                        INNER JOIN registro_frequencia rf ON rfa.registro_frequencia_id = rf.id
                        INNER JOIN aula a ON rf.aula_id = a.id
                        INNER JOIN turma t ON t.turma_id = a.turma_id
                        INNER JOIN periodo_escolar pe ON a.tipo_calendario_id = pe.tipo_calendario_id AND a.data_aula BETWEEN pe.periodo_inicio AND pe.periodo_fim AND pe.bimestre = any(@bimestres)
                        LEFT JOIN anotacao_frequencia_aluno an ON a.id = an.aula_id AND an.codigo_aluno  = rfa.codigo_aluno AND an.excluido = false
                        LEFT JOIN motivo_ausencia ma ON an.motivo_ausencia_id = ma.id
                        WHERE NOT rfa.excluido AND NOT rf.excluido AND NOT a.excluido
                            AND rfa.codigo_aluno = @codigoAluno
                            AND t.id = @turmaId AND a.disciplina_id = any(@aulaDisciplinasIds)
                            {(considerarProfessor ? "AND a.professor_rf = @professor" : string.Empty)}
                         GROUP  BY a.data_aula,a.id,an.id,ma.descricao,rfa.codigo_aluno
                        order by a.data_aula desc)n
                ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                query.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }
    }
}
