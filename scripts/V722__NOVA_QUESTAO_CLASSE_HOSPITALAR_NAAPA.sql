DO $$
DECLARE
    questoesUpdate record;
	questaoId bigint;
	questaoIdExistente bigint;
BEGIN
    questaoIdExistente := (select q.id  
							from questao q 
							inner join questionario q2 on q2.id = q.questionario_id 
							where q2.tipo in (7) and q.nome_componente = 'ESTA_EM_CLASSE_HOSPITALAR');
	if (questaoIdExistente IS NULL) then
		insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
			values((select q.id from questionario q 
					inner join secao_encaminhamento_naapa sen on sen.questionario_id = q.id 
					where q.tipo = 7 and sen.ordem = 1 and sen.nome_componente = 'INFORMACOES_ESTUDANTE')
					, (select max(q.ordem)  +1
						from questao q 
						inner join questionario q2 on q2.id = q.questionario_id 
						inner join secao_encaminhamento_naapa sen on sen.questionario_id = q2.id
						where q2.tipo in (7) and sen.ordem = 1 and sen.nome_componente = 'INFORMACOES_ESTUDANTE'
						and not exists (select id from opcao_questao_complementar oqc where oqc.questao_complementar_id = q.id))
					, 'Está em classe hospitalar', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, '', 'ESTA_EM_CLASSE_HOSPITALAR', false)
			RETURNING id INTO questaoId;
		insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
		insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
	end if;	

    FOR questoesUpdate IN
            select q.id, q.ordem, q.nome_componente, q.questionario_id 
			from questao q 
			inner join questionario q2 on q2.id = q.questionario_id 
			inner join secao_encaminhamento_naapa sen on sen.questionario_id = q2.id
			where q2.tipo in (5) and sen.ordem = 1 and sen.nome_componente = 'INFORMACOES_ESTUDANTE'
			and not exists (select id from opcao_questao_complementar oqc where oqc.questao_complementar_id = q.id)
			and q.ordem >= 11
			and not exists (select q.id  
							from questao q 
							inner join questionario q2 on q2.id = q.questionario_id 
							where q2.tipo in (5) and q.nome_componente = 'ESTA_EM_CLASSE_HOSPITALAR')
			order by q2.nome, q.ordem
    LOOP
		if (questoesUpdate.ordem = 11) then
			update questao set dimensao = 6 where id = questoesUpdate.id;
			
			insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
				values(questoesUpdate.questionario_id
						, 12
						, 'Está em classe hospitalar', '', true, 3, '', NOW(), 'SISTEMA', '0', 6, '', 'ESTA_EM_CLASSE_HOSPITALAR', false)
				RETURNING id INTO questaoId;
			insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
				values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
			insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
				values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
     	ELSE
			update questao set ordem = questoesUpdate.ordem +1 where id = questoesUpdate.id;	   
		end if;
		
		
	END LOOP;
END $$;