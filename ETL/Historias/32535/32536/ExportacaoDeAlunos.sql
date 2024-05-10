IF OBJECT_ID('tempdb..#tempServidorExportacaoAtivos') IS NOT NULL
	DROP TABLE #tempAlunosExportacaoAtivos;
SELECT
	aluno.*
INTO #tempAlunosExportacaoAtivos
FROM
	aluno_classroom aluno (NOLOCK)
LEFT JOIN
	ususario_classroom usuario (NOLOCK)
	ON aluno.nm_email = usuario.Email_Address_Required
WHERE
	usuario.Email_Address_Required IS NULL
	AND aluno.in_ativo = 1;

IF OBJECT_ID('tempdb..#tempServidorExportacaoInativos') IS NOT NULL
	DROP TABLE #tempAlunosExportacaoInativos;
SELECT
	aluno.*
INTO #tempAlunosExportacaoInativos
FROM
	aluno_classroom aluno (NOLOCK)
INNER JOIN
	ususario_classroom usuario (NOLOCK)
	ON aluno.nm_email = usuario.Email_Address_Required
WHERE
	aluno.in_ativo = 0;

IF OBJECT_ID('tempdb..#tempAlunosExportacao') IS NOT NULL
	DROP TABLE #tempAlunosExportacao;
SELECT
	*
INTO #tempAlunosExportacao
FROM
	(SELECT * FROM #tempAlunosExportacaoAtivos) AS Ativos
UNION
	(SELECT * FROM #tempAlunosExportacaoInativos);

-- Totalização
DECLARE @PageNumber AS INT = 1;
DECLARE @RowsOfPage AS INT = 50000;

SELECT
	[dbo].[proc_retorna_primeiro_nome](aluno.nm_aluno) AS 'First Name [Required]',
	[dbo].[proc_retorna_ultimo_nome](aluno.nm_aluno) AS 'Last Name [Required]',
	temp.nm_email AS 'Email Address [Required]',
	CASE WHEN temp.in_ativo = 'True' THEN [dbo].[proc_gerar_password_aluno](aluno.dt_nascimento_aluno) ELSE '****' END AS 'Password [Required]',
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
	'False' AS 'Change Password at Next Sign-In',
	CASE WHEN temp.in_ativo = 'True' THEN 'ACTIVE' ELSE 'SUSPENDED' END AS 'New Status [UPLOAD ONLY]'
FROM
	#tempAlunosExportacao temp
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON temp.cd_aluno_eol = aluno.cd_aluno
ORDER BY
	cd_aluno_eol
OFFSET (@PageNumber-1)*@RowsOfPage ROWS
FETCH NEXT @RowsOfPage ROWS ONLY;