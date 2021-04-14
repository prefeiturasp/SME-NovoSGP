delete from evento_tipo where codigo = 28;

INSERT INTO evento_tipo (descricao, local_ocorrencia, concomitancia, tipo_data, dependencia, letivo, ativo, excluido, criado_em, alterado_em, alterado_por, criado_por, criado_rf, alterado_rf, codigo, somente_leitura, evento_escolaaqui)
VALUES('Itinerância PAAI', 1, true, 1, false, 3, true, false, NOW(), NULL, NULL, 'SISTEMA', '0', null, 28, false, false);
