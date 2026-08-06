begin;

-- 1) f_abrangencia_dres
CREATE OR REPLACE FUNCTION public.f_abrangencia_dres(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_dres
 LANGUAGE sql
AS $function$
select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo,
                act.dre_id as id
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo,
                act.dre_id
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo,
                act.dre_id
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$
;

-- 2) v_abrangencia_nivel_dre
CREATE OR REPLACE VIEW public.v_abrangencia_nivel_dre
AS SELECT DISTINCT a.dre_id,
    a.perfil AS perfil_id,
    a.historico,
    u.login,
    a.ue_id,
    a.turma_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
  WHERE a.dre_id IS NOT NULL AND a.ue_id IS NULL AND a.turma_id IS NULL
  ORDER BY a.dre_id;

-- 3) remover v_abrangencia_nivel_dre_completa
DROP VIEW IF EXISTS public.v_abrangencia_nivel_dre_completa;

-- 4) remover migrations flyway
delete from public.flyway_schema_history
where version in ('774', '775', '776');

commit;