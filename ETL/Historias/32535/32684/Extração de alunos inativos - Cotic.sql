DECLARE @anoLetivo AS INT = 2021;
DECLARE @anoAnterior AS INT = 2020;

DECLARE @situacaoAtivo AS CHAR = 1;
DECLARE @situacaoFinalizado AS CHAR = 5;
DECLARE @situacaoPendenteRematricula AS CHAR = 6;
DECLARE @situacaoRematriculado AS CHAR = 10;
DECLARE @situacaoSemContinuidade AS CHAR = 13;

DECLARE @situacaoAtivoInt AS INT = 1;
DECLARE @situacaoFinalizadoInt AS INT = 5;
DECLARE @situacaoPendenteRematriculaInt AS INT = 6;
DECLARE @situacaoRematriculadoInt AS INT = 10;
DECLARE @situacaoSemContinuidadeInt AS INT = 13;

-- 1. Matrícula COTIC
--- 1.1. Matrículas ano anterior
IF OBJECT_ID('tempdb..#tempAlunosMatriculasCoticAnoAnterior') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasCoticAnoAnterior;
SELECT
	DISTINCT matr.cd_aluno
INTO #tempAlunosMatriculasCoticAnoAnterior
FROM
	v_aluno_cotic aluno (NOLOCK)
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno = matr.cd_aluno
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoFinalizado, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	and mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoFinalizadoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	and matr.an_letivo = @anoAnterior;

-- 1.2. Matrículas no ano letivo
IF OBJECT_ID('tempdb..#tempAlunosMatriculasCoticAnoAnteriorQuePossuemMatriculaNoAnoAtual') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasCoticAnoAnteriorQuePossuemMatriculaNoAnoAtual;

SELECT
	DISTINCT temp.cd_aluno
INTO #tempAlunosMatriculasCoticAnoAnteriorQuePossuemMatriculaNoAnoAtual
FROM
	#tempAlunosMatriculasCoticAnoAnterior temp
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON temp.cd_aluno = matr.cd_aluno
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	and mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	and matr.an_letivo = @anoLetivo;

--- 1.3. Mantém somente as matrículas que não possuem renovação no ano letivo
IF OBJECT_ID('tempdb..#tempAlunosMatriculasCoticAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasCoticAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual;
SELECT
	t1.*
INTO #tempAlunosMatriculasCoticAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual
FROM
	#tempAlunosMatriculasCoticAnoAnterior t1
LEFT JOIN
	#tempAlunosMatriculasCoticAnoAnteriorQuePossuemMatriculaNoAnoAtual t2
	ON t1.cd_aluno = t2.cd_aluno
WHERE t2.cd_aluno IS NULL;

-- 1.4. Totalização
IF OBJECT_ID('tempdb..#tempAlunosMatriculasCoticAtivasNoAnoAnterior') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasCoticAtivasNoAnoAnterior;

SELECT
	aluno.cd_aluno,
	matr.cd_matricula,
	matr.dt_status_matricula
INTO #tempAlunosMatriculasCoticAtivasNoAnoAnterior
FROM
	#tempAlunosMatriculasCoticAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual temp
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluno.cd_aluno = temp.cd_aluno
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno = matr.cd_aluno
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	and mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	and matr.an_letivo = @anoAnterior;

--- 1.5. Busca as datas mais recentes para casos de mais de uma matrícula no ano
IF OBJECT_ID('tempdb..#tempAlunosMatriculasCoticAtivasNoAnoAnteriorDatasMaisRecentes') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasCoticAtivasNoAnoAnteriorDatasMaisRecentes;

SELECT 
	cd_aluno,
	MAX(dt_status_matricula) AS dt_status_matricula
INTO #tempAlunosMatriculasCoticAtivasNoAnoAnteriorDatasMaisRecentes
FROM #tempAlunosMatriculasCoticAtivasNoAnoAnterior
GROUP BY cd_aluno;

--- 1.6. Busca apenas as matriculas mais recentes do ano anterior
IF OBJECT_ID('tempdb..#tempAlunosMatriculasCoticAtivasNoAnoAnteriorRemovendoDuplicadas') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasCoticAtivasNoAnoAnteriorRemovendoDuplicadas;

SELECT
	t1.*
INTO #tempAlunosMatriculasCoticAtivasNoAnoAnteriorRemovendoDuplicadas
FROM
	#tempAlunosMatriculasCoticAtivasNoAnoAnterior t1
INNER JOIN
	#tempAlunosMatriculasCoticAtivasNoAnoAnteriorDatasMaisRecentes t2
	ON t1.cd_aluno = t2.cd_aluno AND t1.dt_status_matricula = t2.dt_status_matricula;

--- 1.7. Final
IF OBJECT_ID('tempdb..#tempAlunosCoticQueSeraoInativados') IS NOT NULL
	DROP TABLE #tempAlunosCoticQueSeraoInativados;

SELECT
	NULL AS cd_aluno_classroom,
	aluno.cd_aluno AS cd_aluno_eol,
	[dbo].[proc_gerar_email_aluno](aluno.nm_aluno, aluno.dt_nascimento_aluno) AS nm_email,
	'False' AS in_ativo,
	'/Alunos/Inativos' AS nm_organizacao
INTO #tempAlunosCoticQueSeraoInativados
FROM
	#tempAlunosMatriculasCoticAtivasNoAnoAnteriorRemovendoDuplicadas temp
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluno.cd_aluno = temp.cd_aluno
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno = matr.cd_aluno AND matr.cd_matricula = temp.cd_matricula
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
INNER JOIN 
	turma_escola te (NOLOCK) 
	ON mte.cd_turma_escola = te.cd_turma_escola 
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoFinalizado, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	and mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoFinalizadoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	and matr.an_letivo = @anoAnterior;

INSERT INTO aluno_classroom
SELECT DISTINCT cd_aluno_classroom, cd_aluno_eol, nm_email, in_ativo, nm_organizacao 
FROM #tempAlunosCoticQueSeraoInativados
WHERE NOT cd_aluno_eol IN (SELECT cd_aluno_eol FROM aluno_classroom);
