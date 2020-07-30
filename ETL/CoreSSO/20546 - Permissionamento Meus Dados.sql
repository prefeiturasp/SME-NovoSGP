BEGIN TRY
    BEGIN TRAN
	PRINT 'Inserindo permissionamento do menu Meus Dados para os novos perfis de infantil'

	-- insere permissão do menu Meus Dados para perfil Professor Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 28 and gru_id = '60E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('60E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 28, 1, 1, 1, 1)
	END

	-- insere permissão do menu Meus Dados para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 28 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 28, 1, 1, 1, 1)
	END

	-- insere permissão do menu Meus Dados para perfil ATE Secretaria
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 28 and gru_id = '62E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('62E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 28, 1, 1, 1, 1)
	END

     PRINT 'Permissionamento do menu Meus Dados para os novos perfis de infantil'

    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao definir permissionamento'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO