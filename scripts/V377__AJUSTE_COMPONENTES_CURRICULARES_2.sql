delete from grade_disciplina where grade_id = (select id from grade where nome = 'EMEFM - 1ª Série - Integral') and componente_curricular_id = 1314;
delete from grade_disciplina where grade_id = (select id from grade where nome = 'EMEFM - 1ª Série - Integral') and componente_curricular_id = 1351;

insert into grade_disciplina (grade_id, ano, componente_curricular_id, quantidade_aulas, criado_em, criado_por, criado_rf)
values ((select id from grade where nome = 'EMEFM - 1ª Série - Integral'), 1, 1351, 1, now(), 'Carga', 'Carga');

update componente_curricular set grupo_matriz_id = 5 where id in (1351,1357,1432);
