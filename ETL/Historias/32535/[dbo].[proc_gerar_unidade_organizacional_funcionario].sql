CREATE FUNCTION [dbo].[proc_gerar_unidade_organizacional_funcionario](@p_rf_funcionario as VARCHAR(10))
RETURNS VARCHAR(50)
BEGIN
	DECLARE @codigo_servidor INT;

	SET @codigo_servidor = (select
		cd_servidor as codigo_servidor
	from
		v_servidor_cotic
	where
		cd_registro_funcional is not null
		and cd_registro_funcional = @p_rf_funcionario);

	declare @Usuario TABLE 
	( 
		CodigoRf CHAR(7) ,
		Nome VARCHAR(60),
		Cpf VARCHAR(11),
		CodigoServidor INT,
		 TipoLaudo CHAR(1),
		 DataCessacaoLaudo DATETIME,
		 CargoId INT,
		CodigoCargoSobrePostoReferencia INT,
		 IdCargoBase INT,
         CodigoComponenteCurricular INT,
		 TipoEscolaBase INT,
		DataAtribuicao DATETIME,
		CargoBase INT,
		TipoUnidadeEducacaoBase INT,
		CargoSobrePostoId INT,
		IdCargoSobreposto INT,
		TipoEscolaSobreposto INT ,
		Codigo INT,
		TipoUnidadeEducacao INT,
		CodigoFuncaoAtividade INT,
		TipoEscolaFuncaoAtividade INT,
		TipoUnidadeEducacaoFuncaoAtividade INT
	)

		INSERT INTO @Usuario
		select distinct
				   Rf as CodigoRf,
				   Nome,
				   Cpf,
				   CodigoServidor,
				   laudo.cd_tipo_laudo                    as TipoLaudo,
				   laudo.dt_publicacao_doc_cessacao_laudo as DataCessacaoLaudo,
				   CdCargoBaseServidor                    as CargoId,
				   CargoSobre							  as CodigoCargoSobrePostoReferencia,
				   CdCargoBaseServidor                    as IdCargoBase,
                   atb.cd_componente_curricular           as CodigoComponenteCurricular,
				   coalesce(ue.tp_escola,TipoEscolaBase)  as TipoEscolaBase,
				   COALESCE(atb.dt_atribuicao_aula,Atribuicao) as DataAtribuicao,
				   CargoBase,
				   TipoUnidadeEducacaoBase,
				   CdCargoSobrepostoServidor              as CargoSobrePostoId,
				   CdCargoSobrepostoServidor              as IdCargoSobreposto,
				   TipoEscolaSobre                        as TipoEscolaSobreposto,
				   CargoSobre                             as Codigo,
				   TipoUnidadeEducacaoSobre               as TipoUnidadeEducacao,
				   CdTipoFuncaoAtividade                  as CodigoFuncaoAtividade,
				   TipoEscolaFuncaoAtividade,
				   TipoUnidadeEducacaoFuncaoAtividade
			         from (
                        select sev.cd_registro_funcional        AS Rf,
                            sev.nm_pessoa                    AS Nome,
                            sev.cd_cpf_pessoa                AS Cpf,
                            cba.cd_cargo_base_servidor       as CdCargoBaseServidor,
                            esc_base.tp_escola               AS TipoEscolaBase,
                            cba.cd_cargo                     AS CargoBase,
                            ue_base.tp_unidade_educacao      AS TipoUnidadeEducacaoBase,
                            css.cd_cargo_sobreposto_servidor as CdCargoSobrepostoServidor,
                            esc_sobre.tp_escola              as TipoEscolaSobre,
                            css.cd_cargo                     AS CargoSobre,
                            ue_sobre.tp_unidade_educacao     as TipoUnidadeEducacaoSobre,
                            esc_fat.tp_escola                as TipoEscolaFuncaoAtividade,
                            fat.cd_tipo_funcao               as CdTipoFuncaoAtividade,
                            ue_fat.tp_unidade_educacao       as TipoUnidadeEducacaoFuncaoAtividade,
							sev.cd_servidor                  as CodigoServidor,
							COALESCE(ls.dt_fim, fat.dt_fim_funcao_atividade) as Atribuicao
                        from se1426.dbo.v_servidor_cotic sev with (nolock)
                                -- Cargo Base
                                inner join v_cargo_base_cotic cba with (nolock) on sev.cd_servidor = cba.cd_servidor
                                left join lotacao_servidor ls with (nolock)
                                        on cba.cd_cargo_base_servidor = ls.cd_cargo_base_servidor and (ls.dt_fim is null  or cba.dt_fim_nomeacao > '2020-07-01')
                            -- Cargo Sobreposto
                                left join cargo_sobreposto_servidor css with (nolock)
                                        on cba.cd_cargo_base_servidor = css.cd_cargo_base_servidor AND
                                            (css.dt_fim_cargo_sobreposto IS NULL OR
                                            css.dt_fim_cargo_sobreposto > '2020-07-01')
                            -- Funcao Atividade
                                left join funcao_atividade_cargo_servidor fat with (nolock)
                                        on fat.cd_cargo_base_servidor = cba.cd_cargo_base_servidor
                                            and (fat.dt_cancelamento is null or dt_fim_funcao_atividade > '2020-07-01')
                                            and  fat.dt_fim_funcao_atividade is null
                            --Unidades
                                left join v_cadastro_unidade_educacao ue_base with (nolock)
                                        on (ls.cd_unidade_educacao = ue_base.cd_unidade_educacao)
                                left join escola esc_base with (nolock) on esc_base.cd_escola = ls.cd_unidade_educacao
                                left join v_cadastro_unidade_educacao ue_sobre with (nolock)
                                        on (css.cd_unidade_local_servico = ue_sobre.cd_unidade_educacao)
                                left join escola esc_sobre with (nolock) on esc_sobre.cd_escola = ue_sobre.cd_unidade_educacao
                                left join v_cadastro_unidade_educacao ue_fat with (nolock)
                                        on (fat.cd_unidade_local_servico = ue_fat.cd_unidade_educacao)
                                left join escola esc_fat with (nolock) on esc_fat.cd_escola = fat.cd_unidade_local_servico
                        where
                            sev.cd_servidor = @codigo_servidor
							and (cba.dt_fim_nomeacao is null or cba.dt_fim_nomeacao > '2020-07-01')
							and (cba.dt_cancelamento is null  or cba.dt_cancelamento > '2020-07-01')) servidor
                        -- Atribuicao
                        left join (
                select distinct cd_componente_curricular, cd_cargo_base_servidor, dt_atribuicao_aula, cd_unidade_educacao
                from atribuicao_aula atb with (nolock)
                where atb.an_atribuicao = year(getdate())
                    and (atb.dt_cancelamento is null or atb.dt_cancelamento > '2020-07-01')
                    and atb.dt_disponibilizacao_aulas is null
            ) atb on atb.cd_cargo_base_servidor = servidor.CdCargoBaseServidor
                -- laudo
                        left join (select cd_cargo_base_servidor,
                                        cd_laudo_medico,
                                        row_number()
                                                over (partition by cd_cargo_base_servidor order by cd_laudo_medico desc ) ordem,
                                        cd_tipo_laudo,
                                        dt_inicio,
                                        dt_publicacao_doc_cessacao_laudo
                                from laudo_medico with (nolock)
                                where dt_publicacao_doc_cessacao_laudo is null) laudo
                                on laudo.cd_cargo_base_servidor = servidor.CdCargoBaseServidor and ordem = 1
						left join escola ue on ue.cd_escola = atb.cd_unidade_educacao;

						DECLARE @componente_cj_infantil INT;
						SET @componente_cj_infantil = 504;

						DECLARE @cargo_CP INT;
						SET @cargo_CP = 3379;

						DECLARE @cargo_AD INT;
						SET @cargo_AD = 3085;

						DECLARE @cargo_diretor INT;
						SET @cargo_diretor = 3360;

						DECLARE @cargo_supervisor INT;
						SET @cargo_supervisor = 3352;

						DECLARE @cargo_ATE INT;
						SET @cargo_ATE = 4906;

						declare @CargosSupervisorTecnico TABLE (Id INT,Elemento VARCHAR(10))

						declare @ComponentesCJ TABLE (Id INT,Elemento VARCHAR(10))

						declare @CargosProfessor TABLE (Id INT,Elemento VARCHAR(10))

						INSERT INTO @CargosSupervisorTecnico
						SELECT ROW_NUMBER() OVER (ORDER BY (select null)) as id, elemento
								FROM dbo.proc_string_split('433,433', ',');

						INSERT INTO @ComponentesCJ
						SELECT ROW_NUMBER() OVER (ORDER BY (select null)) as id, elemento
								FROM dbo.proc_string_split('514,526,527,528,529,530,531,532,533', ',');
						
						INSERT INTO @CargosProfessor
						SELECT ROW_NUMBER() OVER (ORDER BY (select null)) as id, elemento
								FROM dbo.proc_string_split('3131,3212,3213,3220,3239,3247,3255,3263,3271,3280,3298,3301,3310,3336,3344,3395,3425,3433,3450,3816,3840,3859,3867,3874,3875,3877,3880,3883,3884', ',');
						
						DECLARE @cargo_base_servidor INT;
						SET @cargo_base_servidor = (SELECT TOP 1 CargoBase from @Usuario);

						DECLARE @cargo_sobreposto_servidor INT;
						SET @cargo_sobreposto_servidor = (SELECT TOP 1 CodigoCargoSobrePostoReferencia from @Usuario);

		IF @cargo_base_servidor = @cargo_CP OR  @cargo_sobreposto_servidor = @cargo_CP
			RETURN '/Admin/CP'
		ELSE IF @cargo_base_servidor = @cargo_AD OR @cargo_sobreposto_servidor = @cargo_AD
			RETURN '/Admin/AD'
		ELSE IF @cargo_base_servidor = @cargo_diretor OR @cargo_sobreposto_servidor = @cargo_diretor
			RETURN '/Admin/DIRETOR'
		ELSE IF @cargo_base_servidor = @cargo_ATE OR @cargo_sobreposto_servidor = @cargo_ATE
			RETURN '/Professor/ATE'
		ELSE IF EXISTS(SELECT 1 FROM @CargosSupervisorTecnico WHERE elemento in  (@cargo_base_servidor, @cargo_sobreposto_servidor))
			 OR @cargo_base_servidor = @cargo_supervisor OR @cargo_sobreposto_servidor = @cargo_supervisor
			RETURN '/Admin/Supervisores'
		ELSE IF EXISTS(SELECT * FROM @CargosProfessor WHERE elemento in  (@cargo_base_servidor, @cargo_sobreposto_servidor))
		BEGIN
			DECLARE @tipo_laudo CHAR(1);
			SET @tipo_laudo = (SELECT TOP 1 TipoLaudo from @Usuario);

			DECLARE @data_laudo DATETIME;
			SET @data_laudo = (SELECT TOP 1 DataCessacaoLaudo from @Usuario);

			IF UPPER(@tipo_laudo) = 'R' AND (@data_laudo IS NULL OR @data_laudo > GETDATE())
				RETURN '/Professores/Readaptados'
			ELSE
			BEGIN
				DECLARE @componente_curricular INT;
				SET @componente_curricular = (SELECT TOP 1 CodigoComponenteCurricular from @Usuario);

				IF @componente_curricular = @componente_cj_infantil OR 
					EXISTS(SELECT 1 FROM @ComponentesCJ WHERE elemento = @componente_curricular)
					RETURN ' /Professores/Sem Atribuição'
				ELSE
					RETURN '/Professores'
			END;
		END;

		RETURN NULL;
END