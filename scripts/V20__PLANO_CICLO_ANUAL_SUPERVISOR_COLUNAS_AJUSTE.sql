ALTER TABLE if exists public.plano_anual ADD if not exists migrado bool NOT NULL DEFAULT false;
ALTER TABLE if exists public.plano_ciclo ALTER COLUMN migrado SET NOT NULL;
ALTER TABLE if exists public.supervisor_escola_dre ALTER COLUMN escola_id TYPE varchar(10) USING escola_id::varchar;