do $$
declare 
	gradeId bigint;	
begin
	
	--EMEFM - 1ª Série - Noturno
	insert into grade (nome, criado_em, criado_por, criado_rf)
	values('EMEBS - 1ª Série - Integral', now(), 'Carga', 'Carga') returning id into gradeId;
	
	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 4, 6, 7, now(), 'Carga', 'Carga');

	--Língua Portuguesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 138, 4, now(), 'Carga', 'Carga');
	
	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1328, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 139, 2, now(), 'Carga', 'Carga');
		
	--Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 218, 2, now(), 'Carga', 'Carga');
		
	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 6, 2, now(), 'Carga', 'Carga');
		
	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 2, 4, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 8, 2, now(), 'Carga', 'Carga');
	
	--História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 7, 2, now(), 'Carga', 'Carga');

	--Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 98, 2, now(), 'Carga', 'Carga');

	--Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 103, 2, now(), 'Carga', 'Carga');

	--Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 54, 2, now(), 'Carga', 'Carga');
	
	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 52, 2, now(), 'Carga', 'Carga');
	
	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 53, 2, now(), 'Carga', 'Carga');
	
	--Língua Portuguesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1329, 2, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1330, 1, now(), 'Carga', 'Carga');
	
	--Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1432, 1, now(), 'Carga', 'Carga');
	
	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1331, 1, now(), 'Carga', 'Carga');
	
	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1332, 1, now(), 'Carga', 'Carga');
	
	--História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1333, 1, now(), 'Carga', 'Carga');
	
	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1334, 1, now(), 'Carga', 'Carga');
	
	--Expressões Culturais e Artísticas
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1340, 2, now(), 'Carga', 'Carga');
	
	--Investigação Ciêntífica e Processos Matemáticos
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1314, 1, now(), 'Carga', 'Carga');
	
	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1350, 2, now(), 'Carga', 'Carga');
	
	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1359, 1, now(), 'Carga', 'Carga');
	
	--Sala de Leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1347, 1, now(), 'Carga', 'Carga');
	
end $$;
