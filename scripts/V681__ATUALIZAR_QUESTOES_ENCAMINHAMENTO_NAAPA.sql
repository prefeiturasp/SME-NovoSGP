-- Critério 1

update secao_encaminhamento_naapa set nome = 'Apoio e Acompanhamento' where nome_componente = 'QUESTOES_ITINERACIA' and nome = 'Itinerância';

-- Critério 2

update questao set nome = 'Modalidade de atenção' where nome_componente ='TIPO_DO_ATENDIMENTO' and nome = 'Tipo de atendimento';  

insert into opcao_resposta(questao_id, ordem, nome, excluido, criado_em, criado_por, criado_rf)
select id, 5, 'Atendimento Remoto', false, now(), 'SISTEMA', '0' 
from questao 
where nome_componente = 'TIPO_DO_ATENDIMENTO' 
and not exists (select 1 from opcao_resposta where questao_id = questao.id and nome = 'Atendimento Remoto'); 

update opcao_resposta set excluido = true
where questao_id = (select id from questao where nome_componente ='TIPO_DO_ATENDIMENTO')
and not excluido
and not nome in('Itinerância', 'Grupo de Trabalho NAAPA', 'Atendimento pedagógico domiciliar', 'Atendimento presencial na DRE', 'Atendimento Remoto');

update opcao_resposta set ordem = 1 
where questao_id = (select id from questao where nome_componente ='TIPO_DO_ATENDIMENTO')
and nome = 'Itinerância'
and ordem <> 1;

update opcao_resposta set ordem = 2
where questao_id = (select id from questao where nome_componente ='TIPO_DO_ATENDIMENTO')
and nome = 'Grupo de Trabalho NAAPA'
and ordem <> 2;

update opcao_resposta set ordem = 3
where questao_id = (select id from questao where nome_componente ='TIPO_DO_ATENDIMENTO')
and nome = 'Atendimento pedagógico domiciliar'
and ordem <> 3;

update opcao_resposta set ordem = 4
where questao_id = (select id from questao where nome_componente ='TIPO_DO_ATENDIMENTO')
and nome = 'Atendimento presencial na DRE'
and ordem <> 4;

-- Critério 3

update questao set tipo = 9 where nome_componente = 'PROCEDIMENTO_DE_TRABALHO' and tipo = 4;

insert into opcao_resposta(questao_id, ordem, nome, excluido, criado_em, criado_por, criado_rf)
select id, 10, 'Grupo focal', false, now(), 'SISTEMA', '0' 
from questao 
where nome_componente = 'PROCEDIMENTO_DE_TRABALHO' 
and not exists (select 1 from opcao_resposta where questao_id = questao.id and nome = 'Grupo focal'); 

insert into opcao_resposta(questao_id, ordem, nome, excluido, criado_em, criado_por, criado_rf)
select id, 11, 'Reunião compartilhada', false, now(), 'SISTEMA', '0' 
from questao 
where nome_componente ='PROCEDIMENTO_DE_TRABALHO' 
and not exists (select 1 from opcao_resposta where questao_id = questao.id and nome = 'Reunião compartilhada'); 

insert into opcao_resposta(questao_id, ordem, nome, excluido, criado_em, criado_por, criado_rf)
select id, 12, 'Reunião de Rede Macro (formada pelo território)', false, now(), 'SISTEMA', '0' 
from questao 
where nome_componente ='PROCEDIMENTO_DE_TRABALHO' 
and not exists (select 1 from opcao_resposta where questao_id = questao.id and nome = 'Reunião de Rede Macro (formada pelo território)'); 

insert into opcao_resposta(questao_id, ordem, nome, excluido, criado_em, criado_por, criado_rf)
select id, 13, 'Reunião de Rede Micro (formada pelo NAAPA)', false, now(), 'SISTEMA', '0' 
from questao 
where nome_componente ='PROCEDIMENTO_DE_TRABALHO' 
and not exists (select 1 from opcao_resposta where questao_id = questao.id and nome = 'Reunião de Rede Micro (formada pelo NAAPA)'); 

insert into opcao_resposta(questao_id, ordem, nome, excluido, criado_em, criado_por, criado_rf)
select id, 14, 'Reunião de Rede Micro na UE', false, now(), 'SISTEMA', '0' 
from questao 
where nome_componente ='PROCEDIMENTO_DE_TRABALHO' 
and not exists (select 1 from opcao_resposta where questao_id = questao.id and nome = 'Reunião de Rede Micro na UE'); 

insert into opcao_resposta(questao_id, ordem, nome, excluido, criado_em, criado_por, criado_rf)
select id, 15, 'Reunião em Horários Coletivos', false, now(), 'SISTEMA', '0' 
from questao 
where nome_componente ='PROCEDIMENTO_DE_TRABALHO' 
and not exists (select 1 from opcao_resposta where questao_id = questao.id and nome = 'Reunião em Horários Coletivos');