DROP VIEW IF EXISTS public.v_abrangencia_historica;
CREATE OR REPLACE VIEW public.v_abrangencia_historica
AS SELECT act.dre_codigo,
    act.dre_abreviacao,
    act.dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    act.ue_codigo,
    act.ue_nome,
    act.turma_ano,
    act.turma_ano_letivo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.turma_codigo AS turma_id,
    act.dt_fim_turma,
    a.dt_fim_vinculo
   FROM abrangencia a
     JOIN v_abrangencia_cadeia_turmas act ON a.turma_id = act.turma_id
  WHERE a.historico = true AND act.turma_historica = true AND act.dre_codigo IS NOT NULL AND act.dre_abreviacao IS NOT NULL AND act.dre_nome IS NOT NULL AND act.ue_codigo IS NOT NULL AND act.ue_nome IS NOT NULL AND act.ue_nome IS NOT NULL AND act.turma_ano IS NOT NULL AND act.turma_ano_letivo IS NOT NULL AND act.modalidade_codigo IS NOT NULL AND act.turma_nome IS NOT NULL AND act.turma_semestre IS NOT NULL AND act.qt_duracao_aula IS NOT NULL AND act.tipo_turno IS NOT NULL AND act.turma_codigo IS NOT NULL;

-- Permissions

ALTER TABLE public.v_abrangencia_historica OWNER TO postgres;
GRANT ALL ON TABLE public.v_abrangencia_historica TO postgres;
