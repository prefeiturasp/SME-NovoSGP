declare @tabela_lista_turmas_componentes table (cd_turma_escola int,
												cd_componente_curricular int,
												an_letivo smallint)

declare @tabela_turmas_duplicadas table (cd_turma_escola int,									     
										 an_letivo smallint);

insert into @tabela_lista_turmas_componentes
select distinct te.cd_turma_escola,
				cc.cd_componente_curricular,
				te.an_letivo			 			
                    from turma_escola te
                             inner join escola esc ON te.cd_escola = esc.cd_escola
                             inner join v_cadastro_unidade_educacao ue on ue.cd_unidade_educacao = esc.cd_escola
                             inner join unidade_administrativa dre on dre.tp_unidade_administrativa = 24
                        and ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa

                        --Serie Ensino
                             left join serie_turma_escola ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
                             left join serie_turma_grade ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola
                             left join escola_grade ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
                             left join grade ON escola_grade.cd_grade = grade.cd_grade
                             left join grade_componente_curricular gcc on gcc.cd_grade = grade.cd_grade
                             left join componente_curricular cc on cc.cd_componente_curricular = gcc.cd_componente_curricular
                        and cc.dt_cancelamento is null
                             left join serie_ensino
                                       ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino

                        -- Programa
                             left join tipo_programa tp on te.cd_tipo_programa = tp.cd_tipo_programa
                             left join turma_escola_grade_programa tegp on tegp.cd_turma_escola = te.cd_turma_escola
                             left join escola_grade teg on teg.cd_escola_grade = tegp.cd_escola_grade
                             left join grade pg on pg.cd_grade = teg.cd_grade
                             left join grade_componente_curricular pgcc on pgcc.cd_grade = teg.cd_grade
                             left join componente_curricular pcc on pgcc.cd_componente_curricular = pcc.cd_componente_curricular
                        and pcc.dt_cancelamento is null
                        -- Turno     
                             inner join duracao_tipo_turno dtt on te.cd_tipo_turno = dtt.cd_tipo_turno and te.cd_duracao = dtt.cd_duracao							 																								 
                    where te.st_turma_escola in ('O', 'A', 'C') and
						  te.an_letivo between 2014 and 2019 and 
						  cc.cd_componente_curricular in (9, 1106)

insert into @tabela_turmas_duplicadas
select distinct cd_turma_escola,				
				an_letivo				
	from @tabela_lista_turmas_componentes
group by cd_turma_escola,		 
	     an_letivo		 
having count(distinct cd_componente_curricular) > 1
order by 2, 1

--select distinct td.cd_turma_escola,
--				td.an_letivo
--	from @tabela_turmas_duplicadas td		
--		inner join serie_turma_grade stg
--			on td.cd_turma_escola = stg.cd_turma_escola and
--			   stg.dt_fim is null 
--		inner join escola_grade eg
--			on stg.cd_escola_grade = eg.cd_escola_grade
--		inner join grade_componente_curricular gcc
--			on eg.cd_grade = gcc.cd_grade
--where gcc.cd_componente_curricular = 1106

--select stg.cd_turma_escola, 
--	   YEAR(stg.dt_fim), 
--	   gcc.cd_componente_curricular,
--	   ttd.an_letivo
--	from serie_turma_grade stg 
--		inner join escola_grade eg
--			on stg.cd_escola_grade = eg.cd_escola_grade
--		inner join grade_componente_curricular gcc
--			on eg.cd_grade = gcc.cd_grade
--		inner join @tabela_turmas_duplicadas ttd
--			on stg.cd_turma_escola = ttd.cd_turma_escola
--where gcc.cd_componente_curricular in (9, 1106) and
--	  YEAR(stg.dt_fim) = ttd.an_letivo  
--order by stg.cd_turma_escola


select distinct te.cd_turma_escola,				
				te.an_letivo			 			
                    from turma_escola te
                             inner join escola esc ON te.cd_escola = esc.cd_escola
                             inner join v_cadastro_unidade_educacao ue on ue.cd_unidade_educacao = esc.cd_escola
                             inner join unidade_administrativa dre on dre.tp_unidade_administrativa = 24
                        and ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa

                        --Serie Ensino
                             left join serie_turma_escola ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
                             left join serie_turma_grade ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola
                             left join escola_grade ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
                             left join grade ON escola_grade.cd_grade = grade.cd_grade
                             left join grade_componente_curricular gcc on gcc.cd_grade = grade.cd_grade
                             left join componente_curricular cc on cc.cd_componente_curricular = gcc.cd_componente_curricular
                        and cc.dt_cancelamento is null
                             left join serie_ensino
                                       ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino

                        -- Programa
                             left join tipo_programa tp on te.cd_tipo_programa = tp.cd_tipo_programa
                             left join turma_escola_grade_programa tegp on tegp.cd_turma_escola = te.cd_turma_escola
                             left join escola_grade teg on teg.cd_escola_grade = tegp.cd_escola_grade
                             left join grade pg on pg.cd_grade = teg.cd_grade
                             left join grade_componente_curricular pgcc on pgcc.cd_grade = teg.cd_grade
                             left join componente_curricular pcc on pgcc.cd_componente_curricular = pcc.cd_componente_curricular
                        and pcc.dt_cancelamento is null
                        -- Turno     
                             inner join duracao_tipo_turno dtt on te.cd_tipo_turno = dtt.cd_tipo_turno and te.cd_duracao = dtt.cd_duracao							 																								 
                    where te.st_turma_escola in ('O', 'A', 'C') and
						  te.an_letivo between 2014 and 2019 and 
						  cc.cd_componente_curricular = 9 and
						  te.cd_turma_escola not in (select cd_turma_escola
													 from @tabela_turmas_duplicadas)					  
