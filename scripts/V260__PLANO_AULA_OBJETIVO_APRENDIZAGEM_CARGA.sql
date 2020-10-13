update objetivo_aprendizagem_aula oa
	set objetivo_aprendizagem_id = p.objetivo_aprendizagem_jurema_id
  from objetivo_aprendizagem_plano p 
 where p.id = oa.objetivo_aprendizagem_plano_id;
 
ALTER TABLE public.objetivo_aprendizagem_aula DROP CONSTRAINT IF EXISTS objetivo_aprendizagem_aula_obj_apr_plano_id_fk;
DROP INDEX if exists objetivo_aprendizagem_aula_objetivo_aprendizagem_idx;
ALTER TABLE public.objetivo_aprendizagem_aula DROP COLUMN IF EXISTS  objetivo_aprendizagem_plano_id;
