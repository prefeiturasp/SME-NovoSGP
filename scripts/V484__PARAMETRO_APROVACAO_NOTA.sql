delete from parametros_sistema where tipo = 78;
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('AprovacaoAlteracaoNotasFechamento', 78, 'Solicita aprovação nas alterações de notas de fechamento ou conselho', '', 2021, true, now(), 'SISTEMA', '0');

delete from parametros_sistema where tipo = 79;
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('AprovacaoAlteracaoParecerConclusivo', 79, 'Solicita aprovação nas alterações de parecer conclusivo do aluno', '', 2021, true, now(), 'SISTEMA', '0');
