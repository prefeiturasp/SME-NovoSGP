-- insere permissão leitura e escrita para o adm sme no territórios do saber
if not exists(select * from SYS_GrupoPermissao gp where gp.sis_id = 1000 and mod_id = 51 and gru_id = '5AE1E074-37D6-E911-ABD6-F81654FE895D')
begin
	insert into SYS_GrupoPermissao(gru_id,sis_id,mod_id,grp_consultar,grp_inserir,grp_alterar,grp_excluir) select gru.gru_id,gru.sis_id, 51 as mod_id , 1 as grp_consulta, 1 as grp_inserir, 1 as grp_alterar, 1 as grp_excluir  from SYS_Grupo as gru where gru.sis_id = 1000 and gru_id = '5AE1E074-37D6-E911-ABD6-F81654FE895D'
end

-- insere permissão leitura e escrita para o adm dre no territórios do saber
if not exists(select * from SYS_GrupoPermissao gp where gp.sis_id = 1000 and mod_id = 51 and gru_id = '48E1E074-37D6-E911-ABD6-F81654FE895D')
begin
	insert into SYS_GrupoPermissao(gru_id,sis_id,mod_id,grp_consultar,grp_inserir,grp_alterar,grp_excluir) select gru.gru_id, gru.sis_id, 51 as mod_id , 1 as grp_consulta, 1 as grp_inserir, 1 as grp_alterar, 1 as grp_excluir  from SYS_Grupo as gru where gru.sis_id = 1000 and gru_id = '48E1E074-37D6-E911-ABD6-F81654FE895D'
end