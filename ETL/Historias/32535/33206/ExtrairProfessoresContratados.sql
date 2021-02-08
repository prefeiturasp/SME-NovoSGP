IF OBJECT_ID('tempdb..#tempCargosProfessoresContratados') IS NOT NULL 
	DROP TABLE #tempCargosProfessoresContratados

SELECT 
	DISTINCT
	cb.cd_cargo_base_servidor,
	cb.cd_servidor,
	cb.cd_cargo
INTO #tempCargosProfessoresContratados
FROM 
	v_cargo_base_cotic cb
INNER join 
	cargo c 
	on cb.cd_cargo = c.cd_cargo
LEFT JOIN
	atribuicao_aula atr
	ON atr.cd_cargo_base_servidor = cb.cd_cargo_base_servidor AND atr.an_atribuicao = 2021
where
	dt_inicio_exercicio > '2020-07-01'
	AND cb.dt_fim_nomeacao is null
	AND cb.dt_cancelamento is null
	AND cb.cd_situacao_funcional = 6
	AND cb.cd_cargo not in (4906,4510)
	AND atr.cd_atribuicao_aula IS NULL;

IF OBJECT_ID('tempdb..#tempCargosProfessoresContratados_MaiorCargo') IS NOT NULL 
	DROP TABLE #tempCargosProfessoresContratados_MaiorCargo
SELECT
	cd_servidor,
	MAX(cd_cargo) AS cd_cargo
INTO #tempCargosProfessoresContratados_MaiorCargo
FROM
	#tempCargosProfessoresContratados
GROUP BY
	cd_servidor;

IF OBJECT_ID('tempdb..#tempCargosProfessoresContratados_RemoveDuplicados') IS NOT NULL 
	DROP TABLE #tempCargosProfessoresContratados_RemoveDuplicados
SELECT
	t1.cd_servidor,
	t1.cd_cargo,
	t2.cd_cargo_base_servidor
INTO #tempCargosProfessoresContratados_RemoveDuplicados
FROM
	#tempCargosProfessoresContratados_MaiorCargo t1
CROSS APPLY
(
	SELECT TOP 1 * FROM #tempCargosProfessoresContratados temp WHERE temp.cd_servidor = t1.cd_servidor AND temp.cd_cargo = t1.cd_cargo
) AS t2;

IF OBJECT_ID('tempdb..#tempProfessoresContratadosAtivos') IS NOT NULL 
	DROP TABLE #tempProfessoresContratadosAtivos
SELECT
	serv.cd_registro_funcional AS cd_servidor_cotic,
	NULL AS cd_servidor_classroom,
	[dbo].[proc_gerar_email_funcionario](serv.nm_pessoa, serv.cd_registro_funcional) AS nm_email,
	'True' AS in_ativo,
	'/Professores' AS nm_organizacao,
	cbc.cd_cargo
INTO #tempProfessoresContratadosAtivos
FROM
	v_servidor_cotic serv (NOLOCK)
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor
INNER JOIN
	#tempCargosProfessoresContratados_RemoveDuplicados temp
	ON cbc.cd_cargo_base_servidor = temp.cd_cargo_base_servidor
INNER JOIN
	cargo (NOLOCK)
	ON cbc.cd_cargo = cargo.cd_cargo;

INSERT INTO servidor_classroom
SELECT DISTINCT * FROM #tempProfessoresContratadosAtivos WHERE NOT cd_servidor_cotic IN (SELECT cd_servidor_cotic from servidor_classroom);

DECLARE @passwordPadrao AS VARCHAR(8) = '12345678';

SELECT
	[dbo].[proc_retorna_primeiro_nome](servidor.nm_pessoa) AS 'First Name [Required]',
	[dbo].[proc_retorna_ultimo_nome](servidor.nm_pessoa) AS 'Last Name [Required]',
	temp.nm_email AS 'Email Address [Required]',
	@passwordPadrao AS 'Password [Required]',
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
	#tempProfessoresContratadosAtivos temp
INNER JOIN
	v_servidor_cotic servidor (NOLOCK)
	ON temp.cd_servidor_cotic = servidor.cd_registro_funcional
ORDER BY
	cd_servidor_cotic;
	