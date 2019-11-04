ALTER TABLE public.tipo_calendario ADD migrado bool NOT NULL DEFAULT false;

ALTER TABLE public.periodo_escolar ADD migrado bool NOT NULL DEFAULT false;

ALTER TABLE public.evento ALTER COLUMN nome TYPE varchar(200) USING nome::varchar;

ALTER TABLE public.evento ADD migrado bool NOT NULL DEFAULT false;