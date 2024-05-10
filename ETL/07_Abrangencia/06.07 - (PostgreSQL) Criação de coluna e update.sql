 -- ADD coluna id na tabela 'public.etl_abrangencia'
alter table public.etl_abrangencia
ADD COLUMN id int

-- update no campo id da tabela 'public.etl_abrangencia'
update etl_abrangencia
set id = null