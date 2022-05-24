drop procedure if exists public.RELATORIO_BID_FREQUENCIA;
CREATE PROCEDURE public.RELATORIO_BID_FREQUENCIA(
	p_anoLetivo int,
	p_ueId int8,
	p_bimestre int
	)
language SQL
as $$
	with turmas as (
		select t.id as turma_id, t.nome as turma_nome, t.ano as turma_ano, t.ano_letivo , t.turma_id as turma_codigo
		  from turma t 
		 where t.ano_letivo = p_anoLetivo
		   and t.ano in ('1', '2', '3', '4', '5')
		   and t.modalidade_codigo = 5
		   and t.ue_id = p_ueId
	), alunos as (
		select distinct fa.turma_id as turma_codigo, fa.codigo_aluno 
		  from frequencia_aluno fa 
		 inner join turmas t on t.turma_codigo = fa.turma_id 
		 where not fa.excluido 
	), frequencias as (
	    select distinct t.turma_id
	    	, fa.codigo_aluno 
	    	, fa.bimestre
	    	, fa.total_aulas
	    	, fa.total_ausencias
	    	, fa.total_compensacoes 
	    	, row_number() over (partition by t.turma_id, fa.codigo_aluno order by fa.id desc) ordem
	      from frequencia_aluno fa
	     inner join turmas t on t.turma_codigo = fa.turma_id 
	     inner join alunos a on a.turma_codigo = fa.turma_id and fa.codigo_aluno = a.codigo_aluno 
	     where not fa.excluido 
	       and fa.tipo = 2
	       and fa.bimestre = p_bimestre
	)
	
	insert into relatorio_bid_frequencia (
		  turma_id
		, turma_nome
		, turma_ano
		, ano_letivo
		, aluno_codigo 
		, bimestre
		, total_aulas
		, total_ausencias
		, total_compensacoes 
		)
		
	select t.turma_id
		, t.turma_nome
		, t.turma_ano
		, t.ano_letivo
		, al.codigo_aluno as aluno_codigo 
		, f.bimestre
	    , coalesce(f.total_aulas, 0) as total_aulas
    	, coalesce(f.total_ausencias, 0) as total_ausencias
    	, coalesce(f.total_compensacoes, 0) as total_compensacoes
	 from turmas t
	inner join alunos al on al.turma_codigo = t.turma_codigo
	inner join frequencias f on f.turma_id = t.turma_id and f.codigo_aluno = al.codigo_aluno and f.ordem = 1
	
	on conflict (turma_id, aluno_codigo, bimestre)
	do UPDATE SET 
	     total_aulas = excluded.total_aulas
	    ,total_ausencias = excluded.total_ausencias
	    ,total_compensacoes = excluded.total_compensacoes;
$$