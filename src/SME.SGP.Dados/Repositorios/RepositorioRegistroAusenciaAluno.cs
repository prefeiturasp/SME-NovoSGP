using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAusenciaAluno : RepositorioBase<RegistroAusenciaAluno>, IRepositorioRegistroAusenciaAluno
    {
        public RepositorioRegistroAusenciaAluno(ISgpContext database) : base(database)
        {
        }

        public bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            var query = @"update
                            registro_ausencia_aluno
                        set
                            excluido = true
                        where
                            registro_frequencia_id = @registroFrequenciaId";

            return database.Conexao.Execute(query, new { registroFrequenciaId }) > 0;
        }

        public int ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, string turmaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select ");
            query.AppendLine("sum(distinct a.quantidade) ");
            query.AppendLine("from ");
            query.AppendLine("aula a ");
            query.AppendLine("inner join registro_frequencia rf on ");
            query.AppendLine("rf.aula_id = a.id ");
            query.AppendLine("inner join periodo_escolar p on ");
            query.AppendLine("a.tipo_calendario_id = p.tipo_calendario_id ");
            query.AppendLine("where not a.excluido");
            query.AppendLine("and p.periodo_inicio <= @dataAula ");
            query.AppendLine("and p.periodo_fim >= @dataAula ");
            query.AppendLine("and a.data_aula >= p.periodo_inicio");
            query.AppendLine("and a.data_aula <= p.periodo_fim ");

            if (!string.IsNullOrWhiteSpace(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId ");

            query.AppendLine("and a.turma_id = @turmaId ");

            return database.Conexao.QueryFirstOrDefault<int>(query.ToString(), new { dataAula, disciplinaId, turmaId });
        }

        public AusenciaPorDisciplinaDto ObterTotalAusenciasPorAlunoETurma(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("count(ra.id) as TotalAusencias, ");
            query.AppendLine("p.periodo_inicio as PeriodoInicio, ");
            query.AppendLine("p.periodo_fim as PeriodoFim, ");
            query.AppendLine("p.bimestre ");
            query.AppendLine("from ");
            query.AppendLine("registro_ausencia_aluno ra ");
            query.AppendLine("inner join registro_frequencia rf on ");
            query.AppendLine("ra.registro_frequencia_id = rf.id ");
            query.AppendLine("inner join aula a on ");
            query.AppendLine("rf.aula_id = a.id ");
            query.AppendLine("inner join periodo_escolar p on ");
            query.AppendLine("a.tipo_calendario_id = p.tipo_calendario_id ");
            query.AppendLine("where not ra.excluido and not a.excluido and ");
            query.AppendLine("ra.codigo_aluno = @codigoAluno ");

            if (!string.IsNullOrWhiteSpace(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId ");

            query.AppendLine("and a.turma_id = @turmaId ");
            query.AppendLine("and p.periodo_inicio <= @dataAula ");
            query.AppendLine("and p.periodo_fim >= @dataAula ");
            query.AppendLine("and a.data_aula >= p.periodo_inicio");
            query.AppendLine("and a.data_aula <= p.periodo_fim ");
            query.AppendLine("and not ra.excluido");
            query.AppendLine("and not a.excluido");
            query.AppendLine("group by");
            query.AppendLine("p.periodo_inicio,");
            query.AppendLine("p.periodo_fim,");
            query.AppendLine("p.bimestre");

            return database.Conexao.QueryFirstOrDefault<AusenciaPorDisciplinaDto>(query.ToString(), new { dataAula, codigoAluno, disciplinaId, turmaId });
        }
    }
}