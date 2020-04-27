DROP VIEW IF EXISTS public.v_abrangencia;
DROP VIEW IF EXISTS public.v_abrangencia_historica;
DROP VIEW IF EXISTS public.v_abrangencia_cadeia_dres_somente;
DROP VIEW IF EXISTS public.v_abrangencia_cadeia_ues_somente;

-- v_abrangencia_cadeia_dres_somente
CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_dres_somente
AS
SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    NULL::bigint AS ue_id,
    NULL::character varying AS ue_codigo,
    NULL::character varying AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer
AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres;

-- v_abrangencia_cadeia_ues_somente
CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_ues_somente
AS
SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id AS ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL
::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id;

-- v_abrangencia
CREATE OR REPLACE VIEW public.v_abrangencia
AS
SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
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

-- v_abrangencia_historica
CREATE OR REPLACE VIEW public.v_abrangencia_historica
AS
SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
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
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id,
    COALESCE(turma.dt_fim_turma, ue.dt_fim_turma, dre.dt_fim_turma) AS dt_fim_turma,
    a.dt_fim_vinculo
FROM abrangencia a
    JOIN usuario u ON a.usuario_id = u.id
    LEFT JOIN v_abrangencia_cadeia_dres_somente dre ON dre.dre_id = a.dre_id
    LEFT JOIN v_abrangencia_cadeia_ues_somente ue ON ue.ue_id = a.ue_id
    LEFT JOIN v_abrangencia_cadeia_turmas turma ON a.dre_id = turma.dre_id OR a.ue_id = turma.ue_id OR a.turma_id = turma.turma_id
WHERE a.historico = true OR dre.turma_historica = true;