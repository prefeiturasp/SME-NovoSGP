do $$
declare 
	alunos_eja record;
	fechamento_turma_id_referencia bigint;
    fechamento_turma_disciplina_id_referencia bigint;
    fechamento_aluno_id_referencia bigint;

begin
	for alunos_eja in
		select distinct ae.*, nccp.tipo_nota
			from alunos_eja_2020 ae 
				left join (select fa.aluno_codigo,
								  t.turma_id::int,
								  ftd.disciplina_id
						      from fechamento_aluno fa
						         inner join fechamento_turma_disciplina ftd
						         	on fa.fechamento_turma_disciplina_id = ftd.id
						         inner join fechamento_turma ft
						         	on ftd.fechamento_turma_id = ft.id
						         inner join turma t
						         	on ft.turma_id = t.id
						   where t.ano_letivo = 2020) a
					on ae.cd_aluno = a.aluno_codigo and
					   ae.cd_turma_escola = a.turma_id and
					   ae.cd_componente_curricular = a.disciplina_id
				inner join turma t2
					on ae.cd_turma_escola = t2.turma_id::int
				inner join tipo_ciclo_ano tca on
				    tca.ano = t2.ano
				    and tca.modalidade = t2.modalidade_codigo
				inner join tipo_ciclo tc on
				    tca.tipo_ciclo_id = tc.id
				inner join notas_conceitos_ciclos_parametos nccp on
				    nccp.ciclo = tc.id		
		order by 1, 2, 3
	loop
		select ft2.id into fechamento_turma_id_referencia
			from fechamento_turma ft2
				inner join turma t2
					on ft2.turma_id = t2.id
		where t2.turma_id::int = alunos_eja.cd_turma_escola;
	
		select ftd2.id into fechamento_turma_disciplina_id_referencia
			from fechamento_turma_disciplina ftd2
		where ftd2.fechamento_turma_id = fechamento_turma_id_referencia and
			  ftd2.disciplina_id = alunos_eja.cd_componente_curricular;
			 
		if fechamento_turma_disciplina_id_referencia is null then
			insert into fechamento_turma_disciplina (disciplina_id, 
													 migrado, 
													 excluido, 
													 criado_em, 
													 criado_por, 
													 alterado_em, 
													 alterado_por, 
													 criado_rf,
													 alterado_rf,
													 situacao,
													 justificativa,
													 fechamento_turma_id)
			values (alunos_eja.cd_componente_curricular::int8, false, false, current_date, 'Sistema', null, null, 'Sistema', null, 3, null, fechamento_turma_id_referencia)
			returning id into fechamento_turma_disciplina_id_referencia;
		end if;
	
		insert into fechamento_aluno (fechamento_turma_disciplina_id,
									  aluno_codigo,
									  anotacao,
									  migrado,
									  excluido,
									  criado_em,
									  criado_por,
									  alterado_em,
									  alterado_por,
									  criado_rf,
									  alterado_rf)
		values (fechamento_turma_disciplina_id_referencia, alunos_eja.cd_aluno, null, false, false, current_date, 'Sistema', null, null, 'Sistema', null)
		returning id into fechamento_aluno_id_referencia;
	
		insert into fechamento_nota (disciplina_id,
									 nota,
									 conceito_id,
									 migrado,
									 excluido,
									 criado_em,
									 criado_por,
									 alterado_em,
									 alterado_por,
									 criado_rf,
									 alterado_rf,
									 sintese_id,
									 fechamento_aluno_id)
		values (alunos_eja.cd_componente_curricular::int8, case when alunos_eja.tipo_nota = 1 then 5 else null end, case when alunos_eja.tipo_nota = 2 then 1 else null end, false, false, current_date, 'Sistema', null, null, 'Sistema', null, null, fechamento_aluno_id_referencia);		
	end loop;
end $$;