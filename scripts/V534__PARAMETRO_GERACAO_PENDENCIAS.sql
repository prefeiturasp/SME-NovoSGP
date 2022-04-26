delete from parametros_sistema where tipo in (87);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecutaPendenciaAulaDiarioBordo', 87, 'Controle de geração de pendências do diário de bordo', '', 2022, true, now(), 'SISTEMA', '0');


delete from parametros_sistema where tipo in (88);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecutaPendenciaAulaAvaliacao', 88, 'Controle de geração de pendências da avaliação', '', 2022, true, now(), 'SISTEMA', '0');


delete from parametros_sistema where tipo in (89);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecutaPendenciaAulaFrequencia', 89, 'Controle de geração de pendências de frequência', '', 2022, true, now(), 'SISTEMA', '0');


delete from parametros_sistema where tipo in (90);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecutaPendenciaPlanoAula', 90, 'Controle de geração de pendências de plano de aula', '', 2022, true, now(), 'SISTEMA', '0');

