-- Após o IMPORT dos csv's a tabela public.etl_abrangencia
-- criação do campo e atualização dos dados 

alter table public.etl_abrangencia
ADD COLUMN turma_id_sgp int4 NULL

 
UPDATE etl_abrangencia
SET turma_id_sgp =  turma.id
FROM
   turma
WHERE
   turma.turma_id = cast(etl_abrangencia.turma_id_eol as varchar)
