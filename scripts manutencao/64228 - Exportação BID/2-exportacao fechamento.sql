drop procedure if exists public.RELATORIO_BID_FECHAMENTO;
CREATE PROCEDURE public.RELATORIO_BID_FECHAMENTO(
	p_anoLetivo int,
	p_ueId int8
	)
language SQL
as $$
	with turmas as (
		select t.id, t.nome, t.ano, t.ano_letivo 
		  from turma t 
		 where t.ano_letivo = p_anoLetivo
		   and t.ano in ('1', '2', '3', '4', '5')
		   and t.modalidade_codigo = 5
		   and t.ue_id = p_ueId
	), fechamentos as (
	    select t.id as turma_id, t.nome as turma_nome, t.ano as turma_ano, t.ano_letivo
	    	, fn.disciplina_id as componente_curricular_id
	    	, fa.aluno_codigo 
	    	, fn.conceito_id as conceito_fechamento_id, fn.nota as nota_fechamento
	    	, fn.criado_em 
	    	, row_number() over(partition by t.id, fn.disciplina_id, fa.aluno_codigo 
	    					order by fn.criado_em desc) as ordem
	      from fechamento_turma ft
	     inner join turmas t on t.id = ft.turma_id 
	     inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
	     inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
	     inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
	     where ft.periodo_escolar_id is null
	       and not ft.excluido 
	       and fn.disciplina_id in (138, 2, 89)
	)
		
	insert into relatorio_bid (
			  turma_id
			, turma_nome
			, turma_ano
			, ano_letivo
			, componente_curricular_id
			, aluno_codigo
			, conceito_fechamento_id
			, nota_fechamento)
		select 
			  turma_id
			, turma_nome
			, turma_ano
			, ano_letivo
			, componente_curricular_id
			, aluno_codigo
			, conceito_fechamento_id
			, nota_fechamento
		from fechamentos 
		where ordem = 1
	on conflict (turma_id, ano_letivo, componente_curricular_id, aluno_codigo)
	do UPDATE SET 
	     conceito_fechamento_id = excluded.conceito_fechamento_id
	    ,nota_fechamento = excluded.nota_fechamento;

$$