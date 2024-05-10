delete from parametros_sistema where tipo in (56);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('NovasModalidades', 56, 'Novas modalidades atendidos pelo SGP no ano', '2,4,7,8,9', 2021, true, now(), 'SISTEMA', '0');
