DROP FUNCTION IF EXISTS public.f_abrangencia_turmas;
DROP VIEW IF EXISTS public.v_estrutura_abrangencia_turmas;

CREATE VIEW public.v_estrutura_abrangencia_turmas
AS SELECT v_abrangencia_cadeia_turmas.turma_ano AS ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo AS anoletivo,
    v_abrangencia_cadeia_turmas.turma_codigo AS codigo,
    v_abrangencia_cadeia_turmas.modalidade_codigo AS codigomodalidade,
    v_abrangencia_cadeia_turmas.turma_nome AS nome,
    v_abrangencia_cadeia_turmas.turma_semestre AS semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula AS qtduracaoaula,
    v_abrangencia_cadeia_turmas.tipo_turno AS tipoturno
   FROM v_abrangencia_cadeia_turmas;

create function f_abrangencia_turmas
(
	varchar(50),
	uuid,
	bool,
	int4 = 0,
	int4 = 0,
	varchar(15) = null,
	int4 = 0
)
returns setof v_estrutura_abrangencia_turmas as 
'
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
			on a.dre_id = act.dre_id and
			   a.ue_id = act.ue_id and
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
'
language sql;