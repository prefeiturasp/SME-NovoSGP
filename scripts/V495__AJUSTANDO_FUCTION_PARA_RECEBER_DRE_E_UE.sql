CREATE OR REPLACE FUNCTION public.f_abrangencia_semestres(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_ano_letivo integer DEFAULT 0, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.turma_semestre
from v_abrangencia_nivel_dre a
         inner join v_abrangencia_cadeia_turmas act
                    on a.dre_id = act.dre_id
where a.login = p_login
  and a.perfil_id = p_perfil_id
  and act.turma_historica = p_historico
  and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo))
  and (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
  and (p_dre_id is null or (p_dre_id is not null and act.dre_codigo = p_dre_id))

union

select distinct act.turma_semestre
from v_abrangencia_nivel_ue a
         inner join v_abrangencia_cadeia_turmas act
                    on a.ue_id = act.ue_id
where a.login = p_login
  and a.perfil_id = p_perfil_id
  and act.turma_historica = p_historico
  and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo))
  and (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
  and (p_ue_id is null or (p_ue_id is not null and act.ue_codigo = p_ue_id))

union

select distinct act.turma_semestre
from v_abrangencia_nivel_turma a
         inner join v_abrangencia_cadeia_turmas act
                    on a.turma_id = act.turma_id
where a.login = p_login
  and a.perfil_id = p_perfil_id
  and ((p_historico = true and a.historico = true) or
       (p_historico = false and a.historico = false and act.turma_historica = false))
  and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo))
  and (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
  and (p_ue_id is null or (p_ue_id is not null and act.ue_codigo = p_ue_id))
  and (p_dre_id is null or (p_dre_id is not null and act.dre_codigo = p_dre_id));
$function$
;
