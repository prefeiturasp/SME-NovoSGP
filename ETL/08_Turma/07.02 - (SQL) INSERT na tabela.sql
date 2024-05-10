USE [Manutencao]

INSERT INTO ETL_TURMAS
  SELECT DISTINCT 
	   a.cd_escola										AS CodEscola,
	   a.cd_turma_escola								AS CodTurma,
       a.an_letivo										AS AnoLetivo,
       'Fundamental'									AS Modalidade,
       0												AS Semestre,
       5												AS CodModalidade,
       dre.cd_unidade_educacao							AS CodDre,
       dre.nm_unidade_educacao							AS Dre,
       dre.nm_exibicao_unidade							AS DreAbrev,
       escola.nm_unidade_educacao						AS UE,
       escola.nm_exibicao_unidade						AS UEAbrev,
       a.dc_turma_escola								AS NomeTurma,
	   '0'												AS Ano,
       tue.dc_tipo_unidade_educacao						AS TipoUE,
       tue.tp_unidade_educacao							AS CodTipoUE,
       tipo_escola.tp_escola							AS CodTipoEscola,
       tipo_escola.sg_tp_escola							AS TipoEscola,
       duracaoTipoTurno.qt_hora_duracao					AS DuracaoTurno,
       tipoTurno.cd_tipo_turno							AS TipoTurno,
	   cast(cast(a.dt_atualizacao_tabela as date) as datetime)  AS data_atualizacao,
	   cast(a.dt_fim_turma as datetime) 				AS dt_fim_eol	
FROM  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[turma_escola] a
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[turma_escola_grade_programa] b 
	ON a.cd_turma_escola = b.cd_turma_escola
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[escola] esc 
	ON a.cd_escola = esc.cd_escola
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_cadastro_unidade_educacao] escola 
ON escola.cd_unidade_educacao = esc.cd_escola
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_cadastro_unidade_educacao] dre 
	ON dre.cd_unidade_educacao = escola.cd_unidade_administrativa_referencia
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[escola_grade] 
	ON escola_grade.cd_escola = esc.cd_escola
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[turma_escola_grade_programa] 
	ON turma_escola_grade_programa.cd_turma_escola = a.cd_turma_escola
AND turma_escola_grade_programa.cd_escola_grade = escola_grade.cd_escola_grade
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[grade]
	ON grade.cd_grade = escola_grade.cd_grade
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[tipo_unidade_educacao] tue 
	ON dre.tp_unidade_educacao = tue.tp_unidade_educacao
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[tipo_escola]
	ON esc.tp_escola = tipo_escola.tp_escola
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[tipo_turno] tipoTurno 
	ON a.cd_tipo_turno = tipoTurno.cd_tipo_turno
INNER JOIN  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[duracao_tipo_turno] duracaoTipoTurno 
	ON tipoTurno.cd_tipo_turno = duracaoTipoTurno.cd_tipo_turno
	AND a.cd_duracao = duracaoTipoTurno.cd_duracao
WHERE a.cd_escola COLLATE SQL_Latin1_General_CP1_CI_AI IN   
	(SELECT DISTINCT CodEscola  FROM [dbo].[ETL_TURMAS])
	AND esc.tp_escola IN (1,3,4,16)--EMEF,EMEFM,EMEBS, CEU EMEF
	AND a.st_turma_escola <>'E'--IN ('O','A')
	AND a.cd_tipo_turma IN (2,5,3)
	AND a.an_letivo BETWEEN 2014 AND 2019
	AND a.cd_tipo_turma <> '4'