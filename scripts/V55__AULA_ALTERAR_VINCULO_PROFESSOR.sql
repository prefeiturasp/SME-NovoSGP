begin transaction;

ALTER TABLE	public.aula DROP CONSTRAINT if exists "aula_usuario_fk";

ALTER TABLE public.aula ALTER COLUMN professor_id TYPE varchar(15) USING professor_id::varchar;

ALTER TABLE public.aula RENAME COLUMN professor_id TO professor_rf;

CREATE index if not exists professor_rf_idx ON public.aula (professor_rf);

DELETE FROM public.aula where 1=1;

end transaction;