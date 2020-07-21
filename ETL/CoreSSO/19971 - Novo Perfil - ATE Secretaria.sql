-- Data de criação: 20/07/2020
-- Descrição: Insere novo perfil ATE Secretaria
BEGIN TRY
    BEGIN TRAN
	PRINT 'Inserindo permissionamento do perfil ATE Secretaria'

	if not exists(select * from SYS_Grupo where sis_id = 1000 and vis_id = 10 and gru_nome = 'ATE Secretaria')
	begin
		print 'inserindo perfil ATE Secretaria'
		insert into SYS_Grupo(gru_id, gru_nome, gru_situacao, gru_dataCriacao, gru_dataAlteracao, vis_id, sis_id, gru_integridade) values( '62E1E074-37D6-E911-ABD6-F81654FE895D', 'ATE Secretaria', 1, getdate(), getdate(), 10, 1000, 0);
	end

	-- insere permissão Frequência /Plano de Aula para perfil ATE Secretaria
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 24 and gru_id = '62E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('62E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 24, 1, 0, 0, 0)
	END

	-- insere permissão Calendário Escolar para perfil ATE Secretaria
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 20 and gru_id = '62E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('62E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 20, 1, 0, 0, 0)
	END

	-- insere permissão Eventos para perfil ATE Secretaria
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 37 and gru_id = '62E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('62E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 37, 1, 0, 0, 0)
	END

	-- insere permissão Notificações para perfil ATE Secretaria
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 26 and gru_id = '62E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('62E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 26, 1, 1, 1, 1)
	END

     PRINT 'Permissionamento para perfil ATE Secretaria definido'

    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao definir permissionamento'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO