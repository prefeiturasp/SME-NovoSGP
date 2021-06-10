delete from parametros_sistema where tipo in (49,50);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('GerarPendenciasEncaminhamentoAEE', 49, 'Controle de geração de pendências para os processos do Encaminhamento AEE', '', 2021, true, now(), 'SISTEMA', '0');

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('GerarPendenciasPlanoAEE', 50, 'Controle de geração de pendências para os processos do Plano AEE', '', 2021, true, now(), 'SISTEMA', '0');