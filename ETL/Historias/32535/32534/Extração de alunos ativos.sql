USE se1426;

--BEGIN TRAN

DECLARE @anoLetivo AS INT = 2021;
DECLARE @situacaoAtivo AS CHAR = 1;
DECLARE @situacaoPendenteRematricula AS CHAR = 6;
DECLARE @situacaoRematriculado AS CHAR = 10;
DECLARE @situacaoSemContinuidade AS CHAR = 13;

DECLARE @situacaoAtivoInt AS INT = 1;
DECLARE @situacaoPendenteRematriculaInt AS INT = 6;
DECLARE @situacaoRematriculadoInt AS INT = 10;
DECLARE @situacaoSemContinuidadeInt AS INT = 13;

-- 1. Busca matrículas regulares
IF OBJECT_ID('tempdb..#tempAlunosMatriculasAtivas') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasAtivas;
SELECT
	aluno.cd_aluno,
	matr.cd_matricula,
	matr.dt_status_matricula
INTO #tempAlunosMatriculasAtivas
FROM
	v_aluno_cotic aluno (NOLOCK)
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno = matr.cd_aluno
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	AND mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	AND matr.an_letivo = @anoLetivo
	AND NOT cd_serie_ensino IS NULL;

--- 1.1 Agrupa para buscar a mais recente em caso de mais de uma no ano por aluno
IF OBJECT_ID('tempdb..#tempAlunosMatriculasAtivasDatasMaisRecentes') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasAtivasDatasMaisRecentes;
SELECT 
	cd_aluno,
	MAX(dt_status_matricula) AS dt_status_matricula
INTO #tempAlunosMatriculasAtivasDatasMaisRecentes
FROM #tempAlunosMatriculasAtivas
GROUP BY cd_aluno;

--- 1.2 Mantém apenas a matrícula mais recente de cada aluno
IF OBJECT_ID('tempdb..#tempAlunosMatriculasAtivasRemovendoDuplicadas') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasAtivasRemovendoDuplicadas;
SELECT
	t1.*
INTO #tempAlunosMatriculasAtivasRemovendoDuplicadas
FROM
	#tempAlunosMatriculasAtivas t1
INNER JOIN
	#tempAlunosMatriculasAtivasDatasMaisRecentes t2
	ON t1.cd_aluno = t2.cd_aluno AND t1.dt_status_matricula = t2.dt_status_matricula;
GO

--- 1.3 Montagem da tabela de inserção
DECLARE @anoLetivo AS INT = 2021;
DECLARE @situacaoAtivo AS CHAR = 1;
DECLARE @situacaoPendenteRematricula AS CHAR = 6;
DECLARE @situacaoRematriculado AS CHAR = 10;
DECLARE @situacaoSemContinuidade AS CHAR = 13;

DECLARE @situacaoAtivoInt AS INT = 1;
DECLARE @situacaoPendenteRematriculaInt AS INT = 6;
DECLARE @situacaoRematriculadoInt AS INT = 10;
DECLARE @situacaoSemContinuidadeInt AS INT = 13;

IF OBJECT_ID('tempdb..#tempAlunosAtivos') IS NOT NULL
	DROP TABLE #tempAlunosAtivos;
SELECT
	NULL AS cd_aluno_classroom,
	aluno.cd_aluno AS cd_aluno_eol,
	[dbo].[proc_gerar_email_aluno](aluno.nm_aluno, aluno.dt_nascimento_aluno) AS nm_email,
	'True' AS in_ativo,
	[dbo].[proc_gerar_unidade_organizacional_aluno](se.cd_modalidade_ensino, se.cd_etapa_ensino, ce.cd_ciclo_ensino) AS nm_organizacao
INTO #tempAlunosAtivos
FROM
	#tempAlunosMatriculasAtivasRemovendoDuplicadas temp
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
INNER JOIN 
	serie_turma_grade stg (NOLOCK) 
	ON stg.cd_turma_escola = te.cd_turma_escola
INNER JOIN 
	serie_ensino se (NOLOCK) 
	ON se.cd_serie_ensino = stg.cd_serie_ensino
INNER JOIN 
	etapa_ensino ee (NOLOCK) 
	ON se.cd_etapa_ensino = ee.cd_etapa_ensino and ee.cd_modalidade_ensino = se.cd_modalidade_ensino
INNER JOIN
	ciclo_ensino ce (NOLOCK)
	on ce.cd_etapa_ensino = ee.cd_etapa_ensino and ce.cd_modalidade_ensino = ee.cd_modalidade_ensino and ce.cd_ciclo_ensino = se.cd_ciclo_ensino
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	and mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	and matr.an_letivo = @anoLetivo;
GO

-- 2. Busca matrículas de programa
DECLARE @anoLetivo AS INT = 2021;
DECLARE @situacaoAtivo AS CHAR = 1;
DECLARE @situacaoPendenteRematricula AS CHAR = 6;
DECLARE @situacaoRematriculado AS CHAR = 10;
DECLARE @situacaoSemContinuidade AS CHAR = 13;

DECLARE @situacaoAtivoInt AS INT = 1;
DECLARE @situacaoPendenteRematriculaInt AS INT = 6;
DECLARE @situacaoRematriculadoInt AS INT = 10;
DECLARE @situacaoSemContinuidadeInt AS INT = 13;
IF OBJECT_ID('tempdb..#tempAlunosMatriculasProgramaAtivas') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasProgramaAtivas;
SELECT
	aluno.cd_aluno,
	matr.cd_matricula,
	matr.dt_status_matricula
INTO #tempAlunosMatriculasProgramaAtivas
FROM
	v_aluno_cotic aluno (NOLOCK)
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno = matr.cd_aluno
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	AND mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	AND matr.an_letivo = @anoLetivo
	AND NOT matr.cd_tipo_programa IS NULL;

--- 2.1 Agrupa para buscar a mais recente em caso de mais de uma no ano por aluno
IF OBJECT_ID('tempdb..#tempAlunosMatriculasProgramaAtivasDatasMaisRecentes') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasProgramaAtivasDatasMaisRecentes;
SELECT 
	cd_aluno,
	MAX(dt_status_matricula) AS dt_status_matricula
INTO #tempAlunosMatriculasProgramaAtivasDatasMaisRecentes
FROM #tempAlunosMatriculasProgramaAtivas
GROUP BY cd_aluno;

--- 2.2 Mantém apenas a matrícula mais recente de cada aluno
IF OBJECT_ID('tempdb..#tempAlunosMatriculasProgramaAtivasRemovendoDuplicadas') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasProgramaAtivasRemovendoDuplicadas;
SELECT
	t1.*
INTO #tempAlunosMatriculasProgramaAtivasRemovendoDuplicadas
FROM
	#tempAlunosMatriculasProgramaAtivas t1
INNER JOIN
	#tempAlunosMatriculasProgramaAtivasDatasMaisRecentes t2
	ON t1.cd_aluno = t2.cd_aluno AND t1.dt_status_matricula = t2.dt_status_matricula;
GO

--- 2.3 Montagem da tabela de inserção
DECLARE @anoLetivo AS INT = 2021;
DECLARE @situacaoAtivo AS CHAR = 1;
DECLARE @situacaoPendenteRematricula AS CHAR = 6;
DECLARE @situacaoRematriculado AS CHAR = 10;
DECLARE @situacaoSemContinuidade AS CHAR = 13;

DECLARE @situacaoAtivoInt AS INT = 1;
DECLARE @situacaoPendenteRematriculaInt AS INT = 6;
DECLARE @situacaoRematriculadoInt AS INT = 10;
DECLARE @situacaoSemContinuidadeInt AS INT = 13;

IF OBJECT_ID('tempdb..#tempAlunosProgramaAtivos') IS NOT NULL
	DROP TABLE #tempAlunosProgramaAtivos;
SELECT
	NULL AS cd_aluno_classroom,
	aluno.cd_aluno AS cd_aluno_eol,
	[dbo].[proc_gerar_email_aluno](aluno.nm_aluno, aluno.dt_nascimento_aluno) AS nm_email,
	'True' AS in_ativo,
	'/Alunos/FUNDAMENTAL' AS nm_organizacao
INTO #tempAlunosProgramaAtivos
FROM
	#tempAlunosMatriculasProgramaAtivasRemovendoDuplicadas temp
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluno.cd_aluno = temp.cd_aluno
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno = matr.cd_aluno AND matr.cd_matricula = temp.cd_matricula
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	and mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	and matr.an_letivo = @anoLetivo;

-- 3. União dos dois tipos de matrículas
IF OBJECT_ID('tempdb..#tempAlunosMatriculasAtivasFinal') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasAtivasFinal;
SELECT
	DISTINCT
	*
INTO #tempAlunosMatriculasAtivasFinal
FROM
	(SELECT DISTINCT *, 0 AS email_alterado, 1 AS AlunoRegular, 0 AS AlunoPrograma, 0 AS AlunoNovo FROM #tempAlunosAtivos) AS Regulares
UNION
	(SELECT DISTINCT *, 0 AS email_alterado, 0 AS AlunoRegular, 1 AS AlunoPrograma, 0 AS AlunoNovo FROM #tempAlunosProgramaAtivos 
	 WHERE NOT cd_aluno_eol IN (SELECT DISTINCT cd_aluno_eol FROM #tempAlunosAtivos));


INSERT INTO aluno_classroom
SELECT DISTINCT * FROM #tempAlunosMatriculasAtivasFinal;

-- COMMIT
-- ROLLBACK