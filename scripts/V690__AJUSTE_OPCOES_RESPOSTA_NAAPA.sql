update opcao_resposta set excluido = true
where questao_id = (select id from questao where nome_componente ='PROCEDIMENTO_DE_TRABALHO')
and not excluido and ordem = 16;

update opcao_resposta set excluido = true
where questao_id = (select id from questao where nome_componente ='TIPO_DO_ATENDIMENTO')
and nome = 'Atendimento não presencial'
and ordem = 1;