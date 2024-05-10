do $$
declare 
	gradeId bigint;	
begin

	--EMEFM - 2ª Série - Integral Turno 7h
	insert into grade (nome, criado_em, criado_por, criado_rf)
	values('EMEFM - 2ª Série - Integral Turno 7h', now(), 'Carga', 'Carga') returning id into gradeId;
	
	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 7, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 138, 3, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1328, 1, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 139, 1, now(), 'Carga', 'Carga');

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 6, 1, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 2, 2, now(), 'Carga', 'Carga');
	
	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 8, 1, now(), 'Carga', 'Carga');
	
	--História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 7, 1, now(), 'Carga', 'Carga');

	--Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 98, 1, now(), 'Carga', 'Carga');

	--Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 103, 1, now(), 'Carga', 'Carga');

	--Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 54, 1, now(), 'Carga', 'Carga');
	
	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 52, 1, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 53, 1, now(), 'Carga', 'Carga');

	--Língua Portuguesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1329, 1, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1330, 1, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1331, 1, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1332, 1, now(), 'Carga', 'Carga');

	--História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1333, 1, now(), 'Carga', 'Carga');

	--Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1336, 1, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1335, 1, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1334, 1, now(), 'Carga', 'Carga');

	--Língua, Literatura e Cultura dos países de Língua Espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1346, 2, now(), 'Carga', 'Carga');
	
	--Expressões Culturais e Artísticas
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1318, 1, now(), 'Carga', 'Carga');
	
	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1350, 2, now(), 'Carga', 'Carga');
	
	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1359, 2, now(), 'Carga', 'Carga');
	
	--Sala de Leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1347, 2, now(), 'Carga', 'Carga');



	--EMEFM - 2ª Série - Integral Turno 4h
	insert into grade (nome, criado_em, criado_por, criado_rf)
	values('EMEFM - 2ª Série - Integral Turno 4h', now(), 'Carga', 'Carga') returning id into gradeId;
	
	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 4, now(), 'Carga', 'Carga');

	--Língua e Literatura de Língua Portuguesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1417, 5, now(), 'Carga', 'Carga');

	--Língua, Literatura e Cultura dos países de Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1418, 3, now(), 'Carga', 'Carga');

	--Expressões Multiculturais e Artísticas
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1318, 3, now(), 'Carga', 'Carga');

	--Expressões Multiculturais e Artísticas
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1419, 2, now(), 'Carga', 'Carga');

	--Formação do Mundo Multipolar
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1425, 3, now(), 'Carga', 'Carga');

	--Formação do Mundo Multipolar
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1420, 2, now(), 'Carga', 'Carga');

	--Tragetória dos Direitos Humanos
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1421, 2, now(), 'Carga', 'Carga');
	
	--Tragetória dos Direitos Humanos
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1422, 2, now(), 'Carga', 'Carga');
	
	--Sociedade, Cultura e Interculturalismo
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1423, 1, now(), 'Carga', 'Carga');

	--Sociedade, Cultura e Interculturalismo
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1424, 1, now(), 'Carga', 'Carga');
	
	--Sociedade, Cultura e Interculturalismo
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1426, 1, now(), 'Carga', 'Carga');

	--Sociedade, Cultura e Interculturalismo
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1427, 1, now(), 'Carga', 'Carga');
	
	--Investigações Matemáticas
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1428, 4, now(), 'Carga', 'Carga');
	
	--Características dos seres vivos e as teorias unificadoras da Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1337, 3, now(), 'Carga', 'Carga');

	--Características dos seres vivos e as teorias unificadoras da Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1431, 1, now(), 'Carga', 'Carga');

	--Investigações do mundo físico e químico
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1429, 3, now(), 'Carga', 'Carga');
	
	--Investigações do mundo físico e químico
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1430, 2, now(), 'Carga', 'Carga');


end $$;
