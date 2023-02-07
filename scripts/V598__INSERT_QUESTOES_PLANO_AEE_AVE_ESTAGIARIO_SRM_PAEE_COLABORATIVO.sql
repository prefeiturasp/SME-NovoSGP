do $$
declare 
	questaoId bigint;
	
begin

-- Questão Possui AVE
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf,nome_componente)
values((select id from questionario where nome = 'Questionário Plano AEE'), 11, 'Assistência de AVE(Auxiliar de vida escola)', '', true, 3, '', NOW(), 'SISTEMA', '0','POSSUI_AVE')
RETURNING id INTO questaoId;

-- Resposta Questão Possui AVE
insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Sim', '', NOW(), 'SISTEMA', '0');

insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');

-- Questão possui estagiário na turma
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf,nome_componente)
values((select id from questionario where nome = 'Questionário Plano AEE'), 12, 'Assistência de estagiário na turma', '', true, 3, '', NOW(), 'SISTEMA', '0','POSSUI_ESTAGIARIO_TURMA')
RETURNING id INTO questaoId;


-- Resposta Questão possui estagiário na turma
insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Sim', '', NOW(), 'SISTEMA', '0'); 

insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');


-- Questão Informações da matrícula em SRM (Sem resposta é uma tabela na tela com dados do EOL)
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf,somente_leitura,nome_componente)
values((select id from questionario where nome = 'Questionário Plano AEE'), 13, 'Informações da matrícula em SRM', '', false, 20, '', NOW(), 'SISTEMA', '0',true,'MATRICULA_SRM')
RETURNING id INTO questaoId;

-- Resposta Questão Informações da matrícula em SRM (Sem resposta é uma tabela na tela com dados do EOL)
insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
values(questaoId, 2, '[{"DreUe":"","TurmaTurno":"","ComponenteCurricular":""}]', '', NOW(), 'SISTEMA', '0');


-- Questão Possui assistência do PAEE Colaborativo
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf,nome_componente)
values((select id from questionario where nome = 'Questionário Plano AEE'), 14, 'Possui assistência do PAEE Colaborativo', '', true, 3, '', NOW(), 'SISTEMA', '0','POSSUI_PAEE_COLABORATIVO')
RETURNING id INTO questaoId;

-- Resposta Questão Possui assistência do PAEE Colaborativo
insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Sim', '', NOW(), 'SISTEMA', '0');

insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');

end $$;
