USE [se1426]
GO

/****** Object:  UserDefinedFunction [dbo].[proc_gerar_email_alternativo_aluno]    Script Date: 08/02/2021 19:20:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- 
-- Function para geracao do email
--
CREATE FUNCTION [dbo].[proc_gerar_email_alternativo_aluno](@p_nm_aluno as VARCHAR(128), @p_dt_nascimento_aluno AS DATETIME, @p_separador_nome AS CHAR)
RETURNS VARCHAR(256)
BEGIN
    DECLARE @palavras_nomes TABLE( id INT, elemento VARCHAR(64) );
    DECLARE @ultimo_indice_palavras AS INT;
    DECLARE @str_dt_nascimento AS VARCHAR(16);
    DECLARE @nm_email AS VARCHAR(128);
    
    DECLARE @dominio_email AS VARCHAR(32);
    SET @dominio_email = '@edu.sme.prefeitura.sp.gov.br';

    -- Extrai as palavras no nome do aluno e a quantidade
    INSERt into @palavras_nomes 
        SELECT ROW_NUMBER() OVER (ORDER BY (select null)) as id, elemento 
        FROM proc_string_split(@p_nm_aluno, ' ');
        
    SET @ultimo_indice_palavras = (SELECT COUNT(id) FROM @palavras_nomes);

    SET @p_nm_aluno = '';
    SELECT @p_nm_aluno = COALESCE(@p_nm_aluno + (
        CASE
            WHEN id = 1 THEN elemento
			WHEN id = @ultimo_indice_palavras THEN @p_separador_nome + elemento
            ELSE CASE 
                WHEN LOWER(elemento) IN ('da', 'das', 'de', 'do', 'dos', 'os') THEN ''
                ELSE LEFT(elemento, 1)
            END 
        END
    ), elemento) FROM @palavras_nomes;
        
    -- Extrai e formata a data de nascimento
    SET @str_dt_nascimento = (
        RIGHT('00' + LTRIM(CAST(DATEPART(DAY, @p_dt_nascimento_aluno) as NVARCHAR(2))), 2) + 
        RIGHT('00' + LTRIM(CAST(DATEPART(MONTH, @p_dt_nascimento_aluno) as NVARCHAR(2))), 2) + 
        RIGHT('0000' + LTRIM(CAST(DATEPART(YEAR, @p_dt_nascimento_aluno) as NVARCHAR(4))), 4)
    );

    -- Monta o email
    SET @nm_email = LOWER(@p_nm_aluno + '.' + @str_dt_nascimento + @dominio_email);
    RETURN @nm_email;
END;
GO


