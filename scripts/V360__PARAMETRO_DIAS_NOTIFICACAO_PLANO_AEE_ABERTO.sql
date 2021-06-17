delete from parametros_sistema where tipo in (53);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('DiasParaNotificacarPlanoAEEAberto', 53, 'Dia para notificar plano aee que estejam abertos', '10/03', 2021, true, now(), 'SISTEMA', '0');

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('DiasParaNotificacarPlanoAEEAberto', 53, 'Dia para notificar plano aee que estejam abertos', '10/07', 2021, true, now(), 'SISTEMA', '0');

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('DiasParaNotificacarPlanoAEEAberto', 53, 'Dia para notificar plano aee que estejam abertos', '10/12', 2021, true, now(), 'SISTEMA', '0');
