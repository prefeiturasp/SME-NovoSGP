USE [se1426]

DECLARE @cargoCP AS INT = 3379;
DECLARE @cargoAD AS INT = 3085;
DECLARE @cargoDiretor AS INT = 3360;
DECLARE @cargoSupervisor AS INT = 3352;
DECLARE @cargoSupervisorTecnico433 AS INT = 433;
DECLARE @cargoSupervisorTecnico434 AS INT = 434;
DECLARE @cargoATE AS INT = 4906;
DECLARE @cargoAuxDesenvolvimentoInfantil AS INT = (SELECT cd_cargo FROM cargo (NOLOCK) WHERE dc_cargo = 'AUXILIAR DE DESENVOLVIMENTO INFANTIL');

-- 1. Busca os funcionários por cargo fixo
IF OBJECT_ID('tempdb..#tempServidorCargosBaseFixos') IS NOT NULL
	DROP TABLE #tempServidorCargosBaseFixos;
SELECT
	cbc.cd_cargo_base_servidor, 
	cbc.cd_cargo,
	cbc.cd_servidor,
	serv.cd_registro_funcional,
	ls.cd_unidade_educacao,
	servClass.nm_email
INTO #tempServidorCargosBaseFixos
FROM
	servidor_classroom servClass (NOLOCK)
INNER JOIN
	v_servidor_cotic serv (NOLOCK)
	ON servClass.cd_servidor_cotic = serv.cd_registro_funcional
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor
INNER JOIN
	lotacao_servidor ls (NOLOCK)
	ON ls.cd_cargo_base_servidor = cbc.cd_cargo_base_servidor
WHERE
	servClass.in_ativo = 1
	AND cbc.cd_cargo IN (@cargoCP, @cargoAD, @cargoDiretor)
	AND dt_fim_nomeacao IS NULL
	AND (ls.dt_fim IS NULL OR ls.dt_fim > GETDATE());
GO

-- 2. Busca os funcionários por cargo sobreposto fixo
DECLARE @cargoCP AS INT = 3379;
DECLARE @cargoAD AS INT = 3085;
DECLARE @cargoDiretor AS INT = 3360;
DECLARE @cargoSupervisor AS INT = 3352;
DECLARE @cargoSupervisorTecnico433 AS INT = 433;
DECLARE @cargoSupervisorTecnico434 AS INT = 434;
DECLARE @cargoATE AS INT = 4906;
DECLARE @cargoAuxDesenvolvimentoInfantil AS INT = (SELECT cd_cargo FROM cargo (NOLOCK) WHERE dc_cargo = 'AUXILIAR DE DESENVOLVIMENTO INFANTIL');

IF OBJECT_ID('tempdb..#tempServidorCargosSobrepostosFixos') IS NOT NULL
	DROP TABLE #tempServidorCargosSobrepostosFixos;
SELECT
	cbc.cd_cargo_base_servidor, 
	css.cd_cargo,
	cbc.cd_servidor,
	serv.cd_registro_funcional,
	css.cd_unidade_local_servico AS cd_unidade_educacao,
	servClass.nm_email
INTO #tempServidorCargosSobrepostosFixos
FROM
	servidor_classroom servClass (NOLOCK)
INNER JOIN
	v_servidor_cotic serv (NOLOCK)
	ON servClass.cd_servidor_cotic = serv.cd_registro_funcional
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor
INNER JOIN
	cargo_sobreposto_servidor css (NOLOCK)
	ON cbc.cd_cargo_base_servidor = css.cd_cargo_base_servidor
INNER JOIN
	escola esc 
	ON css.cd_unidade_local_servico = esc.cd_escola AND esc.tp_escola IN (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
WHERE
	servClass.in_ativo = 1
	AND css.cd_cargo IN (@cargoCP, @cargoAD, @cargoDiretor)
	AND (css.dt_fim_cargo_sobreposto IS NULL OR css.dt_fim_cargo_sobreposto > GETDATE());
GO

IF OBJECT_ID('tempdb..#tempServidorCargosFixos') IS NOT NULL
	DROP TABLE #tempServidorCargosFixos;
SELECT
	*
INTO #tempServidorCargosFixos
FROM
	(SELECT * FROM #tempServidorCargosSobrepostosFixos) AS Sobrepostos
UNION
	(SELECT * FROM #tempServidorCargosBaseFixos WHERE NOT cd_cargo_base_servidor IN (SELECT cd_cargo_base_servidor FROM #tempServidorCargosSobrepostosFixos))

-- 3. Busca os funcionários por função
DECLARE @tipoFuncaoPAP AS INT = 30;
DECLARE @tipoFuncaoPAEE AS INT = 6;
DECLARE @tipoFuncaoCIEJAASSISTPED AS INT = 42;
DECLARE @tipoFuncaoCIEJAASSISTCOORD AS INT = 43;
DECLARE @tipoFuncaoCIEJACOORD AS INT = 44;

IF OBJECT_ID('tempdb..#tempServidorFuncao') IS NOT NULL
	DROP TABLE #tempServidorFuncao;
SELECT
	cbc.cd_cargo_base_servidor, 
	cbc.cd_cargo,
	cbc.cd_servidor,
	serv.cd_registro_funcional,
	esc.cd_escola AS cd_unidade_educacao,
	servClass.nm_email
INTO #tempServidorFuncao
FROM
	servidor_classroom servClass (NOLOCK)
INNER JOIN
	v_servidor_cotic serv (NOLOCK)
	ON servClass.cd_servidor_cotic = serv.cd_registro_funcional
INNER JOIN
	v_cargo_base_cotic cbc (NOLOCK)
	ON serv.cd_servidor = cbc.cd_servidor
INNER JOIN
	funcao_atividade_cargo_servidor facs (NOLOCK)
	ON cbc.cd_cargo_base_servidor = facs.cd_cargo_base_servidor
INNER JOIN
	escola esc 
	ON facs.cd_unidade_local_servico = esc.cd_escola AND esc.tp_escola IN (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
WHERE
	servClass.in_ativo = 1
	AND facs.cd_tipo_funcao IN (@tipoFuncaoPAP, @tipoFuncaoPAEE, @tipoFuncaoCIEJAASSISTPED, @tipoFuncaoCIEJAASSISTCOORD, @tipoFuncaoCIEJACOORD)
	AND (facs.dt_fim_funcao_atividade IS NULL OR facs.dt_fim_funcao_atividade > GETDATE())
	AND dt_fim_nomeacao IS NULL;
GO

-- 4. União de cargos fixos e funções
IF OBJECT_ID('tempdb..#tempServidorCargosEscola') IS NOT NULL
	DROP TABLE #tempServidorCargosEscola;
SELECT
	*
INTO #tempServidorCargosEscola
FROM
	(SELECT * FROM #tempServidorCargosFixos) AS CargosFixos
UNION
	(SELECT * FROM #tempServidorFuncao);

-- 5. Busca os cargos dos funcionários da tabela de curso
IF OBJECT_ID('tempdb..#tempCursoCriadorFuncionario') IS NOT NULL
	DROP TABLE #tempCursoCriadorFuncionario;
SELECT
	DISTINCT
	temp.*,
	curso.cd_curso_classroom
INTO #tempCursoCriadorFuncionario
FROM
	turma_componente_curricular_classroom curso
INNER JOIN
	#tempServidorCargosEscola temp
	ON curso.email_criador = temp.nm_email;

-- 6. Seleciona os funcionários da mesma escola e com cargo semelhante para serem adicionados
SELECT
	t1.cd_cargo_base_servidor,
	t1.cd_cargo,
	t1.cd_servidor,
	t1.cd_registro_funcional,
	t1.cd_unidade_educacao,
	t1.nm_email AS EmailCriador,
	t1.cd_curso_classroom AS CursoGoogleId,
	t2.nm_email AS Email
FROM
	#tempCursoCriadorFuncionario t1
INNER JOIN
	#tempServidorCargosEscola t2
	ON t1.cd_unidade_educacao = t2.cd_unidade_educacao AND t1.cd_cargo_base_servidor <> t2.cd_cargo_base_servidor;


