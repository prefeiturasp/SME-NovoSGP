ALTER TABLE if exists public.tipo_calendario ADD column if not exists migrado bool NOT NULL DEFAULT false;

ALTER TABLE if exists public.periodo_escolar ADD column if not exists migrado bool NOT NULL DEFAULT false;

ALTER TABLE if exists public.evento ALTER COLUMN nome TYPE varchar(200) USING nome::varchar;

ALTER TABLE if exists public.evento ADD column if not exists migrado bool NOT NULL DEFAULT false;