using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

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

        public IEnumerable<AulasPorDisciplinaDto> ObterTotalAulasPorDisciplina(int anoLetivo)
        {
            var dataAtual = DateTime.Now;
            var query = @"select
	                        distinct a.disciplina_id as DisciplinaId,
	                        p.periodo_inicio as PeriodoInicio,
	                        p.periodo_fim as PeriodoFim,
	                        count(a.id) as TotalAulas
                        from
	                        aula a
                        inner join registro_frequencia rf on
	                        rf.aula_id = a.id
                        inner join periodo_escolar p on
	                        a.tipo_calendario_id = p.tipo_calendario_id
                        where
	                        p.periodo_inicio <= @dataAtual
                            and p.periodo_fim >= @dataAtual
	                        and a.data_aula >= p.periodo_inicio
	                        and a.data_aula <= p.periodo_fim
	                        and not a.excluido
                        group by
	                        a.disciplina_id,
	                        p.periodo_inicio,
	                        p.periodo_fim;";

            return database.Conexao.Query<AulasPorDisciplinaDto>(query, new { anoLetivo, dataAtual });
        }

        public int ObterTotalAulasPorDisciplinaETurma(DateTime dataAtual, string disciplinaId, string turmaId)
        {
            var query = @"select
	                            count(distinct rf.id)
                            from
	                            aula a
                            inner join registro_frequencia rf on
	                            rf.aula_id = a.id
                            inner join registro_ausencia_aluno ra on
	                            rf.id = ra.registro_frequencia_id
                            where
	                            a.disciplina_id = @disciplinaId
	                            and a.turma_id = @turmaId
	                            and not ra.excluido
	                            and not a.excluido";

            return database.Conexao.QueryFirstOrDefault<int>(query, new { dataAtual, disciplinaId, turmaId });
        }

        public AusenciaPorDisciplinaDto ObterTotalAusenciasPorAlunoEDisciplina(DateTime periodo, string codigoAluno, string disciplinaId, string turmaId)
        {
            var query = @"select
	                            count(ra.id) as TotalAusencias,
	                            p.periodo_inicio,
	                            p.periodo_fim,
                                p.bimestre
                            from
	                            registro_ausencia_aluno ra
                            inner join registro_frequencia rf on
	                            ra.registro_frequencia_id = rf.id
                            inner join aula a on
	                            rf.aula_id = a.id
                            inner join periodo_escolar p on
	                            a.tipo_calendario_id = p.tipo_calendario_id
                            where
	                            ra.codigo_aluno = @codigoAluno
	                            and a.disciplina_id = @disciplinaId
	                            and a.turma_id = @turmaId
                                and p.periodo_inicio <= @periodo
	                            and p.periodo_fim >= @periodo
	                            and not ra.excluido
	                            and not a.excluido
                            group by
	                            p.periodo_inicio,
	                            p.periodo_fim,
                                p.bimestre";

            return database.Conexao.QueryFirstOrDefault<AusenciaPorDisciplinaDto>(query, new { periodo, codigoAluno, disciplinaId, turmaId });
        }
    }
}