ALTER TABLE if exists public.plano_anual_territorio_saber
DROP CONSTRAINT plano_anual_territorio_saber_un;

ALTER TABLE if exists public.plano_anual_territorio_saber
ADD CONSTRAINT plano_anual_territorio_saber_un UNIQUE (escola_id, turma_id, ano, bimestre, territorio_experiencia_id);