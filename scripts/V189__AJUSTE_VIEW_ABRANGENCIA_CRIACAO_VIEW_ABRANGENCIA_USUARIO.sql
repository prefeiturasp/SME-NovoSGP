drop view if exists public.v_abrangencia;
drop view if exists public.v_abrangencia_usuario;

CREATE OR REPLACE VIEW public.v_abrangencia
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
   FROM abrangencia a
     LEFT JOIN v_abrangencia_cadeia_dres dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON turma.turma_id = a.turma_id
  WHERE a.historico = false AND (COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) = false OR COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) IS NULL);

CREATE OR REPLACE VIEW public.v_abrangencia_usuario
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    u.login,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
     LEFT JOIN v_abrangencia_cadeia_dres_somente dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues_somente ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON a.dre_id = turma.dre_id OR a.ue_id = turma.ue_id OR turma.turma_id = a.turma_id
  WHERE a.historico = false AND (COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) = false OR COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) IS NULL);