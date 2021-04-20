DROP FUNCTION public.f_abrangencia_modalidades;

CREATE OR REPLACE FUNCTION public.f_abrangencia_modalidades(p_login character varying, p_perfil_id uuid, p_historico boolean, p_ano_letivo integer, p_ignorar_modalidades integer[] DEFAULT NULL::integer[])
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.modalidade_codigo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and      
	  act.turma_ano_letivo = p_ano_letivo
	  and (p_ignorar_modalidades is null or not(act.modalidade_codigo = ANY(p_ignorar_modalidades)))
	 
union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and      	  
	  act.turma_ano_letivo = p_ano_letivo
	  and (p_ignorar_modalidades is null or not(act.modalidade_codigo = ANY(p_ignorar_modalidades)))

union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and	        
	  act.turma_ano_letivo = p_ano_letivo
	 and (p_ignorar_modalidades is null or not(act.modalidade_codigo = ANY(p_ignorar_modalidades)));
$function$
;