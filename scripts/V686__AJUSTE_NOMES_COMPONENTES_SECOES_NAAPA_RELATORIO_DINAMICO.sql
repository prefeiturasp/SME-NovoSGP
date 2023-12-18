update secao_encaminhamento_naapa set nome = 'Informações do Estudante'
where id = (select sen.id from secao_encaminhamento_naapa sen 
inner join questionario q on q.id = sen.questionario_id 
where sen.nome_componente = 'INFORMACOES_ESTUDANTE' and q.tipo = 7);

update secao_encaminhamento_naapa set nome = 'Questões apresentadas - Somente Infantil'
where id = (select sen.id from secao_encaminhamento_naapa sen 
inner join questionario q on q.id = sen.questionario_id 
where sen.nome_componente = 'QUESTOES_APRESENTADAS_INFANTIL' and q.tipo = 7);

update secao_encaminhamento_naapa set nome = 'Questões apresentadas - Todas exceto Infantil'
where id = (select sen.id from secao_encaminhamento_naapa sen 
inner join questionario q on q.id = sen.questionario_id 
where sen.nome_componente = 'QUESTOES_APRESENTADAS_FUNDAMENTAL' and q.tipo = 7);