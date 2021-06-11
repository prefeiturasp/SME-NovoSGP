-- 
-- Function para geracao do email
--
create FUNCTION [dbo].[proc_gerar_email_funcionario](@p_nm_funcionario as VARCHAR(128), @p_rf_funcionario AS VARCHAR(10))
RETURNS VARCHAR(256)
BEGIN
    DECLARE @palavras_nomes TABLE( id INT, elemento VARCHAR(64) );
    DECLARE @nm_email AS VARCHAR(128);
    
    DECLARE @dominio_email AS VARCHAR(32);
    SET @dominio_email = '@edu.sme.prefeitura.sp.gov.br';

    -- Extrai as palavras no nome do aluno e a quantidade
    INSERt into @palavras_nomes 
        SELECT ROW_NUMBER() OVER (ORDER BY (select null)) as id, elemento 
        FROM dbo.proc_string_split(@p_nm_funcionario, ' ');
        
	DECLARE @primeiro_nome as VARCHAR(50);
    
	SET @primeiro_nome = (SELECT TOP 1 elemento FROM @palavras_nomes ORDER BY id);

	DECLARE @ultimo_nome as VARCHAR(50);
    
	SET @ultimo_nome = (SELECT TOP 1 elemento FROM @palavras_nomes ORDER BY id DESC);
    
    -- Monta o email
    SET @nm_email = LOWER(@primeiro_nome +  @ultimo_nome + '.' + @p_rf_funcionario + @dominio_email);
    RETURN @nm_email;
END;