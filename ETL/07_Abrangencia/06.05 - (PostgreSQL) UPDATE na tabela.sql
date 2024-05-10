-- Atualização dos campos na tabela 'etl_user'
update public.etl_user
set criado_em = '2020-01-13'

update public.etl_user
set criado_por = 'ETL'

update public.etl_user
set criado_rf = 0

update public.etl_user
set login = etl_user.usuario_rf