ALTER TABLE if exists public.plano_anual DROP CONSTRAINT if exists plano_anual_un;

alter table if exists public.plano_anual add column if not exists componente_curricular_eol_id bigint not null;

ALTER TABLE if exists public.plano_anual ADD CONSTRAINT plano_anual_un UNIQUE (escola_id,turma_id,ano,bimestre,componente_curricular_eol_id);

