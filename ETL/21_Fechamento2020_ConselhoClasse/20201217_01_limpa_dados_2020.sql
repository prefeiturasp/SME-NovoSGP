-- remove conselho_classe_nota
delete from conselho_classe_nota ccn where conselho_classe_aluno_id in (select cca.id from conselho_classe_aluno cca where conselho_classe_id in (select cc.id from conselho_classe cc where fechamento_turma_id in (
	select ft.id from fechamento_turma ft 
		inner join turma t on t.id = ft.turma_id
		where t.ano_letivo = 2020
		and modalidade_codigo in (3, 5, 6)
		and ft.periodo_escolar_id is null
	)));

-- remove conselho_classe_aluno
delete from conselho_classe_aluno cca where conselho_classe_id in (select cc.id from conselho_classe cc where fechamento_turma_id in (
	select ft.id from fechamento_turma ft 
		inner join turma t on t.id = ft.turma_id
		where t.ano_letivo = 2020
		and modalidade_codigo in (3, 5, 6)
		and ft.periodo_escolar_id is null
	));

-- remove conselho_classe
delete from conselho_classe cc where fechamento_turma_id in (
	select ft.id from fechamento_turma ft 
		inner join turma t on t.id = ft.turma_id
		where t.ano_letivo = 2020
		and modalidade_codigo in (3, 5, 6)
		and ft.periodo_escolar_id is null
	);