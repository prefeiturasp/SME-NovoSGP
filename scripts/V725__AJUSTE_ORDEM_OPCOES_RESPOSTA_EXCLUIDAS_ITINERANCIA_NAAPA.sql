update opcao_resposta set ordem = 10
where nome = 'Atendimento não presencial' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);
update opcao_resposta set ordem = 11
where nome = 'Grupo Focal' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);
update opcao_resposta set ordem = 12
where nome = 'Projeto Tecer' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);
update opcao_resposta set ordem = 13
where nome = 'Reunião de Rede Macro (formada pelo território)' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);
update opcao_resposta set ordem = 14
where nome = 'Reunião de Rede Micro (formada pelo NAAPA)' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);
update opcao_resposta set ordem = 15
where nome = 'Reunião de Rede Micro na UE' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);
update opcao_resposta set ordem = 16
where nome = 'Reunião em Horários Coletivos' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);
update opcao_resposta set ordem = 17
where nome = 'Reunião Compartilhada' and questao_id = (select questao.id from questao 
					inner join questionario on questionario.id = questao.questionario_id 
					where nome_componente ='TIPO_DO_ATENDIMENTO' and questionario.tipo = 5);			