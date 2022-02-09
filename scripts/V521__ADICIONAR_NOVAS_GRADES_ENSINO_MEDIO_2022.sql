do $$
declare 
	gradeId bigint;	
begin

	--EMEFM - 1 Serie integral - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEFM - 1ª Série - Integral', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 7, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 138, 5, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1328, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 139, 2, now(), 'Carga', 'Carga');

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 6, 2, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 2, 5, now(), 'Carga', 'Carga');

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
	values (gradeId, 1, 51, 2, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 53, 2, now(), 'Carga', 'Carga');

	--Língua, Literatura e Cultura dos países de Língua Espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1311, 2, now(), 'Carga', 'Carga');

	--Expressões Culturais e Artísticas - Artes
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1318, 1, now(), 'Carga', 'Carga');

    --Expressões Culturais e Artísticas - Ed. Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1419, 1, now(), 'Carga', 'Carga');

	--Investigação Ciêntífica e Processos Matemáticos - Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1357, 1, now(), 'Carga', 'Carga');

	--Investigação Ciêntífica e Processos Matemáticos - Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1645, 1, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1350, 2, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1359, 2, now(), 'Carga', 'Carga');

	--Literatura na sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1347, 1, now(), 'Carga', 'Carga');

    --Produções textuais
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1313, 2, now(), 'Carga', 'Carga');

    --Mediação e intervenção sociocultural - Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1523, 1, now(), 'Carga', 'Carga');

    --Mediação e intervenção sociocultural - Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1646, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------
do $$
declare 
	gradeId bigint;	
begin

    --EMEFM - 2 Serie integral - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEFM - 2ª Série - Integral', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 7, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 138, 3, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1328, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 139, 2, now(), 'Carga', 'Carga');

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 6, 2, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 2, 3, now(), 'Carga', 'Carga');

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
	values (gradeId, 2, 51, 1, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 53, 2, now(), 'Carga', 'Carga');

	--Língua, Literatura e Cultura dos países de Língua Espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1311, 2, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1350, 1, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1359, 1, now(), 'Carga', 'Carga');

	--Sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1347, 1, now(), 'Carga', 'Carga');

end $$;

---------------------------------------------------//------------------------------------------------------//----------------------------------------
do $$
declare 
	gradeId bigint;	
begin

    --EMEFM - 3 Serie integral - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEFM - 3ª Série - Integral', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 7, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 138, 2, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1328, 1, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 139, 1, now(), 'Carga', 'Carga');

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 6, 1, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 2, 2, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 8, 1, now(), 'Carga', 'Carga');

	--História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 7, 1, now(), 'Carga', 'Carga');

	--Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 98, 1, now(), 'Carga', 'Carga');

	--Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 103, 1, now(), 'Carga', 'Carga');

	--Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 51, 1, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 52, 1, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 53, 2, now(), 'Carga', 'Carga');

	--Língua, Literatura e Cultura dos países de Língua Espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1311, 2, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1350, 1, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1359, 1, now(), 'Carga', 'Carga');

	--Sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1347, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------
do $$
declare 
	gradeId bigint;	
begin

    --EMEFM - 1 Serie - Noturno - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEFM - 1ª Série - Noturno', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 4, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 138, 4, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1328, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 139, 2, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 2, 4, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 8, 3, now(), 'Carga', 'Carga');

	--História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 7, 3, now(), 'Carga', 'Carga');

	--Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 98, 2, now(), 'Carga', 'Carga');

	--Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 103, 2, now(), 'Carga', 'Carga');

	--Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 51, 2, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 53, 2, now(), 'Carga', 'Carga');

	--Língua, Literatura e Cultura dos países de Língua Espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1311, 1, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1350, 2, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1359, 1, now(), 'Carga', 'Carga');

	--Literatura na sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1347, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

    --EMEFM - 2 Serie - Noturno - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEFM - 2ª Série - Noturno', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 4, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 138, 3, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1328, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 139, 1, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 2, 3, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 8, 2, now(), 'Carga', 'Carga');

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
	values (gradeId, 2, 51, 1, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 52, 1, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 53, 2, now(), 'Carga', 'Carga');

	--Língua, Literatura e Cultura dos países de Língua Espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1311, 2, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1350, 1, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1359, 1, now(), 'Carga', 'Carga');

	--Literatura na sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1347, 1, now(), 'Carga', 'Carga');

end $$;

---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

    --EMEFM - 3 Serie - Noturno - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEFM - 3ª Série - Noturno', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 3, 6, 4, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 138, 4, now(), 'Carga', 'Carga');

	--Inglês
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 9, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 139, 1, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 2, 4, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 8, 2, now(), 'Carga', 'Carga');

	--História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 7, 2, now(), 'Carga', 'Carga');

	--Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 98, 2, now(), 'Carga', 'Carga');

	--Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 103, 1, now(), 'Carga', 'Carga');

	--Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 51, 2, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 53, 2, now(), 'Carga', 'Carga');

	--Língua Espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 537, 1, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1312, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

------ EMEBS - 1 Serie Integral - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEBS - 1ª Série - Integral', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

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

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 6, 2, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 218, 2, now(), 'Carga', 'Carga');

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
	values (gradeId, 1, 51, 2, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 53, 2, now(), 'Carga', 'Carga');

	--Expressões Culturais e Artísticas - Artes
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1318, 1, now(), 'Carga', 'Carga');

    --Expressões Culturais e Artísticas - Ed. Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1419, 1, now(), 'Carga', 'Carga');

	--Investigação Ciêntífica e Processos Matemáticos - Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1357, 1, now(), 'Carga', 'Carga');

	--Investigação Ciêntífica e Processos Matemáticos - Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1645, 1, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1350, 2, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1359, 2, now(), 'Carga', 'Carga');

	--Literatura na sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1347, 1, now(), 'Carga', 'Carga');

    --Produções textuais
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1313, 2, now(), 'Carga', 'Carga');

    --Mediação e intervenção sociocultural - Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1523, 1, now(), 'Carga', 'Carga');

    --Mediação e intervenção sociocultural - Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1646, 1, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1432, 2, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

------ EMEBS - 2 Serie Integral - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEBS - 2ª Série - Integral', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 4, 6, 7, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 138, 3, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1328, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 139, 2, now(), 'Carga', 'Carga');

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 6, 2, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 2, 3, now(), 'Carga', 'Carga');

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
	values (gradeId, 2, 51, 1, now(), 'Carga', 'Carga');

	--Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 52, 1, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 53, 2, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 218, 2, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1350, 1, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1359, 1, now(), 'Carga', 'Carga');

	--Sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1347, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

------ EMEBS - 3 Serie Integral - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEBS - 3ª Série - Integral', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 4, 6, 7, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 138, 2, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1328, 1, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 139, 1, now(), 'Carga', 'Carga');

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 6, 1, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 2, 2, now(), 'Carga', 'Carga');

	--Geografia/História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1524, 1, now(), 'Carga', 'Carga');

	--Filosofia/Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1525, 1, now(), 'Carga', 'Carga');

	--Física/Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1526, 1, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 53, 1, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 218, 1, now(), 'Carga', 'Carga');

	--Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1350, 1, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1359, 1, now(), 'Carga', 'Carga');

	--Sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1347, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin
------ EMEBS - 3 Serie Manhã - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEBS - 3ª Série - Manhã', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 4, 6, 5, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 138, 4, now(), 'Carga', 'Carga');

	--Inglês
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 9, 2, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 218, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 139, 2, now(), 'Carga', 'Carga');

	--Educação Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 6, 3, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 2, 3, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 8, 2, now(), 'Carga', 'Carga');

    --História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 7, 2, now(), 'Carga', 'Carga');

	--Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 98, 1, now(), 'Carga', 'Carga');

	--Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 103, 1, now(), 'Carga', 'Carga');

    --Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 51, 2, now(), 'Carga', 'Carga');

    --Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 53, 2, now(), 'Carga', 'Carga');

    --Língua espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 537, 2, now(), 'Carga', 'Carga');

     --Orientação de estudos e projetos
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1293, 3, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1060, 1, now(), 'Carga', 'Carga');

	--Sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1061, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

------ EMEBS - 1 Serie Noturno - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEBS - 1ª Série - Noturno', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 4, 6, 4, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 138, 4, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1328, 2, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 218, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 139, 2, now(), 'Carga', 'Carga');

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
	values (gradeId, 1, 51, 2, now(), 'Carga', 'Carga');

    --Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 53, 2, now(), 'Carga', 'Carga');

    --Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1350, 2, now(), 'Carga', 'Carga');

    --Libras - Formação para estudo e aprofundamento
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1432, 1, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1359, 1, now(), 'Carga', 'Carga');

	--Literatura na sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 1, 1347, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

------ EMEBS - 2 Serie Noturno - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEBS - 2ª Série - Noturno', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 4, 6, 4, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 138, 3, now(), 'Carga', 'Carga');

	--LEM - Língua Inglesa
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1328, 2, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 218, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 139, 1, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 2, 3, now(), 'Carga', 'Carga');

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
	values (gradeId, 2, 51, 1, now(), 'Carga', 'Carga');

    --Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 52, 1, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 53, 1, now(), 'Carga', 'Carga');

    --Projeto de vida
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1350, 1, now(), 'Carga', 'Carga');

    --Libras - Formação para estudo e aprofundamento
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1432, 2, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1359, 1, now(), 'Carga', 'Carga');

	--Sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 2, 1347, 1, now(), 'Carga', 'Carga');

end $$;
---------------------------------------------------//------------------------------------------------------//----------------------------------------

do $$
declare 
	gradeId bigint;	
begin

------ EMEBS - 3 Serie Noturno - 2022
	insert into grade (nome, criado_em, criado_por, criado_rf, inicio_vigencia)
	values('EMEBS - 3ª Série - Noturno', now(), 'Carga', 'Carga', '2022-01-01') returning id into gradeId;

	insert into grade_filtro (grade_id, tipo_escola, modalidade, duracao_turno, criado_em, criado_por, criado_rf)
	values(gradeId, 4, 6, 4, now(), 'Carga', 'Carga');

	--Língua Portuguesa 
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 138, 4, now(), 'Carga', 'Carga');

	--Inglês
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 9, 2, now(), 'Carga', 'Carga');

    --Libras
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 218, 2, now(), 'Carga', 'Carga');

	--Arte
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 139, 2, now(), 'Carga', 'Carga');

	--Matemática
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 2, 3, now(), 'Carga', 'Carga');

	--Geografia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 8, 2, now(), 'Carga', 'Carga');

    --História
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 7, 2, now(), 'Carga', 'Carga');

	--Filosofia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 98, 1, now(), 'Carga', 'Carga');

	--Sociologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 103, 1, now(), 'Carga', 'Carga');

    --Física
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 51, 2, now(), 'Carga', 'Carga');

    --Química
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 52, 2, now(), 'Carga', 'Carga');

	--Biologia
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 53, 2, now(), 'Carga', 'Carga');

    --Língua espanhola
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 537, 2, now(), 'Carga', 'Carga');

	--Tecnologias para Aprendizagem
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1060, 1, now(), 'Carga', 'Carga');

	--Sala de leitura
	insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
	values (gradeId, 3, 1061, 1, now(), 'Carga', 'Carga');

end $$;