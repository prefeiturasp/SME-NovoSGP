do $$
declare 
	questaoId bigint;
	
begin

-- Questão Possui AVE
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
values((select id from questionario where nome = 'Questionário Plano AEE'), 11, 'Assistência de AVE(Auxiliar de vida escola)', '', true, 3, '', NOW(), 'SISTEMA', '0')
RETURNING id INTO questaoId;

-- Resposta Questão Possui AVE
insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Sim', '', NOW(), 'SISTEMA', '0');

insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');

-- Questão possui estagiário na turma
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
values((select id from questionario where nome = 'Questionário Plano AEE'), 12, 'Assistência de estagiário na turma', '', true, 3, '', NOW(), 'SISTEMA', '0')
RETURNING id INTO questaoId;


-- Resposta Questão possui estagiário na turma
insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Sim', '', NOW(), 'SISTEMA', '0');

insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');


-- Questão Informações da matrícula em SRM (Sem resposta é uma tabela na tela com dados do EOL)
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
values((select id from questionario where nome = 'Questionário Plano AEE'), 13, 'Informações da matrícula em SRM', '', true, 20, '', NOW(), 'SISTEMA', '0');


-- Questão Possui assistência do PAEE Colaborativo
insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
values((select id from questionario where nome = 'Questionário Plano AEE'), 13, 'Possui assistência do PAEE Colaborativo', '', true, 3, '', NOW(), 'SISTEMA', '0')
RETURNING id INTO questaoId;

-- Resposta Questão Possui assistência do PAEE Colaborativo
insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Sim', '', NOW(), 'SISTEMA', '0');

insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
 values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');

end $$;