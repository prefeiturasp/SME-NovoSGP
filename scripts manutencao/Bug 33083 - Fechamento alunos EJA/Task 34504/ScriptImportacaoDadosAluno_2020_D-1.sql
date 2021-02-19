select distinct a.cd_aluno, hmte.cd_turma_escola, gcc.cd_componente_curricular
	from v_historico_matricula_cotic hm
		inner join serie_ensino se
			on hm.cd_serie_ensino = se.cd_serie_ensino				
		inner join historico_matricula_turma_escola hmte
			on hm.cd_matricula = hmte.cd_matricula
		inner join v_aluno_cotic a
			on hm.cd_aluno = a.cd_aluno
		inner join serie_turma_grade stg
			on se.cd_serie_ensino = stg.cd_serie_ensino and
			   hmte.cd_turma_escola = stg.cd_turma_escola
		inner join escola_grade eg
			on stg.cd_escola_grade = eg.cd_escola_grade
		inner join grade_componente_curricular gcc
			on eg.cd_grade = gcc.cd_grade
where hm.an_letivo = 2020 and
	  a.cd_aluno = 'XXX'
order by 1, 2, 3