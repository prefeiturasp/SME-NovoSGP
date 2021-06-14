delete from parametros_sistema where tipo in (55);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('NovosTiposUE', 55, 'Novos tipos de UE atendidos pelo SGP no ano', '10,11,12,13,14,15,18,19,22,23,25,26,27,29', 2021, true, now(), 'SISTEMA', '0');
