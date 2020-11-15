CREATE VIEW [dbo].[ix_vw_consulta4] WITH SCHEMABINDING
AS
SELECT 
                    cd_cargo AS CdCargo
		            ,cd_tipo_funcao AS CdTipoFuncao
                    ,Sobreposto
                    ,ComponenteCurricular
		            ,CargoBase
            FROM(
	            SELECT  
		            cargo.cd_cargo
		            ,null AS cd_tipo_funcao
                    ,0 as Sobreposto
                    ,NULL as ComponenteCurricular
		            ,cargoServidor.cd_cargo_base_servidor as CargoBase
	            FROM dbo.v_servidor_cotic servidor
	            INNER JOIN dbo.v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
	            INNER JOIN dbo.cargo AS cargo ON cargoServidor.cd_cargo = cargo.cd_cargo
	            INNER JOIN dbo.lotacao_servidor AS lotacao_servidor 
		            ON cargoServidor.cd_cargo_base_servidor = lotacao_servidor.cd_cargo_base_servidor
	            INNER JOIN dbo.v_cadastro_unidade_educacao ue ON lotacao_servidor.cd_unidade_educacao = ue.cd_unidade_educacao
	            LEFT JOIN dbo.escola ON ue.cd_unidade_educacao = escola.cd_escola
	            WHERE  lotacao_servidor.dt_fim IS NULL 
				--AND servidor.cd_registro_funcional = @RF
		            AND ((escola.tp_escola IS NOT NULL AND escola.tp_escola IN (1,3,4,16,2,17,20,28,31)) OR escola.tp_escola IS NULL) --EMEF,EMEFM,EMEBS, CEU EMEF
	            UNION
	            SELECT  
		            cargo.cd_cargo
		            ,null AS cd_tipo_funcao
                    ,1 as Sobreposto
                    ,NULL as ComponenteCurricular
		            ,cargoServidor.cd_cargo_base_servidor as CargoBase
	            FROM dbo.v_servidor_cotic servidor
		            INNER JOIN dbo.v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
		            LEFT JOIN dbo.lotacao_servidor AS lotacao_servidor ON cargoServidor.cd_cargo_base_servidor = lotacao_servidor.cd_cargo_base_servidor
		            INNER JOIN dbo.cargo_sobreposto_servidor AS cargo_sobreposto_servidor 
			            ON cargo_sobreposto_servidor.cd_cargo_base_servidor = cargoServidor.cd_cargo_base_servidor
			            AND (cargo_sobreposto_servidor.dt_fim_cargo_sobreposto IS NULL
			            OR cargo_sobreposto_servidor.dt_fim_cargo_sobreposto > GETDATE())
                    INNER JOIN dbo.cargo AS cargo ON cargo_sobreposto_servidor.cd_cargo = cargo.cd_cargo
		            INNER JOIN dbo.v_cadastro_unidade_educacao ue ON cargo_sobreposto_servidor.cd_unidade_local_servico = ue.cd_unidade_educacao
		            LEFT JOIN dbo.escola ON ue.cd_unidade_educacao = escola.cd_escola
	            WHERE  lotacao_servidor.dt_fim IS NULL 
					--AND servidor.cd_registro_funcional = @RF
		            AND ((escola.tp_escola IS NOT NULL AND escola.tp_escola IN (1,3,4,16,2,17,20,28,31)) OR escola.tp_escola IS NULL) --EMEF,EMEFM,EMEBS, CEU EMEF
	            UNION
	            SELECT  
		            cargo.cd_cargo
		            ,null AS cd_tipo_funcao
                    ,0 as Sobreposto
                    ,componente.cd_componente_curricular as ComponenteCurricular
		            ,cargoServidor.cd_cargo_base_servidor as CargoBase
	            FROM dbo.v_servidor_cotic servidor
		            INNER JOIN dbo.v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
		            INNER JOIN dbo.cargo AS cargo ON cargoServidor.cd_cargo = cargo.cd_cargo
		            INNER JOIN dbo.atribuicao_aula atribuicao ON atribuicao.cd_cargo_base_servidor = cargoServidor.cd_cargo_base_servidor
                    LEFT JOIN dbo.componente_curricular componente 
	                    ON atribuicao.cd_componente_curricular = componente.cd_componente_curricular
		                AND componente.dt_cancelamento IS NULL 
		            INNER JOIN dbo.v_cadastro_unidade_educacao ue ON atribuicao.cd_unidade_educacao = ue.cd_unidade_educacao
		            LEFT JOIN dbo.escola ON ue.cd_unidade_educacao = escola.cd_escola
	            WHERE   atribuicao.dt_cancelamento IS NULL 
		            --AND servidor.cd_registro_funcional = @RF
		            AND cargoServidor.dt_fim_nomeacao IS NULL
		            AND atribuicao.dt_disponibilizacao_aulas IS  NULL
		            --AND an_atribuicao  = YEAR(GETDATE())
		            AND ((escola.tp_escola IS NOT NULL AND escola.tp_escola IN (1,3,4,16,2,17,20,28,31)) OR escola.tp_escola IS NULL) --EMEF,EMEFM,EMEBS, CEU EMEF
	            UNION
	            SELECT  
		            cargo.cd_cargo
		            ,atividade.cd_tipo_funcao
                    ,0 as Sobreposto
                    ,NULL as ComponenteCurricular
		            ,cargoServidor.cd_cargo_base_servidor as CargoBase
	            FROM dbo.v_servidor_cotic servidor
		            INNER JOIN dbo.v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
		            INNER JOIN dbo.cargo AS cargo ON cargoServidor.cd_cargo = cargo.cd_cargo
		            INNER JOIN dbo.funcao_atividade_cargo_servidor atividade ON atividade.cd_cargo_base_servidor = cargoServidor.cd_cargo_base_servidor
		            INNER JOIN dbo.v_cadastro_unidade_educacao ue ON atividade.cd_unidade_local_servico = ue.cd_unidade_educacao
		            LEFT JOIN dbo.escola ON ue.cd_unidade_educacao = escola.cd_escola
	            WHERE   atividade.dt_fim_funcao_atividade IS NULL 
		            -- AND servidor.cd_registro_funcional = @RF
		            AND ((escola.tp_escola IS NOT NULL AND escola.tp_escola IN (1,3,4,16,2,17,20,28,31)) OR escola.tp_escola IS NULL) --EMEF,EMEFM,EMEBS, CEU EMEF
	            ) Cargos
                GROUP BY  
		             cd_cargo 
		            ,cd_tipo_funcao
					,ComponenteCurricular
                    ,Sobreposto
		            ,CargoBase