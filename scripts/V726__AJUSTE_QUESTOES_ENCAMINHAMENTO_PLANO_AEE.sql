do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	questaoComplementarId bigint;
	opcaoRespostaId bigint;
	questaoIdExistente bigint;
begin
	update questao set nome = 'Com base nas observações e no estudo de caso, por qual(is) motivo(s) a sua unidade educacional está encaminhando o estudante ao Atendimento Educacional Especializado (AEE)?'
	where id in (select q2.id from questao q2
		inner join questionario q on q.id = q2.questionario_id
		where q.tipo = 1 and q.nome like '%Seção 2%' 
		and q2.id not in (select oqc.questao_complementar_id from opcao_questao_complementar oqc)
		and q2.nome_componente = 'PORQUE'
		order by q2.ordem);
		
	questionarioId := (select id from questionario q 
						where q.tipo = 1 and q.nome like '%Etapa 3%');	
		
	questaoIdExistente := (select q.id  
							from questao q 
							where q.questionario_id = questionarioId and q.nome_componente = 'OUTRAS_BARREIRAS');
	if (questaoIdExistente IS NULL) then
		insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente, dimensao)
			values(questionarioId, 4, 'Outras/Nenhuma - Justifique', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OUTRAS_BARREIRAS', 12)
			RETURNING id INTO questaoComplementarId;
		
		questaoId := (select q2.id from questao q2
						where q2.questionario_id = questionarioId  
						and q2.nome_componente = 'BARREIRAS_IDENTIFICADAS');
		insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
			values(questaoId, 4, 'Outras/Nenhuma', '', NOW(), 'SISTEMA', '0')
			RETURNING id INTO opcaoRespostaId;

		insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
			values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	end if;	
	
	questionarioId := (select id from questionario q 
						where q.tipo = 2 and q.nome like '%Plano AEE%');		
	
	update questao set nome = 'Possui assistência de AVE (Auxiliar de vida escolar)'
		where id in (select q2.id from questao q2
			where q2.questionario_id = questionarioId
			and q2.id not in (select oqc.questao_complementar_id from opcao_questao_complementar oqc)
			and q2.nome_componente = 'POSSUI_AVE'
			order by q2.ordem);
		
	update questao set nome = 'Possui assistência de estagiário na turma'
		where id in (select q2.id from questao q2
			where q2.questionario_id = questionarioId
			and q2.id not in (select oqc.questao_complementar_id from opcao_questao_complementar oqc)
			and q2.nome_componente = 'POSSUI_ESTAGIARIO_TURMA'
			order by q2.ordem);	
			
	questaoIdExistente := (select q.id  
							from questao q 
							where q.questionario_id = questionarioId and q.nome_componente = 'AVALIACAO_REESTRUTURACAO_PLANO_BIMESTRE');
	if (questaoIdExistente IS NULL) then					
		insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente, dimensao)
				values(questionarioId, 14, 'Registre aqui a avaliação e reestruturação do plano no bimestre', '', false, 2, '', NOW(), 'SISTEMA', '0', 'AVALIACAO_REESTRUTURACAO_PLANO_BIMESTRE', 12);
	end if;	
end $$;
		