-- Data de cria��o: 17/07/2020
-- Descri��o: Insere novo perfil Professor CJ Ed. Infantil
BEGIN TRY
    BEGIN TRAN
	PRINT 'Inserindo permissionamento do perfil Professor CJ Ed. Infantil'

	if not exists(select * from SYS_Grupo where sis_id = 1000 and vis_id = 10 and gru_nome = 'Professor CJ Ed. Infantil')
	begin
		print 'inserindo perfil Professor CJ Ed. Infantil'
		insert into SYS_Grupo(gru_id, gru_nome, gru_situacao, gru_dataCriacao, gru_dataAlteracao, vis_id, sis_id, gru_integridade) values( '61E1E074-37D6-E911-ABD6-F81654FE895D', 'Professor CJ Ed. Infantil', 1, getdate(), getdate(), 10, 1000, 0);
	end

	-- insere permiss�o Frequ�ncia /Plano de Aula para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 24 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 24, 1, 1, 1, 1)
	END

	-- insere permiss�o Plano Anual / Carta de intens�es para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 23 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 23, 1, 1, 1, 1)
	END

	-- insere permiss�o Calend�rio do Professor para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 42 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 42, 1, 1, 1, 1)
	END

	-- insere permiss�o Atribui��o CJ para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 18 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 18, 1, 1, 1, 1)
	END

	-- insere permiss�o Relat�rios para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 29 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 29, 1, 0, 0, 0)
	END

	-- insere permiss�o Notifica��es para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 26 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 26, 1, 1, 1, 1)
	END

     PRINT 'Permissionamento para perfil Professor CJ Ed. Infantil definido'

    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao definir permissionamento'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO