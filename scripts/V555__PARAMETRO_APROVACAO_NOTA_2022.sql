DO $$
BEGIN
    if not exists (select 1 from parametros_sistema ps where tipo = 78 and ano = 2022) then
         insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
		values('AprovacaoAlteracaoNotaFechamento', 78, 'Solicita aprovação nas alterações de notas de fechamento', '', 2022, true, now(), 'SISTEMA', '0');
    end if;
	if not exists (select 1 from parametros_sistema ps where tipo = 79 and ano = 2022) then
		insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
		values('AprovacaoAlteracaoNotaConselho', 79, 'Solicita aprovação nas alterações de notas do conselho', '', 2022, true, now(), 'SISTEMA', '0');
	end if;
	if not exists (select 1 from parametros_sistema ps where tipo = 80 and ano = 2022) then
		insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
		values('AprovacaoAlteracaoParecerConclusivo', 80, 'Solicita aprovação nas alterações de parecer conclusivo do aluno', '', 2022, true, now(), 'SISTEMA', '0');
	end if;
END $$;
