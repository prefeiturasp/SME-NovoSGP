delete from parametros_sistema where tipo in (54);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('QuantidadeFotosAcompanhamentoAluno', 54, 'Quantidade de fotos permitidas no acompanhamento do estudante/criança', '3', 2021, true, now(), 'SISTEMA', '0');
