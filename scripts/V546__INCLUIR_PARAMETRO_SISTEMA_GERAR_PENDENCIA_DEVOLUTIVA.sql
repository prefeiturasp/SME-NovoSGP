

INSERT INTO	parametros_sistema (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'GerarPendenciaDevolutivaSemDiarioBordo',102,'Executar Worker de Geração de Pendencia Devolutivas','',2022,TRUE,now(),'SISTEMA',NULL,NULL,'0',NULL
WHERE NOT EXISTS  (SELECT * FROM parametros_sistema ps WHERE ps.nome  ='GerarPendenciaDevolutivaSemDiarioBordo' AND ano=2022)	