-- 2. Histórico
--- 2.1. Matrículas ano anterior
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

IF OBJECT_ID('tempdb..#tempAlunosMatriculasHistoricoAnoAnterior') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasHistoricoAnoAnterior;
SELECT
	DISTINCT matr.cd_aluno
INTO #tempAlunosMatriculasHistoricoAnoAnterior
FROM
	v_aluno_cotic aluno (NOLOCK)
INNER JOIN 
	v_historico_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno = matr.cd_aluno
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoFinalizado, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	and matr.an_letivo = @anoAnterior;
GO

-- 2.2. Matrículas no ano letivo
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
IF OBJECT_ID('tempdb..#tempAlunosMatriculasHistoricoAnoAnteriorQuePossuemMatriculaNoAnoAtual') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasHistoricoAnoAnteriorQuePossuemMatriculaNoAnoAtual;

SELECT
	DISTINCT temp.cd_aluno
INTO #tempAlunosMatriculasHistoricoAnoAnteriorQuePossuemMatriculaNoAnoAtual
FROM
	#tempAlunosMatriculasHistoricoAnoAnterior temp
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
GO

--- 2.3. Mantém somente as matrículas que não possuem renovação no ano letivo
IF OBJECT_ID('tempdb..#tempAlunosMatriculasHistoricoAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual') IS NOT NULL
	DROP TABLE #tempAlunosMatriculasHistoricoAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual;
SELECT
	t1.*
INTO #tempAlunosMatriculasHistoricoAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual
FROM
	#tempAlunosMatriculasHistoricoAnoAnterior t1
LEFT JOIN
	#tempAlunosMatriculasHistoricoAnoAnteriorQuePossuemMatriculaNoAnoAtual t2
	ON t1.cd_aluno = t2.cd_aluno
WHERE t2.cd_aluno IS NULL;
GO

--- 2.4. Final
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
IF OBJECT_ID('tempdb..#tempAlunosHistoricoQueSeraoInativados') IS NOT NULL
	DROP TABLE #tempAlunosHistoricoQueSeraoInativados;

SELECT
	NULL AS cd_aluno_classroom,
	aluno.cd_aluno AS cd_aluno_eol,
	[dbo].[proc_gerar_email_aluno](aluno.nm_aluno, aluno.dt_nascimento_aluno) AS nm_email,
	'False' AS in_ativo,
	'/Alunos/Inativos' AS nm_organizacao
INTO #tempAlunosHistoricoQueSeraoInativados
FROM
	#tempAlunosMatriculasHistoricoAnoAnteriorQueNaoPossuemMatriculaNoAnoAtual temp
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON aluno.cd_aluno = temp.cd_aluno
GO

INSERT INTO aluno_classroom
SELECT DISTINCT cd_aluno_classroom, cd_aluno_eol, nm_email, in_ativo, nm_organizacao FROM #tempAlunosHistoricoQueSeraoInativados
WHERE NOT cd_aluno_eol IN (SELECT cd_aluno_eol FROM aluno_classroom);