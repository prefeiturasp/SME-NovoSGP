-- CRITÉRIO 1

	do $$
		declare 
			questao record;
			resposta record;
			idQuestaoInserida bigint;
		begin
			for questao in 
				select id, registro_acao_busca_ativa_secao_id, (select Id from questao where nome_componente = 'OBS_GERAL') as questaoid, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf 
				from registro_acao_busca_ativa_questao 
				where questao_id = (select Id from questao where nome_componente = 'OBS_GERAL_NAO_CONTATOU_RESP')
				  and not excluido
			loop
				insert into registro_acao_busca_ativa_questao(registro_acao_busca_ativa_secao_id, questao_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
				values (questao.registro_acao_busca_ativa_secao_id, questao.questaoid, questao.criado_em, questao, questao.alterado_em, questao.alterado_por, questao.criado_rf, questao.alterado_rf)
				RETURNING id INTO idQuestaoInserida;
			
				update registro_acao_busca_ativa_questao set excluido = true
				where id = questao.id;
				 
				update registro_acao_busca_ativa_resposta set excluido = true
				where questao_registro_acao_id = questao.id;
				 
				insert into registro_acao_busca_ativa_resposta(questao_registro_acao_id, texto, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf ) 
				select idQuestaoInserida, rabar.texto, rabar.criado_em, rabar.criado_por, rabar.alterado_em, rabar.alterado_por, rabar.criado_rf, rabar.alterado_rf 
				from registro_acao_busca_ativa_resposta rabar
				where rabar.questao_registro_acao_id = questao.id;
				 
			end loop;
		END; 
	$$;

	--Remover questao complementar
	delete from opcao_questao_complementar
	where questao_complementar_id in(select Id from questao where nome_componente = 'OBS_GERAL_NAO_CONTATOU_RESP')

	--Excluir questao OBS_GERAL_NAO_CONTATOU_RESP
	update questao set excluido = true
	where id in(select Id from questao where nome_componente = 'OBS_GERAL_NAO_CONTATOU_RESP');
