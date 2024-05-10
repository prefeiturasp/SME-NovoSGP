ALTER TABLE public.atividade_infantil
DROP COLUMN IF EXISTS aviso_classroom_id;

ALTER TABLE public.atividade_infantil
DROP COLUMN IF EXISTS atividade_classroom_id;
ALTER TABLE public.atividade_infantil
ADD COLUMN atividade_classroom_id int8 NOT NULL;

CREATE INDEX atividade_infantil_classroom_id_idx ON public.atividade_infantil USING btree (atividade_classroom_id);
