using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System.Collections.Generic;
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
            => database.Conexao.ExecuteAsync(
                @"update conselho_classe_aluno
			        set conselho_classe_id = @ultimoId
		        where conselho_classe_id in (
			        select id from conselho_classe 
			        where fechamento_turma_id = @fechamentoTurmaId
			          and id <> @ultimoId
		        );", new { fechamentoTurmaId, ultimoId });

        public Task ExcluirConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId)
            => database.Conexao.ExecuteAsync(
                @"delete from conselho_classe 
		        where fechamento_turma_id = @fechamentoTurmaId
		            and id <> @ultimoId;", new { fechamentoTurmaId, ultimoId });

        public Task AtualizarNotasConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteAsync(
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
            => database.Conexao.ExecuteAsync(
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
            => database.Conexao.ExecuteAsync(
				@"delete from conselho_classe_aluno_turma_complementar
				where conselho_classe_aluno_id in (
					select cca.id 
						from conselho_classe_aluno cca 
						where cca.conselho_classe_id = @conselhoClasseId
						and aluno_codigo = @alunoCodigo
						and cca.id <> @ultimoId
				);", new { conselhoClasseId, alunoCodigo, ultimoId });

        public Task AtualizarWfAprovacaoConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId)
            => database.Conexao.ExecuteAsync(
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
            => database.Conexao.ExecuteAsync(
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
            => database.Conexao.ExecuteAsync(
				@"delete from conselho_classe_aluno
				where conselho_classe_id = @conselhoClasseId
					   and aluno_codigo = @alunoCodigo
					   and id <> @ultimoId;", new { conselhoClasseId, alunoCodigo, ultimoId });

		public Task AtualizarWfAprovacaoConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
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
			=> database.Conexao.ExecuteAsync(
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
			=> database.Conexao.ExecuteAsync(
				@"update conselho_classe_nota 
		 			set nota = (select max(nota) from conselho_classe_nota where conselho_classe_aluno_id = @conselhoClasseAlunoId and componente_curricular_codigo = @componenteCurricularId)
		 			  , conceito_id = (select min(conceito_id) from conselho_classe_nota where conselho_classe_aluno_id = @conselhoClasseAlunoId and componente_curricular_codigo = @componenteCurricularId)
				where id = @ultimoId;", new { conselhoClasseAlunoId, componenteCurricularId, ultimoId });

		public Task ExcluirConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from conselho_classe_nota 
				where conselho_classe_aluno_id = @conselhoClasseAlunoId
				  and componente_curricular_codigo = @componenteCurricularId
				  and id <> @ultimoId;", new { conselhoClasseAlunoId, componenteCurricularId, ultimoId });

		public Task AtualizarConselhoClasseFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update conselho_classe
					set fechamento_turma_id = @ultimoId
				where fechamento_turma_id in (
					select id from fechamento_turma 
					where turma_id = @turmaId
					  and coalesce(periodo_escolar_id,0) = @periodoEscolarId
					  and id <> @ultimoId
				);", new { turmaId, periodoEscolarId, ultimoId });

		public Task AtualizarComponenteFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update fechamento_turma_disciplina
					set fechamento_turma_id = @ultimoId
				where fechamento_turma_id in (
					select id from fechamento_turma 
					where turma_id = @turmaId
					  and coalesce(periodo_escolar_id,0) = @periodoEscolarId
					  and id <> @ultimoId
				);", new { turmaId, periodoEscolarId, ultimoId });

		public Task ExcluirFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from fechamento_turma 
				where turma_id = @turmaId
				  and coalesce(periodo_escolar_id,0) = @periodoEscolarId
				  and id <> @ultimoId;", new { turmaId, periodoEscolarId, ultimoId });

		public Task AtualizarAlunoFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update fechamento_aluno
					set fechamento_turma_disciplina_id = @ultimoId
				where fechamento_turma_disciplina_id in (
					select id from fechamento_turma_disciplina 
					where fechamento_turma_id = @fechamentoTurmaId
					  and disciplina_id = @disciplinaId
					  and id <> @ultimoId
				);", new { fechamentoTurmaId, disciplinaId, ultimoId });

		public Task AtualizarAnotacaoAlunoFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update anotacao_aluno_fechamento
					set fechamento_turma_disciplina_id = @ultimoId
				where fechamento_turma_disciplina_id in (
					select id from fechamento_turma_disciplina 
					where fechamento_turma_id = @fechamentoTurmaId
					  and disciplina_id = @disciplinaId
					  and id <> @ultimoId
				);", new { fechamentoTurmaId, disciplinaId, ultimoId });

		public Task AtualizarPendenciaFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update pendencia_fechamento
					set fechamento_turma_disciplina_id = @ultimoId
				where fechamento_turma_disciplina_id in (
					select id from fechamento_turma_disciplina 
					where fechamento_turma_id = @fechamentoTurmaId
					  and disciplina_id = @disciplinaId
					  and id <> @ultimoId
				);", new { fechamentoTurmaId, disciplinaId, ultimoId });
		
		public Task ExcluirFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update pendencia_fechamento
					set fechamento_turma_disciplina_id = @ultimoId
				where fechamento_turma_disciplina_id in (
					select id from fechamento_turma_disciplina 
					where fechamento_turma_id = @fechamentoTurmaId
					  and disciplina_id = @disciplinaId
					  and id <> @ultimoId
				);", new { fechamentoTurmaId, disciplinaId, ultimoId });
		
		public Task AtualizarNotaFechamentoAlunoDuplicados(long fechamentoDisciplinaId, string alunoCodigo, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update fechamento_nota
					set fechamento_aluno_id = @ultimoId
				where fechamento_aluno_id in (
					select id from fechamento_aluno 
					where fechamento_turma_disciplina_id = @fechamentoDisciplinaId
					  and aluno_codigo = @alunoCodigo
					  and id <> @ultimoId
				);", new { fechamentoDisciplinaId, alunoCodigo, ultimoId });
		
		public Task AtualizarAnotacaoFechamentoAlunoDuplicados(long fechamentoDisciplinaId, string alunoCodigo, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update anotacao_fechamento_aluno
					set fechamento_aluno_id = @ultimoId
				where fechamento_aluno_id in (
					select id from fechamento_aluno 
					where fechamento_turma_disciplina_id = @fechamentoDisciplinaId
					  and aluno_codigo = @alunoCodigo
					  and id <> @ultimoId
				);", new { fechamentoDisciplinaId, alunoCodigo, ultimoId });
		
		public Task ExcluirFechamentoAlunoDuplicados(long fechamentoDisciplinaId, string alunoCodigo, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from fechamento_aluno 
				where fechamento_turma_disciplina_id = @fechamentoDisciplinaId
				  and aluno_codigo = @alunoCodigo
				  and id <> @ultimoId;", new { fechamentoDisciplinaId, alunoCodigo, ultimoId });
		
		public Task AtualizarHistoricoFechamentoNotaDuplicados(long fechamentoAlunoId, long disciplinaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update historico_nota_fechamento
					set fechamento_nota_id = @ultimoId
				where fechamento_nota_id in (
					select id from fechamento_nota 
					where fechamento_aluno_id = @fechamentoAlunoId
					  and disciplina_id = @disciplinaId
					  and id <> @ultimoId
				);", new { fechamentoAlunoId, disciplinaId, ultimoId });
		
		public Task AtualizarWfAprovacaoFechamentoNotaDuplicados(long fechamentoAlunoId, long disciplinaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update wf_aprovacao_nota_fechamento
					set fechamento_nota_id = @ultimoId
				where fechamento_nota_id in (
					select id from fechamento_nota 
					where fechamento_aluno_id = @fechamentoAlunoId
					  and disciplina_id = @disciplinaId
					  and id <> @ultimoId
				);", new { fechamentoAlunoId, disciplinaId, ultimoId });
		
		public Task ExcluirFechamentoNotaDuplicados(long fechamentoAlunoId, long disciplinaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from fechamento_nota 
				where fechamento_aluno_id = @fechamentoAlunoId
				  and disciplina_id = @disciplinaId
				  and id <> @ultimoId;", new { fechamentoAlunoId, disciplinaId, ultimoId });
		
		public Task AtualizarNotaConsolidacaoCCAlunoTurmaDuplicado(string alunoCodigo, long turmaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update consolidado_conselho_classe_aluno_turma_nota
					set consolidado_conselho_classe_aluno_turma_id = @ultimoId
					where consolidado_conselho_classe_aluno_turma_id in (
							select id from consolidado_conselho_classe_aluno_turma
							where aluno_codigo = @alunoCodigo
							and turma_id = @turmaId
								and id <> @ultimoId);", new { alunoCodigo, turmaId, ultimoId });
		
		public Task ExcluirConsolidacaoCCAlunoTurmaDuplicado(string alunoCodigo, long turmaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from consolidado_conselho_classe_aluno_turma
				where aluno_codigo = @alunoCodigo
						and turma_id = @turmaId
						  and id <> @ultimoId;", new { alunoCodigo, turmaId, ultimoId });
		
		public Task ExcluirConsolidacaoCCNotaDuplicado(long consolicacaoCCAlunoTurmaId, int bimestre, long componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from consolidado_conselho_classe_aluno_turma_nota  
				where consolidado_conselho_classe_aluno_turma_id = @consolicacaoCCAlunoTurmaId
				  and coalesce(bimestre, 0) = @bimestre
				  and componente_curricular_id = @componenteCurricularId
				  and id <> @ultimoId;", new { consolicacaoCCAlunoTurmaId, bimestre, componenteCurricularId, ultimoId });

		public Task ExcluirFrequenciaAlunoDuplicado(string turmaCodigo, string alunoCodigo, int bimestre, int tipo, string componenteCurricularId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from frequencia_aluno 
				where turma_id = @turmaCodigo
				  and codigo_aluno = @alunoCodigo
				  and bimestre = @bimestre
				  and tipo = @tipo
				  and coalesce(disciplina_id,'0') = @componenteCurricularId
				  and id <> @ultimoId;", new { turmaCodigo, alunoCodigo, bimestre, tipo, componenteCurricularId, ultimoId });

		public Task ExcluirRegistroFrequenciaDuplicado(long aulaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from registro_frequencia
				  where aula_id = @aulaId
					and id <> @ultimoId;", new { aulaId, ultimoId });

		public Task AtualizaAlunoRegistroFrequenciaDuplicado(long aulaId, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"update registro_frequencia_aluno
				set registro_frequencia_id = @ultimoId
				where registro_frequencia_id in (
					select id from registro_frequencia 
					where aula_id = @aulaId
					  and id <> @ultimoId);", new { aulaId, ultimoId });

		public async Task<bool> AtualizarCompensacoesRegistroFrequenciaAlunoDuplicado(long registroFrequenciaId, long aulaId, int numeroAula, string alunoCodigo, long ultimoId)
			=> await (database.Conexao.ExecuteAsync(
				@"update compensacao_ausencia_aluno_aula 
					set registro_frequencia_aluno_id = @ultimoId
				  where id in (
					select caaa.id from compensacao_ausencia_aluno_aula caaa 
					inner join registro_frequencia_aluno rfa on rfa.id = caaa.registro_frequencia_aluno_id 
					where rfa.registro_frequencia_id = @registroFrequenciaId
					and rfa.aula_id = @aulaId
					and rfa.numero_aula = @numeroAula
					and rfa.codigo_aluno = @alunoCodigo
					and rfa.id <> @ultimoId);", new { registroFrequenciaId, aulaId, numeroAula, alunoCodigo, ultimoId })) > 0;

		public Task AtualizarAusenciaRegistroFrequenciaAlunoDuplicado(long ultimoId)
			=> database.Conexao.ExecuteAsync(
				"update registro_frequencia_aluno set valor = 2 where valor <> 2 and id = @ultimoId", new { ultimoId });

		public Task ExcluirRegistroFrequenciaAlunoDuplicado(long registroFrequenciaId, long aulaId, int numeroAula, string alunoCodigo, long ultimoId)
			=> database.Conexao.ExecuteAsync(
				@"delete from registro_frequencia_aluno  
					where registro_frequencia_id = @registroFrequenciaId
					  and aula_id = @aulaId
					  and numero_aula = @numeroAula
					  and codigo_aluno = @alunoCodigo
					  and id <> @ultimoId;", new { registroFrequenciaId, aulaId, numeroAula, alunoCodigo, ultimoId });


        public Task<IEnumerable<ConsolidacaoFrequenciaAlunoMensalInconsistente>> ObterConsolidacaoFrequenciaAlunoMensalInconsistente(long turmaId)
        => database.Conexao.QueryAsync<ConsolidacaoFrequenciaAlunoMensalInconsistente>(
            @"with totalAulas as (
					select
						t.id as TurmaId,
						extract(month from a.data_aula) as Mes,
						sum(a.quantidade) as QuantidadeAulas
					from aula a
					INNER JOIN turma t ON t.turma_id = a.turma_id
					where
						not a.excluido
						and t.id = @turmaId
					group by t.id, extract(month from a.data_aula)
				), totalFrequencia as (
					select
						rfa.codigo_aluno as AlunoCodigo,
						extract(month from a.data_aula) as Mes,
						count(rfa.id) as QuantidadeAusencias,
						count(caaa.id) as QuantidadeCompensacoes
					from aula a
					inner join registro_frequencia_aluno rfa on rfa.aula_id = a.id and not rfa.excluido and rfa.valor = 2
                    inner join turma t on t.turma_id = a.turma_id 
					left join compensacao_ausencia_aluno_aula caaa on caaa.registro_frequencia_aluno_id = rfa.id and not caaa.excluido
					where
						not a.excluido
						and t.id = @turmaId
					group by rfa.codigo_aluno, extract(month from a.data_aula)
				)
 
				select
					cfam.turma_id as TurmaId,
					cfam.aluno_codigo as AlunoCodigo,
					cfam.mes,
					cfam.quantidade_aulas as QuantidadeAulas,
					cfam.quantidade_ausencias as QuantidadeAusencias,
					cfam.quantidade_compensacoes as QuantidadeCompensacoes,
					ta.QuantidadeAulas as QuantidadeAulasCalculado,
					tf.QuantidadeAusencias as QuantidadeAusenciasCalculado,
					tf.QuantidadeCompensacoes as QuantidadeCompensacoesCalculado
				from totalFrequencia tf
				inner join totalAulas ta on ta.mes = tf.mes
				inner join consolidacao_frequencia_aluno_mensal cfam on cfam.turma_id = ta.TurmaId and cfam.aluno_codigo = tf.AlunoCodigo and cfam.mes = ta.mes
				where
					cfam.quantidade_aulas <> ta.QuantidadeAulas or
					cfam.quantidade_ausencias <> tf.QuantidadeAusencias or
					cfam.quantidade_compensacoes <> tf.QuantidadeCompensacoes;", new { turmaId });
    }
}
