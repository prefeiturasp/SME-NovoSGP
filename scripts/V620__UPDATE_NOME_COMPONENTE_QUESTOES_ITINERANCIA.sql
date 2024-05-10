--Questionário Registro Itinerância
update questao set nome_componente = 'ACOMPANHAMENTO_SITUACAO_GERAL' where id =  (select q2.id from questao q2 
     inner join questionario q3 on q3.id = q2.questionario_id 
     where q2.tipo = 2 and q2.nome = 'Acompanhamento da situação' and q3.tipo in (3));
     
update questao set nome_componente = 'ENCAMINHAMENTOS_GERAL' where id =  (select q2.id from questao q2 
     inner join questionario q3 on q3.id = q2.questionario_id 
     where q2.tipo = 2 and q2.nome = 'Encaminhamentos' and q3.tipo in (3));    
     
--Questionário Registro Itinerância do Aluno    
update questao set nome_componente = 'ACOMPANHAMENTO_SITUACAO_ALUNO' where id =  (select q2.id from questao q2 
     inner join questionario q3 on q3.id = q2.questionario_id 
     where q2.tipo = 2 and q2.nome = 'Acompanhamento da situação' and q3.tipo in (4));
     
update questao set nome_componente = 'ENCAMINHAMENTOS_ALUNO' where id =  (select q2.id from questao q2 
     inner join questionario q3 on q3.id = q2.questionario_id 
     where q2.tipo = 2 and q2.nome = 'Encaminhamentos' and q3.tipo in (4));        

update questao set nome_componente = 'DESCRITIVO_ESTUDANTE' where id =  (select q2.id from questao q2 
     inner join questionario q3 on q3.id = q2.questionario_id 
     where q2.tipo = 2 and q2.nome = 'Descritivo do estudante' and q3.tipo in (4));    