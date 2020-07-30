-- Data de criação: 27/07/2020
-- Descrição: Insere prioridade para o perfil ATE Secretaria

insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 275,3,'ATE Secretaria','62E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga'
where not exists(select *  from public.prioridade_perfil where codigo_perfil = '62E1E074-37D6-E911-ABD6-F81654FE895D');