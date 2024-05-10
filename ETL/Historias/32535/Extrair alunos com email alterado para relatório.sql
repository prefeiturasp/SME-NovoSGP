IF OBJECT_ID('tempdb..#tempAlunosComEmailAjustado') IS NOT NULL
	DROP TABLE #tempAlunosComEmailAjustado;

SELECT
	DISTINCT
	aluClass.cd_aluno_eol,
	aluClass.nm_email,
	aluno.nm_aluno,
	aluno.dt_nascimento_aluno,
	esc.cd_escola,
	ue.nm_unidade_educacao,
	ue.cd_unidade_administrativa_referencia,
	ue.nm_exibicao_unidade
INTO #tempAlunosComEmailAjustado
FROM
	aluno_classroom aluClass (NOLOCK)
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluClass.cd_aluno_eol = aluno.cd_aluno
INNER JOIN
	v_matricula_cotic matr (NOLOCK)
	ON aluno.cd_aluno = matr.cd_aluno AND matr.an_letivo = 2021
INNER JOIN
	escola esc (NOLOCK)
	ON matr.cd_escola = esc.cd_escola
INNER JOIN
	v_cadastro_unidade_educacao ue
	ON esc.cd_escola = ue.cd_unidade_educacao
INNER JOIN
	unidade_administrativa dre
	ON ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
WHERE
	email_alterado = 1
	AND matr.an_letivo = 2021
	AND matr.st_matricula IN (1,6,10,13)
	AND NOT matr.cd_serie_ensino IS NULL
ORDER BY aluClass.cd_aluno_eol;

IF OBJECT_ID('tempdb..#tempAlunosProgramaComEmailAjustado') IS NOT NULL
	DROP TABLE #tempAlunosProgramaComEmailAjustado;

SELECT
	DISTINCT
	aluClass.cd_aluno_eol,
	aluClass.nm_email,
	aluno.nm_aluno,
	aluno.dt_nascimento_aluno,
	esc.cd_escola,
	ue.nm_unidade_educacao,
	ue.cd_unidade_administrativa_referencia,
	ue.nm_exibicao_unidade
INTO #tempAlunosProgramaComEmailAjustado
FROM
	aluno_classroom aluClass (NOLOCK)
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluClass.cd_aluno_eol = aluno.cd_aluno
INNER JOIN
	v_matricula_cotic matr (NOLOCK)
	ON aluno.cd_aluno = matr.cd_aluno AND matr.an_letivo = 2021
INNER JOIN
	escola esc (NOLOCK)
	ON matr.cd_escola = esc.cd_escola
INNER JOIN
	v_cadastro_unidade_educacao ue
	ON esc.cd_escola = ue.cd_unidade_educacao
INNER JOIN
	unidade_administrativa dre
	ON ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
WHERE
	email_alterado = 1
	AND matr.an_letivo = 2021
	AND matr.st_matricula IN (1,6,10,13)
	AND NOT matr.cd_tipo_programa IS NULL
ORDER BY aluClass.cd_aluno_eol;

IF OBJECT_ID('tempdb..#tempAlunosMatr2E3ComEmailAjustado') IS NOT NULL
	DROP TABLE #tempAlunosMatr2E3ComEmailAjustado;

SELECT
	DISTINCT
	aluClass.cd_aluno_eol,
	aluClass.nm_email,
	aluno.nm_aluno,
	aluno.dt_nascimento_aluno,
	esc.cd_escola,
	ue.nm_unidade_educacao,
	ue.cd_unidade_administrativa_referencia,
	ue.nm_exibicao_unidade
INTO #tempAlunosMatr2E3ComEmailAjustado
FROM
	aluno_classroom aluClass (NOLOCK)
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluClass.cd_aluno_eol = aluno.cd_aluno
INNER JOIN
	v_matricula_cotic matr (NOLOCK)
	ON aluno.cd_aluno = matr.cd_aluno AND matr.an_letivo = 2021
INNER JOIN
	escola esc (NOLOCK)
	ON matr.cd_escola = esc.cd_escola
INNER JOIN
	v_cadastro_unidade_educacao ue
	ON esc.cd_escola = ue.cd_unidade_educacao
INNER JOIN
	unidade_administrativa dre
	ON ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
WHERE
	email_alterado = 1
	AND matr.an_letivo = 2021
	AND matr.st_matricula IN (2,3)
	AND NOT matr.cd_serie_ensino IS NULL
ORDER BY aluClass.cd_aluno_eol;


IF OBJECT_ID('tempdb..#tempAlunosComEmailAjustadoConsolidado') IS NOT NULL
	DROP TABLE #tempAlunosComEmailAjustadoConsolidado;
SELECT
	*
INTO #tempAlunosComEmailAjustadoConsolidado
FROM
	(SELECT * FROM #tempAlunosComEmailAjustado) AS Regulares
UNION
	(SELECT * FROM #tempAlunosProgramaComEmailAjustado WHERE NOT cd_aluno_eol IN (SELECT DISTINCT cd_aluno_eol FROM #tempAlunosComEmailAjustado))
UNION
	(SELECT * FROM #tempAlunosMatr2E3ComEmailAjustado WHERE NOT cd_aluno_eol IN (SELECT DISTINCT cd_aluno_eol FROM #tempAlunosComEmailAjustado));


SELECT * FROM #tempAlunosComEmailAjustadoConsolidado;