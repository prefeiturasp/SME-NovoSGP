-- Data de criação: 25/03/2021
-- Descriçãoo: Insere prioridade para o perfil Comunicados DRE

insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 380,2,'Comunicados DRE','63e1e074-37d6-e911-abd6-f81654fe895d',now(),'Carga','Carga'
where not exists(select *  from public.prioridade_perfil where codigo_perfil = '63e1e074-37d6-e911-abd6-f81654fe895d');