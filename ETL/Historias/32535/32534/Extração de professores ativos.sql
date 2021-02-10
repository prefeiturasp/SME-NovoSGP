USE se1426;

DECLARE @anoLetivo AS INT = 2021;
DECLARE @senhaPadrao AS VARCHAR(8) = '12345678';

IF OBJECT_ID('tempdb..#tempCargosProfessores') IS NOT NULL
	DROP TABLE #tempCargosProfessores;

SELECT
	distinct cd_cargo_base_servidor
INTO #tempCargosProfessores
FROM
	atribuicao_aula
WHERE
	an_atribuicao = @anoLetivo
	AND dt_cancelamento is null AND (dt_disponibilizacao_aulas is null OR dt_disponibilizacao_aulas > GETDATE());

IF OBJECT_ID('tempdb..#tempProfessoresAtivos') IS NOT NULL
	DROP TABLE #tempProfessoresAtivos;
SELECT
	serv.cd_registro_funcional AS cd_servidor_cotic,
	NULL AS cd_servidor_classroom,
	[dbo].[proc_gerar_email_funcionario](serv.nm_pessoa, serv.cd_registro_funcional) AS nm_email,
	'True' AS in_ativo,
	'/Professores' AS nm_organizacao,
	NULL AS cd_cargo
INTO #tempProfessoresAtivos
FROM
	v_servidor_cotic serv (NOLOCK)
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor
INNER JOIN
	#tempCargosProfessores temp
	ON cbc.cd_cargo_base_servidor = temp.cd_cargo_base_servidor
INNER JOIN
	cargo (NOLOCK)
	ON cbc.cd_cargo = cargo.cd_cargo;

INSERT INTO servidor_classroom
SELECT DISTINCT * FROM #tempProfessoresAtivos

--COMMIT
--ROLLBACK