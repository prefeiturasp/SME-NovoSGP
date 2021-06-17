-- Data de criação: 13/04/2021
-- Descriçãoo: Insere prioridade para o perfil Comunicados UE

insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por,criado_rf)
select 390,2,'Comunicados UE','64E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga'
where not exists(select *  from public.prioridade_perfil where codigo_perfil = '64E1E074-37D6-E911-ABD6-F81654FE895D');