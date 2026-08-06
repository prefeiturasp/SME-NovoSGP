CREATE OR REPLACE VIEW public.v_abrangencia_nivel_dre_completa
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