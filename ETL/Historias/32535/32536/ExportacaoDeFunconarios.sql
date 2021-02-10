IF OBJECT_ID('tempdb..#tempServidorExportacaoAtivos') IS NOT NULL
	DROP TABLE #tempServidorExportacaoAtivos;
SELECT
	servidor.*,
	CASE
		WHEN usuario.Email_Address_Required IS NULL THEN 0
		ELSE 1
	END AS JaEraUsuario
INTO #tempServidorExportacaoAtivos
FROM
	servidor_classroom servidor (NOLOCK)
LEFT JOIN
	ususario_classroom usuario (NOLOCK)
	ON servidor.nm_email = usuario.Email_Address_Required
WHERE
	servidor.in_ativo = 1
	AND Professor = 0;

-- Totalização
DECLARE @PageNumber AS INT = 1;
DECLARE @RowsOfPage AS INT = 50000;
DECLARE @passwordPadrao AS VARCHAR(8) = '12345678';

SELECT
	[dbo].[proc_retorna_primeiro_nome](servidor.nm_pessoa) AS 'First Name [Required]',
	[dbo].[proc_retorna_ultimo_nome](servidor.nm_pessoa) AS 'Last Name [Required]',
	temp.nm_email AS 'Email Address [Required]',
	CASE WHEN temp.JaEraUsuario = '0' THEN @passwordPadrao ELSE '****' END AS 'Password [Required]',
	'' AS 'Password Hash Function [UPLOAD ONLY]',
	temp.nm_organizacao AS 'Org Unit Path [Required]',
	'' AS 'New Primary Email [UPLOAD ONLY]',
	'' AS 'Recovery Email',
	'' AS 'Home Secondary Email',
	'' AS 'Work Secondary Email',
	'' AS 'Recovery Phone [MUST BE IN THE E.164 FORMAT]',
	'' AS 'Work Phone',
	'' AS 'Home Phone',
	'' AS 'Mobile Phone',
	'' AS 'Work Address',
	'' AS 'Home Address',
	'' AS 'Employee ID',
	'' AS 'Employee Type',
	'' AS 'Employee Title',
	'' AS 'Manager Email',
	'' AS 'Department',
	'' AS 'Cost Center',
	'' AS 'Building ID',
	'' AS 'Floor Name',
	'' AS 'Floor Section',
	'True' AS 'Change Password at Next Sign-In',
	CASE WHEN temp.in_ativo = 'True' THEN 'ACTIVE' ELSE 'SUSPENDED' END AS 'New Status [UPLOAD ONLY]'
FROM
	#tempServidorExportacaoAtivos temp
INNER JOIN
	v_servidor_cotic servidor (NOLOCK)
	ON temp.cd_servidor_cotic = servidor.cd_registro_funcional
ORDER BY
	cd_servidor_cotic
OFFSET (@PageNumber-1)*@RowsOfPage ROWS
FETCH NEXT @RowsOfPage ROWS ONLY;