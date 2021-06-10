-- 1. Busca emails duplicados
IF OBJECT_ID('tempdb..#tempEmailsDuplicados') IS NOT NULL
	DROP TABLE #tempEmailsDuplicados;
SELECT
	DISTINCT
	nm_email
INTO #tempEmailsDuplicados
FROM
	aluno_classroom_inicial (NOLOCK)
GROUP BY
	nm_email
HAVING COUNT(*) > 1;

-- 2. Busca todos os registros com os emails levantados
IF OBJECT_ID('tempdb..#tempAlunosComEmailDuplicados') IS NOT NULL
	DROP TABLE #tempAlunosComEmailDuplicados;
SELECT
	DISTINCT
	aluno.*
INTO #tempAlunosComEmailDuplicados
FROM
	aluno_classroom_inicial aluno (NOLOCK)
INNER JOIN
	#tempEmailsDuplicados temp
	ON aluno.nm_email = temp.nm_email
WHERE
	aluno.in_ativo = 1;

-- 3. Seleciona um dos duplicados para ser alterados
IF OBJECT_ID('tempdb..#tempAlunosComEmailDuplicados_MaxCdAluno') IS NOT NULL
	DROP TABLE #tempAlunosComEmailDuplicados_MaxCdAluno;
SELECT MAX(cd_aluno_eol) AS cd_aluno_eol, nm_email 
INTO #tempAlunosComEmailDuplicados_MaxCdAluno
FROM 
	#tempAlunosComEmailDuplicados 
GROUP BY 
	nm_email;
GO

IF OBJECT_ID('tempdb..#tempAlunosComEmailDuplicados_ParaSeremAlterados') IS NOT NULL
	DROP TABLE #tempAlunosComEmailDuplicados_ParaSeremAlterados;
SELECT
	t1.*
INTO #tempAlunosComEmailDuplicados_ParaSeremAlterados
FROM
	#tempAlunosComEmailDuplicados t1
INNER JOIN
	#tempAlunosComEmailDuplicados_MaxCdAluno t2
	ON t1.cd_aluno_eol = t2.cd_aluno_eol AND t1.nm_email = t2.nm_email;

-- 4. Altração do email
BEGIN TRAN

UPDATE t1
SET nm_email = [dbo].[proc_gerar_email_alternativo_aluno](aluno.nm_aluno, aluno.dt_nascimento_aluno, '.'),
	email_alterado = 1
FROM
	aluno_classroom_inicial t1
INNER JOIN
	aluno
	ON t1.cd_aluno_eol = aluno.cd_aluno
WHERE
	t1.email_alterado = 1

UPDATE t1
SET nm_email = [dbo].[proc_gerar_email_alternativo_aluno](aluno.nm_aluno, aluno.dt_nascimento_aluno, '_'),
	email_alterado = 1
FROM
	aluno_classroom_inicial t1
INNER JOIN
	#tempAlunosComEmailDuplicados_ParaSeremAlterados t2
	ON t1.cd_aluno_eol = t2.cd_aluno_eol AND t1.nm_email = t2.nm_email
INNER JOIN
	aluno
	ON t1.cd_aluno_eol = aluno.cd_aluno;
--COMMIT
--ROLLBACK

-- CSV
SELECT
	[dbo].[proc_retorna_primeiro_nome](aluno.nm_aluno) AS 'First Name [Required]',
	[dbo].[proc_retorna_ultimo_nome](aluno.nm_aluno) AS 'Last Name [Required]',
	aluClass.nm_email AS 'Email Address [Required]',
	CASE WHEN aluClass.in_ativo = 'True' THEN [dbo].[proc_gerar_password_aluno](aluno.dt_nascimento_aluno) ELSE '****' END AS 'Password [Required]',
	'' AS 'Password Hash Function [UPLOAD ONLY]',
	aluClass.nm_organizacao AS 'Org Unit Path [Required]',
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
	CASE WHEN aluClass.in_ativo = 'True' THEN 'ACTIVE' ELSE 'SUSPENDED' END AS 'New Status [UPLOAD ONLY]'
FROM
	aluno_classroom_inicial aluClass (NOLOCK)
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluClass.cd_aluno_eol = aluno.cd_aluno
WHERE
	email_alterado = 1
ORDER BY
	cd_aluno_eol;
