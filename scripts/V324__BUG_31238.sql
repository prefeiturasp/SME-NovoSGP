do $$
declare f_nota record;
		nota boolean;

begin

for f_nota in
	select n.*
	,nccp.tipo_nota TipoNota
	from fechamento_nota n
	inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
	inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id
	inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
	inner join turma t on t.id = ft.turma_id
	inner join tipo_ciclo_ano tca on tca.ano = t.ano and tca.modalidade = t.modalidade_codigo
	inner join tipo_ciclo tc on tca.tipo_ciclo_id = tc.id
	inner join notas_conceitos_ciclos_parametos nccp on nccp.ciclo = tc.id
	where n.nota = 'NaN'
loop
-- ---------------------------------
			update fechamento_nota set nota = 5 where id = f_nota.id and f_nota.TipoNota = 1;
			update fechamento_nota set conceito_id = 2 where id = f_nota.id and f_nota.TipoNota <> 1;
-- ---------------------------------
end loop;

end $$;