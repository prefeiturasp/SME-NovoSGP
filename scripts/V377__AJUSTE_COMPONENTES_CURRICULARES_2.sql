update grade_disciplina set componente_curricular_id = 1351 where componente_curricular_id = 1314 and grade_id in (select id from grade where nome in 
('EMEFM - 1ª Série - Integral'));