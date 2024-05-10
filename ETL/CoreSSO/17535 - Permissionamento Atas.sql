-- Data de criação: 29/06/2020
-- Descrição: Insere módulo e permissionamento para Ata
BEGIN TRY
    BEGIN TRAN
	PRINT 'Inserindo permissionamento do módulo Ata'  
	 
	-- insere módulo 55 - Atas caso não exista
	IF NOT EXISTS(select * from SYS_Modulo where sis_id = 1000 AND mod_id = 55)
	BEGIN
		INSERT INTO SYS_Modulo(sis_id, mod_id,mod_nome, mod_descricao, mod_idPai, mod_auditoria, mod_situacao, mod_dataCriacao, mod_dataAlteracao) VALUES(1000, 55, 'Atas', NULL, NULL, 0, 1, GETDATE(), GETDATE())
	END

	-- insere associação de módulo 55 - Atas com visão 10 - Novo SGP
	IF NOT EXISTS(select * from SYS_VisaoModulo where sis_id = 1000 AND mod_id = 55 AND vis_id = 10)
	BEGIN
		INSERT INTO SYS_VisaoModulo(vis_id, sis_id, mod_id) VALUES (10, 1000, 55)
	END

	-- insere mapeamento de modulo para o módulo 55 - Atas
	IF NOT EXISTS(select * from SYS_ModuloSiteMap where sis_id = 1000 AND mod_id = 55 AND msm_id = 55)
	BEGIN
		INSERT INTO SYS_ModuloSiteMap(sis_id, mod_id, msm_id, msm_nome, msm_descricao, msm_url, msm_informacoes, msm_urlHelp) VALUES (1000, 55,55, 'Ata Final de Resultados', NULL, '~/', NULL, NULL)
	END

	--Permissões
	--Escola
	-- insere permissão no módulo de ata para o grupo Professor Readaptado
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '3CE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('3CE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	--UE
	-- insere permissão no módulo de ata para o grupo ADM UE
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '42E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('42E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Secretario
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '43E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('43E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo CP
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '44E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('44E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo AD
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '45E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('45E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Diretor
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '46E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('46E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- DRE
	-- insere permissão no módulo de ata para o grupo DRE Basico
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '47E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('47E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo ADM DRE
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '48E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('48E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo DIPED
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '49E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('49E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo PAAI
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '4AE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4AE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo CEFAI
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '4BE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4BE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo NAAPA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '4CE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4CE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Diretor DIPED
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '4DE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4DE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Supervisor
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '4EE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4EE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Supervisor Consulta
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '5FE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('5FE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Supervisor Técnico
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '4FE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('4FE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Diretor Regional
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '50E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('50E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo COPED Básico
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '51E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('51E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo NTC - NAAPA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '52E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('52E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo NTC
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '53E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('53E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo NTA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '54E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('54E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo DIEE
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '55E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('55E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo DIEJA
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '56E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('56E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo DIEI
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '57E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('57E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo DIEFM
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '58E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('58E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo COPED
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '59E1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('59E1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo Coordenador SME
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '5AF1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('5AF1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo ADM SME
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '5AE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('5AE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

	-- insere permissão no módulo de ata para o grupo ADM COTIC
	IF NOT EXISTS(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id = 55 and gru_id = '5BE1E074-37D6-E911-ABD6-F81654FE895D')
	BEGIN
		INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir) VALUES ('5BE1E074-37D6-E911-ABD6-F81654FE895D', 1000, 55, 1, 0, 0, 0)
	END

     PRINT 'Permissionamento para módulo Ata definido'
    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao definir permissionamento'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO