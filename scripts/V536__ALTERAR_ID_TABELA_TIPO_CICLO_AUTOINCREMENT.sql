ALTER TABLE tipo_ciclo 
ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY;

alter sequence public.tipo_ciclo_id_seq restart with 9;