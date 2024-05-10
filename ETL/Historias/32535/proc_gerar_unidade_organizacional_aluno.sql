USE [se1426]
GO

/****** Object:  UserDefinedFunction [dbo].[proc_gerar_unidade_organizacional_aluno]    Script Date: 08/02/2021 19:21:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[proc_gerar_unidade_organizacional_aluno]
(
	@p_cd_modalidade_ensino AS INT,
	@p_cd_etapa_ensino AS INT,
	@p_cd_ciclo_ensino AS INT
)
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @unidade_organizacional AS VARCHAR(50) =
	(
		SELECT
			CASE
				WHEN (@p_cd_modalidade_ensino = 1 AND @p_cd_etapa_ensino = 1 AND @p_cd_ciclo_ensino = 1) OR
					 (@p_cd_modalidade_ensino = 2 AND @p_cd_etapa_ensino = 10 AND @p_cd_ciclo_ensino = 14) THEN '/Alunos/INFANTIL I'

				WHEN (@p_cd_modalidade_ensino = 1 AND @p_cd_etapa_ensino = 1 AND @p_cd_ciclo_ensino = 2) OR
					 (@p_cd_modalidade_ensino = 2 AND @p_cd_etapa_ensino = 10 AND @p_cd_ciclo_ensino = 23) THEN '/Alunos/INFANTIL II'

				WHEN (@p_cd_modalidade_ensino = 1 AND @p_cd_etapa_ensino = 2 AND @p_cd_ciclo_ensino IN (3, 4)) OR
					 (@p_cd_modalidade_ensino = 1 AND @p_cd_etapa_ensino = 3 AND @p_cd_ciclo_ensino IN (5, 6)) OR
					 (@p_cd_modalidade_ensino = 2 AND @p_cd_etapa_ensino = 11 AND @p_cd_ciclo_ensino IN (15,16)) THEN '/Alunos/EJA'

				WHEN (@p_cd_modalidade_ensino = 1 AND @p_cd_etapa_ensino = 6 AND @p_cd_ciclo_ensino = 10) OR
					 (@p_cd_modalidade_ensino = 1 AND @p_cd_etapa_ensino = 9 AND @p_cd_ciclo_ensino = 13) OR 
					 (@p_cd_modalidade_ensino = 3 AND @p_cd_etapa_ensino = 14 AND @p_cd_ciclo_ensino = 20) OR 
					 (@p_cd_modalidade_ensino = 2 AND @p_cd_etapa_ensino = 17 AND @p_cd_ciclo_ensino = 32) THEN '/Alunos/MEDIO'

				ELSE '/Alunos/FUNDAMENTAL'
			END
	);

	RETURN @unidade_organizacional

END
GO


