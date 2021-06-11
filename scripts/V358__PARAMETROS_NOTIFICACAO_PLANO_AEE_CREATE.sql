delete from parametros_sistema where tipo in (51);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('GerarNotificacoesPlanoAEE', 51, 'Controle de geração de notificações para os processos do Plano AEE', '', 2021, true, now(), 'SISTEMA', '0');
