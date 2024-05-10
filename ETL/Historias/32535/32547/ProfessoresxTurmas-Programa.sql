IF OBJECT_ID('tempdb..#tempRelacaoServidorTurmaProgramaComponenteCurricular') IS NOT NULL
	DROP TABLE #tempRelacaoServidorTurmaProgramaComponenteCurricular;

SELECT 
	DISTINCT 
	serv.cd_registro_funcional,
	CONCAT('P - ',  te.dc_turma_escola, ' - ', 
		te.cd_turma_escola, ue.cd_unidade_educacao, ' - ', LTRIM(RTRIM(tpe.sg_tp_escola)), ' ', ue.nm_unidade_educacao) Secao,
	pcc.cd_componente_curricular ComponenteCurricularId,
	te.cd_turma_escola TurmaId
INTO #tempRelacaoServidorTurmaProgramaComponenteCurricular
FROM
	turma_escola te (NOLOCK)
INNER JOIN
	escola esc (NOLOCK) 
	ON te.cd_escola = esc.cd_escola
INNER JOIN
	v_cadastro_unidade_educacao ue (NOLOCK) 
	ON ue.cd_unidade_educacao = esc.cd_escola
INNER JOIN
	tipo_escola tpe (NOLOCK) 
	ON esc.tp_escola = tpe.tp_escola
INNER JOIN
	unidade_administrativa dre (NOLOCK) 
	ON dre.tp_unidade_administrativa = 24 AND ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
-- Programa
LEFT JOIN 
	tipo_programa tp (NOLOCK) 
	ON te.cd_tipo_programa = tp.cd_tipo_programa
INNER JOIN 
	turma_escola_grade_programa tegp (NOLOCK) 
	ON tegp.cd_turma_escola = te.cd_turma_escola
INNER JOIN 
	escola_grade teg (NOLOCK) 
	ON teg.cd_escola_grade = tegp.cd_escola_grade
INNER JOIN 
	grade pg (NOLOCK) 
	ON pg.cd_grade = teg.cd_grade
INNER JOIN 
	grade_componente_curricular pgcc (NOLOCK) 
	ON pgcc.cd_grade = teg.cd_grade
INNER JOIN 
	componente_curricular pcc (NOLOCK) 
	ON pgcc.cd_componente_curricular = pcc.cd_componente_curricular AND pcc.dt_cancelamento IS NULL
-- Atribuicao turma programa
LEFT JOIN 
	atribuicao_aula atb_pro (NOLOCK) 
	ON pgcc.cd_grade = atb_pro.cd_grade and
		pgcc.cd_componente_curricular = atb_pro.cd_componente_curricular and
		atb_pro.cd_turma_escola_grade_programa = tegp.cd_turma_escola_grade_programa and
		atb_pro.dt_cancelamento IS NULL
		AND atb_pro.dt_disponibilizacao_aulas IS NULL
		AND atb_pro.an_atribuicao = year(getdate())
-- Servidor
LEFT JOIN 
	v_cargo_base_cotic vcbc (NOLOCK) 
	ON atb_pro.cd_cargo_base_servidor = vcbc.cd_cargo_base_servidor AND vcbc.dt_cancelamento IS NULL AND vcbc.dt_fim_nomeacao IS NULL
LEFT JOIN 
	v_servidor_cotic serv (NOLOCK) 
	ON serv.cd_servidor = vcbc.cd_servidor
-- Turno     
INNER JOIN
	duracao_tipo_turno dtt (NOLOCK) 
	ON te.cd_tipo_turno = dtt.cd_tipo_turno AND te.cd_duracao = dtt.cd_duracao
WHERE  
	te.st_turma_escola in ('O', 'A', 'C')
	AND   te.cd_tipo_turma in (1,2,3,5,6)
	AND   esc.tp_escola in (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
	AND   te.an_letivo = 2021
GROUP BY
	serv.cd_registro_funcional,
	ue.cd_unidade_educacao,
	ue.nm_unidade_educacao,
	te.cd_turma_escola, 
	te.dc_turma_escola,
	pcc.cd_componente_curricular,
	tpe.sg_tp_escola;

INSERT INTO servidor_turma_componente_curricular_classroom
SELECT	
	temp.cd_registro_funcional, temp.TurmaId, temp.ComponenteCurricularId, serv.nm_email, NULL, 0, 1, 0
FROM
	#tempRelacaoServidorTurmaProgramaComponenteCurricular temp
INNER JOIN
	servidor_classroom serv (NOLOCK)
	ON temp.cd_registro_funcional =  serv.cd_servidor_cotic
WHERE
	temp.cd_registro_funcional IS NOT NULL;

SELECT
	serv.cd_registro_funcional,
	serv.cd_turma_escola,
	serv.cd_componente_curricular,
	serv.nm_email AS Email,
	curso.email_criador,
	curso.cd_curso_classroom AS CursoGoogleId
FROM
	servidor_turma_componente_curricular_classroom serv (NOLOCK)
INNER JOIN
	turma_componente_curricular_classroom curso
	ON serv.cd_turma_escola = curso.cd_turma_escola AND serv.cd_componente_curricular = curso.cd_componente_curricular AND serv.nm_email <> curso.email_criador
WHERE
	serv.ProfessorPrograma = 1;