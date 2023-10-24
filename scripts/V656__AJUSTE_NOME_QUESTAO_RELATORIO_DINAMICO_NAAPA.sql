update  questao set nome = 'Responsável/Cuidador é migrante'
where id = 
(select q.id from questao q
join questionario q2 on q.questionario_id = q2.id
where q2.tipo = 7 and q.nome_componente = 'RESPONSAVEL_MIGRANTE');


update  questao set nome = 'Criança/Estudante é migrante (autodenominação)'
where id = 
(select q.id from questao q
join questionario q2 on q.questionario_id = q2.id
where q2.tipo = 7 and q.nome_componente = 'ESTUDANTE_MIGRANTE');