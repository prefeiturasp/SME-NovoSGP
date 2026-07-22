BEGIN TRY
    BEGIN TRAN
    PRINT 'Desativação Relatório Analítico Sondagem - INÍCIO'

    UPDATE gp
    SET gp.grp_consultar = 0
    FROM CoreSSO.dbo.SYS_GrupoPermissao gp
    INNER JOIN CoreSSO.dbo.SYS_Modulo m 
        ON gp.mod_id = m.mod_id 
       AND gp.sis_id = m.sis_id
    WHERE m.mod_nome = 'Relatório Analítico Sondagem';

    PRINT 'Desativação Relatório Analítico Sondagem - FIM'

    COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'Erro ao desativar permissionamento'
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO