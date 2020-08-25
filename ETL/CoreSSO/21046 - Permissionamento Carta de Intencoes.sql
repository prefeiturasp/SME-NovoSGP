-- Data de criacao: 06/08/2020
-- Descricao: Insere modulo e permissionamento para Carta de Intenções

BEGIN TRY
    BEGIN TRAN
	PRINT 'Permissionamento do Carta de Intenções - INÍCIO'
	
	--Modulo
	-- insere modulo 58 - Carta de Intenções caso nao exista
	IF NOT EXISTS(select * from SYS_Modulo where sis_id = 1000 and mod_id = 58)
	BEGIN
		INSERT INTO SYS_Modulo(sis_id, mod_id, mod_nome, mod_descricao, mod_idPai, mod_auditoria, mod_situacao, mod_dataCriacao, mod_dataAlteracao) VALUES (1000, 58, 'Carta de Intenções', NULL, NULL, 0, 1, GETDATE(), GETDATE())
	END

	-- insere associacao de modulo 58 - Carta de Intenções com visao 10 - Novo SGP
	IF NOT EXISTS(select * from SYS_VisaoModulo where sis_id = 1000 AND mod_id = 58 AND vis_id = 10)
	BEGIN
		INSERT INTO SYS_VisaoModulo(vis_id, sis_id, mod_id) VALUES (10, 1000, 58)
	END

	-- insere mapeamento de modulo para o modulo 58 - Carta de Intenções
	IF NOT EXISTS(select * from SYS_ModuloSiteMap where sis_id = 1000 AND mod_id = 58 AND msm_id = 58)
	BEGIN
		INSERT INTO SYS_ModuloSiteMap(sis_id, mod_id, msm_id, msm_nome, msm_descricao, msm_url, msm_informacoes, msm_urlHelp) VALUES (1000, 58,58, 'Carta de Intenções', NULL, '~/', NULL, NULL)
	END


	--Permissoes
	-- insere permissão do Carta de Intenções para perfil ATE
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '3BE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('3BE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Professor Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '60E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('60E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 1, 1, 1)
	END

	-- insere permissão do Carta de Intenções para perfil Professor CJ Ed. Infantil
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '61E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('61E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 1, 1, 1)
	END

	-- insere permissão do Carta de Intenções para perfil Professor CJ
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '41E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('41E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 1, 1, 1)
	END

	-- insere permissão do Carta de Intenções para perfil ADM UE
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '42E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('42E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Secretário
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '43E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('43E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil CP
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '44E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('44E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 1, 1, 1)
	END

	-- insere permissão do Carta de Intenções para perfil AD
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '45E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('45E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Diretor
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '46E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('46E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil DRE Básico
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '47E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('47E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil ADM DRE
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '48E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('48E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil DIPED
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '49E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('49E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil PAAI
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '4AE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4AE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil CEFAI
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '4BE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4BE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil NAAPA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '4CE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4CE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Diretor DIPED
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '4DE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4DE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Supervisor
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '4EE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4EE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Supervisor Consulta
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '5FE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('5FE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Supervisor Técnico
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '4FE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4FE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil Diretor Regional
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '50E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('50E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil COPED Básico
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '51E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('51E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil NTC - NAAPA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '52E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('52E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil NTC
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '53E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('53E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil NTA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '54E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('54E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil DIEE
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '55E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('55E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil DIEJA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '56E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('56E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil DIEI
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '58E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('58E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil DIEFM
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '58E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('58E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil COPED
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '59E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('59E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil ADM SME
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '5AE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('5AE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

	-- insere permissão do Carta de Intenções para perfil ADM COTIC
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 58 and gru_id = '5BE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('5BE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 58, 1, 0, 0, 0)
	END

    PRINT 'Permissionamento do Carta de Intenções - FIM'

    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao definir permissionamento'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO