CREATE OR REPLACE VIEW public.v_abrangencia_usuario
AS 
  SELECT COALESCE(turma_dre.dre_codigo, turma_ue.dre_codigo, turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma_dre.dre_abreviacao, turma_ue.dre_abreviacao, turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma_dre.dre_nome, turma_ue.dre_nome, turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    u.login,
    COALESCE(turma_dre.ue_codigo, turma_ue.ue_codigo, turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma_dre.ue_nome, turma_ue.ue_nome, turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma_dre.turma_ano, turma_ue.turma_ano, turma.turma_ano, ue.turma_ano::character varying, dre.turma_ano::character varying) AS turma_ano,
    COALESCE(turma_dre.turma_ano_letivo, turma_ue.turma_ano_letivo, turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma_dre.modalidade_codigo, turma_ue.modalidade_codigo, turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma_dre.turma_nome, turma_ue.turma_nome, turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma_dre.turma_semestre, turma_ue.turma_semestre, turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma_dre.qt_duracao_aula, turma_ue.qt_duracao_aula, turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma_dre.tipo_turno, turma_ue.tipo_turno, turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma_dre.turma_codigo, turma_ue.turma_codigo, turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
     LEFT JOIN v_abrangencia_cadeia_dres_somente dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues_somente ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma_dre ON a.dre_id = turma_dre.dre_id 
     LEFT JOIN v_abrangencia_cadeia_turmas turma_ue ON a.ue_id = turma_ue.ue_id 
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON turma.turma_id = a.turma_id
  WHERE a.historico = false 
  AND (COALESCE(turma_dre.turma_historica, turma_ue.turma_historica, turma.turma_historica, ue.turma_historica, dre.turma_historica) = false OR COALESCE(turma_dre.turma_historica, turma_ue.turma_historica, turma.turma_historica, ue.turma_historica, dre.turma_historica) IS NULL);