USE [se1426]
GO

/****** Object:  UserDefinedFunction [dbo].[proc_retorna_primeiro_nome]    Script Date: 08/02/2021 19:22:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[proc_retorna_primeiro_nome] 
(
	@p_nome_pessoa as VARCHAR(MAX)
)
RETURNS VARCHAR(250)
AS
BEGIN
	DECLARE @primeiroNome AS VARCHAR(250) = (SELECT TOP 1 elemento FROM proc_string_split(@p_nome_pessoa, ' '));
	RETURN @primeiroNome
END
GO


