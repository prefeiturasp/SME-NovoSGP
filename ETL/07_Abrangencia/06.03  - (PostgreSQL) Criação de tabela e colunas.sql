-- Import dos dados (dos csv's) para a tabela 'public.etl_abrangencia'
-- Criação da tabela 'etl_user' com o USUARIO_RF usando clausula distinct 
select distinct USUARIO_RF into etl_user from public.etl_abrangencia

-- ADD colunas na tabela 'etl_user'
alter table public.etl_user
ADD COLUMN criado_em date,
ADD COLUMN criado_por CHAR(3),
ADD COLUMN criado_rf INT,
ADD COLUMN login varchar
