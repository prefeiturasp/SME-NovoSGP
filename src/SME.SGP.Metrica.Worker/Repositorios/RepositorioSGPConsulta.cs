using Nest;
using SME.SGP.Dados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios
{

    public class RepositorioSGPConsulta : IRepositorioSGPConsulta
    {
        private readonly ISgpContext database;

        public RepositorioSGPConsulta(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<IEnumerable<long>> ObterUesIds()
            => database.Conexao.QueryAsync<long>("select id from ue order by id");

        public Task<IEnumerable<long>> ObterTurmasIdsPorUE(long ueId, int? anoLetivo = null)
            => database.Conexao.QueryAsync<long>($"select id from turma where ano_letivo = {(anoLetivo.HasValue ? "@anoLetivo" : "extract(year from NOW())")} and ue_id = @ueId", new { ueId, anoLetivo });

        public Task<IEnumerable<long>> ObterTurmasIds(int[] modalidades)
            => database.Conexao.QueryAsync<long>("select id from turma where ano_letivo = extract(year from NOW()) and modalidade_codigo = ANY(@modalidades) order by id", new { modalidades });

        public Task<IEnumerable<ConselhoClasseAlunoDuplicado>> ObterConselhosClasseAlunoDuplicados(long ueId)
            => database.Conexao.QueryAsync<ConselhoClasseAlunoDuplicado>(
                @"select cca.conselho_classe_id as ConselhoClasseId
        	, cca.aluno_codigo as AlunoCodigo
        	, count(cca.id) as Quantidade
        	, min(cca.criado_em) as PrimeiroRegistro
        	, max(cca.criado_em) as UltimoRegistro
        	, max(cca.id) as UltimoId
          from conselho_classe_aluno cca
         inner join conselho_classe cc on cc.id = cca.conselho_classe_id  
         inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id  
         inner join turma t on t.id = ft.turma_id 
     	where t.ano_letivo = extract(year from NOW())
	      and t.ue_id = @ueId
        group by cca.conselho_classe_id 
        	, cca.aluno_codigo
        having count(cca.id) > 1", new { ueId });

        public Task<IEnumerable<ConselhoClasseDuplicado>> ObterConselhosClasseDuplicados()
            => database.Conexao.QueryAsync<ConselhoClasseDuplicado>(
                @"select cc.fechamento_turma_id as FechamentoTurmaId
        	        , count(cc.id) as Quantidade
        	        , min(cc.criado_em) as PrimeiroRegistro
        	        , max(cc.criado_em) as UltimoRegistro
        	        , max(cc.id) as UltimoId
                  from turma t
                 inner join fechamento_turma ft on ft.turma_id = t.id and not ft.excluido
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id and not cc.excluido 
                 where t.ano_letivo = extract(year from NOW())
                group by cc.fechamento_turma_id
                having count(cc.id) > 1");

        public Task<IEnumerable<ConselhoClasseNotaDuplicado>> ObterConselhosClasseNotaDuplicados()
            => database.Conexao.QueryAsync<ConselhoClasseNotaDuplicado>(
                @"select ccn.conselho_classe_aluno_id as ConselhoClasseAlunoId
        	        , ccn.componente_curricular_codigo as ComponenteCorricularId
        	        , count(ccn.id) as Quantidade
        	        , min(ccn.criado_em) as PrimeiroRegistro
        	        , max(ccn.criado_em) as UltimoRegistro
        	        , max(ccn.id) as UltimoId
                  from conselho_classe_nota ccn 
                 inner join conselho_classe_aluno cca on cca.id = ccn.conselho_classe_aluno_id 
                 inner join conselho_classe cc on cc.id = cca.conselho_classe_id 
                 inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id 
                 inner join turma t on t.id = ft.turma_id
                where t.ano_letivo = extract(year from NOW())
                group by ccn.conselho_classe_aluno_id
        	        , ccn.componente_curricular_codigo 
                having count(ccn.id) > 1");

        public Task<int> ObterQuantidadeAcessosDia(DateTime data)
            => database.Conexao.QueryFirstOrDefaultAsync<int>("select count(u.id) from usuario u where date(u.ultimo_login) = @data", new { data });

        public Task<IEnumerable<FechamentoTurmaDuplicado>> ObterFechamentosTurmaDuplicados()
            => database.Conexao.QueryAsync<FechamentoTurmaDuplicado>(
                @"select ft.turma_id as TurmaId
        	        , ft.periodo_escolar_id as PeriodoEscolarId
        	        , count(ft.id) as Quantidade
        	        , min(ft.criado_em) as PrimeiroRegistro
        	        , max(ft.criado_em) as UltimoRegistro
        	        , max(ft.id) as UltimoId
                  from turma t
                 inner join fechamento_turma ft on ft.turma_id = t.id
                 where not ft.excluido 
                   and t.ano_letivo = extract(year from NOW())
                group by ft.turma_id, ft.periodo_escolar_id
                having count(ft.id) > 1");

        public Task<IEnumerable<FechamentoTurmaDisciplinaDuplicado>> ObterFechamentosTurmaDisciplinaDuplicados()
            => database.Conexao.QueryAsync<FechamentoTurmaDisciplinaDuplicado>(
                @"select ftd.fechamento_turma_id as FechamentoTurmaId
        	        , ftd.disciplina_id as DisciplinaId
        	        , count(ftd.id) as Quantidade
        	        , min(ftd.criado_em) as PrimeiroRegistro
        	        , max(ftd.criado_em) as UltimoRegistro
        	        , max(ftd.id) as UltimoId
                  from turma t
                 inner join fechamento_turma ft on ft.turma_id = t.id and not ft.excluido
                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id and not ftd.excluido 
                 where t.ano_letivo = extract(year from NOW())
                group by ftd.fechamento_turma_id
        	        , ftd.disciplina_id
                having count(ftd.id) > 1");

        public Task<IEnumerable<FechamentoAlunoDuplicado>> ObterFechamentosAlunoDuplicados(long ueId)
            => database.Conexao.QueryAsync<FechamentoAlunoDuplicado>(
                @"select fa.fechamento_turma_disciplina_id
        	        , fa.aluno_codigo 
        	        , count(fa.id) as quantidade
        	        , min(fa.criado_em) as primeiro_registro
        	        , max(fa.criado_em) as ultimo_registro
        	        , max(fa.id) as ultimo_id
                  from fechamento_aluno fa
                 inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                 inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                 inner join turma t on t.id = ft.turma_id 
     	        where t.ano_letivo = extract(year from NOW())
	              and t.ue_id = @ueId
                group by fa.fechamento_turma_disciplina_id
        	        , fa.aluno_codigo
                having count(fa.id) > 1", new { ueId });

        public Task<IEnumerable<FechamentoNotaDuplicado>> ObterFechamentosNotaDuplicados(long turmaId)
            => database.Conexao.QueryAsync<FechamentoNotaDuplicado>(
                @"select fn.fechamento_aluno_id as FechamentoAlunoId
		            , fn.disciplina_id as DisciplinaId
		            , count(fn.id) as Quantidade
		            , min(fn.criado_em) as PrimeiroRegistro
		            , max(fn.criado_em) as UltimoRegistro
		            , max(fn.id) as UltimoId 
		            from fechamento_nota fn
		            inner join fechamento_aluno fa on fa.id = fn.fechamento_aluno_id 
		            inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
		            inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
	            WHERE ft.id = @turmaid
	            group by fn.fechamento_aluno_id
		            , fn.disciplina_id 
	            having count(fn.id) > 1", new { turmaId });

        public Task<IEnumerable<ConsolidacaoConselhoClasseNotaNulos>> ObterConsolidacaoCCNotasNulos()
            => database.Conexao.QueryAsync<ConsolidacaoConselhoClasseNotaNulos>(
				@"select cccatn.id as Id, t.turma_id as TurmaCodigo, cccatn.bimestre, cccat.aluno_codigo as AlunoCodigo, cccatn.componente_curricular_id as ComponenteCurricularId
			        from consolidado_conselho_classe_aluno_turma_nota cccatn 
			       inner join consolidado_conselho_classe_aluno_turma cccat on cccat.id = cccatn.consolidado_conselho_classe_aluno_turma_id  
			       inner join turma t on t.id = cccat.turma_id  
			       where cccatn.nota is null and cccatn.conceito_id is null");

        public Task<IEnumerable<ConsolidacaoConselhoClasseAlunoTurmaDuplicado>> ObterConsolidacaoCCAlunoTurmaDuplicados(long ueId)
            => database.Conexao.QueryAsync<ConsolidacaoConselhoClasseAlunoTurmaDuplicado>(
                @"select cccat.aluno_codigo as AlunoCodigo, 
                        cccat.turma_id as TurmaId,
					    count(cccat.id) as Quantidade,
					    min(cccat.criado_em) as PrimeiroRegistro,
					    max(cccat.criado_em) as UltimoRegistro,
					    max(cccat.id) as UltimoId 
				 from consolidado_conselho_classe_aluno_turma cccat
				 join turma t on t.id = cccat.turma_id 
			    where t.ue_id = @ueId
			    and t.ano_letivo = extract(year from NOW())
			    and not cccat.excluido
			    group by cccat.aluno_codigo, cccat.turma_id 
			    having count(cccat.id) > 1", new { ueId });

        public Task<IEnumerable<ConsolidacaoCCNotaDuplicado>> ObterConsolidacaoCCNotasDuplicados()
            => database.Conexao.QueryAsync<ConsolidacaoCCNotaDuplicado>(
                @"select cccatn.consolidado_conselho_classe_aluno_turma_id as ConsolicacaoCCAlunoTurmaId, 
					    coalesce(cccatn.bimestre, 0) as Bimestre,
					    cccatn.componente_curricular_id as ComponenteCurricularId,
					    cccat.turma_id as TurmaId,
					    count(cccatn.id) as Quantidade,
					    min(cccat.criado_em) as PrimeiroRegistro,
					    max(cccat.criado_em) as UltimoRegistro,
					    max(cccatn.id) as UltimoId 
				 from consolidado_conselho_classe_aluno_turma_nota cccatn
				 join consolidado_conselho_classe_aluno_turma cccat on cccat.id = cccatn.consolidado_conselho_classe_aluno_turma_id 
				 join turma t on t.id = cccat.turma_id 		
			    where not cccat.excluido
			    group by cccatn.consolidado_conselho_classe_aluno_turma_id,
			        cccat.turma_id,
					coalesce(cccatn.bimestre, 0),
					cccatn.componente_curricular_id
			    having count(cccatn.id) > 1");

        public Task<IEnumerable<ConselhoClasseNaoConsolidado>> ObterConselhosClasseNaoConsolidados(long ueId)
            => database.Conexao.QueryAsync<ConselhoClasseNaoConsolidado>(
                @"select fa.aluno_codigo as AlunoCodigo,
	                p.bimestre ,
	                t.id as TurmaId,
	                t.turma_id as TurmaCodigo,
	                u.id as UeId,
	                u.ue_id as UeCodigo, 
	                cc1.id as ComponenteCurricularId,
	                cc1.descricao as ComponenteCurricular,
	                fn.criado_em as CriadoEm, 
	                fn.alterado_em as AlteradoEm, 
	                true as EhFechamento,
	                false as EhConselho,
	                fn.criado_rf as CriadoRF
	            from fechamento_turma ft
	            inner join conselho_classe cc on cc.fechamento_turma_id = ft.id and not cc.excluido
	            inner join turma t on t.id = ft.turma_id and t.tipo_turma = 1
	            inner join ue u on u.id = t.ue_id
	            inner join fechamento_turma_disciplina f on f.fechamento_turma_id = ft.id and not f.excluido
	            inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = f.id and not fa.excluido
	            inner join conselho_classe_aluno cca on cca.conselho_classe_id = cc.id and not cca.excluido and fa.aluno_codigo = cca.aluno_codigo
	            inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id and not fn.excluido
	            inner join componente_curricular cc1 on cc1.id = fn.disciplina_id and cc1.permite_lancamento_nota
	            left join periodo_escolar p on p.id = ft.periodo_escolar_id
	            left join consolidado_conselho_classe_aluno_turma cccat on cccat.aluno_codigo = fa.aluno_codigo and cccat.turma_id = t.id
	            left join consolidado_conselho_classe_aluno_turma_nota cccatn on cccatn.consolidado_conselho_classe_aluno_turma_id = cccat.id
	                                                                            and coalesce(cccatn.bimestre, 0) = coalesce(p.bimestre, 0)
	                                                                            and cccatn.componente_curricular_id = cc1.id
	            where
	                not cccat.excluido
	                and not ft.excluido    
	                and cccatn.id is null
	                and t.ano_letivo = extract(year from NOW())
	                and u.id = @ueId

                union all

	            select cca.aluno_codigo as AlunoCodigo,
	                p.bimestre,
	                t.id as TurmaId,
	                t.turma_id as TurmaCodigo,
	                u.id as UeId, 
	                u.ue_id as UeCodigo, 
	                cc1.id as ComponenteCurricularId,
	                cc1.descricao as ComponenteCurricular,
	                ccn.criado_em as CriadoEm, 
	                ccn.alterado_em as AlteradoEm,
	                false as EhFechamento,
	                true as EhConselho,
	                ccn.criado_rf as CriadoRF
	            from fechamento_turma ft     
	            inner join conselho_classe cc on cc.fechamento_turma_id = ft.id and not cc.excluido
	            inner join turma t on t.id = ft.turma_id and t.tipo_turma = 1
	            inner join ue u on u.id = t.ue_id
	            inner join conselho_classe_aluno cca on cca.conselho_classe_id = cc.id
	            inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id
	            inner join componente_curricular cc1 on cc1.id = ccn.componente_curricular_codigo and cc1.permite_lancamento_nota
	            left join periodo_escolar p on p.id = ft.periodo_escolar_id
	            left join consolidado_conselho_classe_aluno_turma cccat on cccat.aluno_codigo = cca.aluno_codigo and cccat.turma_id = t.id
	            left join consolidado_conselho_classe_aluno_turma_nota cccatn on cccatn.consolidado_conselho_classe_aluno_turma_id = cccat.id
	                                                                            and coalesce(cccatn.bimestre, 0) = coalesce(p.bimestre, 0)
	                                                                            and cccatn.componente_curricular_id = cc1.id
	            where
	                not cccat.excluido
	                and not ft.excluido    
	                and cccatn.id is null
	                and t.ano_letivo = extract(year from NOW())
	                and u.id = @ueId", new { ueId });

        public Task<IEnumerable<FrequenciaAlunoInconsistente>> ObterFrequenciaAlunoInconsistente(long turmaId)
            => database.Conexao.QueryAsync<FrequenciaAlunoInconsistente>(
				@"with consolidado as (
				select
						f.codigo_aluno,
						f.periodo_escolar_id,
						f.turma_id,
						f.disciplina_id, 
						sum(f.total_aulas) total_aulas, 
						sum(f.total_ausencias) total_ausencias, 
						sum(f.total_presencas) total_presencas, 
						sum(f.total_remotos) total_remotos
					from
						frequencia_aluno f
					inner join turma t 
						on t.turma_id = f.turma_id
					where
						not f.excluido
						and f.tipo = 1
						and t.id = @turmaId
					group by
						f.codigo_aluno,
						f.periodo_escolar_id,
						f.turma_id,
						f.disciplina_id
				), frequencia as (
				select
					a.turma_id, 
					a.disciplina_id,
					rfa.codigo_aluno,
					pe.id as periodo_escolar_id,
					count(distinct(rfa.aula_id * rfa.numero_aula)) total_aulas,
					count(distinct(rfa.aula_id * rfa.numero_aula)) filter (where rfa.valor = 2) total_ausencias,
					count(distinct(rfa.aula_id * rfa.numero_aula)) filter (where rfa.valor = 1) total_presencas,
					count(distinct(rfa.aula_id * rfa.numero_aula)) filter (where rfa.valor = 3) total_remoto
				from
					registro_frequencia_aluno rfa
				inner join aula a on
					a.id = rfa.aula_id
				inner join periodo_escolar pe on
					pe.tipo_calendario_id = a.tipo_calendario_id
				inner join turma t on 
					t.turma_id = a.disciplina_id
				where
					not rfa.excluido
					and not a.excluido
					and pe.periodo_inicio <= a.data_aula
					and pe.periodo_fim >= a.data_aula
					and t.id = @turmaId
				group by
					rfa.codigo_aluno,
					pe.bimestre,
					a.turma_id,
					pe.id,
					a.disciplina_id
				)

		select f.turma_id as TurmaCodigo, f.disciplina_id as ComponenteCurricularId
			, f.codigo_aluno as AlunoCodigo, f.periodo_escolar_id as PeriodoEscolarId
			, f.total_aulas as TotalAulas
			, f.total_ausencias as TotalAusencias
			, f.total_presencas as TotalPresencas
			, f.total_remoto as TotalRemotos
			, c.total_aulas as TotalAulasCalculado
			, c.total_ausencias as TotalAusenciasCalculado
			, c.total_presencas as TotalPresencasCalculado
			, c.total_remotos as TotalRemotosCalculado
		from frequencia f
		inner join consolidado c 
			on c.turma_id = f.turma_id 
			and c.codigo_aluno = f.codigo_aluno 
			and c.disciplina_id = f.disciplina_id
			and c.periodo_escolar_id = f.periodo_escolar_id
		where 
			c.total_ausencias <> f.total_ausencias
			or c.total_presencas <> f.total_presencas
			or c.total_remotos <> f.total_remoto
			or c.total_aulas <> f.total_aulas
			or f.total_presencas > f.total_aulas;", new { turmaId });

        public Task<IEnumerable<FrequenciaAlunoDuplicado>> ObterFrequenciaAlunoDuplicados(long ueId)
			=> database.Conexao.QueryAsync<FrequenciaAlunoDuplicado>(
				@"select fa.turma_id as TurmaCodigo, fa.codigo_aluno as AlunoCodigo, 
						fa.bimestre, fa.disciplina_id as ComponenteCurricularId, 
						fa.tipo, t.ue_id as UeId,
						count(fa.id) Quantidade, 
						min(fa.criado_em) as PrimeiroRegistro,
						max(fa.criado_em) as UltimoRegistro,
						min(fa.id) as PrimeiroId,
						max(fa.id) as UltimoId
				from frequencia_aluno fa 
				inner join turma t on t.turma_id = fa.turma_id 
				where fa.periodo_inicio >= DATE_TRUNC('year', current_date) 
					and (fa.criado_em >= DATE_TRUNC('year', current_date)  or fa.alterado_em >= DATE_TRUNC('year', current_date) )
					and t.ue_id = @ueId
				group by fa.turma_id, fa.codigo_aluno, fa.bimestre, fa.tipo, fa.disciplina_id, t.ue_id
				having count(fa.id) > 1
				order by t.ue_id, fa.turma_id, fa.codigo_aluno, fa.bimestre, fa.tipo, fa.disciplina_id", new { ueId });

		public Task<IEnumerable<RegistroFrequenciaDuplicado>> ObterRegistroFrequenciaDuplicados(long ueId)
			=> database.Conexao.QueryAsync<RegistroFrequenciaDuplicado>(
				@"select rf.aula_id as AulaId, 				
						count(rf.id) as Quantidade,
						min(rf.criado_em) as PrimeiroRegistro,
						max(rf.criado_em) as UltimoRegistro,
						max(rf.id) as UltimoId 
				from registro_frequencia rf 
				join aula a on a.id = rf.aula_id  
				join turma t on t.turma_id = a.turma_id 
				where t.ue_id = @ueId 
				  and t.ano_letivo = extract(year from NOW())
				group by rf.aula_id
				having count(rf.id) > 1", new { ueId });

        public Task<IEnumerable<RegistroFrequenciaAlunoDuplicado>> ObterRegistroFrequenciaAlunoDuplicados(long turmaId)
			=> database.Conexao.QueryAsync<RegistroFrequenciaAlunoDuplicado>(
				@"select rfa.registro_frequencia_id as RegistroFrequenciaId, 
					rfa.aula_id as AulaId,
					rfa.numero_aula as NumeroAula,
					rfa.codigo_aluno as AlunoCodigo,
					count(rfa.id) as Quantidade,
					min(rfa.criado_em) as PrimeiroRegistro,
					max(rfa.criado_em) as UltimoRegistro,
					max(rfa.id) as UltimoId
				from registro_frequencia_aluno rfa 
				join aula a on a.id = rfa.aula_id
				join turma t on t.turma_id = a.turma_id
				where t.id = @turmaId
				group by rfa.registro_frequencia_id,
						 rfa.aula_id,
 						 rfa.numero_aula,
						 rfa.codigo_aluno
				having count(rfa.id) > 1", new { turmaId });

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

		public Task<IEnumerable<DiarioBordoDuplicado>> ObterDiariosBordoDuplicados()
			=> database.Conexao.QueryAsync<DiarioBordoDuplicado>(
				@"select db.aula_id as AulaId
        			, db.componente_curricular_id as ComponenteCurricularId
        			, count(db.id) as Quantidade
        			, min(db.id) as PrimeiroId
        			, max(db.id) as UltimoId
        			, min(db.criado_em) as PrimeiroRegistro
        			, max(db.criado_em) as UltimoRegistro
				  from turma t 
				 inner join diario_bordo db on db.turma_id = t.id and not db.excluido 
				 where t.ano_letivo = extract(year from NOW())
				group by db.aula_id
        			, db.componente_curricular_id 
				having count(db.id) > 1 ");

        public Task<int> ObterQuantidadeRegistrosFrequenciaDia(DateTime data)
			=> database.Conexao.QueryFirstOrDefaultAsync<int>(@"select count(rf.id) from registro_frequencia rf
                                                                 where not rf.excluido 
	                                                                   and rf.criado_em between @primeiraHoraDia and @ultimaHoraDia; ",
                                                                new { primeiraHoraDia = data.PrimeiraHoraDia(), ultimaHoraDia = data.UltimaHoraDia() });

        public Task<int> ObterQuantidadeDiariosBordoDia(DateTime data)
        => database.Conexao.QueryFirstOrDefaultAsync<int>(@"select count(db.id) from diario_bordo db
                                                             where not db.excluido
	                                                         and db.criado_em between @primeiraHoraDia and @ultimaHoraDia; ", 
														new { primeiraHoraDia = data.PrimeiraHoraDia(), ultimaHoraDia = data.UltimaHoraDia() });

        public Task<int> ObterQuantidadeDevolutivasDiarioBordoMes(DateTime data)
        => database.Conexao.QueryFirstOrDefaultAsync<int>(@"select count(d.id) from devolutiva d  
                                                                    where not d.excluido 
	                                                                      and d.criado_em between @primeiroDiaMes and @ultimoDiaMes;",
																		  new { primeiroDiaMes = data.PrimeiroDiaMes(), ultimoDiaMes = data.UltimoDiaMes() });

        public Task<int> ObterQuantidadeAulasCJMes(DateTime data)
        => database.Conexao.QueryFirstOrDefaultAsync<int>(@"select count(a.id) from aula a
                                                            where not a.excluido
                                                                  and a.aula_cj
                                                                  and a.criado_em between @primeiroDiaMes and @ultimoDiaMes;",
                                                                          new { primeiroDiaMes = data.PrimeiroDiaMes(), ultimoDiaMes = data.UltimoDiaMes() });

        public Task<int> ObterQuantidadePlanosAulaDia(DateTime data)
        => database.Conexao.QueryFirstOrDefaultAsync<int>(@"select count(pa.id) from plano_aula pa
                                                            where not pa.excluido
                                                                  and pa.criado_em between @primeiraHoraDia and @ultimaHoraDia; ",
                                                            new { primeiraHoraDia = data.PrimeiraHoraDia(), ultimaHoraDia = data.UltimaHoraDia() });

        public Task<int> ObterQuantidadeEncaminhamentosAEEMes(DateTime data)
        => database.Conexao.QueryFirstOrDefaultAsync<int>(@"select count(ea.id) from encaminhamento_aee ea
                                                            where not ea.excluido
                                                                  and ea.criado_em between @primeiroDiaMes and @ultimoDiaMes;",
                                                            new { primeiroDiaMes = data.PrimeiroDiaMes(), ultimoDiaMes = data.UltimoDiaMes() });

        public Task<int> ObterQuantidadePlanosAEEMes(DateTime data)
        => database.Conexao.QueryFirstOrDefaultAsync<int>(@"select count(pa.id) from plano_aee pa
                                                            where not pa.excluido
                                                                  and pa.criado_em between @primeiroDiaMes and @ultimoDiaMes;",
                                                            new { primeiroDiaMes = data.PrimeiroDiaMes(), ultimoDiaMes = data.UltimoDiaMes() });

        public Task<IEnumerable<(int Bimestre, int Quantidade)>> ObterQuantidadeFechamentosNotaDia(DateTime data)
        => database.Conexao.QueryAsync<(int, int)>(@"select coalesce(pe.bimestre, 0) as bimestre, count(fn.id) as quantidade from fechamento_nota fn
                                                            inner join fechamento_aluno fa on fa.id = fn.fechamento_aluno_id 
                                                            inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                                                            inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                                                            left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                                                            where not fn.excluido and not fa.excluido and not ftd.excluido and not ft.excluido
                                                                  and fn.criado_em between @primeiraHoraDia and @ultimaHoraDia
                                                            group by coalesce(pe.bimestre, 0);",
                                                            new { primeiraHoraDia = data.PrimeiraHoraDia(), ultimaHoraDia = data.UltimaHoraDia() });

        public Task<IEnumerable<(int Bimestre, int Quantidade)>> ObterQuantidadeConselhosClasseAlunoDia(DateTime data)
        => database.Conexao.QueryAsync<(int, int)>(@"select coalesce(pe.bimestre, 0) as bimestre, count(cca.id) as quantidade from conselho_classe_aluno cca 
                                                     inner join conselho_classe cc on cc.id = cca.conselho_classe_id 
                                                     inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id 
                                                     left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                                                     where not cca.excluido and not cc.excluido and not ft.excluido
                                                           and cca.criado_em between @primeiraHoraDia and @ultimaHoraDia
                                                     group by coalesce(pe.bimestre, 0);",
                                                     new { primeiraHoraDia = data.PrimeiraHoraDia(), ultimaHoraDia = data.UltimaHoraDia() });

        public Task<IEnumerable<(int Bimestre, int Quantidade)>> ObterQuantidadeFechamentosTurmaDisciplinaDia(DateTime data)
        => database.Conexao.QueryAsync<(int, int)>(@"select coalesce(pe.bimestre, 0) as bimestre, count(ftd.id) as quantidade from fechamento_turma_disciplina ftd 
                                                                     inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                                                                     left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                                                                     left join componente_curricular cc on cc.id = ftd.disciplina_id 
                                                                     where not ftd.excluido and not ft.excluido
                                                                           and ftd.criado_em between @primeiraHoraDia and @ultimaHoraDia
                                                                     group by coalesce(pe.bimestre, 0);",
                                                     new { primeiraHoraDia = data.PrimeiraHoraDia(), ultimaHoraDia = data.UltimaHoraDia() });

        public Task<IEnumerable<string>> ObterUesCodigo()
        => database.Conexao.QueryAsync<string>("select ue_id from ue order by id");

        public Task<IEnumerable<string>> ObterTurmasCodigoPorUE(string ueCodigo, int? anoLetivo = null)
            => database.Conexao.QueryAsync<string>($@"select turma_id from turma 
                                                      inner join ue on ue.id = turma.ue_id
                                                    where ano_letivo = {(anoLetivo.HasValue ? "@anoLetivo" : "extract(year from NOW())")} and ue.ue_id = @ueCodigo", new { ueCodigo, anoLetivo });

		public async Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo)
		=> (await database.Conexao.QueryAsync<Turma, Ue, Dre, Turma>(@"select t.id TurmaId,
                        t.id,
                        t.turma_id CodigoTurma,
                        t.ue_id,
                        t.nome,
                        t.ano,
                        t.ano_letivo AnoLetivo,
                        t.modalidade_codigo ModalidadeCodigo,
                        t.semestre,
                        t.qt_duracao_aula QuantidadeDuracaoAula,
                        t.tipo_turno TipoTurno,
                        t.data_atualizacao DataAtualizacao,
                        t.tipo_turma TipoTurma,
                        t.data_inicio DataInicio,
                        t.historica,
                        u.id as UeId,
                        u.id,
                        u.ue_id CodigoUe,
                        u.nome,
                        u.dre_id,
                        u.tipo_escola TipoEscola,
                        u.data_atualizacao DataAtualizacao,
                        d.id as DreId,
                        d.id,
                        d.nome,
                        d.dre_id,
                        d.abreviacao,
                        d.data_atualizacao DataAtualizacao
                    from
                        turma t
                    inner join ue u on
                        t.ue_id = u.id
                    inner join dre d on
                        u.dre_id = d.id
                    where
                        turma_id = @turmaCodigo", (turma, ue, dre) =>
			{
				ue.AdicionarDre(dre);
				turma.AdicionarUe(ue);
				return turma;
			}, new { turmaCodigo }, splitOn: "TurmaId, UeId, DreId")).FirstOrDefault();

        public Task<bool> ComponenteCurriculareEhRegencia(long id)
        => database.Conexao.QueryFirstOrDefaultAsync<bool>($@"select eh_regencia from componente_curricular WHERE id = @id;", new { id });

        public async Task<Grade> ObterGradeTurmaAno(TipoEscola tipoEscola, Modalidade modalidade, int duracao, int ano, string anoLetivo)
        {
            string query = @"select f.id as FiltroId, g.Id as GradeId, g.*
                  from grade_filtro f
                 inner join grade g on g.id = f.grade_id ";

            if (ano > 0)
                query += " inner join grade_disciplina gd on g.id = gd.grade_id ";

            query += @" where f.tipo_escola = @tipoEscola
                   and f.modalidade = @modalidade
                   and f.duracao_turno = @duracao";

            if (ano > 0)
                query += " and gd.ano = @ano ";

            if (modalidade == Modalidade.Medio)
                query += " and to_char(g.inicio_vigencia, 'YYYY') <= @anoLetivo and (to_char(g.fim_vigencia, 'YYYY') >= @anoLetivo or g.fim_vigencia is null) ";


            var filtro = await database.Conexao.QueryAsync<GradeFiltro, Grade, Grade>(query,
            (gradeFiltro, grade) =>
            {
                return grade;
            }, new
            {
                tipoEscola,
                modalidade,
                duracao,
                ano,
                anoLetivo
            }, splitOn: "FiltroId, GradeId");

            return filtro.FirstOrDefault();
        }

        public async Task<int> ObterHorasComponente(long gradeId, long[] componentesCurriculares, int ano)
        {
            var query = @"select gd.quantidade_aulas
                      from grade_disciplina gd
                     where gd.grade_id = @gradeId
                       and gd.componente_curricular_id = any(@componentesCurriculares)
                       and gd.ano = @ano";

            var consulta = await database.Conexao.QueryAsync<int>(query, new
            {
                gradeId,
                componentesCurriculares,
                ano
            });

            return consulta.Any() ? consulta.Single() : 0;
        }

        public async Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(string turma, DateTime dataAula, bool consideraSomenteAulasComRegistroFrequencia = true)
        {
            var query = $@"select sum(quantidade) from aula
                           left join componente_curricular cc on cc.id = aula.disciplina_id::int8
                           where not excluido
                                 and turma_id = @turma
                                 and (cc.eh_territorio or cc.id is null)
                                 and date(data_aula) = @dataAula
                               {(consideraSomenteAulasComRegistroFrequencia ?
                                  @" and ((coalesce(cc.permite_registro_frequencia, true) 
                                           and exists(select rf.id from registro_frequencia rf where rf.aula_id = aula.id and not rf.excluido))
                                          or not coalesce(cc.permite_registro_frequencia, true)                                          
                                          ) " : string.Empty)}";
            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new
            {
                turma,
                dataAula = dataAula.Date
            }) ?? 0;
            return qtd;
        }

        public async Task<int> ObterQuantidadeAulasTurmaComponenteCurricularDia(string turma, string componenteCurricular, DateTime dataAula, bool consideraSomenteAulasComRegistroFrequencia = true)
        {
            var query = $@"select sum(quantidade) from aula
                          left join componente_curricular cc on cc.id = aula.disciplina_id::int8
                          where not excluido and tipo_aula = @aulaNomal 
                                and turma_id = @turma 
                                and disciplina_id = @componenteCurricular 
                                and date(data_aula) = @dataAula 
                               {(consideraSomenteAulasComRegistroFrequencia ?
                                  @" and ((coalesce(cc.permite_registro_frequencia, true) 
                                           and exists(select rf.id from registro_frequencia rf where rf.aula_id = aula.id and not rf.excluido))
                                          or not coalesce(cc.permite_registro_frequencia, true)                                          
                                          ) " : string.Empty)}";
            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query.ToString(), new
            {
                turma,
                componenteCurricular,
                dataAula = dataAula.Date,
                aulaNomal = TipoAula.Normal
            }) ?? 0;

            return qtd;
        }

        public async Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(string turma, int semana, string componenteCurricular, bool consideraSomenteAulasComRegistroFrequencia = true)
        {
            var query = $@"select sum(quantidade) from aula
                           left join componente_curricular cc on cc.id = aula.disciplina_id::int8
                           where not excluido
                                 and turma_id = @turma
                                 and disciplina_id = @componenteCurricular
                                 and extract('week' from data_aula) = @semana
                               {(consideraSomenteAulasComRegistroFrequencia ?
                                  @" and ((coalesce(cc.permite_registro_frequencia, true) 
                                           and exists(select rf.id from registro_frequencia rf where rf.aula_id = aula.id and not rf.excluido))
                                          or not coalesce(cc.permite_registro_frequencia, true)                                          
                                          ) " : string.Empty)}";
            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new
            {
                turma,
                semana,
                componenteCurricular
            }) ?? 0;

            return qtd;
        }

        public async Task<int> ObterQuantidadeAulasTurmaDisciplinaSemana(string turma, string componenteCurricular, int semana, bool consideraSomenteAulasComRegistroFrequencia = true)
        {
            var query = $@"select sum(quantidade) from aula
                          left join componente_curricular cc on cc.id = aula.disciplina_id::int8
                          where not excluido and tipo_aula = @aulaNomal 
                                and turma_id = @turma 
                                and disciplina_id = @componenteCurricular 
                                and extract('week' from data_aula::date + 1) = @semana
                                {(consideraSomenteAulasComRegistroFrequencia ?
                                  @" and ((coalesce(cc.permite_registro_frequencia, true) 
                                           and exists(select rf.id from registro_frequencia rf where rf.aula_id = aula.id and not rf.excluido))
                                          or not coalesce(cc.permite_registro_frequencia, true)                                          
                                          ) " : string.Empty)}";
            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query.ToString(), new
            {
                turma,
                componenteCurricular,
                semana,
                aulaNomal = TipoAula.Normal
            }) ?? 0;

            return qtd;
        }

        public Task<long> ObterTipoCalendarioId(int anoLetivo, int modalidadeTipoCalendario)
        => database.Conexao.QueryFirstOrDefaultAsync<long>($@"select id
                                                              from tipo_calendario 
                                                              where not excluido
                                                              and ano_letivo = @anoLetivo
                                                              and modalidade = @modalidadeTipoCalendario", new { anoLetivo, modalidadeTipoCalendario });

        public Task<PeriodoIdDto> ObterPeriodoEscolarPorTipoCalendarioData(long tipoCalendarioId, DateTime dataParaVerificar)
        => database.Conexao.QueryFirstOrDefaultAsync<PeriodoIdDto>($@"select pe.id, pe.periodo_inicio as Inicio, pe.periodo_fim as Fim
                                                                    from periodo_escolar pe
                                                                    join tipo_calendario tc on tc.id = pe.tipo_calendario_id
                                                                    where tc.id = @tipoCalendarioId
                                                                          and @dataParaVerificar between symmetric pe.periodo_inicio::date and pe.periodo_fim::date;", new { tipoCalendarioId, dataParaVerificar });

        public Task<PeriodoIdDto> ObterPeriodoFechamentoPorPeriodoEscolar(long periodoEscolarId)
        => database.Conexao.QueryFirstOrDefaultAsync<PeriodoIdDto>($@"select pfb.id, pfb.inicio_fechamento as Inicio, pfb.final_fechamento as Fim  
                                                                    from periodo_fechamento_bimestre pfb 
                                                                    inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id 
                                                                    where pe.id = @periodoEscolarId;", new { periodoEscolarId });

        public Task<IEnumerable<DevolutivaDuplicado>> ObterDevolutivaDuplicados()
			=> database.Conexao.QueryAsync<DevolutivaDuplicado>(
                @"select count(id) Quantidade, descricao, componente_curricular_codigo as ComponenteCurricularId, min(criado_em) as PrimeiroRegistro, 
						max(criado_em) as UltimoRegistro, min(id) as PrimeiroId, max(id) as UltimoId 
						from devolutiva d1
						where exists (select 1 from devolutiva d2 
									  where d1.id <> d2.id 
										and d1.descricao = d2.descricao 
										and d1.componente_curricular_codigo = d2.componente_curricular_codigo  
										and d1.periodo_inicio = d2.periodo_inicio
										and d1.periodo_fim = d2.periodo_fim)       
						group by descricao, componente_curricular_codigo");

        public Task<IEnumerable<DevolutivaMaisDeUmaNoDiario>> ObterDevolutivaMaisDeUmaNoDiario()
            => database.Conexao.QueryAsync<DevolutivaMaisDeUmaNoDiario>(
                @"select count(db.id) as Quantidade, devolutiva_id as DevolutivaId
			      from diario_bordo db inner join
                  devolutiva d on d.id = db.devolutiva_id 
                  group by devolutiva_id                    
                  having count(db.id) > 1");

        public Task<IEnumerable<DevolutivaSemDiario>> ObterDevolutivaSemDiario()
            => database.Conexao.QueryAsync<DevolutivaSemDiario>(
                @"select d.id as DevolutivaId 
                  from devolutiva d  
                  where not exists(select 1 from diario_bordo db where db.devolutiva_id = d.id)");
    }

    internal static class DateTimeExtension
    {
        static public DateTime PrimeiroDiaMes(this DateTime data, int hour = 0, int minute = 0, int second = 0)
        => new DateTime(data.Year, data.Month, 1, hour, minute, second);

        static public DateTime UltimoDiaMes(this DateTime data)
        => data.PrimeiroDiaMes(23, 59, 59).AddMonths(1).AddDays(-1);

        static public DateTime PrimeiraHoraDia(this DateTime data)
        => new DateTime(data.Year, data.Month, data.Day, 0, 0, 0);

        static public DateTime UltimaHoraDia(this DateTime data)
        => new DateTime(data.Year, data.Month, data.Day, 23, 59, 59);
    }
}
