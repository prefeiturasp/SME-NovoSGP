ALTER TABLE  if exists public.registro_frequencia ADD COLUMN IF NOT EXISTS criado_em timestamp not NULL;
ALTER TABLE  if exists public.registro_frequencia ADD COLUMN IF NOT EXISTS criado_por varchar(200) not NULL;
ALTER TABLE  if exists public.registro_frequencia ADD COLUMN IF NOT EXISTS criado_rf varchar(200) not NULL;
ALTER TABLE  if exists public.registro_frequencia ADD COLUMN IF NOT EXISTS alterado_em timestamp NULL;
ALTER TABLE  if exists public.registro_frequencia ADD COLUMN IF NOT EXISTS alterado_por varchar(200) NULL;
ALTER TABLE  if exists public.registro_frequencia ADD COLUMN IF NOT EXISTS alterado_rf varchar(200) NULL;


ALTER TABLE  if exists public.registro_ausencia_aluno ADD COLUMN IF NOT EXISTS excluido bool not null default false;
CREATE INDEX if not exists registro_ausencia_aluno_excluido_idx ON public.registro_ausencia_aluno (excluido);

ALTER TABLE  if exists public.registro_ausencia_aluno ADD COLUMN IF NOT EXISTS criado_em timestamp not NULL;
ALTER TABLE  if exists public.registro_ausencia_aluno ADD COLUMN IF NOT EXISTS criado_por varchar(200) not NULL;
ALTER TABLE  if exists public.registro_ausencia_aluno ADD COLUMN IF NOT EXISTS criado_rf varchar(200) not NULL;
ALTER TABLE  if exists public.registro_ausencia_aluno ADD COLUMN IF NOT EXISTS alterado_em timestamp NULL;
ALTER TABLE  if exists public.registro_ausencia_aluno ADD COLUMN IF NOT EXISTS alterado_por varchar(200) NULL;
ALTER TABLE  if exists public.registro_ausencia_aluno ADD COLUMN IF NOT EXISTS alterado_rf varchar(200) NULL;


