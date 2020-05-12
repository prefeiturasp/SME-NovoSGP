drop function if exists public.f_abrangencia_dres;

create function public.f_abrangencia_dres
(
	varchar(50),
	uuid,
	bool,
	int4 = 0,
	int4 = 0,
	int4 = 0
)
returns setof v_estrutura_abrangencia_dres as 
'
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
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
			   a.turma_id = act.turma_id			   
where a.login = $1 and 
	  a.perfil_id = $2 and
	  a.historico = $3 and	  
	  act.turma_historica = $3 and
      ($4 = 0 or ($4 <> 0 and act.modalidade_codigo = $4)) and
      ($5 = 0 or ($5 <> 0 and act.turma_semestre = $5)) and
      ($6 = 0 or ($6 <> 0 and act.turma_ano_letivo = $6));
'
language sql;