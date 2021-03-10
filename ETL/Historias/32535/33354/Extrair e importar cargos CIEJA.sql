DECLARE @tipoFuncaoCIEJAASSISTPED AS INT = 42;
DECLARE @tipoFuncaoCIEJAASSISTCOORD AS INT = 43;
DECLARE @tipoFuncaoCIEJACOORD AS INT = 44;

IF OBJECT_ID('tempdb..#tempProfessores_PAP_PAEE_CIEJA') IS NOT NULL
	DROP TABLE #tempProfessores_PAP_PAEE_CIEJA;
SELECT
	cbc.cd_cargo_base_servidor,
	cbc.cd_servidor,
	facs.cd_unidade_local_servico
INTO #tempProfessores_PAP_PAEE_CIEJA
FROM
	v_cargo_base_cotic cbc (NOLOCK)
INNER JOIN
	funcao_atividade_cargo_servidor facs (NOLOCK)
	ON cbc.cd_cargo_base_servidor = facs.cd_cargo_base_servidor
WHERE
	facs.cd_tipo_funcao IN (@tipoFuncaoCIEJAASSISTPED, @tipoFuncaoCIEJAASSISTCOORD, @tipoFuncaoCIEJACOORD)
	AND (facs.dt_fim_funcao_atividade IS NULL OR facs.dt_fim_funcao_atividade > GETDATE())
	AND dt_fim_nomeacao IS NULL;

IF OBJECT_ID('tempdb..#tempCargosFuncionariosRegularesAtivos') IS NOT NULL
	DROP TABLE #tempCargosFuncionariosRegularesAtivos;
SELECT
	serv.cd_registro_funcional AS cd_servidor_cotic,
	NULL AS cd_servidor_classroom,
	[dbo].[proc_gerar_email_funcionario](serv.nm_pessoa, serv.cd_registro_funcional) AS nm_email,
	'True' AS in_ativo,
	[dbo].[proc_gerar_unidade_organizacional_funcionario_v2](cbc.cd_cargo, '') AS nm_organizacao
INTO #tempCargosFuncionariosRegularesAtivos
FROM
	v_servidor_cotic serv (NOLOCK)
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor
INNER JOIN
	#tempProfessores_PAP_PAEE_CIEJA temp
	ON cbc.cd_cargo_base_servidor = temp.cd_cargo_base_servidor;

IF OBJECT_ID('tempdb..#tempFuncionariosQueSeraoAdicionados') IS NOT NULL
	DROP TABLE #tempFuncionariosQueSeraoAdicionados;
SELECT
	temp.*
INTO #tempFuncionariosQueSeraoAdicionados
FROM
	#tempCargosFuncionariosRegularesAtivos temp
LEFT JOIN
	servidor_classroom servClass (NOLOCK)
	ON temp.nm_email = servClass.nm_email AND servClass.in_ativo = 1
WHERE
	servClass.nm_email IS NULL;

-- Totalização
DECLARE @passwordPadrao AS VARCHAR(8) = '12345678';

SELECT
	[dbo].[proc_retorna_primeiro_nome](servidor.nm_pessoa) AS 'First Name [Required]',
	[dbo].[proc_retorna_ultimo_nome](servidor.nm_pessoa) AS 'Last Name [Required]',
	temp.nm_email AS 'Email Address [Required]',
	CASE WHEN temp.in_ativo = 'True' THEN @passwordPadrao ELSE '****' END AS 'Password [Required]',
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
	#tempFuncionariosQueSeraoAdicionados temp
INNER JOIN
	v_servidor_cotic servidor (NOLOCK)
	ON temp.cd_servidor_cotic = servidor.cd_registro_funcional
ORDER BY
	cd_servidor_cotic;