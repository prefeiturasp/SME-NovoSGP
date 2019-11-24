using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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
	                        date_part('year', p.periodo_inicio) = @anoLetivo
	                        and date_part('year', p.periodo_fim)= @anoLetivo
	                        and a.data_aula >= p.periodo_inicio
	                        and a.data_aula <= p.periodo_fim
	                        and not a.excluido
                        group by
	                        a.disciplina_id,
	                        p.periodo_inicio,
	                        p.periodo_fim;";

            return database.Conexao.Query<AulasPorDisciplinaDto>(query, new { anoLetivo });
        }

        public IEnumerable<AusenciaPorDisciplinaDto> ObterTotalAusenciasPorAlunoEDisciplina(int anoLetivo)
        {
            var query = @"select
	                        distinct a.disciplina_id as DisciplinaId,
	                        ra.codigo_aluno as CodigoAluno,
	                        p.periodo_inicio as PeriodoInicio,
	                        p.periodo_fim as PeriodoFim,
	                        p.bimestre,
	                        a.turma_id as TurmaId,
	                        count(a.disciplina_id) as TotalAusencias
                        from
	                        registro_ausencia_aluno ra
                        inner join registro_frequencia rf on
	                        ra.registro_frequencia_id = rf.id
                        inner join aula a on
	                        rf.aula_id = a.id
                        inner join periodo_escolar p on
	                        a.tipo_calendario_id = p.tipo_calendario_id
                        where
	                        date_part('year', p.periodo_inicio) = @anoLetivo
	                        and date_part('year', p.periodo_fim)= @anoLetivo
	                        and a.data_aula >= p.periodo_inicio
	                        and a.data_aula <= p.periodo_fim
	                        and not ra.excluido
                        group by
	                        a.disciplina_id,
	                        ra.codigo_aluno,
	                        p.bimestre,
	                        p.periodo_inicio,
	                        a.turma_id,
	                        p.periodo_fim;";

            return database.Conexao.Query<AusenciaPorDisciplinaDto>(query, new { anoLetivo });
        }
    }
}