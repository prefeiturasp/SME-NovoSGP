DO $$
DECLARE
    questoesUpdate record;
    opcaoQuestaoIdExistente int;
BEGIN
    FOR questoesUpdate IN
            select q.id, q.nome_componente, q2.tipo from questao q 
			inner join questionario q2 on q2.id = q.questionario_id 
			inner join secao_encaminhamento_naapa sen on sen.questionario_id = q2.id
			where q2.tipo in (5, 7) and sen.ordem = 2 
			and q.tipo in (4, 9)
			and not q.obrigatorio
			and not exists (select id from opcao_questao_complementar oqc where oqc.questao_complementar_id = q.id)
			order by q2.nome, q.ordem
    LOOP
    	update questao set obrigatorio = true where id = questoesUpdate.id and questoesUpdate.tipo = 5;

        opcaoQuestaoIdExistente := (select id from opcao_resposta where questao_id = questoesUpdate.id and nome = 'Não se aplica');
		if (opcaoQuestaoIdExistente IS NULL) then
		   insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
				values(questoesUpdate.id, (select max(ordem)+1 from opcao_resposta where questao_id = questoesUpdate.id), 'Não se aplica', NOW(), 'SISTEMA', '0');
     	end if;
     
		   
	END LOOP;
END $$;