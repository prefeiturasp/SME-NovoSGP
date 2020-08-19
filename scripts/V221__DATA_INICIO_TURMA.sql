ALTER TABLE if exists public.turma ADD if not exists data_inicio date NULL;
ALTER TABLE if exists public.turma ADD if not exists situacao varchar(5) NOT NULL DEFAULT 'A';
