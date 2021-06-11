-- Data de criação: 23/04/2021
-- Descriçãoo: Devolve o campo tipo_turma

-- altera a view v_abrangencia_cadeia_turmas para retornar o novo campo de tipo_turma
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
    ab_turma.dt_fim_eol AS dt_fim_turma,
    ab_turma.ensino_especial AS ensino_especial,
    ab_turma.tipo_turma AS tipo_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id
     JOIN turma ab_turma ON ab_turma.ue_id = ab_ues.id;