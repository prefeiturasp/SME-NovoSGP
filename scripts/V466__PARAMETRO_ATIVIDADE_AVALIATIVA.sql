delete from parametros_sistema where tipo = 76;
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('AtualizacaoDeAtividadesAvaliativas', 76, 'Data da última atualização atividades avaliativas classroom', '2021-01-01', 2021, true, now(), 'SISTEMA', '0');

delete from parametros_sistema where tipo = 77;
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('TipoAtividadeAvaliativaClassroom', 77, 'Tipo de Atividade Avaliativa da importações do Classroom', '17', 2021, true, now(), 'SISTEMA', '0');


delete from tipo_avaliacao where nome = 'Atividade Avaliativa';
insert into tipo_avaliacao (codigo, nome, descricao, situacao, criado_em, criado_por, criado_rf, excluido)
select 17, 'Atividade no Google Classroom', 'Atividade importada do google sala de aula', true, now(), 'Carga Inicial','Carga Inicial', false;

