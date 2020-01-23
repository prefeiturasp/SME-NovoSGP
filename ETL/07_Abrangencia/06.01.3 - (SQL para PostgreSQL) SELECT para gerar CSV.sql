		-- #######################################################################
		-- T A B E L A 
		-- A B R A N G E N C I A
		-- Ano: 2016
		-- #######################################################################
-- 1º SELECT
SELECT	DISTINCT
		serv.cd_registro_funcional							AS	usuario_rf, -- char 7
		educ.cd_unidade_administrativa_referencia			AS  dre_id_eol, -- char 6
		aula.cd_unidade_educacao							AS  ue_id_eol, -- char 6 
		serie.cd_turma_escola								AS  turma_id_eol, -- int 4
		(CASE WHEN aula.dt_cancelamento IS NULL
			THEN aula.dt_disponibilizacao_aulas 
				ELSE aula.dt_cancelamento END) 				AS  dt_fim_vinculo
FROM atribuicao_aula aula
INNER JOIN v_cargo_base_cotic cargo
ON aula.cd_cargo_base_servidor = cargo.cd_cargo_base_servidor
INNER JOIN v_servidor_cotic serv
ON cargo.cd_servidor = serv.cd_servidor
INNER JOIN v_cadastro_unidade_educacao educ
ON aula.cd_unidade_educacao = educ.cd_unidade_educacao
INNER JOIN serie_turma_grade serie
ON serie.cd_serie_grade = aula.cd_serie_grade
INNER JOIN escola escola
ON aula.cd_unidade_educacao = escola.cd_escola
WHERE    aula.an_atribuicao  = '2016'
	AND  serv.cd_registro_funcional IS NOT NULL
	AND  dt_disponibilizacao_aulas <> dt_atribuicao_aula
	AND  escola.tp_escola in ('3','4','16','1')
	AND  year(CASE WHEN aula.dt_cancelamento IS NULL
			THEN aula.dt_disponibilizacao_aulas 
				ELSE aula.dt_cancelamento END)  = '2016'
UNION ALL
--- 2º SELECT 
SELECT	DISTINCT
		serv.cd_registro_funcional							AS	usuario_rf, -- char 7
		educ.cd_unidade_administrativa_referencia			AS  dre_id_eol, -- char 6
		aula.cd_unidade_educacao							AS  ue_id_eol,  -- char 6 
		programa.cd_turma_escola							AS  turma_id_eol, -- int 4
		(CASE WHEN aula.dt_cancelamento IS NULL
			THEN aula.dt_disponibilizacao_aulas 
				ELSE aula.dt_cancelamento END) 				AS  dt_fim_vinculo
FROM atribuicao_aula aula
INNER JOIN v_cargo_base_cotic cargo
ON aula.cd_cargo_base_servidor = cargo.cd_cargo_base_servidor
INNER JOIN v_servidor_cotic serv
ON cargo.cd_servidor = serv.cd_servidor
INNER JOIN v_cadastro_unidade_educacao educ
ON aula.cd_unidade_educacao = educ.cd_unidade_educacao
INNER JOIN turma_escola_grade_programa programa
ON programa.cd_turma_escola_grade_programa = aula.cd_turma_escola_grade_programa
INNER JOIN escola escola
ON aula.cd_unidade_educacao = escola.cd_escola
INNER JOIN turma_escola turma
ON programa.cd_turma_escola = turma.cd_turma_escola
WHERE    aula.an_atribuicao  = '2016'
	AND  serv.cd_registro_funcional IS NOT NULL
	AND  dt_disponibilizacao_aulas <> dt_atribuicao_aula
	AND  escola.tp_escola in ('3','4','16','1')
	AND  year(CASE WHEN aula.dt_cancelamento IS NULL
			THEN aula.dt_disponibilizacao_aulas 
				ELSE aula.dt_cancelamento END) = '2016'
	AND turma.cd_tipo_turma <> '4'