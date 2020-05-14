-- public.v_abrangencia_sintetica source

CREATE OR REPLACE VIEW public.v_abrangencia_sintetica
AS SELECT a.id,
    a.usuario_id,
    u.login,
    a.dre_id,
    dre.dre_id AS codigo_dre,
    a.ue_id,
    ue.ue_id AS codigo_ue,
    a.turma_id,
    turma.turma_id AS codigo_turma,
    a.perfil,
    a.historico 
   FROM abrangencia a
     JOIN usuario u ON u.id = a.usuario_id
     LEFT JOIN dre dre ON dre.id = a.dre_id
     LEFT JOIN ue ue ON ue.id = a.ue_id
     LEFT JOIN turma turma ON turma.id = a.turma_id;