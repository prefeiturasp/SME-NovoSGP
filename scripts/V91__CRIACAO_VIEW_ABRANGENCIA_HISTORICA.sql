-- v_abrangencia_cadeia_turmas
CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_turmas
AS SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id AS ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    ab_turma.id AS turma_id,
    ab_turma.ano AS turma_ano,
    ab_turma.ano_letivo AS turma_ano_letivo,
    ab_turma.modalidade_codigo,
    ab_turma.nome AS turma_nome,
    ab_turma.semestre AS turma_semestre,
    ab_turma.qt_duracao_aula,
    ab_turma.tipo_turno,
    ab_turma.turma_id AS turma_codigo,
    ab_turma.historica AS turma_historica,
    ab_turma.dt_fim_eol AS dt_fim_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id
     JOIN turma ab_turma ON ab_turma.ue_id = ab_ues.id;     
-- v_abrangencia_cadeia_dres
CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_dres
AS SELECT v_abrangencia_cadeia_turmas.dre_id,
    v_abrangencia_cadeia_turmas.dre_codigo,
    v_abrangencia_cadeia_turmas.dre_abreviacao,
    v_abrangencia_cadeia_turmas.dre_nome,
    v_abrangencia_cadeia_turmas.ue_id,
    v_abrangencia_cadeia_turmas.ue_codigo,
    v_abrangencia_cadeia_turmas.ue_nome,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.turma_ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.turma_nome,
    v_abrangencia_cadeia_turmas.turma_semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula,
    v_abrangencia_cadeia_turmas.tipo_turno,
    v_abrangencia_cadeia_turmas.turma_codigo,
    v_abrangencia_cadeia_turmas.turma_historica,
    v_abrangencia_cadeia_turmas.dt_fim_turma
   FROM v_abrangencia_cadeia_turmas
UNION ALL
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
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    null turma_historica,
    null dt_fim_turma
   FROM dre ab_dres;
-- v_abrangencia_cadeia_ues
CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_ues
AS SELECT v_abrangencia_cadeia_turmas.dre_id,
    v_abrangencia_cadeia_turmas.dre_codigo,
    v_abrangencia_cadeia_turmas.dre_abreviacao,
    v_abrangencia_cadeia_turmas.dre_nome,
    v_abrangencia_cadeia_turmas.ue_id,
    v_abrangencia_cadeia_turmas.ue_codigo,
    v_abrangencia_cadeia_turmas.ue_nome,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.turma_ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.turma_nome,
    v_abrangencia_cadeia_turmas.turma_semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula,
    v_abrangencia_cadeia_turmas.tipo_turno,
    v_abrangencia_cadeia_turmas.turma_codigo,
    v_abrangencia_cadeia_turmas.turma_historica,
    v_abrangencia_cadeia_turmas.dt_fim_turma
   FROM v_abrangencia_cadeia_turmas
UNION ALL
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
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    null turma_historica,
    null dt_fim_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id;  
-- v_abrangencia
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
   where a.historico = false and (COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) = false or COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) is null);   
-- v_abrangencia_historica
CREATE OR REPLACE VIEW public.v_abrangencia_historica
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
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id,
    COALESCE(turma.dt_fim_turma, ue.dt_fim_turma, dre.dt_fim_turma) AS dt_fim_turma,
    a.dt_fim_vinculo
   FROM abrangencia a
     LEFT JOIN v_abrangencia_cadeia_dres dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON turma.turma_id = a.turma_id
   where a.historico = true or dre.turma_historica = true or ue.turma_historica = true or turma.turma_historica = true; 
drop view public.v_abrangencia_magra;