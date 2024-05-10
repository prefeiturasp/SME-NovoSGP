-- Data de criacao: 12/08/2020
-- Descricao: Remove permiss�o de Plano Anual para Professor Ed. Infantil

BEGIN TRY
    BEGIN TRAN
	PRINT 'Permissionamento do Plano Anual - IN�CIO'

	--Permissoes
	-- remove permiss�o do Plano Anual para perfil Professor Ed. Infantil
	IF EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 23 and gru_id = '60E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		DELETE FROM SYS_GrupoPermissao WHERE gru_id = '60E1E074-37D6-E911-ABD6-F81654FE895D' and sis_id = 1000 and mod_id = 23
	END

	--Permissoes
	-- remove permissão do Plano Anual para perfil Professor CJ Ed. Infantil
	IF EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 23 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		DELETE FROM SYS_GrupoPermissao WHERE gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D' and sis_id = 1000 and mod_id = 23
	END


    PRINT 'Permissionamento do Plano de Aula - FIM'

    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao definir permissionamento'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO