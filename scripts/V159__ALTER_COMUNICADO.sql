ALTER TABLE public.comunicado ADD COLUMN if not exists excluido boolean not null default false;
ALTER TABLE public.grupo_comunicado ADD COLUMN if not exists excluido boolean not null default false;