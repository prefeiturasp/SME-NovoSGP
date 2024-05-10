USE [GestaoPedagogica]
GO

DROP FUNCTION [dbo].[fn_TiraLetras]
GO

CREATE FUNCTION [dbo].[fn_TiraLetras]
 (
	@Resultado VARCHAR(8000)
 )
 RETURNS VARCHAR(8000)
 
 AS
 
 BEGIN
	DECLARE @CharInvalido SMALLINT
	    SET @CharInvalido = PATINDEX('%[^0-9]%', @Resultado)

	WHILE @CharInvalido > 0
	BEGIN
		SET @Resultado = STUFF(@Resultado, @CharInvalido, 1, '')
		SET @CharInvalido = PATINDEX('%[^0-9]%', @Resultado)
	END

	SET @Resultado = @Resultado
	RETURN @Resultado
 END

GO


