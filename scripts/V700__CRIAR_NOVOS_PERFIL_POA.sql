--Remover perfil POA atual
delete from prioridade_perfil where codigo_perfil = '3fe1e074-37d6-e911-abd6-f81654fe895d';

--POA Alfabetização
insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 393,3,'POA Alfabetização','2e89cf10-e42b-476f-8673-2dfbeeee3cd0',now(),'Carga','Carga'
where not exists(select 1 from public.prioridade_perfil where codigo_perfil = '2e89cf10-e42b-476f-8673-2dfbeeee3cd0');
--POA Língua Portuguesa
insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 394,3,'POA Língua Portuguesa','57a7b9ab-8e61-4093-b692-a0bb1f9f46bd',now(),'Carga','Carga'
where not exists(select 1 from public.prioridade_perfil where codigo_perfil = '57a7b9ab-8e61-4093-b692-a0bb1f9f46bd');
--POA Matemática
insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 395,3,'POA Matemática','cf181fd4-dd30-47cf-a97d-57e602fd8d10',now(),'Carga','Carga'
where not exists(select 1 from public.prioridade_perfil where codigo_perfil = 'cf181fd4-dd30-47cf-a97d-57e602fd8d10');
--POA Humanas
insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 396,3,'POA Humanas','2c7ced81-7109-4276-9262-5c56efd8992f',now(),'Carga','Carga'
where not exists(select 1 from public.prioridade_perfil where codigo_perfil = '2c7ced81-7109-4276-9262-5c56efd8992f');
--POA Naturais
insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 397,3,'POA Naturais','3104735d-c369-4710-ae64-bca37bc78f3b',now(),'Carga','Carga'
where not exists(select 1 from public.prioridade_perfil where codigo_perfil = '3104735d-c369-4710-ae64-bca37bc78f3b');