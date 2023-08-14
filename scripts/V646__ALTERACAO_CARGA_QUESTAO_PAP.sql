do $$
declare
	questaoId bigint;
begin
	SELECT id INTO questaoId FROM questao WHERE nome_componente = 'DIFIC_APRESENTADAS';
	
	UPDATE questao SET dimensao = 12 WHERE Id = questaoId;

    DELETE FROM opcao_resposta WHERE questao_id = questaoId;
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 1, 'Relacionar a situação comunicativa à produção de texto oral', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 2, 'Elaborar e planejar textos orais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 3, 'Planejar e escrever textos conhecidos (de memória e reescritas)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 4, 'Planejar e escrever textos de autoria da ordem do narrar', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 5, 'Planejar e escrever textos de autoria da ordem do argumentar', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 6, 'Planejar e escrever textos de autoria da ordem do relatar', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 7, 'Revisar textos escritos', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 8, 'Escrever textos com coesão e coerência', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 9, 'Utilizar paragrafação e pontuação nas produções textuais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 10, 'Escrever convencionalmente palavras com regularidades de escrita', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 11, 'Escrever convencionalmente algumas palavras com irregularidades de escrita', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 12, 'Eliminar repetições indesejadas no texto, substituindo o referente por outra palavra (nome, pronome, etc...)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 13, 'Ler autonomamente textos de diferentes gêneros, reconhecendo sua finalidade', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 14, 'Realizar antecipações, localizações, inferências (locais e globais) e checagem de informações no texto lido', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 15, 'Ler textos para estudar', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 16, 'Ler textos para revisar', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 17, 'Ler textos para realizar apreciação e réplica', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 18, 'Compreender a função social dos números', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 19, 'Ler, escrever, números naturais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 20, 'Comparar e ordenar números naturais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 21, 'Compor e decompor números naturais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 22, 'Ler, escrever, números racionais (com representação decimal)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 23, 'Comparar e ordenar números racionais (com representação decimal)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 24, 'Ler, escrever, números racionais (com representação fracionária)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 25, 'Comparar e ordenar números racionais (com representação fracionária)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 26, 'Analisar e interpretar situações-problema do campo aditivo com números naturais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 27, 'Resolver situações-problema do campo aditivo com números naturais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 28, 'Analisar, interpretar situações-problema do campo multiplicativo com números naturais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 29, 'Resolver situações-problema do campo multiplicativo com números naturais', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 30, 'Analisar, interpretar situações-problema do campo aditivo com números racionais (com representação decimal)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 31, 'Resolver situações-problema do campo aditivo com números racionais (com representação decimal)', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 32, 'Analisar, interpretar situações-problema do campo multiplicativo com números racionais (com representação decimal)', NOW(), 'SISTEMA', '0');

	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 33, 'Resolver situações-problema do campo multiplicativo com números racionais (com representação decimal)', NOW(), 'SISTEMA', '0');

	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 34, 'Analisar, interpretar situações-problema do campo aditivo com números racionais (com representação fracionária)', NOW(), 'SISTEMA', '0');

	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 35, 'Resolver situações-problema do campo multiplicativo com números racionais (com representação fracionária)', NOW(), 'SISTEMA', '0');

	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 36, 'Identificar padrões numéricos e figurais', NOW(), 'SISTEMA', '0');

	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 37, 'Identificar sequências numéricas ou figurais recursivas', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 38, 'Realizar generalizações', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 39, 'Analisar, interpretar problemas envolvendo regularidades', NOW(), 'SISTEMA', '0');

	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 40, 'Solucionar problemas envolvendo regularidades', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 41, 'Identificar o valor de cédulas e moedas', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 42, 'Realizar operações envolvendo dinheiro', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 43, 'Utilizar o relógio analógico para identificar as horas e minutos', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 44, 'Interpretar as ideias contidas nas situações problema do campo aditivo, utilizando sistema monetário', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 45, 'Resolver situações problema do campo aditivo, utilizando sistema monetário', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 46, 'Interpretar as ideias contidas nas situações problema do campo multiplicativo, utilizando sistema monetário', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 47, 'Resolver situações problema do campo Multiplicativo, utilizando sistema monetário', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 48, 'Mensurar grandezas e medidas com', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 49, 'Localizar pessoas e objetos em representações planas', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 50, 'Movimentar pessoas e objetos em representações planas', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 51, 'Nomear as características dos sólidos geométricos', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 52, 'Identificar as características dos sólidos geométricos', NOW(), 'SISTEMA', '0');
		
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 53, 'Identificar as planificações dos sólidos geométricos', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 54, 'Ler e interpretar dados em tabelas simples e de dupla entrada', NOW(), 'SISTEMA', '0');
	
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	VALUES(questaoId, 55, 'Ler e interpretar dados em e gráficos, de colunas, barras e de setor', NOW(), 'SISTEMA', '0');
end $$;