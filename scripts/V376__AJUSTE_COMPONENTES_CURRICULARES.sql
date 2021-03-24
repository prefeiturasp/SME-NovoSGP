delete from grade_disciplina where grade_id = (select id from grade where nome = 'EMEFM - 1ª Série - Noturno') and componente_curricular_id = 98;

--Filosofia
insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
values ((select id from grade where nome = 'EMEFM - 1ª Série - Noturno'), 1, 98, 1, now(), 'Carga', 'Carga');

--Atualiza o id do componente curricular de física
update grade_disciplina set componente_curricular_id = 51 where componente_curricular_id = 54 and grade_id in (select id from grade where nome in 
('EMEFM - 1ª Série - Integral','EMEFM - 2ª Série - Integral Turno 7h','EMEFM - 1ª Série - Noturno','EMEBS - 1ª Série - Integral','EMEBS - 1ª Série - Noturno'));

--Atualiza o id do componente curricular de Língua, Literatura e Cultura dos países de Língua Espanhola
update grade_disciplina set componente_curricular_id = 1311 where componente_curricular_id = 1346 and grade_id in (select id from grade where nome in 
('EMEFM - 1ª Série - Integral','EMEFM - 2ª Série - Integral Turno 7h'));

--Atualiza o id do componente curricular de Expressões Culturais e Artísticas
update grade_disciplina set componente_curricular_id = 1318 where componente_curricular_id = 1340 and grade_id in (select id from grade where nome in 
('EMEFM - 1ª Série - Integral','EMEBS - 1ª Série - Integral','EMEBS - 1ª Série - Noturno'));

--Atualiza o id do componente curricular de Investigação Ciêntífica e Processos Matemáticos
update grade_disciplina set componente_curricular_id = 1428 where componente_curricular_id = 1314 and grade_id in (select id from grade where nome in 
('EMEBS - 1ª Série - Integral'));