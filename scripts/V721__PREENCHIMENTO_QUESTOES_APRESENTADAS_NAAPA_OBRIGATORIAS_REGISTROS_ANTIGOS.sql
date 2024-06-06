DO $$
DECLARE
    naapasUpdate record;
	questoes record;
    encaminhamento_secao_id int;
	encaminhamento_questao_id int;
	encaminhamento_resposta_id int;
BEGIN
    FOR naapasUpdate IN
            with secoes as (
				select sen.id as secao_id, q.id as questionario_id, sen.nome_componente
				from secao_encaminhamento_naapa sen 
				inner join questionario q on q.id = sen.questionario_id
				where q.tipo = 5 and sen.nome_componente in ('QUESTOES_APRESENTADAS_INFANTIL', 'QUESTOES_APRESENTADAS_FUNDAMENTAL')
			)
			select en.id, t.modalidade_codigo, s.secao_id, s.questionario_id, s.nome_componente, 
			(select max(ens2.id) from encaminhamento_naapa_secao  ens2
			 where ens2.secao_encaminhamento_id = s.secao_id 
			 and ens2.encaminhamento_naapa_id = en.id and not ens2.excluido) as encaminhamento_secao_id
			from encaminhamento_naapa en 
			inner join turma t on t.id = en.turma_id
			inner join secoes s on s.nome_componente = (case when t.modalidade_codigo = 1 then 'QUESTOES_APRESENTADAS_INFANTIL' else 'QUESTOES_APRESENTADAS_FUNDAMENTAL' end)
			where not en.excluido
			order by t.ano_letivo, en.criado_em
    LOOP
		if (naapasUpdate.encaminhamento_secao_id IS NULL) then
		   INSERT INTO public.encaminhamento_naapa_secao
			(encaminhamento_naapa_id, secao_encaminhamento_id, concluido, criado_em, criado_por, criado_rf)
			VALUES(naapasUpdate.id, naapasUpdate.secao_id, true, NOW(), 'SISTEMA', 'SISTEMA') RETURNING id INTO encaminhamento_secao_id;
		else
			encaminhamento_secao_id := naapasUpdate.encaminhamento_secao_id;
			update public.encaminhamento_naapa_secao set concluido = true where id = naapasUpdate.encaminhamento_secao_id;
		end if;
		
    	FOR questoes IN
            select or2.id as opcao_resposta_id, q.id as questao_id, q.nome_componente, q.questionario_id 
			from opcao_resposta or2 
			inner join questao q on q.id = or2.questao_id 
			inner join questionario q2 on q2.id = q.questionario_id 
			where or2.nome = 'Não se aplica' and q2.tipo = 5 and q2.id = naapasUpdate.questionario_id
			order by q.questionario_id, q.ordem
		LOOP
			encaminhamento_questao_id := (select enq.id from encaminhamento_naapa_questao enq
											where enq.questao_id = questoes.questao_id
												and enq.encaminhamento_naapa_secao_id = encaminhamento_secao_id
												and not enq.excluido);
		 
			if (encaminhamento_questao_id IS NULL) then
			   INSERT INTO public.encaminhamento_naapa_questao
					(encaminhamento_naapa_secao_id, questao_id, criado_em, criado_por, criado_rf)
			   VALUES(encaminhamento_secao_id, questoes.questao_id, NOW(), 'SISTEMA', 'SISTEMA') RETURNING id INTO encaminhamento_questao_id;
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