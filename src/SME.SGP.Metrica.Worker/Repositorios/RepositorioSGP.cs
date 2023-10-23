using Dapper;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios
{
    public class RepositorioSGP : IRepositorioSGP
    {
        private readonly ISgpContext database;

        public RepositorioSGP(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public Task AtualizarConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
                @"update conselho_classe_aluno
			        set conselho_classe_id = @ultimoId
		        where conselho_classe_id in (
			        select id from conselho_classe 
			        where fechamento_turma_id = @fechamentoTurmaId
			          and id <> @ultimoId
		        );", new { fechamentoTurmaId, ultimoId });

        public Task ExcluirConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
                @"delete from conselho_classe 
		        where fechamento_turma_id = @fechamentoTurmaId
		            and id <> @ultimoId;", new { fechamentoTurmaId, ultimoId });

        public Task AtualizarNotasConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
                @"update conselho_classe_nota 
			        set conselho_classe_aluno_id = @ultimoId
		        where conselho_classe_aluno_id in (
			        select cca.id 
			          from conselho_classe_aluno cca 
			         where cca.conselho_classe_id = @conselhoClasseId
			           and aluno_codigo = @alunoCodigo 
			           and cca.id <> @ultimoId
		        );", new { conselhoClasseId, alunoCodigo, ultimoId });

        public Task AtualizarRecomendacoesConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
				@"update conselho_classe_aluno_recomendacao
					set conselho_classe_aluno_id = @ultimoId
				where conselho_classe_aluno_id in (
					select cca.id 
					  from conselho_classe_aluno cca 
					 where cca.conselho_classe_id = @conselhoClasseId 
					   and aluno_codigo = @alunoCodigo
					   and cca.id <> @ultimoId
				);", new { conselhoClasseId, alunoCodigo, ultimoId });

        public Task ExcluirTurmasComplementaresConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
				@"delete from conselho_classe_aluno_turma_complementar
				where conselho_classe_aluno_id in (
					select cca.id 
						from conselho_classe_aluno cca 
						where cca.conselho_classe_id = @conselhoClasseId
						and aluno_codigo = @alunoCodigo
						and cca.id <> @ultimoId
				);", new { conselhoClasseId, alunoCodigo, ultimoId });

        public Task AtualizarWfAprovacaoConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
				@"update wf_aprovacao_parecer_conclusivo
					set conselho_classe_aluno_id = @ultimoId
				where conselho_classe_aluno_id in (
					select cca.id 
					  from conselho_classe_aluno cca 
					 where cca.conselho_classe_id = @conselhoClasseId
					   and aluno_codigo = @alunoCodigo
					   and cca.id <> @ultimoId
				);", new { conselhoClasseId, alunoCodigo, ultimoId });

        public Task AtualizarParecerConclusivoConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
				@"update conselho_classe_aluno 
				   set conselho_classe_parecer_id = (select min(conselho_classe_parecer_id) from conselho_classe_aluno where id in (
		   			select cca.id 
					  from conselho_classe_aluno cca 
					 where cca.conselho_classe_id = @conselhoClasseId
					   and aluno_codigo = @alunoCodigo
					   and cca.id <> @ultimoId
				)) where id = @ultimoId
					and conselho_classe_parecer_id is null;", new { conselhoClasseId, alunoCodigo, ultimoId });

        public Task ExcluirConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
				@"delete from conselho_classe_aluno
				where conselho_classe_id = @conselhoClasseId
					   and aluno_codigo = @alunoCodigo
					   and id <> @ultimoId;", new { conselhoClasseId, alunoCodigo, ultimoId });

		public Task AtualizarWfAprovacaoConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteScalarAsync(
				@"update wf_aprovacao_nota_conselho 
					set conselho_classe_nota_id = @ultimoId
				where conselho_classe_nota_id in (
					select ccn.id 
					  from conselho_classe_nota ccn
					 where ccn.conselho_classe_aluno_id = @conselhoClasseAlunoId
					   and ccn.componente_curricular_codigo = @componenteCurricularId
					   and ccn.id <> @ultimoId
				);", new { conselhoClasseAlunoId, componenteCurricularId, ultimoId });

		public Task AtualizarHistoricoNotaConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteScalarAsync(
				@"update historico_nota_conselho_classe 
					set conselho_classe_nota_id = @ultimoId
				where conselho_classe_nota_id in (
					select ccn.id 
					  from conselho_classe_nota ccn
					 where ccn.conselho_classe_aluno_id = @conselhoClasseAlunoId
					   and ccn.componente_curricular_codigo = @componenteCurricularId
					   and ccn.id <> @ultimoId
				);", new { conselhoClasseAlunoId, componenteCurricularId, ultimoId });

		public Task AtualizarMaiorNotaConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteScalarAsync(
				@"update conselho_classe_nota 
		 			set nota = (select max(nota) from conselho_classe_nota where conselho_classe_aluno_id = @conselhoClasseAlunoId and componente_curricular_codigo = @componenteCurricularId)
		 			  , conceito_id = (select min(conceito_id) from conselho_classe_nota where conselho_classe_aluno_id = @conselhoClasseAlunoId and componente_curricular_codigo = @componenteCurricularId)
				where id = @ultimoId;", new { conselhoClasseAlunoId, componenteCurricularId, ultimoId });

		public Task ExcluirConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteScalarAsync(
				@"delete from conselho_classe_nota 
				where conselho_classe_aluno_id = @conselhoClasseAlunoId
				  and componente_curricular_codigo = @componenteCurricularId
				  and id <> @ultimoId;", new { conselhoClasseAlunoId, componenteCurricularId, ultimoId });

		public Task AtualizarConselhoClasseFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId)
			=> database.Conexao.ExecuteScalarAsync(
				@"update conselho_classe
					set fechamento_turma_id = @ultimoId
				where fechamento_turma_id in (
					select id from fechamento_turma 
					where turma_id = @turmaId
					  and coalesce(periodo_escolar_id,0) = @periodoEscolarId
					  and id <> @ultimoId
				);", new { turmaId, periodoEscolarId, ultimoId });

		public Task AtualizarComponenteFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId)
			=> database.Conexao.ExecuteScalarAsync(
				@"update fechamento_turma_disciplina
					set fechamento_turma_id = @ultimoId
				where fechamento_turma_id in (
					select id from fechamento_turma 
					where turma_id = @turmaId
					  and coalesce(periodo_escolar_id,0) = @periodoEscolarId
					  and id <> @ultimoId
				);", new { turmaId, periodoEscolarId, ultimoId });

		public Task ExcluirFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId)
			=> database.Conexao.ExecuteScalarAsync(
				@"delete from fechamento_turma 
				where turma_id = @turmaId
				  and coalesce(periodo_escolar_id,0) = @periodoEscolarId
				  and id <> @ultimoId;", new { turmaId, periodoEscolarId, ultimoId });

	}
}
