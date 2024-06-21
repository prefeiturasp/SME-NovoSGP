update questao set nome = 'Qual a hipótese de escrita do estudante?'
where id in (
select q.id from questao q 
inner join questionario q2 on q2.id = q.questionario_id 
where q.nome_componente = 'HIPOTESE_ESCRITA' and q2.tipo = '9');