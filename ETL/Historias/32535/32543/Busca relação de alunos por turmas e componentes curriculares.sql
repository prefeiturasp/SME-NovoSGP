USE se1426;

-- 1. Busca os Alunos ativos que foram adicionados no Google - 74059
IF OBJECT_ID('tempdb..#tempAlunosExportacaoAtivos') IS NOT NULL
	DROP TABLE #tempAlunosExportacaoAtivos;
SELECT
	aluno.cd_aluno_eol,
	aluno.nm_email
INTO #tempAlunosExportacaoAtivos
FROM
	aluno_classroom aluno (NOLOCK)
WHERE
	aluno.in_ativo = 1;

-- 2. Busca as matrículas ativas do ano letivo dos alunos

--- Filtro por DRE
DECLARE @CodigoDre INT = 109000; 

DECLARE @anoLetivo AS INT = 2021;
DECLARE @situacaoAtivo AS CHAR = 1;
DECLARE @situacaoPendenteRematricula AS CHAR = 6;
DECLARE @situacaoRematriculado AS CHAR = 10;
DECLARE @situacaoSemContinuidade AS CHAR = 13;

DECLARE @situacaoAtivoInt AS INT = 1;
DECLARE @situacaoPendenteRematriculaInt AS INT = 6;
DECLARE @situacaoRematriculadoInt AS INT = 10;
DECLARE @situacaoSemContinuidadeInt AS INT = 13;

IF OBJECT_ID('tempdb..#tempAlunosMatriculaExportacaoAtivos') IS NOT NULL
	DROP TABLE #tempAlunosMatriculaExportacaoAtivos;
SELECT
	DISTINCT
	aluno.cd_aluno_eol,
	te.cd_turma_escola
INTO #tempAlunosMatriculaExportacaoAtivos
FROM
	#tempAlunosExportacaoAtivos aluno (NOLOCK)
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno_eol = matr.cd_aluno
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
INNER JOIN
	turma_escola te (NOLOCK)
	ON te.cd_turma_escola = mte.cd_turma_escola
INNER JOIN 
	escola esc 
	ON te.cd_escola = esc.cd_escola
INNER JOIN
	v_cadastro_unidade_educacao ue 
	on ue.cd_unidade_educacao = esc.cd_escola
INNER JOIN 
	unidade_administrativa dre 
	on ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	AND mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	AND matr.an_letivo = @anoLetivo
	AND dre.cd_unidade_administrativa = @CodigoDre;

-- 3. Criar tabela de relacionamento
IF OBJECT_ID('tempdb..#tempRelacaoTurmaComponenteCurricular') IS NOT NULL
	DROP TABLE #tempRelacaoTurmaComponenteCurricular;
CREATE TABLE #tempRelacaoTurmaComponenteCurricular
(
	cd_turma_escola INT NOT NULL,
	cd_componente_curricular INT NOT NULL
);

--- 3.1. Pegar todas as turmas distintas
IF OBJECT_ID('tempdb..#tempTurmasAlunosMatriculaExportacaoAtivos') IS NOT NULL
	DROP TABLE #tempTurmasAlunosMatriculaExportacaoAtivos;
SELECT
	DISTINCT
	temp.cd_turma_escola
INTO #tempTurmasAlunosMatriculaExportacaoAtivos
FROM
	#tempAlunosMatriculaExportacaoAtivos temp;

--- 3.2. Busca relação entre turmas e componentes curriculares
IF OBJECT_ID('tempdb..#tempComponentesCurricularesPorTurmas') IS NOT NULL
	DROP TABLE #tempComponentesCurricularesPorTurmas;
select distinct 
	CASE
			WHEN etapa_ensino.cd_etapa_ensino IN( 1 )
		THEN 'REGÊNCIA DE CLASSE INFANTIL'
	ELSE
		iif(tgt.cd_serie_grade is not null, concat(LTRIM(RTRIM(ter.dc_territorio_saber)), ' - ',  LTRIM(RTRIM(tep.dc_experiencia_pedagogica))), 
		coalesce(LTRIM(RTRIM(pcc.dc_componente_curricular)),   LTRIM(RTRIM(cc.dc_componente_curricular))))
	END Nome,
	CONCAT(CASE
			WHEN etapa_ensino.cd_etapa_ensino IN( 2, 3, 7, 11 )
		THEN 
		--eja  
		'EJA'
			WHEN etapa_ensino.cd_etapa_ensino IN ( 4, 5, 12, 13 )
		THEN 
		--fundamental     
		'EF'
			WHEN etapa_ensino.cd_etapa_ensino IN
				( 6, 7, 8, 9, 17, 14 ) THEN 
		--médio  
		'EM' 
		WHEN etapa_ensino.cd_etapa_ensino IN ( 1 )
			THEN 
		--infantil
		'EI'
		ELSE 'P'
		END, ' - ',  te.dc_turma_escola, ' - ', 
		te.cd_turma_escola, ue.cd_unidade_educacao, ' - ', LTRIM(RTRIM(tpe.sg_tp_escola)), ' ', ue.nm_unidade_educacao) Secao,
	CASE
			WHEN etapa_ensino.cd_etapa_ensino IN( 1 )
		THEN 512
	ELSE
		MIN(iif(pcc.cd_componente_curricular is not null, pcc.cd_componente_curricular,  cc.cd_componente_curricular))
	END ComponenteCurricularId,
	te.cd_turma_escola TurmaId
INTO #tempComponentesCurricularesPorTurmas
from turma_escola te
inner join escola esc ON te.cd_escola = esc.cd_escola
inner join v_cadastro_unidade_educacao ue on ue.cd_unidade_educacao = esc.cd_escola
inner join tipo_escola tpe on esc.tp_escola = tpe.tp_escola
inner join unidade_administrativa dre on dre.tp_unidade_administrativa = 24
and ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
--Serie Ensino
left join serie_turma_escola ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
left join serie_turma_grade ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola and serie_turma_grade.dt_fim is null
left join escola_grade ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
left join grade ON escola_grade.cd_grade = grade.cd_grade
left join grade_componente_curricular gcc on gcc.cd_grade = grade.cd_grade
left join componente_curricular cc on cc.cd_componente_curricular = gcc.cd_componente_curricular
and cc.dt_cancelamento is null
left join turma_grade_territorio_experiencia tgt on tgt.cd_serie_grade = serie_turma_grade.cd_serie_grade and tgt.cd_componente_curricular = cc.cd_componente_curricular
left join territorio_saber ter on ter.cd_territorio_saber = tgt.cd_territorio_saber 
left join tipo_experiencia_pedagogica tep on tep.cd_experiencia_pedagogica = tgt.cd_experiencia_pedagogica
left join serie_ensino
    ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino
LEFT JOIN etapa_ensino
ON serie_ensino.cd_etapa_ensino = etapa_ensino.cd_etapa_ensino
-- Programa
left join tipo_programa tp on te.cd_tipo_programa = tp.cd_tipo_programa
left join turma_escola_grade_programa tegp on tegp.cd_turma_escola = te.cd_turma_escola
left join escola_grade teg on teg.cd_escola_grade = tegp.cd_escola_grade
left join grade pg on pg.cd_grade = teg.cd_grade
left join grade_componente_curricular pgcc on pgcc.cd_grade = teg.cd_grade
left join componente_curricular pcc on pgcc.cd_componente_curricular = pcc.cd_componente_curricular
and pcc.dt_cancelamento is null
-- Atribuicao turma regular
left join atribuicao_aula atb_ser
	on gcc.cd_grade = atb_ser.cd_grade and
		gcc.cd_componente_curricular = atb_ser.cd_componente_curricular
		and atb_ser.cd_serie_grade = serie_turma_grade.cd_serie_grade and atb_ser.dt_cancelamento is null
		and (atb_ser.dt_disponibilizacao_aulas is null)
		and atb_ser.an_atribuicao = year(getdate())
-- Atribuicao turma programa
left join atribuicao_aula atb_pro
	on pgcc.cd_grade = atb_pro.cd_grade and
		pgcc.cd_componente_curricular = atb_pro.cd_componente_curricular and
		atb_pro.cd_turma_escola_grade_programa = tegp.cd_turma_escola_grade_programa and
		atb_pro.dt_cancelamento is null
		and atb_pro.dt_disponibilizacao_aulas is null
		and atb_pro.an_atribuicao = year(getdate())
-- Servidor
left join v_cargo_base_cotic vcbc on (atb_ser.cd_cargo_base_servidor = vcbc.cd_cargo_base_servidor or
								atb_pro.cd_cargo_base_servidor = vcbc.cd_cargo_base_servidor)
								and vcbc.dt_cancelamento is null and vcbc.dt_fim_nomeacao is null
left join v_servidor_cotic serv on serv.cd_servidor = vcbc.cd_servidor
-- Turno     
inner join duracao_tipo_turno dtt on te.cd_tipo_turno = dtt.cd_tipo_turno and te.cd_duracao = dtt.cd_duracao
where  
	te.st_turma_escola in ('O', 'A', 'C')
	and   te.cd_tipo_turma in (1,2,3,5,6)
	and   esc.tp_escola in (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
	and    NOT esc.cd_escola IN ('200242', '019673')
	and   te.an_letivo = 2021
group by 
	ue.cd_unidade_educacao,
	ue.nm_unidade_educacao,
	tgt.cd_serie_grade, 
CASE
	WHEN etapa_ensino.cd_etapa_ensino IN( 1 ) THEN 'REGÊNCIA DE CLASSE INFANTIL'
ELSE
	iif(tgt.cd_serie_grade is not null, concat(LTRIM(RTRIM(ter.dc_territorio_saber)), ' - ',  LTRIM(RTRIM(tep.dc_experiencia_pedagogica))), 
	coalesce( LTRIM(RTRIM(pcc.dc_componente_curricular)),   LTRIM(RTRIM(cc.dc_componente_curricular))))
END,
te.cd_turma_escola, 
te.dc_turma_escola,
tpe.sg_tp_escola,
etapa_ensino.cd_etapa_ensino;

--- 3.3. Insere os componentes curriculares das turmas de alunos ativos
INSERT INTO #tempRelacaoTurmaComponenteCurricular (cd_turma_escola, cd_componente_curricular)
SELECT
	t1.cd_turma_escola,
	t2.ComponenteCurricularId AS cd_componente_curricular
FROM
	#tempTurmasAlunosMatriculaExportacaoAtivos t1
INNER JOIN
	#tempComponentesCurricularesPorTurmas t2
	ON t1.cd_turma_escola = t2.TurmaId;

-- 4. Final
IF OBJECT_ID('tempdb..#tempAlunosCurso') IS NOT NULL
	DROP TABLE #tempAlunosCurso;
SELECT
	DISTINCT
	t1.cd_aluno_eol,
	t1.nm_email,
	t2.cd_turma_escola,
	t3.cd_componente_curricular
INTO #tempAlunosCurso
FROM
	#tempAlunosExportacaoAtivos t1
INNER JOIN
	#tempAlunosMatriculaExportacaoAtivos t2
	ON t1.cd_aluno_eol = t2.cd_aluno_eol
INNER JOIN
	#tempRelacaoTurmaComponenteCurricular t3
	ON t2.cd_turma_escola = t3.cd_turma_escola
ORDER BY
	nm_email;

-- 5. CSV
SELECT
	temp.cd_aluno_eol AS AlunoEol,
	temp.nm_email AS Email,
	temp.cd_turma_escola AS TurmaId,
	temp.cd_componente_curricular AS ComponenteCurricularId,
	curso.cd_curso_classroom AS CursoId
FROM
	#tempAlunosCurso temp
INNER JOIN
	turma_componente_curricular_classroom curso (NOLOCK)
	ON temp.cd_turma_escola = curso.cd_turma_escola AND temp.cd_componente_curricular = curso.cd_componente_curricular;
--------------------------------------------------------
select count(*) from #tempAlunosCurso

select cd_turma_escola, cd_componente_curricular, COUNT(*) from turma_componente_curricular_classroom group by cd_turma_escola, cd_componente_curricular HAVING count(*) > 1

SELECT
	DISTINCT
	temp.cd_turma_escola, temp.cd_componente_curricular
FROM
	#tempAlunosCurso temp
LEFT JOIN
	turma_componente_curricular_classroom curso
	ON temp.cd_turma_escola = curso.cd_turma_escola AND temp.cd_componente_curricular = curso.cd_componente_curricular
WHERE
	curso.cd_componente_curricular IS NULL;


SELECT * FROM componente_curricular where cd_componente_curricular IN (89)
SELECT * FROM turma_componente_curricular_classroom where cd_turma_escola = 2324872 IN (2337204,2336861,2336944,2336947,2336856,2337200,2336934,2336940,2336854,2337209,2336857,2337212)
SELECT * FROM turma_escola where cd_turma_escola IN (2324872)
SELECT * FROM v_cadastro_unidade_educacao where cd_unidade_educacao IN ('200242', '019673', '093106')


select tgt.* from turma_escola te
left join serie_turma_escola ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
left join serie_turma_grade ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola and serie_turma_grade.dt_fim is null
left join escola_grade ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
left join grade ON escola_grade.cd_grade = grade.cd_grade
left join grade_componente_curricular gcc on gcc.cd_grade = grade.cd_grade
left join componente_curricular cc on cc.cd_componente_curricular = gcc.cd_componente_curricular
and cc.dt_cancelamento is null
left join turma_grade_territorio_experiencia tgt on tgt.cd_serie_grade = serie_turma_grade.cd_serie_grade and tgt.cd_componente_curricular = cc.cd_componente_curricular
left join territorio_saber ter on ter.cd_territorio_saber = tgt.cd_territorio_saber 
left join tipo_experiencia_pedagogica tep on tep.cd_experiencia_pedagogica = tgt.cd_experiencia_pedagogica
where te.cd_turma_escola = 2324872
