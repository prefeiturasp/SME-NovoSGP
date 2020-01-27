USE [Manutencao]

INSERT INTO ETL_TURMAS
SELECT DISTINCT esc.cd_escola AS CodEscola,
                turma_escola.cd_turma_escola AS CodTurma,
                turma_escola.an_letivo AS AnoLetivo,
                CASE
                    WHEN etapa_ensino.cd_etapa_ensino IN (2,3,7,11) THEN 'EJA'
                    WHEN etapa_ensino.cd_etapa_ensino IN (4,5,12,13) THEN 'Fundamental'
                    WHEN etapa_ensino.cd_etapa_ensino IN (6,7,8,17,14) THEN 'Médio'
                END AS Modalidade,
                CASE
                    WHEN etapa_ensino.cd_etapa_ensino IN (2,3,7,11) THEN CASE
                                                                       WHEN DATEPART(MONTH, turma_escola.dt_inicio_turma) > 6 THEN 2
                                                                       ELSE 1
                                                                   END
                    ELSE 0
                END AS Semestre,
                CASE
                    WHEN etapa_ensino.cd_etapa_ensino IN (2,3,7,11) THEN --eja
 3
                    WHEN etapa_ensino.cd_etapa_ensino IN (4,5,12,13) THEN --fundamental
 5
                    WHEN etapa_ensino.cd_etapa_ensino IN (6,7,8,17,14) THEN --médio
 6
                END AS CodModalidade,
                dre.cd_unidade_educacao AS CodDre,
                dre.nm_unidade_educacao AS Dre,
                dre.nm_exibicao_unidade AS DreAbrev,
                ue.nm_unidade_educacao AS UE,
                ue.nm_exibicao_unidade AS UEAbrev,
                turma_escola.dc_turma_escola NomeTurma,
                serie_ensino.sg_resumida_serie AS Ano,
                tue.dc_tipo_unidade_educacao AS TipoUE,
                tue.tp_unidade_educacao AS CodTipoUE,
                tipo_escola.tp_escola AS CodTipoEscola,
                tipo_escola.sg_tp_escola AS TipoEscola,
                duracaoTipoTurno.qt_hora_duracao AS DuracaoTurno,
                tipoTurno.cd_tipo_turno AS TipoTurno,
				cast(cast(turma_escola.dt_atualizacao_tabela as date) as datetime)  as data_atualizacao,
				cast(turma_escola.dt_fim_turma as datetime) 					as dt_fim_eol	
FROM  [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_cadastro_unidade_educacao] ue
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_cadastro_unidade_educacao] dre 
	ON dre.cd_unidade_educacao = ue.cd_unidade_administrativa_referencia
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[turma_escola] 
	ON ue.cd_unidade_educacao = turma_escola.cd_escola
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[escola] esc 
	ON turma_escola.cd_escola = esc.cd_escola
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[serie_turma_escola] 
	ON serie_turma_escola.cd_turma_escola = turma_escola.cd_turma_escola
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[serie_turma_grade] 
	ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[escola_grade]
	ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[grade]
	ON escola_grade.cd_grade = grade.cd_grade
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[serie_ensino]
	ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[unidade_administrativa]
	ON ue.cd_unidade_administrativa_referencia = unidade_administrativa.cd_unidade_administrativa
AND tp_unidade_administrativa = 24
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[etapa_ensino]
	ON serie_ensino.cd_etapa_ensino = etapa_ensino.cd_etapa_ensino
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[tipo_unidade_educacao] tue 
	ON dre.tp_unidade_educacao = tue.tp_unidade_educacao
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[tipo_escola]
	ON esc.tp_escola = tipo_escola.tp_escola
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[atribuicao_aula]
	ON grade.cd_grade = atribuicao_aula.cd_grade
AND an_atribuicao = turma_escola.an_letivo
AND ue.cd_unidade_educacao = turma_escola.cd_escola
AND atribuicao_aula.cd_serie_grade = serie_turma_grade.cd_serie_grade
AND atribuicao_aula.cd_grade = grade.cd_grade
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[tipo_turno] tipoTurno 
	ON turma_escola.cd_tipo_turno = tipoTurno.cd_tipo_turno
INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[duracao_tipo_turno] duracaoTipoTurno 
	ON tipoTurno.cd_tipo_turno = duracaoTipoTurno.cd_tipo_turno
AND turma_escola.cd_duracao = duracaoTipoTurno.cd_duracao
WHERE atribuicao_aula.dt_cancelamento IS NULL
  AND esc.tp_escola IN (1,3,4,16)--EMEF,EMEFM,EMEBS, CEU EMEF
  AND etapa_ensino.cd_etapa_ensino IN (2,3,7,11,4,5,12,13,6,7,8,17,14 )--EJAS,Fundamental e Médio
  AND (atribuicao_aula.dt_atribuicao_aula <= Getdate())
  AND turma_escola.an_letivo BETWEEN 2014 AND 2019
  AND turma_escola.st_turma_escola <> 'E' 
  AND turma_escola.cd_tipo_turma <> '4'
