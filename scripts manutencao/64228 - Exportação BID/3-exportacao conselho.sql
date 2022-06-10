drop procedure if exists public.RELATORIO_BID_CONSELHO;
CREATE PROCEDURE public.RELATORIO_BID_CONSELHO(
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
	), conselhos as (
	    select t.id as turma_id, t.nome as turma_nome, t.ano as turma_ano, t.ano_letivo
	    	, ccn.componente_curricular_codigo as componente_curricular_id
	    	, cca.aluno_codigo as aluno_codigo
	    	, ccn.conceito_id as conceito_conselho_id, ccn.nota as nota_conselho
	    	, ccn.criado_em 
	    	, row_number() over(partition by t.id, ccn.componente_curricular_codigo, cca.aluno_codigo 
	    					order by ccn.criado_em desc) as ordem
	      from fechamento_turma ft
	     inner join turmas t on t.id = ft.turma_id 
	     inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
	     inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
	     inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id 
	     where ft.periodo_escolar_id is null
	       and not ft.excluido 
	       and ccn.componente_curricular_codigo in (138, 2, 89)
	)
	
	insert into relatorio_bid (
			  turma_id
			, turma_nome
			, turma_ano
			, ano_letivo
			, componente_curricular_id
			, aluno_codigo
			, conceito_conselho_id
			, nota_conselho)
		select 
			  turma_id
			, turma_nome
			, turma_ano
			, ano_letivo
			, componente_curricular_id
			, aluno_codigo
			, conceito_conselho_id
			, nota_conselho
		from conselhos 
		where ordem = 1
	on conflict (turma_id, ano_letivo, componente_curricular_id, aluno_codigo)
	do UPDATE SET 
	     conceito_conselho_id = excluded.conceito_conselho_id
	    ,nota_conselho = excluded.nota_conselho;

$$