DROP VIEW public.v_abrangencia;
CREATE OR REPLACE VIEW public.v_abrangencia
AS SELECT ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    ab_turmas.ano AS turma_ano,
    ab_turmas.ano_letivo AS turma_ano_letivo,
    ab_turmas.modalidade_codigo,
    ab_turmas.nome AS turma_nome,
    ab_turmas.semestre AS turma_semestre,    
    ab_dres.usuario_id,
    ab_dres.perfil AS usuario_perfil,
    ab_turmas.turma_id AS turma_id
   FROM abrangencia_dres ab_dres
     JOIN abrangencia_ues ab_ues ON ab_ues.abrangencia_dres_id = ab_dres.id
     JOIN abrangencia_turmas ab_turmas ON ab_turmas.abrangencia_ues_id = ab_ues.id;
