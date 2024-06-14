update questao set nome = 'Nacionalidade', nome_componente = 'NACIONALIDADE', tipo = 1, obrigatorio = false
where 
nome_componente = 'MIGRANTE'
and questionario_id in (select id from questionario where tipo = 9);

update mapeamento_estudante_resposta 
set texto = '', resposta_id = null 
where resposta_id in 
(select id from opcao_resposta 
 where questao_id = (select id from questao 
 				   	 where nome_componente = 'NACIONALIDADE'
					 and questionario_id in (select id from questionario where tipo = 9)));	

delete from opcao_resposta 
where questao_id = 
(select id from questao 
	where nome_componente = 'NACIONALIDADE'
	and questionario_id in (select id from questionario where tipo = 9));