-- INSERT de dados na tabela 'usuario' dos USUARIO_RF não existentes na tabela
-- Usando como origem a tabela etl_user
 truncate table public.usuario cascade


 insert into public.usuario(rf_codigo,criado_em,criado_por,criado_rf,login)
   SELECT
    usuario_rf, criado_em, criado_por, criado_rf , login
FROM
    etl_user
where usuario_rf not in 
    (select usuario_rf from etl_user A
   inner join usuario B
   on A.usuario_rf = B.rf_codigo)