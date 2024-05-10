delete from parametros_sistema where tipo in (58);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoFrequenciaTurma', 58, 'Data da última execução da rotina de consolidação de frequência', '', 2021, true, now(), 'SISTEMA', '0');
