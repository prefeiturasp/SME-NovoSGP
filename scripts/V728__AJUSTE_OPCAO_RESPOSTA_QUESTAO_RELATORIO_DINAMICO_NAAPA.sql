do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	opcaoRespostaId bigint;
begin	
	questionarioId := (select q.id from questionario q 
						where q.tipo = 7 and q.nome = 'Questionário Relatório Dinâmico Encaminhamento NAAPA');	
		
	questaoId := (select q.id  
							from questao q 
							where q.questionario_id = 45 and q.nome_componente = 'GRUPO_ETNICO');
						
	opcaoRespostaId := (select or2.id from opcao_resposta or2 
							where or2.questao_id = questaoId and ordem = 7);
	if (opcaoRespostaId IS NULL) then
		insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
			values(questaoId, 7, 'Preta', '', NOW(), 'SISTEMA', '0')
			RETURNING id INTO opcaoRespostaId;
	end if;	
end $$;