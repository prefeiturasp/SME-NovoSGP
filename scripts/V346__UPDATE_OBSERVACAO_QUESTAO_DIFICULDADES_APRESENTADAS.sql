update questao q 
	set observacao = 'Quais avanços foram observados? (Descreva detalhadamente os objetivos e atividades realizadas com o estudante, observações e avanços)'
where ordem = 6 and questionario_id = (select questionario_id from secao_encaminhamento_aee where etapa = 1 and ordem = 2)
