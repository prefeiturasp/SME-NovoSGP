DECLARE @CodigoDre INT = 109000; 

IF OBJECT_ID('tempdb..#ResponsaveisUe') IS NOT NULL DROP TABLE #ResponsaveisUe

CREATE TABLE #ResponsaveisUe
(
	CodigoUe varchar(10) NOT NULL,
    NomeServidor varchar(70) NULL,
    CodigoRf varchar(7) NULL
)

INSERT INTO #ResponsaveisUe
 select distinct ue.cd_unidade_educacao, servsub.NomeServidor, servsub.CodigoRf from v_cadastro_unidade_educacao dre
	                    INNER JOIN v_cadastro_unidade_educacao ue ON dre.cd_unidade_educacao =  ue.cd_unidade_administrativa_referencia 
	                    INNER JOIN turma_escola on ue.cd_unidade_educacao = turma_escola.cd_escola
	                    INNER JOIN escola esc  ON turma_escola.cd_escola = esc.cd_escola 	            
	                    INNER JOIN serie_turma_escola ON serie_turma_escola.cd_turma_escola =   turma_escola.cd_turma_escola 
                        INNER JOIN serie_turma_grade  ON serie_turma_grade.cd_turma_escola =    serie_turma_escola.cd_turma_escola 
                        INNER JOIN escola_grade  ON serie_turma_grade.cd_escola_grade =  escola_grade.cd_escola_grade 
                        INNER JOIN grade ON escola_grade.cd_grade = grade.cd_grade 
                        INNER JOIN serie_ensino  ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino 
                        INNER JOIN etapa_ensino  ON serie_ensino.cd_etapa_ensino = etapa_ensino.cd_etapa_ensino 
						cross apply [dbo].[proc_obter_nivel](null, ue.cd_unidade_educacao) servsub
	                    where esc.tp_escola IN (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
                        AND etapa_ensino.cd_etapa_ensino IN ( 2, 3, 7, 11, 4, 5, 12, 13, 6, 7, 8, 9, 17, 14 ) 
						and dre.cd_unidade_educacao = @codigoDre;

select * from (
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
										   te.cd_turma_escola, ' - ', ue.cd_unidade_educacao, ' - ', LTRIM(RTRIM(tpe.sg_tp_escola)), ' ', ue.nm_unidade_educacao) Secao,
										CASE
											 WHEN etapa_ensino.cd_etapa_ensino IN( 1 )
										   THEN 512
										ELSE
											MIN(iif(pcc.cd_componente_curricular is not null, pcc.cd_componente_curricular,  cc.cd_componente_curricular))
										END ComponenteCurricularId,
										te.cd_turma_escola TurmaId,
										coalesce(serv.cd_registro_funcional, resp.CodigoRf, '') as ProfessorRF,
										coalesce(serv.nm_pessoa, resp.NomeServidor, '') as NomeProfessor,
										ue.cd_unidade_educacao,
										[dbo].[proc_gerar_email_funcionario](coalesce(serv.nm_pessoa, resp.NomeServidor, ''), coalesce(serv.cd_registro_funcional, resp.CodigoRf, '')) EmailProfessor
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
							 left join #ResponsaveisUe resp on resp.CodigoUe = ue.cd_unidade_educacao
                        -- Turno     
                             inner join duracao_tipo_turno dtt on te.cd_tipo_turno = dtt.cd_tipo_turno and te.cd_duracao = dtt.cd_duracao
							
                    where  te.st_turma_escola in ('O', 'A', 'C')
					 and   te.cd_tipo_turma in (1,2,3,5,6)
					 and   esc.tp_escola in (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
					 and   te.an_letivo = 2021
					 and   dre.cd_unidade_administrativa = @CodigoDre
					group by 
					ue.cd_unidade_educacao,
					ue.nm_unidade_educacao,
					tgt.cd_serie_grade, 
					CASE
							WHEN etapa_ensino.cd_etapa_ensino IN( 1 )
						THEN 'REGÊNCIA DE CLASSE INFANTIL'
					ELSE
						iif(tgt.cd_serie_grade is not null, concat(LTRIM(RTRIM(ter.dc_territorio_saber)), ' - ',  LTRIM(RTRIM(tep.dc_experiencia_pedagogica))), 
						coalesce(LTRIM(RTRIM(pcc.dc_componente_curricular)),   LTRIM(RTRIM(cc.dc_componente_curricular))))
					END,
					te.cd_turma_escola, 
					te.dc_turma_escola,
					tpe.sg_tp_escola,
					etapa_ensino.cd_etapa_ensino,
					coalesce(serv.cd_registro_funcional, resp.CodigoRf, ''),
					coalesce(serv.nm_pessoa, resp.NomeServidor, ''),
					ue.cd_unidade_educacao
					) as teste
					where teste.turmaid = 2324872
					and teste.componentecurricularid in (1215,1216,1217)