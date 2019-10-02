ALTER TABLE public.plano_anual ADD migrado bool NOT NULL DEFAULT false;
ALTER TABLE public.plano_ciclo ALTER COLUMN migrado SET NOT NULL;
ALTER TABLE public.supervisor_escola_dre ALTER COLUMN escola_id TYPE varchar(10) USING escola_id::varchar;