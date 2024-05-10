delete from registro_poa;
ALTER TABLE public.registro_poa DROP COLUMN IF EXISTS mes;
ALTER TABLE public.registro_poa ADD bimestre  int8 NOT NULL;