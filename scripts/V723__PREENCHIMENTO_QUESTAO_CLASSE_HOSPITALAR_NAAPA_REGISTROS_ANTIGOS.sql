DO $$
DECLARE
    naapasUpdate record;
	questoes record;
    encaminhamento_secao_id int;
	encaminhamento_questao_id int;
	encaminhamento_resposta_id int;
BEGIN
    FOR naapasUpdate IN
			select en.id,  
			ens2.id as encaminhamento_secao_id,
			sen.questionario_id 
			from encaminhamento_naapa en 
			inner join encaminhamento_naapa_secao  ens2 on ens2.encaminhamento_naapa_id = en.id and not ens2.excluido
 			inner join secao_encaminhamento_naapa sen on sen.id = ens2.secao_encaminhamento_id and sen.nome_componente in ('INFORMACOES_ESTUDANTE')
			where not en.excluido
			order by en.criado_em
    LOOP
    	FOR questoes IN
            select q.id as questao_id, q.nome_componente, q.questionario_id, or2.id as opcao_resposta_id 
			from questao q  
			inner join opcao_resposta or2 on or2.questao_id = q.id and or2.nome = 'Não'
			where q.nome_componente = 'ESTA_EM_CLASSE_HOSPITALAR' and q.questionario_id = naapasUpdate.questionario_id
			order by q.questionario_id, q.ordem
		LOOP
			encaminhamento_questao_id := (select enq.id from encaminhamento_naapa_questao enq
											where enq.questao_id = questoes.questao_id
												and enq.encaminhamento_naapa_secao_id = naapasUpdate.encaminhamento_secao_id
												and not enq.excluido);
		 
			if (encaminhamento_questao_id IS NULL) then
			   INSERT INTO public.encaminhamento_naapa_questao
					(encaminhamento_naapa_secao_id, questao_id, criado_em, criado_por, criado_rf)
			   VALUES(naapasUpdate.encaminhamento_secao_id, questoes.questao_id, NOW(), 'SISTEMA', 'SISTEMA') RETURNING id INTO encaminhamento_questao_id;
			end if;   
			
			encaminhamento_resposta_id := (select max(enr.id) from encaminhamento_naapa_resposta enr 
											where enr.questao_encaminhamento_id = encaminhamento_questao_id
												and not enr.excluido);
			
			if (encaminhamento_resposta_id IS NULL) then
			   INSERT INTO public.encaminhamento_naapa_resposta
				    (questao_encaminhamento_id, resposta_id, texto, criado_em, criado_por, criado_rf)
			   VALUES(encaminhamento_questao_id, questoes.opcao_resposta_id, '', NOW(), 'SISTEMA', 'SISTEMA');
			else
				update public.encaminhamento_naapa_resposta set resposta_id = questoes.opcao_resposta_id 
					where id = encaminhamento_resposta_id
					and resposta_id is null
					and (texto is null or texto = '');
			end if;  
			
		END LOOP;
     
		   
	END LOOP;
END $$;