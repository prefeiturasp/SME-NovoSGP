ALTER FUNCTION [dbo].[proc_obter_func_cargo_ue] (@p_cod_ue as varchar(10))
RETURNS @retFuncCargoUe TABLE
(
    NomeServidor varchar(70) NOT NULL,
    CodigoRf varchar(7) NOT NULL,
	DataInicio datetime NULL,
	DataFim datetime NULL,
    Cargo varchar(50) NULL,
	CodigoCargo int null
)
AS
BEGIN
WITH FCUE_cte(NomeServidor, CodigoRf, DataInicio, DataFim, Cargo, CodigoCargo) -- CTE name and columns
    AS (
      SELECT 
	            NomeServidor
	            ,CodigoRF
				,DataInicio 
			    ,DataFim  
	            ,Cargo
				,CodigoCargo
	            FROM(
	                SELECT DISTINCT 
							nm_pessoa              NomeServidor 
			                ,cd_registro_funcional            CodigoRF 
			                ,cargoServidor.dt_posse           DataInicio 
			                ,cargoServidor.dt_fim_nomeacao    DataFim  
	                        ,CASE WHEN cargoSobreposto.dc_cargo IS NOT NULL THEN cargoSobreposto.dc_cargo ELSE cargo.dc_cargo END AS Cargo
				            ,CASE WHEN cargoSobreposto.cd_cargo IS NOT NULL THEN cargoSobreposto.cd_cargo ELSE cargo.cd_cargo END AS CodigoCargo
	                FROM v_servidor_cotic servidor
	                INNER JOIN v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
	                INNER JOIN cargo AS cargo ON cargoServidor.cd_cargo = cargo.cd_cargo
	                LEFT JOIN lotacao_servidor AS lotacao_servidor 
		                ON cargoServidor.cd_cargo_base_servidor = lotacao_servidor.cd_cargo_base_servidor
	                INNER JOIN v_cadastro_unidade_educacao dre 
		                ON lotacao_servidor.cd_unidade_educacao = dre.cd_unidade_educacao 
		            LEFT JOIN (
			            SELECT 
				             cargoSobreposto.cd_cargo
				            ,cargoSobreposto.dc_cargo
				            ,cargo_sobreposto_servidor.cd_cargo_base_servidor 
				            ,cargo_sobreposto_servidor.cd_unidade_local_servico
			            FROM cargo_sobreposto_servidor AS cargo_sobreposto_servidor 
				            INNER JOIN cargo AS cargoSobreposto ON cargo_sobreposto_servidor.cd_cargo = cargoSobreposto.cd_cargo
				            INNER JOIN lotacao_servidor AS lotacao_servidor_sobreposto 
					            ON cargo_sobreposto_servidor.cd_cargo_base_servidor = lotacao_servidor_sobreposto.cd_cargo_base_servidor
			            WHERE (cargo_sobreposto_servidor.dt_fim_cargo_sobreposto IS NULL
			            OR cargo_sobreposto_servidor.dt_fim_cargo_sobreposto > GETDATE())) cargoSobreposto
				            ON cargoSobreposto.cd_cargo_base_servidor = cargoServidor.cd_cargo_base_servidor
					            AND cargoSobreposto.cd_unidade_local_servico = dre.cd_unidade_educacao
	                WHERE  lotacao_servidor.dt_fim IS NULL AND dre.cd_unidade_educacao = @p_cod_ue
		            UNION
		            SELECT DISTINCT nm_pessoa              NomeServidor 
			                ,cd_registro_funcional            CodigoRF 
			                ,cargoServidor.dt_posse           DataInicio 
			                ,cargoServidor.dt_fim_nomeacao    DataFim  
			                ,RTRIM(LTRIM(cargo.dc_cargo))     Cargo
			                ,cargo.cd_cargo					  CodigoCargo
	                FROM v_servidor_cotic servidor
		                INNER JOIN v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
		                LEFT JOIN lotacao_servidor AS lotacao_servidor ON cargoServidor.cd_cargo_base_servidor = lotacao_servidor.cd_cargo_base_servidor
		                INNER JOIN cargo_sobreposto_servidor AS cargo_sobreposto_servidor 
			                ON cargo_sobreposto_servidor.cd_cargo_base_servidor = cargoServidor.cd_cargo_base_servidor
			                AND (cargo_sobreposto_servidor.dt_fim_cargo_sobreposto IS NULL
			                OR cargo_sobreposto_servidor.dt_fim_cargo_sobreposto > GETDATE())
                        INNER JOIN cargo AS cargo ON cargo_sobreposto_servidor.cd_cargo = cargo.cd_cargo
		                INNER JOIN v_cadastro_unidade_educacao dre 
			                ON cargo_sobreposto_servidor.cd_unidade_local_servico = dre.cd_unidade_educacao
		            WHERE  lotacao_servidor.dt_fim IS NULL AND dre.cd_unidade_educacao = @p_cod_ue
                           AND  cargoServidor.dt_fim_nomeacao IS NULL
	                UNION
	                SELECT DISTINCT nm_pessoa              NomeServidor 
			                ,cd_registro_funcional            CodigoRF 
			                ,cargoServidor.dt_posse           DataInicio 
			                ,cargoServidor.dt_fim_nomeacao    DataFim  
			                ,RTRIM(LTRIM(cargo.dc_cargo))     Cargo
			                ,cargo.cd_cargo					  CodigoCargo
	                FROM v_servidor_cotic servidor
		                INNER JOIN v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
		                INNER JOIN cargo AS cargo ON cargoServidor.cd_cargo = cargo.cd_cargo
		                INNER JOIN atribuicao_aula atribuicao ON atribuicao.cd_cargo_base_servidor = cargoServidor.cd_cargo_base_servidor
		                INNER JOIN v_cadastro_unidade_educacao dre 
			                ON atribuicao.cd_unidade_educacao = dre.cd_unidade_educacao
	                WHERE   atribuicao.dt_cancelamento IS NULL 
		                AND dre.cd_unidade_educacao = @p_cod_ue 
		                AND cargoServidor.dt_fim_nomeacao IS NULL
		                AND atribuicao.dt_disponibilizacao_aulas IS  NULL
		                AND YEAR(dt_atribuicao_aula)  = YEAR(GETDATE())                        
	                UNION
		                SELECT DISTINCT nm_pessoa              NomeServidor 
			                ,cd_registro_funcional            CodigoRF 
			                ,cargoServidor.dt_posse           DataInicio 
			                ,cargoServidor.dt_fim_nomeacao    DataFim  
			                ,RTRIM(LTRIM(cargo.dc_cargo))     Cargo
			                ,cargo.cd_cargo					  CodigoCargo
	                FROM v_servidor_cotic servidor
		                INNER JOIN v_cargo_base_cotic AS cargoServidor ON cargoServidor.CD_SERVIDOR = servidor.cd_servidor
		                INNER JOIN cargo AS cargo ON cargoServidor.cd_cargo = cargo.cd_cargo
		                INNER JOIN funcao_atividade_cargo_servidor atividade ON atividade.cd_cargo_base_servidor = cargoServidor.cd_cargo_base_servidor
		                INNER JOIN v_cadastro_unidade_educacao dre 
			                ON atividade.cd_unidade_local_servico = dre.cd_unidade_educacao
	                WHERE   atividade.dt_fim_funcao_atividade IS NULL 
		                AND dre.cd_unidade_educacao = @p_cod_ue 
                        AND cargoServidor.dt_fim_nomeacao IS NULL
            ) Funcionarios
			 WHERE Funcionarios.DataFim is null and CodigoCargo in (3379, 3085, 3360, 3352, 433, 42, 43, 44)
        )
-- copy the required columns to the result of the function
    INSERT @retFuncCargoUe
    SELECT  NomeServidor, CodigoRF, DataInicio, DataFim, Cargo, CodigoCargo
    FROM FCUE_cte
    RETURN
END;
