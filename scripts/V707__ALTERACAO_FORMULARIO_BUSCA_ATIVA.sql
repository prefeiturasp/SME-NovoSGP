-- CRITÉRIO 1

	do $$
		declare 
			questao record;
			resposta record;
			idQuestaoInserida bigint;
		begin
			for questao in 
				select id, registro_acao_busca_ativa_secao_id, (select Id from questao where nome_componente = 'PROCEDIMENTO_REALIZADO') as questaoid, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf 
				from registro_acao_busca_ativa_questao 
				where questao_id = (select Id from questao where nome_componente = 'PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP')
			loop
				insert into registro_acao_busca_ativa_questao(registro_acao_busca_ativa_secao_id, questao_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
				values (questao.registro_acao_busca_ativa_secao_id, questao.questaoid, questao.criado_em, questao, questao.alterado_em, questao.alterado_por, questao.criado_rf, questao.alterado_rf)
				RETURNING id INTO idQuestaoInserida;
			
				update registro_acao_busca_ativa_questao set excluido = true
				where id = questao.id;
				 
				update registro_acao_busca_ativa_resposta set excluido = true
				where questao_registro_acao_id = questao.id;
				 
				insert into registro_acao_busca_ativa_resposta(questao_registro_acao_id, resposta_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf ) 
				select idQuestaoInserida, (select id from opcao_resposta where questao_id = (select Id from questao where nome_componente = 'PROCEDIMENTO_REALIZADO') and ordem = opr.ordem), 
					 rabar.criado_em, rabar.criado_por, rabar.alterado_em, rabar.alterado_por, rabar.criado_rf, rabar.alterado_rf 
				from registro_acao_busca_ativa_resposta rabar
				inner join opcao_resposta opr on opr.id = rabar.resposta_id 
				where rabar.questao_registro_acao_id = questao.id;
				 
			end loop;
		END; 
	$$;

	--Remover questao complementar
	delete from opcao_questao_complementar
	where opcao_resposta_id in(select id from opcao_resposta 
	where questao_id in(select Id from questao where nome_componente = 'CONSEGUIU_CONTATO_RESP'))
	and questao_complementar_id in(select Id from questao where nome_componente in ('PROCEDIMENTO_REALIZADO', 'PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP'));

	--Excluir opcoes resposta PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP
	update opcao_resposta set excluido = true  
	where questao_id in(select Id from questao where nome_componente = 'PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP');

	--Excluir questao PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP
	update questao set excluido = true
	where id in(select Id from questao where nome_componente = 'PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP');

	--alterar ordem PROCEDIMENTO_REALIZADO 2
	update questao set ORDEM = 2 where nome_componente = 'PROCEDIMENTO_REALIZADO';

	--alterar ordem CONSEGUIU_CONTATO_RESP 3
	update questao set ORDEM = 3 where nome_componente = 'CONSEGUIU_CONTATO_RESP';

-- CRITÉRIO 2
	
	--Remover questão CONTATO_COM_RESPONSAVEL
	update registro_acao_busca_ativa_resposta set excluido = true
	where resposta_Id in (select id from opcao_resposta 
	where questao_id = (select Id from questao where nome_componente = 'CONTATO_COM_RESPONSAVEL'));

	update registro_acao_busca_ativa_questao set excluido = true 
	where questao_id = (select id from questao where nome_componente = 'CONTATO_COM_RESPONSAVEL');

	update questao set excluido = true where nome_componente = 'CONTATO_COM_RESPONSAVEL';
	
	--Remover questão APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA
	update registro_acao_busca_ativa_resposta set excluido = true
	where resposta_Id in (select id from opcao_resposta 
	where questao_id = (select Id from questao where nome_componente = 'APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA'));

	update registro_acao_busca_ativa_questao set excluido = true 
	where questao_id = (select id from questao where nome_componente = 'APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA');

	update questao set excluido = true where nome_componente = 'APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA';

-- CRITÉRIO 3
	
	update opcao_resposta set excluido = true  
	where questao_id = (select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 1, 'Estudante com questões de saúde mental (depressão, ansiedade, etc.)', NOW(), 'SISTEMA', '0');

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 2, 'Estudante em luto por familiar/ responsável falecido', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 3, 'Estudante com doenças crônicas (diabete, câncer, doença do coração, epilepsia, etc.)', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 4, 'Estudante está doente (enjoo, diarreia, vômito, gripe, resfriado, etc.)', NOW(), 'SISTEMA', '0');

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 5, 'Estudante grávida', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 6, 'Estudante é pessoa com deficiência', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 7, 'Estudante está cuidando de familiares', NOW(), 'SISTEMA', '0');

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 8, 'Trabalho infantil (vende bala no farol, pede coisas, cuida de outras crianças, responsável por trabalho doméstico)', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 9, 'Mora na rua', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 10, 'Não tem moradia fixa (ficando temporariamente em casas diferentes)', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 11, 'Responsável justifica que não tem material escolar/uniforme', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 12, 'Falta de transporte escolar', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 13, 'Família em situação de extrema pobreza', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 14, 'Responsável justifica a ausência da estudante por questões relacionadas à menstruação', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 15, 'Estudante não deseja voltar para a escola', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 16, 'Não há um responsável para levar para a escola', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 17, 'Responsável não compreende a obrigatoriedade da frequência escolar', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 18, 'Responsável não quer levar para a escola', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 19, 'Responsável é pessoa com deficiência', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 20, 'Mora distante da escola e apresenta dificuldades para o deslocamento', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 21, 'Violência no território (comunidade/ bairro)', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 22, 'Estudante é vítima de preconceito/ discriminação/ bullying na escola', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 23, 'Estudante faleceu', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 24, 'Adolescente na Fundação Casa (medidas socioeducativas, aplicáveis a adolescentes envolvidos na prática de um ato infracional)', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 25, 'Responsável informa que o bebê/criança/adolescente perdeu a vaga', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 26, 'Viagem', NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values((select Id from questao where nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA'), 27, 'Outros', NOW(), 'SISTEMA', '0');