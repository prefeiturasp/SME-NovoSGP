-- Criação da tabela de frequência que serão recalculadas
CREATE TABLE public.frequencia_aluno_sem_periodo_escolar_bkp (
	id int8 NOT NULL,
	codigo_aluno varchar(100) NOT NULL,
	tipo int4 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	periodo_inicio timestamp NOT NULL,
	periodo_fim timestamp NOT NULL,
	bimestre int4 NOT NULL,
	total_aulas int4 NOT NULL,
	total_ausencias int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	total_compensacoes int4 NOT NULL DEFAULT 0,
	turma_id varchar(15) NULL,
	periodo_escolar_id int8 NULL,
	calculada bool NULL DEFAULT false
);

-- Inclusão dos registros
insert into frequencia_aluno_sem_periodo_escolar_bkp
select id,
	   codigo_aluno,
	   tipo,
	   disciplina_id,
	   periodo_inicio,
	   periodo_fim,
	   bimestre,
	   total_aulas,
	   total_ausencias,
	   criado_em,
	   criado_por,
	   alterado_em,
	   alterado_por,
	   criado_rf,
	   alterado_rf,
	   excluido,
	   migrado,
	   total_compensacoes,
	   turma_id,
	   periodo_escolar_id,
	   false calculada
	from frequencia_aluno
where periodo_escolar_id is null;

-- Exclusão dos registros inválidos
delete from frequencia_aluno where periodo_escolar_id is null;

-- Conatgem de registros a serem recalculados
select count(0)
	from frequencia_aluno_sem_periodo_escolar_bkp fab				
		left join frequencia_aluno fa
			on fab.codigo_aluno = fa.codigo_aluno and
			   fab.tipo = fa.tipo and
			   fab.disciplina_id = fa.disciplina_id and
			   fab.periodo_inicio  = fa.periodo_inicio and
			   fab.periodo_fim = fa.periodo_fim
where fa.id is null	 
	and fab.calculada = false
	and fab.turma_id is not null
	and exists (select 1
					from periodo_escolar pe 
				where pe.periodo_inicio = fab.periodo_inicio and
				 	  pe.periodo_fim  = fab.periodo_fim);

-- Registros a serem recalculados
select fab.*
	from frequencia_aluno_sem_periodo_escolar_bkp fab
		left join frequencia_aluno fa
			on fab.codigo_aluno = fa.codigo_aluno and
			   fab.tipo = fa.tipo and
			   fab.disciplina_id = fa.disciplina_id and
			   fab.periodo_inicio  = fa.periodo_inicio and
			   fab.periodo_fim = fa.periodo_fim
where fa.id is null
	and fab.turma_id is not null	
	and exists (select 1
					from periodo_escolar pe 
				where pe.periodo_inicio = fab.periodo_inicio and
				 	  pe.periodo_fim  = fab.periodo_fim)
order by fab.turma_id,
		 fab.codigo_aluno,		 
	     fab.tipo;