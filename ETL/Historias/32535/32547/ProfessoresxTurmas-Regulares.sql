IF OBJECT_ID('tempdb..#tempRelacaoServidorTurmaComponenteCurricular') IS NOT NULL
	DROP TABLE #tempRelacaoServidorTurmaComponenteCurricular;

SELECT
	DISTINCT
	serv.cd_registro_funcional,
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
		cc.cd_componente_curricular
	END ComponenteCurricularId,
	te.cd_turma_escola TurmaId
INTO #tempRelacaoServidorTurmaComponenteCurricular
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
--Serie Ensino
INNER JOIN 
	serie_turma_escola (NOLOCK) 
	ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
INNER JOIN 
	serie_turma_grade (NOLOCK) 
	ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola AND serie_turma_grade.dt_fim IS NULL
INNER JOIN 
	escola_grade (NOLOCK) 
	ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
INNER JOIN 
	grade (NOLOCK) 
	ON escola_grade.cd_grade = grade.cd_grade
INNER JOIN 
	grade_componente_curricular gcc (NOLOCK) 
	ON gcc.cd_grade = grade.cd_grade
INNER JOIN 
	componente_curricular cc (NOLOCK) 
	ON cc.cd_componente_curricular = gcc.cd_componente_curricular AND cc.dt_cancelamento IS NULL
LEFT JOIN 
	turma_grade_territorio_experiencia tgt (NOLOCK) 
	ON tgt.cd_serie_grade = serie_turma_grade.cd_serie_grade AND tgt.cd_componente_curricular = cc.cd_componente_curricular
LEFT JOIN 
	territorio_saber ter (NOLOCK) 
	ON ter.cd_territorio_saber = tgt.cd_territorio_saber 
LEFT JOIN 
	tipo_experiencia_pedagogica tep (NOLOCK) 
	ON tep.cd_experiencia_pedagogica = tgt.cd_experiencia_pedagogica
LEFT JOIN 
	serie_ensino (NOLOCK) 
	ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino
LEFT JOIN 
	etapa_ensino (NOLOCK) 
	ON serie_ensino.cd_etapa_ensino = etapa_ensino.cd_etapa_ensino
-- Atribuição de aula
LEFT JOIN 
	atribuicao_aula atb_ser (NOLOCK) 
	on gcc.cd_grade = atb_ser.cd_grade
		AND gcc.cd_componente_curricular = atb_ser.cd_componente_curricular
		AND atb_ser.cd_serie_grade = serie_turma_grade.cd_serie_grade AND atb_ser.dt_cancelamento is null
		AND (atb_ser.dt_disponibilizacao_aulas is null)
		AND atb_ser.an_atribuicao = 2021
-- Servidor
LEFT JOIN 
	v_cargo_base_cotic vcbc (NOLOCK) 
	on atb_ser.cd_cargo_base_servidor = vcbc.cd_cargo_base_servidor
	AND vcbc.dt_cancelamento is null AND vcbc.dt_fim_nomeacao is null
LEFT JOIN 
	v_servidor_cotic serv (NOLOCK) 
	on serv.cd_servidor = vcbc.cd_servidor
WHERE  
	te.st_turma_escola in ('O', 'A', 'C')
	AND   te.cd_tipo_turma in (1,2,3,5,6)
	AND   esc.tp_escola in (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
	AND   te.an_letivo = 2021
GROUP BY
	serv.cd_registro_funcional,
	ue.cd_unidade_educacao,
	ue.nm_unidade_educacao,
	tgt.cd_serie_grade, 
	te.cd_turma_escola, 
	te.dc_turma_escola,
	tpe.sg_tp_escola,
	cc.cd_componente_curricular,
	etapa_ensino.cd_etapa_ensino;
GO

INSERT INTO servidor_turma_componente_curricular_classroom
SELECT	
	temp.cd_registro_funcional, temp.TurmaId, temp.ComponenteCurricularId, serv.nm_email, NULL
FROM
	#tempRelacaoServidorTurmaComponenteCurricular temp
INNER JOIN
	servidor_classroom serv (NOLOCK)
	ON temp.cd_registro_funcional =  serv.cd_servidor_cotic AND serv.in_ativo = 1;

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
	ON serv.cd_turma_escola = curso.cd_turma_escola AND serv.cd_componente_curricular = curso.cd_componente_curricular AND serv.nm_email <> curso.email_criador;

--------------------------------------------------------------------------
SELECT * FROM turma_escola where cd_turma_escola = 2237198 --componente 9
select * from serie_turma_grade where cd_turma_escola = 2237198


SELECT
	serv.*
FROM
	(SELECT DISTINCT cd_registro_funcional FROM servidor_turma_componente_curricular_classroom) servComp
RIGHT JOIN
	(SELECT DISTINCT cd_servidor_cotic FROM servidor_classroom WHERE in_ativo = 1) serv
	ON servComp.cd_registro_funcional = serv.cd_servidor_cotic
WHERE
	servComp.cd_registro_funcional IS NULL;


SELECT * FROM v_servidor_cotic serv WHERE cd_registro_funcional = 8577765
SELECT * FROM v_cargo_base_cotic where cd_servidor = 280970
select * from cargo where cd_cargo = 3344
select * from atribuicao_aula where an_atribuicao = 2021 AND cd_componente_curricular = 9 AND cd_serie_grade = 615377
select * from servidor_turma_componente_curricular_classroom where cd_registro_funcional = 1389173
select * from servidor_classroom where cd_servidor_cotic = 1389173
select * from ususario_classroom where Email_Address_Required = 'alaydesilva.1389173@edu.sme.prefeitura.sp.gov.br'

SELECT * FROM #tempRelacaoServidorTurmaComponenteCurricular where cd_registro_funcional = 1389173
