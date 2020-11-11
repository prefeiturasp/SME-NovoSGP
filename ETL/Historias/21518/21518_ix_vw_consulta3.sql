CREATE VIEW [dbo].[ix_vw_consulta3] WITH SCHEMABINDING
AS
SELECT
				tur.cd_escola       AS codescola,
				tur.cd_turma_escola AS codturma,
				tur.dt_inicio as dataInicioTurma,
				tur.an_letivo AS anoletivo,
				/*Iif(tur.cd_tipo_turma = 1,
					CASE
						WHEN ee.cd_etapa_ensino IN (@etapasInfantil1,@etapasInfantil2) THEN 'Infantil'
						WHEN ee.cd_etapa_ensino IN (@etapasEja1,@etapasEja2) THEN 'EJA' 
                        WHEN ee.cd_etapa_ensino IN (@etapasFundamental1,@etapasFundamental2,@etapasFundamental3,@etapasFundamental4) THEN 'Fundamental' 
                      WHEN ee.cd_etapa_ensino IN (@etapasMedio1,@etapasMedio2,@etapasMedio3,@etapasMedio4,@etapasMedio5) THEN 'Médio' 
				END, 'Fundamental') AS modalidade,
				Iif(tur.cd_tipo_turma = 1,
					CASE
					WHEN ee.cd_etapa_ensino IN (@etapasEja1,@etapasEja2) THEN
						Iif(Datepart(month, tur.dt_inicio_turma) > 6, 2, 1)
						ELSE 0
				END, 0) AS semestre,
				Iif(tur.cd_tipo_turma in (1,2,3,5),
				CASE
							WHEN ee.cd_etapa_ensino IN (@etapasInfantil1,@etapasInfantil2) THEN--infantil
										@infantil							
							WHEN ee.cd_etapa_ensino IN (@etapasEja1,@etapasEja2) THEN--eja
										@eja
							WHEN ee.cd_etapa_ensino IN (@etapasFundamental1,@etapasFundamental2,@etapasFundamental3,@etapasFundamental4) THEN--fundamental
										@fundamental
							WHEN ee.cd_etapa_ensino IN (@etapasMedio1,@etapasMedio2,@etapasMedio3,@etapasMedio4,@etapasMedio5) THEN--médio
										@medio
							WHEN esc.tp_escola IN (@tiposEscolaInfantil1,@tiposEscolaInfantil2,@tiposEscolaInfantil3,@tiposEscolaInfantil4,@tiposEscolaInfantil5) THEN--infantil
										@infantil
							WHEN esc.tp_escola NOT IN (@tiposEscolaInfantil1,@tiposEscolaInfantil2,@tiposEscolaInfantil3,@tiposEscolaInfantil4,@tiposEscolaInfantil5) and tur.cd_tipo_turma <> 1 THEN--infantil
										@fundamental
				END, 5)                                          AS codmodalidade,*/
				dre.cd_unidade_educacao AS coddre, 
				dre.nm_unidade_educacao AS dre, 
				dre.nm_exibicao_unidade AS dreabrev, 
				vue.nm_unidade_educacao AS ue, 
				vue.nm_exibicao_unidade AS ueabrev, 
				tur.dc_turma_escola AS nometurma, 
				Iif(tur.cd_tipo_turma = 1, se.sg_resumida_serie, '0') AS ano,
				tue.dc_tipo_unidade_educacao AS tipoue, 
				vue.tp_unidade_educacao AS codtipoue, 
				esc.tp_escola AS codtipoescola, 
				tesc.sg_tp_escola AS tipoescola, 
				dtt.qt_hora_duracao AS duracaoturno, 
				tur.cd_tipo_turno AS tipoturno, 
				tur.dt_atualizacao_tabela AS dataatualizacao,
				Iif((se.cd_etapa_ensino = 13) AND (se.cd_modalidade_ensino = 2) , 1, 0) AS ensinoEspecial,
				se.dc_serie_ensino AS serieEnsino
				FROM       dbo.turma_escola tur
					INNER JOIN dbo.tipo_turno t_trn
						ON t_trn.cd_tipo_turno = tur.cd_tipo_turno
					INNER JOIN dbo.duracao_tipo_turno dtt
						ON         t_trn.cd_tipo_turno = dtt.cd_tipo_turno
					AND tur.cd_duracao = dtt.cd_duracao
					INNER JOIN dbo.tipo_periodicidade tper
						ON tur.cd_tipo_periodicidade = tper.cd_tipo_periodicidade
							   -- Unidades Educacionais
					INNER JOIN dbo.v_cadastro_unidade_educacao vue
						ON vue.cd_unidade_educacao = tur.cd_escola
					INNER JOIN dbo.escola esc
						ON esc.cd_escola = vue.cd_unidade_educacao
					INNER JOIN
					(
								SELECT v_ua.cd_unidade_educacao,
											v_ua.nm_unidade_educacao,
											v_ua.nm_exibicao_unidade
								FROM       dbo.unidade_administrativa ua
									INNER JOIN dbo.v_cadastro_unidade_educacao v_ua
										ON v_ua.cd_unidade_educacao = ua.cd_unidade_administrativa
								--WHERE tp_unidade_administrativa = 24
								) dre
				   ON         dre.cd_unidade_educacao = vue.cd_unidade_administrativa_referencia
			   INNER JOIN dbo.tipo_escola tesc
			   ON         esc.tp_escola = tesc.tp_escola
			   AND        esc.tp_dependencia_administrativa = tesc.tp_dependencia_administrativa
			   INNER JOIN dbo.tipo_unidade_educacao tue
			   ON         tue.tp_unidade_educacao = vue.tp_unidade_educacao 
					-- Serie Ensino(turma tipo = 1)
					LEFT JOIN  dbo.serie_turma_escola ste
						ON tur.cd_turma_escola = ste.cd_turma_escola
							AND        ste.dt_fim IS NULL
					LEFT JOIN dbo.serie_ensino se
						ON         se.cd_serie_ensino = ste.cd_serie_ensino
					LEFT JOIN  dbo.etapa_ensino ee
						ON se.cd_etapa_ensino = ee.cd_etapa_ensino
					LEFT JOIN  dbo.serie_turma_grade stg
						ON tur.cd_turma_escola = stg.cd_turma_escola
							AND tur.cd_escola = stg.cd_escola
							AND ste.cd_serie_ensino = stg.cd_serie_ensino
							AND stg.dt_fim IS NULL
					LEFT JOIN dbo.escola_grade egse
					ON         stg.cd_escola_grade = egse.cd_escola_grade
						AND egse.cd_escola = tur.cd_escola
							   -- Programa (turma tipo = 3, 2 , 5)
					LEFT JOIN  dbo.turma_escola_grade_programa tegpro
						ON tegpro.cd_turma_escola = tur.cd_turma_escola
							AND        tegpro.dt_fim IS NULL
					LEFT JOIN dbo.escola_grade egpro
						ON         egpro.cd_escola_grade = tegpro.cd_escola_grade
							AND egpro.cd_escola = tur.cd_escola
					LEFT JOIN  dbo.grade gpro
						ON gpro.cd_grade = egpro.cd_grade
					LEFT JOIN  dbo.grade_componente_curricular gccpro
						ON gpro.cd_grade = gccpro.cd_grade
					LEFT JOIN  dbo.componente_curricular ccpro
						ON gccpro.cd_componente_curricular = ccpro.cd_componente_curricular
					/*WHERE tur.an_letivo = Year(Getdate())
						AND vue.tp_situacao_unidade = 1
						AND tur.st_turma_escola IN ('O','A')
						AND esc.tp_escola IN (@tiposEscola1,@tiposEscola2,@tiposEscola3,@tiposEscola4,@tiposEscola5,@tiposEscola6,@tiposEscola7,@tiposEscola8,@tiposEscola9)
						AND((
									tur.cd_tipo_turma = 1
									AND ee.cd_etapa_ensino IN (@etapas1,@etapas2,@etapas3,@etapas4,@etapas5,@etapas6,@etapas7,@etapas8,@etapas9,@etapas10,@etapas11,@etapas12,@etapas13)
						AND ((ee.cd_etapa_ensino IN (@etapasInfantil1,@etapasInfantil2) and se.cd_serie_ensino in (@seriesEnsinoInfantil1,@seriesEnsinoInfantil2,@seriesEnsinoInfantil3,@seriesEnsinoInfantil4,@seriesEnsinoInfantil5,@seriesEnsinoInfantil6,@seriesEnsinoInfantil7,@seriesEnsinoInfantil8,@seriesEnsinoInfantil9,@seriesEnsinoInfantil10)) or (ee.cd_etapa_ensino not IN (@etapasInfantil1,@etapasInfantil2)))) 
						   OR(
												 tur.cd_tipo_turma = 3
									  AND tur.dt_fim_turma >= Getdate()
									  AND        Year(tur.dt_inicio_turma) = Year(Getdate())
									  AND gccpro.cd_componente_curricular NOT IN (1033,1051,1052,1053,1054,1322)
									  AND EXISTS
												 (
														SELECT 1 
														FROM matricula_turma_escola mte
														WHERE  mte.cd_turma_escola = tegpro.cd_turma_escola) ) 
						   OR((
															tur.cd_tipo_turma IN (2,
																				  5)
												 OR gccpro.cd_componente_curricular IN (1033,1051,1052,1053,1054,1322)) 
									  AND tur.dt_fim_turma >= Getdate()
									  AND        Year(tur.dt_inicio_turma) = Year(Getdate()))
					)
					and dre.cd_unidade_educacao = 108100*/