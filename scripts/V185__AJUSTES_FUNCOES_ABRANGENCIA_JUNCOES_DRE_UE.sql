create or REPLACE FUNCTION public.f_abrangencia_anos_letivos(character varying, uuid, boolean)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3
	 
union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3

union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3

union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = cast(act.dre_codigo as int8) and
			   a.ue_id = cast(act.ue_codigo as int8) and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3;
$function$;

create or REPLACE FUNCTION public.f_abrangencia_dres(character varying, uuid, boolean, integer DEFAULT 0, integer DEFAULT 0, integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_dres
 LANGUAGE sql
AS $function$
select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  a.historico = $3 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6))
	 
union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  a.historico = $3 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6))

union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  a.historico = $3 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6))

union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = cast(act.dre_codigo as int8) and
			   a.ue_id = cast(act.ue_codigo as int8) and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  a.historico = $3 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6));
$function$;

create or REPLACE FUNCTION public.f_abrangencia_modalidades(character varying, uuid, boolean, integer)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.modalidade_codigo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4
	 
union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4

union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4

union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = cast(act.dre_codigo as int8) and
			   a.ue_id = cast(act.ue_codigo as int8) and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and      
	  act.turma_ano_letivo = $4;
$function$;

create or REPLACE FUNCTION public.f_abrangencia_semestres(character varying, uuid, boolean, integer DEFAULT 0, integer DEFAULT 0)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.turma_semestre
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and      
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5))
	 
union

select distinct act.turma_semestre
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5))

union

select distinct act.turma_semestre
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5))

union

select distinct act.turma_semestre
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = cast(act.dre_codigo as int8) and
			   a.ue_id = cast(act.ue_codigo as int8) and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_ano_letivo = $5));
$function$;


create or REPLACE FUNCTION public.f_abrangencia_turmas(character varying, uuid, boolean, integer DEFAULT 0, integer DEFAULT 0, character varying DEFAULT NULL::character varying, integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_turmas
 LANGUAGE sql
AS $function$
select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
	  ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))
	 
union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))

union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))

union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = cast(act.dre_codigo as int8) and
			   a.ue_id = cast(act.ue_codigo as int8) and
			   a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id			   
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.ue_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7));
$function$;

create or REPLACE FUNCTION public.f_abrangencia_ues(character varying, uuid, boolean, integer DEFAULT 0, integer DEFAULT 0, character varying DEFAULT NULL::character varying, integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_ues
 LANGUAGE sql
AS $function$
select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
		inner join ue
			on act.ue_id = ue.id		
where a.login = $1 and 
	  a.perfil_id = $2 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
	  ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))
	 
union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))

union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7))

union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola
	from v_abrangencia_nivel_especifico a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = cast(act.dre_codigo as int8) and
			   a.ue_id = cast(act.ue_codigo as int8) and
			   a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 is null or ($6 is not null and act.dre_codigo = $6)) and
      ($7 = 0 or ($7 <> 0 and act.turma_ano_letivo = $7));
$function$;