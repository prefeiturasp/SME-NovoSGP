-- update do campo id usando INNER JOIN com a tabela 'public.usuario'
UPDATE etl_abrangencia
SET id =  usuario.id
FROM
   usuario
WHERE
   usuario.rf_codigo = etl_abrangencia.usuario_rf;