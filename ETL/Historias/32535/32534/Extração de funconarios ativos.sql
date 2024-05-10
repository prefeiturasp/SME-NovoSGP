USE se1426;

DECLARE @cargoCP AS INT = 3379;
DECLARE @cargoAD AS INT = 3085;
DECLARE @cargoDiretor AS INT = 3360;
DECLARE @cargoSupervisor AS INT = 3352;
DECLARE @cargoSupervisorTecnico433 AS INT = 433;
DECLARE @cargoSupervisorTecnico434 AS INT = 434;
DECLARE @cargoATE AS INT = 4906;
DECLARE @cargoAuxDesenvolvimentoInfantil AS INT = (SELECT cd_cargo FROM cargo (NOLOCK) WHERE dc_cargo = 'AUXILIAR DE DESENVOLVIMENTO INFANTIL');

-- Cargos base Fixos
IF OBJECT_ID('tempdb..#tempCargosBaseFuncionarios_Fixos') IS NOT NULL
	DROP TABLE #tempCargosBaseFuncionarios_Fixos;
SELECT
	cbc.cd_cargo_base_servidor,
	cbc.cd_servidor,
	cbc.cd_cargo,
	CASE
		WHEN cbc.cd_cargo = @cargoSupervisorTecnico433 OR cbc.cd_cargo = @cargoSupervisorTecnico434 THEN 1
		WHEN cbc.cd_cargo = @cargoSupervisor THEN 2
		WHEN cbc.cd_cargo = @cargoDiretor THEN 3
		WHEN cbc.cd_cargo = @cargoAD THEN 4
		WHEN cbc.cd_cargo = @cargoCP THEN 5
		WHEN cbc.cd_cargo = @cargoATE THEN 8
		WHEN cbc.cd_cargo = @cargoAuxDesenvolvimentoInfantil THEN 9
	END AS prioridade
INTO #tempCargosBaseFuncionarios_Fixos
FROM
	v_cargo_base_cotic cbc (NOLOCK)
WHERE
	cbc.cd_cargo IN (@cargoCP, @cargoAD, @cargoDiretor, @cargoSupervisor, @cargoSupervisorTecnico433, @cargoSupervisorTecnico434, @cargoATE, @cargoAuxDesenvolvimentoInfantil)
	AND dt_fim_nomeacao IS NULL;

-- 2. Cargos sobrepostos fixos
IF OBJECT_ID('tempdb..#tempCargosSobrepostosFuncionarios_Fixos') IS NOT NULL
	DROP TABLE #tempCargosSobrepostosFuncionarios_Fixos;
SELECT
	css.cd_cargo_base_servidor,
	cbc.cd_servidor,
	css.cd_cargo,
	CASE
		WHEN css.cd_cargo = @cargoSupervisorTecnico433 OR css.cd_cargo = @cargoSupervisorTecnico434 THEN 1
		WHEN css.cd_cargo = @cargoSupervisor THEN 2
		WHEN css.cd_cargo = @cargoDiretor THEN 3
		WHEN css.cd_cargo = @cargoAD THEN 4
		WHEN css.cd_cargo = @cargoCP THEN 5
		WHEN css.cd_cargo = @cargoATE THEN 8
		WHEN css.cd_cargo = @cargoAuxDesenvolvimentoInfantil THEN 9
	END AS prioridade
INTO #tempCargosSobrepostosFuncionarios_Fixos
FROM
	v_servidor_cotic serv (NOLOCK)
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor
INNER JOIN
	cargo_sobreposto_servidor css (NOLOCK)
	ON cbc.cd_cargo_base_servidor = css.cd_cargo_base_servidor
WHERE
	css.cd_cargo IN (@cargoCP, @cargoAD, @cargoDiretor, @cargoSupervisor, @cargoSupervisorTecnico433, @cargoSupervisorTecnico434, @cargoATE, @cargoAuxDesenvolvimentoInfantil)
	AND (css.dt_fim_cargo_sobreposto IS NULL OR css.dt_fim_cargo_sobreposto > GETDATE());
GO

-- 3. União das tabelas de cargo fixo
IF OBJECT_ID('tempdb..#tempCargosFuncionarios_Fixos') IS NOT NULL
	DROP TABLE #tempCargosFuncionarios_Fixos;

SELECT
	*
INTO #tempCargosFuncionarios_Fixos
FROM
	(SELECT * FROM #tempCargosSobrepostosFuncionarios_Fixos) AS sobrepostos
UNION
	(SELECT * FROM #tempCargosBaseFuncionarios_Fixos 
	 WHERE NOT cd_cargo_base_servidor IN (SELECT DISTINCT cd_cargo_base_servidor FROM #tempCargosSobrepostosFuncionarios_Fixos));

-- 4. Funções específicas ativas
DECLARE @tipoFuncaoPAP AS INT = 30;
DECLARE @tipoFuncaoPAEE AS INT = 6;
DECLARE @tipoFuncaoCIEJAASSISTPED AS INT = 42;
DECLARE @tipoFuncaoCIEJAASSISTCOORD AS INT = 43;
DECLARE @tipoFuncaoCIEJACOORD AS INT = 44;

IF OBJECT_ID('tempdb..#tempProfessores_PAP_PAEE_CIEJA') IS NOT NULL
	DROP TABLE #tempProfessores_PAP_PAEE_CIEJA;
SELECT
	cbc.cd_cargo_base_servidor,
	cbc.cd_servidor,
	cbc.cd_cargo,
	CASE
		WHEN facs.cd_tipo_funcao = @tipoFuncaoPAP THEN 6
		WHEN facs.cd_tipo_funcao = @tipoFuncaoPAEE THEN 7
		ELSE 10
	END AS prioridade
INTO #tempProfessores_PAP_PAEE_CIEJA
FROM
	v_cargo_base_cotic cbc (NOLOCK)
INNER JOIN
	funcao_atividade_cargo_servidor facs (NOLOCK)
	ON cbc.cd_cargo_base_servidor = facs.cd_cargo_base_servidor
WHERE
	facs.cd_tipo_funcao IN (@tipoFuncaoPAP, @tipoFuncaoPAEE, @tipoFuncaoCIEJAASSISTPED, @tipoFuncaoCIEJAASSISTCOORD, @tipoFuncaoCIEJACOORD)
	AND (facs.dt_fim_funcao_atividade IS NULL OR facs.dt_fim_funcao_atividade > GETDATE())
	AND dt_fim_nomeacao IS NULL;

-- 5. União das tabelas de cargo fixo e função
IF OBJECT_ID('tempdb..#tempCargosFuncionarios') IS NOT NULL
	DROP TABLE #tempCargosFuncionarios;
SELECT
	*
INTO #tempCargosFuncionarios
FROM
	(SELECT * FROM #tempCargosFuncionarios_Fixos) AS fixos
UNION
	(SELECT * FROM #tempProfessores_PAP_PAEE_CIEJA);

-- 6. Ajustar prioridade para funcionários com mais de um cargo
IF OBJECT_ID('tempdb..#tempFuncionariosCargoPrioridade') IS NOT NULL
	DROP TABLE #tempFuncionariosCargoPrioridade;
SELECT
	cd_servidor,
	MIN(prioridade) AS prioridade
INTO #tempFuncionariosCargoPrioridade
FROM
	#tempCargosFuncionarios
GROUP BY
	cd_servidor

IF OBJECT_ID('tempdb..#tempCargosFuncionariosRemovendoDuplicados') IS NOT NULL
	DROP TABLE #tempCargosFuncionariosRemovendoDuplicados;
SELECT
	DISTINCT
	t2.cd_cargo,
	t1.cd_servidor
INTO #tempCargosFuncionariosRemovendoDuplicados
FROM
	#tempFuncionariosCargoPrioridade t1
CROSS APPLY
(
	SELECT TOP 1 * FROM #tempCargosFuncionarios temp WHERE temp.cd_servidor = t1.cd_servidor AND temp.prioridade = t1.prioridade
) AS t2;

-- 7. Tabela final de omportação
IF OBJECT_ID('tempdb..#tempCargosFuncionariosRegularesAtivos') IS NOT NULL
	DROP TABLE #tempCargosFuncionariosRegularesAtivos;
SELECT
	serv.cd_registro_funcional AS cd_servidor_cotic,
	NULL AS cd_servidor_classroom,
	[dbo].[proc_gerar_email_funcionario](serv.nm_pessoa, serv.cd_registro_funcional) AS nm_email,
	'True' AS in_ativo,
	[dbo].[proc_gerar_unidade_organizacional_funcionario_v2](temp.cd_cargo, '') AS nm_organizacao,
	temp.cd_cargo
INTO #tempCargosFuncionariosRegularesAtivos
FROM
	v_servidor_cotic serv (NOLOCK)
INNER JOIN
	#tempCargosFuncionariosRemovendoDuplicados temp
	ON temp.cd_servidor = serv.cd_servidor
GO

-- 8. Readaptados
DECLARE @tipoLaudoReadaptados AS CHAR = 'R';

IF OBJECT_ID('tempdb..#tempProfessores_Readaptados') IS NOT NULL
	DROP TABLE #tempProfessores_Readaptados;
SELECT
	cbc.cd_cargo_base_servidor,
	cbc.cd_cargo,
	cd_servidor
INTO #tempProfessores_Readaptados
FROM
	v_cargo_base_cotic cbc (NOLOCK)
INNER JOIN
	laudo_medico lm (NOLOCK)
	ON cbc.cd_cargo_base_servidor = lm.cd_cargo_base_servidor
WHERE 
	lm.cd_tipo_laudo = @tipoLaudoReadaptados
	AND lm.dt_publicacao_doc_cessacao_laudo IS NULL
	AND dt_fim_nomeacao IS NULL;

IF OBJECT_ID('tempdb..#tempProfessores_ReadaptadosMaiorCargo') IS NOT NULL
	DROP TABLE #tempProfessores_ReadaptadosMaiorCargo;
SELECT
	cd_servidor,
	MAX(cd_cargo) AS cd_cargo
INTO #tempProfessores_ReadaptadosMaiorCargo
FROM
	#tempProfessores_Readaptados
GROUP BY
	cd_servidor;

IF OBJECT_ID('tempdb..#tempProfessores_ReadaptadosRemoveDuplicados') IS NOT NULL
	DROP TABLE #tempProfessores_ReadaptadosRemoveDuplicados;
SELECT
	t1.cd_servidor,
	t1.cd_cargo,
	t2.cd_cargo_base_servidor
INTO #tempProfessores_ReadaptadosRemoveDuplicados
FROM
	#tempProfessores_ReadaptadosMaiorCargo t1
CROSS APPLY
(
	SELECT TOP 1 * FROM #tempProfessores_Readaptados temp WHERE temp.cd_servidor = t1.cd_servidor AND temp.cd_cargo = t1.cd_cargo
) AS t2

IF OBJECT_ID('tempdb..#tempProfessores_ReadaptadosAtivos') IS NOT NULL
	DROP TABLE #tempProfessores_ReadaptadosAtivos;
SELECT
	serv.cd_registro_funcional AS cd_servidor_cotic,
	NULL AS cd_servidor_classroom,
	[dbo].[proc_gerar_email_funcionario](serv.nm_pessoa, serv.cd_registro_funcional) AS nm_email,
	'True' AS in_ativo,
	[dbo].[proc_gerar_unidade_organizacional_funcionario_v2](cbc.cd_cargo, 'R') AS nm_organizacao,
	cbc.cd_cargo
INTO #tempProfessores_ReadaptadosAtivos
FROM
	v_servidor_cotic serv (NOLOCK)
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor 
INNER JOIN
	#tempProfessores_ReadaptadosRemoveDuplicados temp
	ON cbc.cd_cargo_base_servidor = temp.cd_cargo_base_servidor;

-- Totalização
IF OBJECT_ID('tempdb..#tempFuncionariosAtivos') IS NOT NULL
	DROP TABLE #tempFuncionariosAtivos;
SELECT
	*
INTO #tempFuncionariosAtivos
FROM
	(SELECT DISTINCT * FROM #tempCargosFuncionariosRegularesAtivos) AS regulares
UNION
	(SELECT DISTINCT * FROM #tempProfessores_ReadaptadosAtivos WHERE NOT cd_servidor_cotic IN (SELECT cd_servidor_cotic FROM #tempCargosFuncionariosRegularesAtivos));


IF OBJECT_ID('tempdb..#tempFuncionariosQueSeraoAdicionados') IS NOT NULL
	DROP TABLE #tempFuncionariosQueSeraoAdicionados;
SELECT
	temp.*
INTO #tempFuncionariosQueSeraoAdicionados
FROM
	#tempFuncionariosAtivos temp
LEFT JOIN
	servidor_classroom servClass (NOLOCK)
	ON temp.cd_servidor_cotic = servClass.cd_servidor_cotic
WHERE
	servClass.nm_email IS NULL;

INSERT INTO servidor_classroom
SELECT DISTINCT *, 0 FROM #tempFuncionariosQueSeraoAdicionados;
-- ROLLBACK
-- COMMIT
