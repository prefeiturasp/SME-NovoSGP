delete from parametros_sistema where tipo = 76;

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('AtualizacaoDeAtividadesAvaliativas', 76, 'Data da última atualização', '', 2021, true, now(), 'SISTEMA', '0');

delete from tipo_avaliacao where nome = 'Atividade Avaliativa';

insert into tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select 'Atividade Avaliativa', 'Atividade Avaliativa', true, now(), 'Carga Inicial','Carga Inicial', false

    delete from parametros_sistema where tipo = 77;

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('AtividadeAvaliativa', 77, 'Data de atividades avaliativas', '', 2021, true, now(), 'SISTEMA', '0');