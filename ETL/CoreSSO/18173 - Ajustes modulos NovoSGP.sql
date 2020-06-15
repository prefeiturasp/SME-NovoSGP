BEGIN TRY
    BEGIN TRAN
	PRINT 'Ajustando módulos do NovoSGP'  
	 
	-- renomeia módulo 24
	if exists(select * from sys_modulo where sis_id = 1000 and mod_id = 24)
	begin
		update SYS_Modulo set mod_nome = 'Frequência/Plano de aula' where sis_id = 1000 and mod_id = 24
	end

	-- exclui grupopermissao
	if exists(select * from SYS_GrupoPermissao where sis_id = 1000 and mod_id in (18,21,27))
	begin
		delete from SYS_GrupoPermissao where sis_id = 1000 and mod_id in (18,21,27)
	end

	-- exclui visaomodulomenu
	if exists(select * from SYS_VisaoModuloMenu where sis_id = 1000 and mod_id in (18,21,27))
	begin
		delete from SYS_VisaoModuloMenu where sis_id = 1000 and mod_id in (18,21,27)
	end

	-- exclui visaomodulo
	if exists(select * from SYS_VisaoModulo where sis_id = 1000 and mod_id in (18,21,27))
	begin
		delete from SYS_VisaoModulo where sis_id = 1000 and mod_id in (18,21,27)
	end

	-- exclui modulos
	if exists(select * from SYS_Modulo where sis_id = 1000 and mod_id in (18,21,27))
	begin
		delete from SYS_Modulo where sis_id = 1000 and mod_id in (18,21,27)
	end

     PRINT 'Módulos do NovoSGP ajustados'
    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao atualizar módulos'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO