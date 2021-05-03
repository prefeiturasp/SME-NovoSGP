-- DROP SCHEMA public;

CREATE SCHEMA public AUTHORIZATION postgres;

COMMENT ON SCHEMA public IS 'standard public schema';

-- DROP SEQUENCE public.abrangencia_id_seq;

CREATE SEQUENCE public.abrangencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.acompanhamento_aluno_foto_id_seq;

CREATE SEQUENCE public.acompanhamento_aluno_foto_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.acompanhamento_aluno_id_seq;

CREATE SEQUENCE public.acompanhamento_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.acompanhamento_aluno_semestre_id_seq;

CREATE SEQUENCE public.acompanhamento_aluno_semestre_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.acompanhamento_turma_id_seq;

CREATE SEQUENCE public.acompanhamento_turma_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.aluno_foto_id_seq;

CREATE SEQUENCE public.aluno_foto_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.anotacao_aluno_fechamento_id_seq;

CREATE SEQUENCE public.anotacao_aluno_fechamento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.anotacao_frequencia_aluno_id_seq;

CREATE SEQUENCE public.anotacao_frequencia_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.arquivo_id_seq;

CREATE SEQUENCE public.arquivo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.atividade_avaliativa_disciplina_id_seq;

CREATE SEQUENCE public.atividade_avaliativa_disciplina_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.atividade_avaliativa_id_seq;

CREATE SEQUENCE public.atividade_avaliativa_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.atividade_avaliativa_regencia_id_seq;

CREATE SEQUENCE public.atividade_avaliativa_regencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.atribuicao_cj_id_seq;

CREATE SEQUENCE public.atribuicao_cj_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.atribuicao_esporadica_id_seq;

CREATE SEQUENCE public.atribuicao_esporadica_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.auditoria_id_seq;

CREATE SEQUENCE public.auditoria_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.aula_id_seq;

CREATE SEQUENCE public.aula_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.aula_prevista_bimestre_id_seq;

CREATE SEQUENCE public.aula_prevista_bimestre_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.aula_prevista_id_seq;

CREATE SEQUENCE public.aula_prevista_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.carta_intencoes_id_seq;

CREATE SEQUENCE public.carta_intencoes_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.carta_intencoes_observacao_id_seq;

CREATE SEQUENCE public.carta_intencoes_observacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.carta_intencoes_observacao_notificacao_id_seq;

CREATE SEQUENCE public.carta_intencoes_observacao_notificacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.ciclo_ensino_id_seq;

CREATE SEQUENCE public.ciclo_ensino_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.classificacao_documento_id_seq;

CREATE SEQUENCE public.classificacao_documento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.compensacao_ausencia_aluno_id_seq;

CREATE SEQUENCE public.compensacao_ausencia_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.compensacao_ausencia_disciplina_regencia_id_seq;

CREATE SEQUENCE public.compensacao_ausencia_disciplina_regencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.compensacao_ausencia_id_seq;

CREATE SEQUENCE public.compensacao_ausencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.componente_curricular_id_seq;

CREATE SEQUENCE public.componente_curricular_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.componente_curriculo_cidade_id_seq;

CREATE SEQUENCE public.componente_curriculo_cidade_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.comunicado_aluno_id_seq;

CREATE SEQUENCE public.comunicado_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.comunicado_id_seq;

CREATE SEQUENCE public.comunicado_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.comunicado_turma_id_seq;

CREATE SEQUENCE public.comunicado_turma_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.comunidado_grupo_id_seq;

CREATE SEQUENCE public.comunidado_grupo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.conceito_valores_id_seq;

CREATE SEQUENCE public.conceito_valores_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.configuracao_email_id_seq;

CREATE SEQUENCE public.configuracao_email_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.conselho_classe_aluno_id_seq;

CREATE SEQUENCE public.conselho_classe_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.conselho_classe_id_seq;

CREATE SEQUENCE public.conselho_classe_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.conselho_classe_nota_id_seq;

CREATE SEQUENCE public.conselho_classe_nota_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.conselho_classe_parecer_ano_id_seq;

CREATE SEQUENCE public.conselho_classe_parecer_ano_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.conselho_classe_parecer_id_seq;

CREATE SEQUENCE public.conselho_classe_parecer_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.conselho_classe_recomendacao_id_seq;

CREATE SEQUENCE public.conselho_classe_recomendacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.devolutiva_diario_bordo_notificacao_id_seq;

CREATE SEQUENCE public.devolutiva_diario_bordo_notificacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.devolutiva_id_seq;

CREATE SEQUENCE public.devolutiva_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.diario_bordo_id_seq;

CREATE SEQUENCE public.diario_bordo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.diario_bordo_observacao_id_seq;

CREATE SEQUENCE public.diario_bordo_observacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.diario_bordo_observacao_notificacao_id_seq;

CREATE SEQUENCE public.diario_bordo_observacao_notificacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.documento_id_seq;

CREATE SEQUENCE public.documento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.dre_id_seq;

CREATE SEQUENCE public.dre_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.encaminhamento_aee_id_seq;

CREATE SEQUENCE public.encaminhamento_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.encaminhamento_aee_secao_id_seq;

CREATE SEQUENCE public.encaminhamento_aee_secao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.etl_sgp_atribuicaocj_id_seq;

CREATE SEQUENCE public.etl_sgp_atribuicaocj_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.etl_sgp_aula_plano_aula_dados_ok_id_seq;

CREATE SEQUENCE public.etl_sgp_aula_plano_aula_dados_ok_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.etl_sgp_compensacao_ausencia_id_seq;

CREATE SEQUENCE public.etl_sgp_compensacao_ausencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.evento_fechamento_id_seq;

CREATE SEQUENCE public.evento_fechamento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.evento_id_seq;

CREATE SEQUENCE public.evento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.evento_matricula_id_seq;

CREATE SEQUENCE public.evento_matricula_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.evento_tipo_id_seq;

CREATE SEQUENCE public.evento_tipo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_aluno_id_seq;

CREATE SEQUENCE public.fechamento_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_bimestre_id_seq;

CREATE SEQUENCE public.fechamento_bimestre_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_id_seq;

CREATE SEQUENCE public.fechamento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_reabertura_bimestre_id_seq;

CREATE SEQUENCE public.fechamento_reabertura_bimestre_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_reabertura_id_seq;

CREATE SEQUENCE public.fechamento_reabertura_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_reabertura_notificacao_id_seq;

CREATE SEQUENCE public.fechamento_reabertura_notificacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_turma_disciplina_id_seq;

CREATE SEQUENCE public.fechamento_turma_disciplina_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.fechamento_turma_id_seq;

CREATE SEQUENCE public.fechamento_turma_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.feriado_calendario_id_seq;

CREATE SEQUENCE public.feriado_calendario_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.frequencia_aluno_id_seq;

CREATE SEQUENCE public.frequencia_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.grade_disciplina_id_seq;

CREATE SEQUENCE public.grade_disciplina_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.grade_filtro_id_seq;

CREATE SEQUENCE public.grade_filtro_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.grade_id_seq;

CREATE SEQUENCE public.grade_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.grupo_comunicado_id_seq;

CREATE SEQUENCE public.grupo_comunicado_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.historico_email_usuario_id_seq;

CREATE SEQUENCE public.historico_email_usuario_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.historico_nota_conselho_classe_id_seq;

CREATE SEQUENCE public.historico_nota_conselho_classe_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.historico_nota_fechamento_id_seq;

CREATE SEQUENCE public.historico_nota_fechamento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.historico_nota_id_seq;

CREATE SEQUENCE public.historico_nota_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.historico_reinicio_senha_id_seq;

CREATE SEQUENCE public.historico_reinicio_senha_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_aluno_id_seq;

CREATE SEQUENCE public.itinerancia_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_aluno_questao_id_seq;

CREATE SEQUENCE public.itinerancia_aluno_questao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_evento_id_seq;

CREATE SEQUENCE public.itinerancia_evento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_id_seq;

CREATE SEQUENCE public.itinerancia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_objetivo_base_id_seq;

CREATE SEQUENCE public.itinerancia_objetivo_base_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_objetivo_id_seq;

CREATE SEQUENCE public.itinerancia_objetivo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_questao_id_seq;

CREATE SEQUENCE public.itinerancia_questao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.itinerancia_ue_id_seq;

CREATE SEQUENCE public.itinerancia_ue_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.matriz_saber_id_seq;

CREATE SEQUENCE public.matriz_saber_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.matriz_saber_plano_id_seq;

CREATE SEQUENCE public.matriz_saber_plano_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.motivo_ausencia_id_seq;

CREATE SEQUENCE public.motivo_ausencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.nota_conceito_bimestre_id_seq;

CREATE SEQUENCE public.nota_conceito_bimestre_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notas_conceito_id_seq;

CREATE SEQUENCE public.notas_conceito_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notas_conceitos_ciclos_parametos_id_seq;

CREATE SEQUENCE public.notas_conceitos_ciclos_parametos_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notas_parametros_id_seq;

CREATE SEQUENCE public.notas_parametros_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notas_tipo_valor_id_seq;

CREATE SEQUENCE public.notas_tipo_valor_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notificacao_aula_id_seq;

CREATE SEQUENCE public.notificacao_aula_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notificacao_aula_prevista_id_seq;

CREATE SEQUENCE public.notificacao_aula_prevista_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notificacao_compensacao_ausencia_id_seq;

CREATE SEQUENCE public.notificacao_compensacao_ausencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notificacao_frequencia_id_seq;

CREATE SEQUENCE public.notificacao_frequencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notificacao_id_seq;

CREATE SEQUENCE public.notificacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notificacao_plano_aee_id_seq;

CREATE SEQUENCE public.notificacao_plano_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.notificacao_plano_aee_observacao_id_seq;

CREATE SEQUENCE public.notificacao_plano_aee_observacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.objetivo_aprendizagem_aula_id_seq;

CREATE SEQUENCE public.objetivo_aprendizagem_aula_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.objetivo_aprendizagem_plano_id_seq;

CREATE SEQUENCE public.objetivo_aprendizagem_plano_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.objetivo_desenvolvimento_id_seq;

CREATE SEQUENCE public.objetivo_desenvolvimento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.objetivo_desenvolvimento_plano_id_seq;

CREATE SEQUENCE public.objetivo_desenvolvimento_plano_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.objetivo_resposta_id_seq;

CREATE SEQUENCE public.objetivo_resposta_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.ocorrencia_aluno_id_seq;

CREATE SEQUENCE public.ocorrencia_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.ocorrencia_id_seq;

CREATE SEQUENCE public.ocorrencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.ocorrencia_tipo_id_seq;

CREATE SEQUENCE public.ocorrencia_tipo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.opcao_questao_complementar_id_seq;

CREATE SEQUENCE public.opcao_questao_complementar_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.opcao_resposta_id_seq;

CREATE SEQUENCE public.opcao_resposta_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.parametros_sistema_id_seq;

CREATE SEQUENCE public.parametros_sistema_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_aula_id_seq;

CREATE SEQUENCE public.pendencia_aula_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_calendario_ue_id_seq;

CREATE SEQUENCE public.pendencia_calendario_ue_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_encaminhamento_aee_id_seq;

CREATE SEQUENCE public.pendencia_encaminhamento_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_fechamento_id_seq;

CREATE SEQUENCE public.pendencia_fechamento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_id_seq;

CREATE SEQUENCE public.pendencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_parametro_evento_id_seq;

CREATE SEQUENCE public.pendencia_parametro_evento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_plano_aee_id_seq;

CREATE SEQUENCE public.pendencia_plano_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_professor_id_seq;

CREATE SEQUENCE public.pendencia_professor_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_registro_individual_aluno_id_seq;

CREATE SEQUENCE public.pendencia_registro_individual_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_registro_individual_id_seq;

CREATE SEQUENCE public.pendencia_registro_individual_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.pendencia_usuario_id_seq;

CREATE SEQUENCE public.pendencia_usuario_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.perfil_evento_tipo_id_seq;

CREATE SEQUENCE public.perfil_evento_tipo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.periodo_escolar_id_seq;

CREATE SEQUENCE public.periodo_escolar_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.planejamento_anual_componente_id_seq;

CREATE SEQUENCE public.planejamento_anual_componente_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.planejamento_anual_id_seq;

CREATE SEQUENCE public.planejamento_anual_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.planejamento_anual_objetivos_aprendizagem_id_seq;

CREATE SEQUENCE public.planejamento_anual_objetivos_aprendizagem_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.planejamento_anual_periodo_escolar_id_seq;

CREATE SEQUENCE public.planejamento_anual_periodo_escolar_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_aee_id_seq;

CREATE SEQUENCE public.plano_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_aee_observacao_id_seq;

CREATE SEQUENCE public.plano_aee_observacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_aee_questao_id_seq;

CREATE SEQUENCE public.plano_aee_questao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_aee_reestruturacao_id_seq;

CREATE SEQUENCE public.plano_aee_reestruturacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_aee_resposta_id_seq;

CREATE SEQUENCE public.plano_aee_resposta_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_aee_versao_id_seq;

CREATE SEQUENCE public.plano_aee_versao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_anual_id_seq;

CREATE SEQUENCE public.plano_anual_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_anual_territorio_saber_id_seq;

CREATE SEQUENCE public.plano_anual_territorio_saber_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_aula_id_seq;

CREATE SEQUENCE public.plano_aula_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.plano_ciclo_id_seq;

CREATE SEQUENCE public.plano_ciclo_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.prioridade_perfil_id_seq;

CREATE SEQUENCE public.prioridade_perfil_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.processo_executando_id_seq;

CREATE SEQUENCE public.processo_executando_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.questao_encaminhamento_aee_id_seq;

CREATE SEQUENCE public.questao_encaminhamento_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.questao_id_seq;

CREATE SEQUENCE public.questao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.questionario_id_seq;

CREATE SEQUENCE public.questionario_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.recuperacao_paralela_id_seq;

CREATE SEQUENCE public.recuperacao_paralela_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.recuperacao_paralela_periodo_objetivo_resposta_id_seq;

CREATE SEQUENCE public.recuperacao_paralela_periodo_objetivo_resposta_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.registro_ausencia_aluno_id_seq;

CREATE SEQUENCE public.registro_ausencia_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.registro_frequencia_id_seq;

CREATE SEQUENCE public.registro_frequencia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.registro_individual_id_seq;

CREATE SEQUENCE public.registro_individual_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.registro_individual_sugestao_id_seq;

CREATE SEQUENCE public.registro_individual_sugestao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.registro_poa_id_seq;

CREATE SEQUENCE public.registro_poa_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.relatorio_correlacao_id_seq;

CREATE SEQUENCE public.relatorio_correlacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.relatorio_correlacao_jasper_id_seq;

CREATE SEQUENCE public.relatorio_correlacao_jasper_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.relatorio_semestral_pap_aluno_id_seq;

CREATE SEQUENCE public.relatorio_semestral_pap_aluno_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.relatorio_semestral_pap_aluno_secao_id_seq;

CREATE SEQUENCE public.relatorio_semestral_pap_aluno_secao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.relatorio_semestral_turma_pap_id_seq;

CREATE SEQUENCE public.relatorio_semestral_turma_pap_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.resposta_encaminhamento_aee_id_seq;

CREATE SEQUENCE public.resposta_encaminhamento_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.secao_encaminhamento_aee_id_seq;

CREATE SEQUENCE public.secao_encaminhamento_aee_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.secao_relatorio_semestral_pap_id_seq;

CREATE SEQUENCE public.secao_relatorio_semestral_pap_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.sintese_valores_id_seq;

CREATE SEQUENCE public.sintese_valores_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.supervisor_escola_dre_id_seq;

CREATE SEQUENCE public.supervisor_escola_dre_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.tipo_avaliacao_id_seq;

CREATE SEQUENCE public.tipo_avaliacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.tipo_calendario_id_seq;

CREATE SEQUENCE public.tipo_calendario_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.tipo_ciclo_ano_id_seq;

CREATE SEQUENCE public.tipo_ciclo_ano_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.tipo_documento_id_seq;

CREATE SEQUENCE public.tipo_documento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.tipo_escola_id_seq;

CREATE SEQUENCE public.tipo_escola_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.turma_id_seq;

CREATE SEQUENCE public.turma_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.ue_id_seq;

CREATE SEQUENCE public.ue_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.usuario_id_seq;

CREATE SEQUENCE public.usuario_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.wf_aprovacao_id_seq;

CREATE SEQUENCE public.wf_aprovacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.wf_aprovacao_itinerancia_id_seq;

CREATE SEQUENCE public.wf_aprovacao_itinerancia_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.wf_aprovacao_nivel_id_seq;

CREATE SEQUENCE public.wf_aprovacao_nivel_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.wf_aprovacao_nivel_notificacao_id_seq;

CREATE SEQUENCE public.wf_aprovacao_nivel_notificacao_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.wf_aprovacao_nivel_usuario_id_seq;

CREATE SEQUENCE public.wf_aprovacao_nivel_usuario_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.wf_aprovacao_nota_fechamento_id_seq;

CREATE SEQUENCE public.wf_aprovacao_nota_fechamento_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;-- public.arquivo definition

-- Drop table

-- DROP TABLE public.arquivo;

CREATE TABLE public.arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar NOT NULL,
	codigo uuid NOT NULL,
	tipo int4 NOT NULL,
	tipo_conteudo varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT arquivo_pk PRIMARY KEY (id)
);


-- public.atribuicao_cj definition

-- Drop table

-- DROP TABLE public.atribuicao_cj;

CREATE TABLE public.atribuicao_cj (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	disciplina_id int8 NOT NULL,
	dre_id varchar(15) NOT NULL,
	ue_id varchar(15) NOT NULL,
	professor_rf varchar(10) NOT NULL,
	turma_id varchar(15) NOT NULL,
	modalidade int4 NOT NULL,
	substituir bool NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT atribuicao_cj_pk PRIMARY KEY (id)
);
CREATE INDEX atribuicao_cj_componente_cur_idx ON public.atribuicao_cj USING btree (disciplina_id);
CREATE INDEX atribuicao_cj_modalidade_idx ON public.atribuicao_cj USING btree (modalidade);
CREATE INDEX atribuicao_cj_professor_rf_idx ON public.atribuicao_cj USING btree (professor_rf);
CREATE INDEX atribuicao_cj_turma_id_idx ON public.atribuicao_cj USING btree (turma_id);
CREATE INDEX atribuicao_cj_ue_id_idx ON public.atribuicao_cj USING btree (ue_id);


-- public.atribuicao_esporadica definition

-- Drop table

-- DROP TABLE public.atribuicao_esporadica;

CREATE TABLE public.atribuicao_esporadica (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	professor_rf varchar(10) NOT NULL,
	ue_id varchar(15) NOT NULL,
	dre_id varchar(15) NOT NULL,
	data_inicio date NOT NULL,
	data_fim date NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false
);


-- public.auditoria definition

-- Drop table

-- DROP TABLE public.auditoria;

CREATE TABLE public.auditoria (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	"data" timestamp NOT NULL,
	entidade varchar(200) NOT NULL,
	chave int8 NOT NULL,
	usuario varchar(200) NOT NULL,
	acao varchar(1) NOT NULL,
	rf varchar(200) NOT NULL
);


-- public.ciclo_ensino definition

-- Drop table

-- DROP TABLE public.ciclo_ensino;

CREATE TABLE public.ciclo_ensino (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	cod_ciclo_ensino_eol int8 NOT NULL,
	descricao varchar(60) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	data_atualizacao timestamp NULL,
	codigo_modalidade_ensino int8 NULL,
	codigo_etapa_ensino int8 NULL,
	CONSTRAINT ciclo_ensino_pk PRIMARY KEY (id)
);


-- public.componente_curricular_area_conhecimento definition

-- Drop table

-- DROP TABLE public.componente_curricular_area_conhecimento;

CREATE TABLE public.componente_curricular_area_conhecimento (
	id int8 NOT NULL,
	nome varchar(200) NOT NULL,
	CONSTRAINT componente_curricular_area_conhecimento_pk PRIMARY KEY (id)
);


-- public.componente_curricular_grupo_matriz definition

-- Drop table

-- DROP TABLE public.componente_curricular_grupo_matriz;

CREATE TABLE public.componente_curricular_grupo_matriz (
	id int8 NOT NULL,
	nome varchar(200) NOT NULL,
	CONSTRAINT componente_curricular_grupo_matriz_pk PRIMARY KEY (id)
);


-- public.componente_curricular_jurema definition

-- Drop table

-- DROP TABLE public.componente_curricular_jurema;

CREATE TABLE public.componente_curricular_jurema (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_jurema int8 NOT NULL,
	codigo_eol int8 NOT NULL,
	descricao_eol varchar(100) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT componente_curricular_pk PRIMARY KEY (id),
	CONSTRAINT componente_curricular_un UNIQUE (codigo_jurema, codigo_eol)
);


-- public.comunicado definition

-- Drop table

-- DROP TABLE public.comunicado;

CREATE TABLE public.comunicado (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	titulo varchar(50) NOT NULL,
	descricao varchar NOT NULL,
	data_envio timestamp NOT NULL,
	data_expiracao timestamp NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	ano_letivo int4 NOT NULL DEFAULT 2020,
	modalidade int4 NULL,
	semestre int4 NULL,
	tipo_comunicado int4 NOT NULL DEFAULT 1,
	codigo_dre varchar(50) NULL,
	codigo_ue varchar(50) NULL,
	alunos_especificados bool NOT NULL DEFAULT false,
	series_resumidas varchar(100) NULL,
	tipo_calendario_id int8 NULL,
	evento_id int8 NULL,
	CONSTRAINT comunicado_pk PRIMARY KEY (id)
);


-- public.comunicado_aluno definition

-- Drop table

-- DROP TABLE public.comunicado_aluno;

CREATE TABLE public.comunicado_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aluno_codigo varchar(30) NOT NULL,
	comunicado_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL
);


-- public.comunicado_turma definition

-- Drop table

-- DROP TABLE public.comunicado_turma;

CREATE TABLE public.comunicado_turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_codigo varchar(30) NOT NULL,
	comunicado_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL
);


-- public.conceito_valores definition

-- Drop table

-- DROP TABLE public.conceito_valores;

CREATE TABLE public.conceito_valores (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor varchar(50) NOT NULL,
	descricao varchar(200) NOT NULL,
	aprovado bool NOT NULL DEFAULT true,
	ativo bool NOT NULL DEFAULT true,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT conceito_valores_pk PRIMARY KEY (id)
);
CREATE INDEX conceito_valores_valor_idx ON public.conceito_valores USING btree (valor);
CREATE INDEX sintese_valores_valor_idx ON public.conceito_valores USING btree (valor);


-- public.configuracao_email definition

-- Drop table

-- DROP TABLE public.configuracao_email;

CREATE TABLE public.configuracao_email (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	email_remetente varchar(100) NOT NULL,
	nome_remetente varchar(100) NOT NULL,
	servidor_smtp varchar(100) NOT NULL,
	usuario varchar(50) NOT NULL,
	senha varchar(50) NOT NULL,
	porta int4 NOT NULL,
	usar_tls bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT configuracao_email_pk PRIMARY KEY (id)
);


-- public.conselho_classe_parecer definition

-- Drop table

-- DROP TABLE public.conselho_classe_parecer;

CREATE TABLE public.conselho_classe_parecer (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) NOT NULL,
	aprovado bool NOT NULL DEFAULT false,
	frequencia bool NOT NULL DEFAULT false,
	conselho bool NOT NULL DEFAULT false,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(20) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(20) NULL,
	alterado_em timestamp NULL,
	nota bool NOT NULL DEFAULT false,
	CONSTRAINT conselho_classe_parecer_pkey PRIMARY KEY (id)
);


-- public.conselho_classe_recomendacao definition

-- Drop table

-- DROP TABLE public.conselho_classe_recomendacao;

CREATE TABLE public.conselho_classe_recomendacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	recomendacao varchar NOT NULL,
	tipo int4 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT conselho_classe_recomendacao_pk PRIMARY KEY (id)
);
CREATE INDEX ix_conselho_classe_recomendacao_tipo ON public.conselho_classe_recomendacao USING btree (tipo);


-- public.devolutiva definition

-- Drop table

-- DROP TABLE public.devolutiva;

CREATE TABLE public.devolutiva (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar NOT NULL,
	componente_curricular_codigo int8 NOT NULL,
	periodo_inicio timestamp NOT NULL,
	periodo_fim timestamp NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT devolutiva_pk PRIMARY KEY (id)
);


-- public.dre definition

-- Drop table

-- DROP TABLE public.dre;

CREATE TABLE public.dre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	dre_id varchar(15) NULL,
	abreviacao varchar(10) NULL,
	nome varchar(100) NULL,
	data_atualizacao date NOT NULL,
	CONSTRAINT dre_pk PRIMARY KEY (id)
);
CREATE INDEX dre_dre_id_idx ON public.dre USING btree (dre_id);


-- public.etl_sgp_abrangencia_cj_ano definition

-- Drop table

-- DROP TABLE public.etl_sgp_abrangencia_cj_ano;

CREATE TABLE public.etl_sgp_abrangencia_cj_ano (
	tud_id int8 NULL,
	tau_data date NULL,
	disciplina_id int8 NOT NULL,
	dre_id varchar(200) NULL,
	ue_id varchar(200) NULL,
	servidor varchar(200) NOT NULL,
	professor_rf varchar(200) NOT NULL,
	turma_id varchar(200) NOT NULL,
	modalidade varchar(200) NULL,
	substituir varchar(200) NULL,
	criado_em date NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em date NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL
);


-- public.etl_sgp_atividade_avaliativa definition

-- Drop table

-- DROP TABLE public.etl_sgp_atividade_avaliativa;

CREATE TABLE public.etl_sgp_atividade_avaliativa (
	atividade_avaliativa_id int4 NULL,
	dre_id varchar(15) NULL,
	ue_id varchar(15) NULL,
	professor_rf varchar(10) NULL,
	turma_id varchar(15) NULL,
	categoria_id int4 NULL,
	tipo_avaliacao_id int4 NULL,
	tipoavaliacaolegado int4 NULL,
	nome_avaliacao varchar(100) NULL,
	descricao_avaliacao varchar NULL,
	data_avaliacao timestamp NULL,
	criado_em timestamp NULL,
	criado_por varchar(200) NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NULL,
	migrado bool NULL,
	eh_regencia bool NULL,
	sgp_legado_disciplina_ideolregencia int4 NULL,
	sgp_legado_nomedisciplinaregencia varchar(100) NULL,
	sgp_legado_disciplina_ideol int4 NULL,
	sgp_legado_nome_disciplina varchar(100) NULL,
	sgp_legado_tipoturmadisciplina int4 NULL,
	sgp_legado_tnt_id int4 NULL,
	sgp_legado_tud_id int4 NULL,
	sgp_legado_turnomeeol varchar(15) NULL,
	sgp_legado_turcodigoeol varchar(15) NULL,
	sgp_legado_turma_id_interno int4 NULL,
	sgp_legado_nomedre varchar(200) NULL
);


-- public.etl_sgp_atribuicaocj definition

-- Drop table

-- DROP TABLE public.etl_sgp_atribuicaocj;

CREATE TABLE public.etl_sgp_atribuicaocj (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	disciplina_id int8 NOT NULL,
	dre_id varchar(15) NOT NULL,
	ue_id varchar(15) NOT NULL,
	professor_rf varchar(10) NOT NULL,
	turma_id varchar(15) NOT NULL,
	modalidade int4 NOT NULL,
	CONSTRAINT etl_sgp_atribuicaocj_pk PRIMARY KEY (id)
);
CREATE INDEX etl_sgp_atribuicaocj_disciplina_id_idx ON public.etl_sgp_atribuicaocj USING btree (disciplina_id);
CREATE INDEX etl_sgp_atribuicaocj_dre_id_idx ON public.etl_sgp_atribuicaocj USING btree (dre_id);
CREATE INDEX etl_sgp_atribuicaocj_modalidade_idx ON public.etl_sgp_atribuicaocj USING btree (modalidade);
CREATE INDEX etl_sgp_atribuicaocj_professor_rf_idx ON public.etl_sgp_atribuicaocj USING btree (professor_rf);
CREATE INDEX etl_sgp_atribuicaocj_turma_id_idx ON public.etl_sgp_atribuicaocj USING btree (turma_id);
CREATE INDEX etl_sgp_atribuicaocj_ue_id_idx ON public.etl_sgp_atribuicaocj USING btree (ue_id);


-- public.etl_sgp_aula_plano_aula_dados_ok definition

-- Drop table

-- DROP TABLE public.etl_sgp_aula_plano_aula_dados_ok;

CREATE TABLE public.etl_sgp_aula_plano_aula_dados_ok (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aul_ue_id varchar(15) NOT NULL,
	aul_disciplina_id varchar(15) NOT NULL,
	aul_turma_id varchar(15) NOT NULL,
	aul_tipo_calendario_id int8 NOT NULL,
	aul_professor_rf varchar(15) NOT NULL,
	aul_quantidade int8 NOT NULL,
	aul_data_aula date NOT NULL,
	aul_recorrencia_aula int8 NOT NULL,
	aul_tipo_aula int8 NOT NULL,
	aul_criado_em timestamp NOT NULL,
	aul_criado_por varchar(200) NOT NULL,
	aul_alterado_em timestamp NULL,
	aul_alterado_por varchar(200) NULL,
	aul_criado_rf varchar(15) NOT NULL,
	aul_alterado_rf varchar(15) NULL,
	pla_descricao varchar NULL,
	pla_desenvolvimento_aula varchar NOT NULL,
	pla_recuperacao_aula varchar NULL,
	pla_licao_casa varchar NULL,
	sql_tud_id int8 NULL,
	sql_tpc_id int8 NULL,
	sql_tau_id int8 NULL,
	sql_tur_id int8 NULL,
	sql_tur_codigoeol int8 NULL,
	sql_esc_id int8 NULL,
	sql_esc_codigoeol varchar(20) NULL
);


-- public.etl_sgp_compensacao_ausencia definition

-- Drop table

-- DROP TABLE public.etl_sgp_compensacao_ausencia;

CREATE TABLE public.etl_sgp_compensacao_ausencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	bimestre int4 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	turma_id int8 NOT NULL,
	nome varchar NOT NULL,
	descricao varchar NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	ano_letivo int4 NOT NULL,
	codigo_aluno varchar(100) NOT NULL,
	qtd_faltas_compensadas int4 NOT NULL,
	CONSTRAINT etl_sgp_compensacao_ausencia_pk PRIMARY KEY (id)
);


-- public.etl_sgp_notas_conceitos definition

-- Drop table

-- DROP TABLE public.etl_sgp_notas_conceitos;

CREATE TABLE public.etl_sgp_notas_conceitos (
	notas_conceito_id int4 NULL,
	atividade_avaliativa_id int4 NULL,
	alu_id int4 NULL,
	sgp_legado_nome_aluno varchar(500) NULL,
	sgp_legado_avaliacao_valor_original varchar(200) NULL,
	nota numeric(5,2) NULL,
	conceito int4 NULL,
	tipo_nota int4 NULL,
	criado_por varchar(200) NULL,
	criado_rf varchar(200) NULL,
	criado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	disciplina_id varchar(15) NULL,
	sgp_legado_nome_disciplina varchar(100) NULL,
	sgp_legado_tnt_id int4 NULL,
	sgp_legado_tud_id int4 NULL,
	sgp_legado_turcodigoeol varchar(15) NULL,
	sgp_legado_turma_id_interno int4 NULL,
	sgp_legado_turnomeeol varchar(15) NULL
);


-- public.etl_sgp_turma definition

-- Drop table

-- DROP TABLE public.etl_sgp_turma;

CREATE TABLE public.etl_sgp_turma (
	id int8 NULL,
	turma_id varchar(15) NULL,
	ue_id int8 NULL,
	nome varchar(10) NULL,
	ano bpchar(1) NULL,
	ano_letivo int4 NULL,
	modalidade_codigo int4 NULL,
	semestre int4 NULL,
	qt_duracao_aula int2 NULL,
	tipo_turno int2 NULL,
	data_atualizacao date NULL,
	historica bool NULL,
	dt_fim_eol date NULL
);


-- public.evento_matricula definition

-- Drop table

-- DROP TABLE public.evento_matricula;

CREATE TABLE public.evento_matricula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno varchar(100) NOT NULL,
	tipo int4 NOT NULL,
	data_evento timestamp NOT NULL,
	nome_escola varchar(200) NULL,
	nome_turma varchar(200) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT evento_matricula_un UNIQUE (codigo_aluno, tipo, data_evento)
);
CREATE INDEX evento_matricula_aluno_idx ON public.evento_matricula USING btree (codigo_aluno);


-- public.evento_tipo definition

-- Drop table

-- DROP TABLE public.evento_tipo;

CREATE TABLE public.evento_tipo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(200) NOT NULL,
	local_ocorrencia int4 NOT NULL,
	concomitancia bool NOT NULL DEFAULT true,
	tipo_data int4 NOT NULL,
	dependencia bool NOT NULL DEFAULT false,
	letivo int4 NOT NULL,
	ativo bool NOT NULL DEFAULT true,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	codigo int8 NOT NULL,
	somente_leitura bool NOT NULL DEFAULT false,
	evento_escolaaqui bool NOT NULL DEFAULT false,
	CONSTRAINT evento_tipo_pk PRIMARY KEY (id)
);
CREATE INDEX evento_tipo_excluido_idx ON public.evento_tipo USING btree (excluido);


-- public.feriado_calendario definition

-- Drop table

-- DROP TABLE public.feriado_calendario;

CREATE TABLE public.feriado_calendario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(50) NOT NULL,
	abrangencia int4 NOT NULL,
	data_feriado timestamp NOT NULL,
	tipo int4 NOT NULL,
	ativo bool NOT NULL DEFAULT true,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT feriado_calendario_un UNIQUE (id)
);


-- public.flyway_schema_history definition

-- Drop table

-- DROP TABLE public.flyway_schema_history;

CREATE TABLE public.flyway_schema_history (
	installed_rank int4 NOT NULL,
	"version" varchar(50) NULL,
	description varchar(200) NOT NULL,
	"type" varchar(20) NOT NULL,
	script varchar(1000) NOT NULL,
	checksum int4 NULL,
	installed_by varchar(100) NOT NULL,
	installed_on timestamp NOT NULL DEFAULT now(),
	execution_time int4 NOT NULL,
	success bool NOT NULL,
	CONSTRAINT flyway_schema_history_pk PRIMARY KEY (installed_rank)
);
CREATE INDEX flyway_schema_history_s_idx ON public.flyway_schema_history USING btree (success);


-- public.grade definition

-- Drop table

-- DROP TABLE public.grade;

CREATE TABLE public.grade (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT grade_pk PRIMARY KEY (id)
);


-- public.grupo_comunicado definition

-- Drop table

-- DROP TABLE public.grupo_comunicado;

CREATE TABLE public.grupo_comunicado (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(30) NULL,
	tipo_escola_id varchar(200) NULL,
	tipo_ciclo_id varchar(200) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	etapa_ensino_id varchar(50) NULL,
	CONSTRAINT grupo_comunicado_pk PRIMARY KEY (id)
);


-- public.historico_reinicio_senha definition

-- Drop table

-- DROP TABLE public.historico_reinicio_senha;

CREATE TABLE public.historico_reinicio_senha (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_rf varchar(200) NOT NULL,
	dre_codigo varchar(200) NOT NULL,
	ue_codigo varchar(200) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT historico_reinicio_senha_pk PRIMARY KEY (id)
);
CREATE INDEX historico_reinicio_senha_dre_idx ON public.historico_reinicio_senha USING btree (dre_codigo);
CREATE INDEX historico_reinicio_senha_ue_idx ON public.historico_reinicio_senha USING btree (ue_codigo);
CREATE INDEX historico_reinicio_senha_usuario_idx ON public.historico_reinicio_senha USING btree (usuario_rf);


-- public.itinerancia_objetivo_base definition

-- Drop table

-- DROP TABLE public.itinerancia_objetivo_base;

CREATE TABLE public.itinerancia_objetivo_base (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) NOT NULL,
	tem_descricao bool NOT NULL,
	permite_varias_ues bool NOT NULL,
	ordem int4 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT itinerancia_objetivo_base_pk PRIMARY KEY (id)
);


-- public.matriz_saber definition

-- Drop table

-- DROP TABLE public.matriz_saber;

CREATE TABLE public.matriz_saber (
	descricao varchar(100) NOT NULL,
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT matriz_saber_pk PRIMARY KEY (id)
);


-- public.motivo_ausencia definition

-- Drop table

-- DROP TABLE public.motivo_ausencia;

CREATE TABLE public.motivo_ausencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(150) NOT NULL,
	CONSTRAINT motivo_ausencia_pap_pk PRIMARY KEY (id)
);


-- public.notas_parametros definition

-- Drop table

-- DROP TABLE public.notas_parametros;

CREATE TABLE public.notas_parametros (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor_minimo float4 NULL,
	valor_medio float4 NOT NULL,
	valor_maximo float4 NOT NULL,
	incremento float4 NOT NULL,
	ativo bool NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT notas_parametros_pk PRIMARY KEY (id)
);


-- public.notas_tipo_valor definition

-- Drop table

-- DROP TABLE public.notas_tipo_valor;

CREATE TABLE public.notas_tipo_valor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_nota int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	ativo bool NOT NULL DEFAULT false,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT notas_tipo_valor_pk PRIMARY KEY (id)
);
CREATE INDEX notas_tipo_valor_tipo_nota_idx ON public.notas_tipo_valor USING btree (tipo_nota);


-- public.notificacao_aula_prevista definition

-- Drop table

-- DROP TABLE public.notificacao_aula_prevista;

CREATE TABLE public.notificacao_aula_prevista (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	bimestre int4 NOT NULL,
	notificacao_id int8 NOT NULL,
	disciplina_id varchar(20) NOT NULL,
	turma_id varchar(15) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT notificacao_aula_prevista_pk PRIMARY KEY (id)
);
CREATE INDEX notificacao_aula_prevista_disciplina_idx ON public.notificacao_aula_prevista USING btree (disciplina_id);
CREATE INDEX notificacao_aula_prevista_notificacao_idx ON public.notificacao_aula_prevista USING btree (notificacao_id);


-- public.objetivo_aprendizagem definition

-- Drop table

-- DROP TABLE public.objetivo_aprendizagem;

CREATE TABLE public.objetivo_aprendizagem (
	id int8 NOT NULL,
	descricao varchar(1000) NOT NULL,
	codigo varchar(20) NOT NULL,
	ano_turma varchar(10) NOT NULL,
	componente_curricular_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em date NOT NULL,
	atualizado_em date NOT NULL,
	CONSTRAINT objetivo_aprendizagem_pk PRIMARY KEY (id)
);


-- public.ocorrencia_tipo definition

-- Drop table

-- DROP TABLE public.ocorrencia_tipo;

CREATE TABLE public.ocorrencia_tipo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	descricao varchar(50) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT ocorrencia_tipo_pk PRIMARY KEY (id)
);


-- public.parametros_sistema definition

-- Drop table

-- DROP TABLE public.parametros_sistema;

CREATE TABLE public.parametros_sistema (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(50) NOT NULL,
	tipo int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	valor varchar(100) NOT NULL,
	ano int4 NULL,
	ativo bool NOT NULL DEFAULT true,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT parametros_sistema_pk PRIMARY KEY (id)
);


-- public.pendencia definition

-- Drop table

-- DROP TABLE public.pendencia;

CREATE TABLE public.pendencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	titulo varchar(200) NOT NULL,
	descricao varchar NOT NULL,
	situacao int4 NOT NULL,
	tipo int4 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	instrucao varchar(200) NULL,
	CONSTRAINT pendencia_pk PRIMARY KEY (id)
);


-- public.periodo_escolar definition

-- Drop table

-- DROP TABLE public.periodo_escolar;

CREATE TABLE public.periodo_escolar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_calendario_id int8 NOT NULL,
	bimestre int4 NOT NULL,
	periodo_inicio timestamp NOT NULL,
	periodo_fim timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT periodo_escolar_pk PRIMARY KEY (id)
);


-- public.plano_anual definition

-- Drop table

-- DROP TABLE public.plano_anual;

CREATE TABLE public.plano_anual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	escola_id varchar(10) NOT NULL,
	turma_id int8 NOT NULL,
	ano int8 NOT NULL,
	bimestre int4 NOT NULL,
	descricao varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	migrado bool NOT NULL DEFAULT false,
	componente_curricular_eol_id int8 NOT NULL,
	objetivos_opcionais bool NOT NULL DEFAULT false,
	CONSTRAINT plano_anual_pk PRIMARY KEY (id),
	CONSTRAINT plano_anual_un UNIQUE (escola_id, turma_id, ano, bimestre, componente_curricular_eol_id)
);


-- public.plano_anual_territorio_saber definition

-- Drop table

-- DROP TABLE public.plano_anual_territorio_saber;

CREATE TABLE public.plano_anual_territorio_saber (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	escola_id varchar(10) NOT NULL,
	turma_id int8 NOT NULL,
	ano int8 NOT NULL,
	bimestre int4 NOT NULL,
	territorio_experiencia_id int8 NOT NULL,
	desenvolvimento varchar NOT NULL,
	reflexao varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_anual_territorio_saber_pk PRIMARY KEY (id),
	CONSTRAINT plano_anual_territorio_saber_un UNIQUE (escola_id, turma_id, ano, bimestre, territorio_experiencia_id)
);


-- public.plano_ciclo definition

-- Drop table

-- DROP TABLE public.plano_ciclo;

CREATE TABLE public.plano_ciclo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar NOT NULL,
	ano int8 NOT NULL,
	ciclo_id int8 NOT NULL,
	escola_id varchar(10) NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_ciclo_pk PRIMARY KEY (id),
	CONSTRAINT plano_ciclo_un UNIQUE (ano, ciclo_id, escola_id)
);


-- public.prioridade_perfil definition

-- Drop table

-- DROP TABLE public.prioridade_perfil;

CREATE TABLE public.prioridade_perfil (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ordem int8 NOT NULL,
	tipo int4 NOT NULL,
	nome_perfil varchar(100) NOT NULL,
	codigo_perfil uuid NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT prioridade_perfil_ordem_un UNIQUE (ordem),
	CONSTRAINT prioridade_perfil_un UNIQUE (codigo_perfil)
);


-- public.processo_executando definition

-- Drop table

-- DROP TABLE public.processo_executando;

CREATE TABLE public.processo_executando (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_processo int4 NOT NULL,
	turma_id varchar(15) NULL,
	disciplina_id varchar(20) NULL,
	bimestre int4 NOT NULL,
	aula_id int8 NULL,
	CONSTRAINT processo_executando_pk PRIMARY KEY (id)
);
CREATE INDEX processo_executando_disciplina_idx ON public.processo_executando USING btree (disciplina_id);
CREATE INDEX processo_executando_turma_idx ON public.processo_executando USING btree (turma_id);


-- public.questionario definition

-- Drop table

-- DROP TABLE public.questionario;

CREATE TABLE public.questionario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	tipo int4 NOT NULL DEFAULT 1,
	CONSTRAINT questionario_pk PRIMARY KEY (id)
);


-- public.recuperacao_paralela_objetivo_desenvolvimento definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_objetivo_desenvolvimento;

CREATE TABLE public.recuperacao_paralela_objetivo_desenvolvimento (
	descricao varchar(100) NOT NULL,
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_desenvolvimento_pk PRIMARY KEY (id)
);


-- public.recuperacao_paralela_periodo definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_periodo;

CREATE TABLE public.recuperacao_paralela_periodo (
	id int8 NOT NULL,
	nome varchar(100) NOT NULL,
	descricao varchar(200) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	bimestre_edicao int4 NOT NULL DEFAULT 0,
	CONSTRAINT recuperacao_paralela_periodo_pk PRIMARY KEY (id)
);


-- public.recuperacao_paralela_resposta definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_resposta;

CREATE TABLE public.recuperacao_paralela_resposta (
	id int8 NOT NULL,
	nome varchar(100) NOT NULL,
	descricao varchar(600) NOT NULL,
	sim bool NULL,
	excluido bool NOT NULL DEFAULT false,
	dt_inicio timestamp NOT NULL,
	dt_fim timestamp NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	ordem int8 NULL,
	CONSTRAINT resposta_pk PRIMARY KEY (id)
);


-- public.registro_individual_sugestao definition

-- Drop table

-- DROP TABLE public.registro_individual_sugestao;

CREATE TABLE public.registro_individual_sugestao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	mes int4 NOT NULL,
	descricao varchar(150) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT registro_individual_sugestao_pk PRIMARY KEY (id)
);


-- public.registro_poa definition

-- Drop table

-- DROP TABLE public.registro_poa;

CREATE TABLE public.registro_poa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_rf varchar(20) NOT NULL,
	titulo varchar(50) NOT NULL,
	descricao varchar NOT NULL,
	dre_id varchar(50) NOT NULL,
	ue_id varchar(50) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(30) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(30) NULL,
	ano_letivo int2 NOT NULL DEFAULT 2019,
	bimestre int8 NOT NULL
);
CREATE INDEX registro_poa_codigo_rf_idx ON public.registro_poa USING btree (codigo_rf);
CREATE INDEX registro_poa_dre_id_idx ON public.registro_poa USING btree (dre_id);
CREATE INDEX registro_poa_titulo_idx ON public.registro_poa USING btree (titulo);
CREATE INDEX registro_poa_ue_id_idx ON public.registro_poa USING btree (ue_id);


-- public.secao_relatorio_semestral_pap definition

-- Drop table

-- DROP TABLE public.secao_relatorio_semestral_pap;

CREATE TABLE public.secao_relatorio_semestral_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(50) NOT NULL,
	descricao varchar NOT NULL,
	obrigatorio bool NOT NULL DEFAULT false,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	ordem int4 NOT NULL,
	CONSTRAINT secao_relatorio_semestral_pap_pk PRIMARY KEY (id)
);


-- public.sintese_valores definition

-- Drop table

-- DROP TABLE public.sintese_valores;

CREATE TABLE public.sintese_valores (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor varchar(50) NOT NULL,
	descricao varchar(200) NOT NULL,
	aprovado bool NOT NULL DEFAULT true,
	ativo bool NOT NULL DEFAULT true,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT sinstese_valores_pk PRIMARY KEY (id)
);


-- public.supervisor_escola_dre definition

-- Drop table

-- DROP TABLE public.supervisor_escola_dre;

CREATE TABLE public.supervisor_escola_dre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	supervisor_id varchar(15) NOT NULL,
	escola_id varchar(10) NOT NULL,
	dre_id varchar(15) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT supervisor_escola_dre_ck UNIQUE (supervisor_id, escola_id, dre_id),
	CONSTRAINT supervisor_escola_pk PRIMARY KEY (id)
);


-- public.temp_etl_sgp_turma definition

-- Drop table

-- DROP TABLE public.temp_etl_sgp_turma;

CREATE TABLE public.temp_etl_sgp_turma (
	id int8 NULL,
	turma_id varchar(15) NULL,
	ue_id int8 NULL,
	nome varchar(10) NULL,
	ano bpchar(1) NULL,
	ano_letivo int4 NULL,
	modalidade_codigo int4 NULL,
	semestre int4 NULL,
	qt_duracao_aula int2 NULL,
	tipo_turno int2 NULL,
	data_atualizacao date NULL,
	historica bool NULL,
	dt_fim_eol date NULL
);


-- public.temp_turma definition

-- Drop table

-- DROP TABLE public.temp_turma;

CREATE TABLE public.temp_turma (
	id int8 NULL,
	turma_id varchar(15) NULL,
	ue_id int8 NULL,
	nome varchar(10) NULL,
	ano bpchar(1) NULL,
	ano_letivo int4 NULL,
	modalidade_codigo int4 NULL,
	semestre int4 NULL,
	qt_duracao_aula int2 NULL,
	tipo_turno int2 NULL,
	data_atualizacao date NULL,
	historica bool NULL,
	dt_fim_eol date NULL
);


-- public.temp_ue definition

-- Drop table

-- DROP TABLE public.temp_ue;

CREATE TABLE public.temp_ue (
	id int8 NULL,
	ue_id varchar(15) NULL,
	dre_id int8 NULL,
	nome varchar(200) NULL,
	tipo_escola int2 NULL,
	data_atualizacao date NULL
);


-- public.tempconselhoalunos definition

-- Drop table

-- DROP TABLE public.tempconselhoalunos;

CREATE TABLE public.tempconselhoalunos (
	conselhoclassealunoid int8 NULL
);


-- public.tempturmacomplementarconselhoaluno definition

-- Drop table

-- DROP TABLE public.tempturmacomplementarconselhoaluno;

CREATE TABLE public.tempturmacomplementarconselhoaluno (
	turmacodigo varchar(15) NULL,
	turmaregularcodigo varchar(15) NULL,
	modalidade int4 NULL,
	alunocodigo varchar(15) NULL,
	ano bpchar(1) NULL,
	etapaeja int4 NULL,
	ciclo varchar(200) NULL,
	tipoturma int4 NULL
);


-- public.tempturmaconselho definition

-- Drop table

-- DROP TABLE public.tempturmaconselho;

CREATE TABLE public.tempturmaconselho (
	turmacodigo int8 NULL,
	alunocodigo varchar(15) NULL,
	ano bpchar(1) NULL,
	conselhoclassealunoid int8 NULL
);


-- public.tempturmaregularconselhoaluno definition

-- Drop table

-- DROP TABLE public.tempturmaregularconselhoaluno;

CREATE TABLE public.tempturmaregularconselhoaluno (
	turmacodigo varchar(15) NULL,
	turmaregularcodigo text NULL,
	modalidade int4 NULL,
	alunocodigo varchar(15) NULL,
	ano bpchar(1) NULL,
	etapaeja int4 NULL,
	ciclo varchar(200) NULL,
	tipoturma int4 NULL,
	conselhoclassealunoid int8 NULL
);


-- public.teste definition

-- Drop table

-- DROP TABLE public.teste;

CREATE TABLE public.teste (
	ue_id int8 NOT NULL,
	dre_id int8 NOT NULL
);


-- public.tipo_avaliacao definition

-- Drop table

-- DROP TABLE public.tipo_avaliacao;

CREATE TABLE public.tipo_avaliacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(30) NOT NULL,
	descricao varchar(200) NOT NULL,
	situacao bool NOT NULL DEFAULT true,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	avaliacoes_necessarias_bimestre int4 NOT NULL DEFAULT 0,
	codigo int4 NULL,
	CONSTRAINT tipo_avaliacao_pk PRIMARY KEY (id)
);


-- public.tipo_calendario definition

-- Drop table

-- DROP TABLE public.tipo_calendario;

CREATE TABLE public.tipo_calendario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ano_letivo int4 NOT NULL,
	nome varchar(50) NOT NULL,
	periodo int4 NOT NULL,
	modalidade int4 NOT NULL,
	situacao bool NOT NULL DEFAULT true,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT tipo_calendario_un UNIQUE (id)
);


-- public.tipo_ciclo definition

-- Drop table

-- DROP TABLE public.tipo_ciclo;

CREATE TABLE public.tipo_ciclo (
	id int8 NOT NULL,
	descricao varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT tipo_ciclo_pk PRIMARY KEY (id),
	CONSTRAINT tipo_ciclo_un UNIQUE (descricao)
);


-- public.tipo_documento definition

-- Drop table

-- DROP TABLE public.tipo_documento;

CREATE TABLE public.tipo_documento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(60) NULL,
	CONSTRAINT tipo_documento_pk PRIMARY KEY (id)
);


-- public.tipo_escola definition

-- Drop table

-- DROP TABLE public.tipo_escola;

CREATE TABLE public.tipo_escola (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	cod_tipo_escola_eol int8 NOT NULL,
	descricao varchar(60) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	data_atualizacao timestamp NULL,
	CONSTRAINT tipo_escola_pk PRIMARY KEY (id)
);


-- public.usuario definition

-- Drop table

-- DROP TABLE public.usuario;

CREATE TABLE public.usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	rf_codigo varchar(12) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	login varchar(50) NULL,
	ultimo_login timestamp NULL,
	nome varchar(100) NULL,
	expiracao_recuperacao_senha timestamp NULL,
	token_recuperacao_senha uuid NULL,
	CONSTRAINT usuario_pk PRIMARY KEY (id),
	CONSTRAINT usuario_un_login UNIQUE (login)
);
CREATE INDEX usuario_codigo_rf_idx ON public.usuario USING btree (rf_codigo);
CREATE INDEX usuario_login_idx ON public.usuario USING btree (login);


-- public.wf_aprovacao definition

-- Drop table

-- DROP TABLE public.wf_aprovacao;

CREATE TABLE public.wf_aprovacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id varchar(15) NULL,
	dre_id varchar(15) NULL,
	ano int4 NULL,
	turma_id varchar(15) NULL,
	notificacao_mensagem varchar NOT NULL,
	notificacao_titulo varchar(500) NOT NULL,
	notificacao_tipo int4 NOT NULL,
	notificacao_categoria int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	tipo int4 NOT NULL DEFAULT 1,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT wf_aprova_niveis_pk PRIMARY KEY (id)
);


-- public.abrangencia definition

-- Drop table

-- DROP TABLE public.abrangencia;

CREATE TABLE public.abrangencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	dre_id int8 NULL,
	ue_id int8 NULL,
	turma_id int8 NULL,
	perfil uuid NOT NULL,
	historico bool NOT NULL DEFAULT false,
	dt_fim_vinculo date NULL,
	turma_id_eol int4 NULL,
	usuario_rf varchar(7) NULL,
	dre_id_eol varchar(6) NULL,
	ue_id_eol varchar(6) NULL,
	CONSTRAINT abrangencia_pk PRIMARY KEY (id),
	CONSTRAINT abrangencia_dre_usario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);
CREATE INDEX abrangencia_dre_id_idx ON public.abrangencia USING btree (dre_id);
CREATE INDEX abrangencia_turma_idx ON public.abrangencia USING btree (turma_id);
CREATE INDEX abrangencia_ue_idx ON public.abrangencia USING btree (ue_id);
CREATE INDEX abrangencia_usuario_idx ON public.abrangencia USING btree (usuario_id);


-- public.aluno_foto definition

-- Drop table

-- DROP TABLE public.aluno_foto;

CREATE TABLE public.aluno_foto (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aluno_codigo varchar(15) NOT NULL,
	miniatura_id int8 NULL,
	arquivo_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT aluno_foto_pk PRIMARY KEY (id),
	CONSTRAINT aluno_foto_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id),
	CONSTRAINT aluno_foto_miniatura_fk FOREIGN KEY (miniatura_id) REFERENCES aluno_foto(id)
);
CREATE INDEX aluno_foto_aluno_codigo_idx ON public.aluno_foto USING btree (aluno_codigo);
CREATE INDEX aluno_foto_arquivo_idx ON public.aluno_foto USING btree (arquivo_id);
CREATE INDEX aluno_foto_miniatura_idx ON public.aluno_foto USING btree (miniatura_id);


-- public.atividade_avaliativa definition

-- Drop table

-- DROP TABLE public.atividade_avaliativa;

CREATE TABLE public.atividade_avaliativa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	dre_id varchar(15) NOT NULL,
	ue_id varchar(15) NOT NULL,
	professor_rf varchar(10) NOT NULL,
	turma_id varchar(15) NOT NULL,
	categoria_id int4 NOT NULL,
	tipo_avaliacao_id int4 NOT NULL,
	nome_avaliacao varchar(100) NOT NULL,
	descricao_avaliacao varchar(5000) NULL,
	data_avaliacao timestamp NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	eh_regencia bool NOT NULL DEFAULT false,
	eh_cj bool NULL DEFAULT false,
	migrado bool NULL,
	CONSTRAINT atividade_avaliativa_pk PRIMARY KEY (id),
	CONSTRAINT atividade_avaliativa_tipo_atividade_avaliativa_fk FOREIGN KEY (tipo_avaliacao_id) REFERENCES tipo_avaliacao(id)
);
CREATE INDEX ix_filter_atividade_avaliativa ON public.atividade_avaliativa USING btree (excluido, data_avaliacao, dre_id, ue_id, turma_id, professor_rf);


-- public.atividade_avaliativa_disciplina definition

-- Drop table

-- DROP TABLE public.atividade_avaliativa_disciplina;

CREATE TABLE public.atividade_avaliativa_disciplina (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	atividade_avaliativa_id int8 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT atividade_avaliativa_disciplina_pk PRIMARY KEY (id),
	CONSTRAINT atividade_avaliativa_disciplina_fk FOREIGN KEY (atividade_avaliativa_id) REFERENCES atividade_avaliativa(id)
);
CREATE INDEX atividade_avaliativa_disciplina_disciplina_id_ix ON public.atividade_avaliativa_disciplina USING btree (disciplina_id) WHERE (excluido = false);
CREATE INDEX ix_atividade_avaliativa_id ON public.atividade_avaliativa_disciplina USING btree (atividade_avaliativa_id);


-- public.atividade_avaliativa_regencia definition

-- Drop table

-- DROP TABLE public.atividade_avaliativa_regencia;

CREATE TABLE public.atividade_avaliativa_regencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	atividade_avaliativa_id int8 NOT NULL,
	disciplina_contida_regencia_id varchar(15) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT atividade_avaliativa_regencia_pk PRIMARY KEY (id),
	CONSTRAINT atividade_avaliativa_regencia_atividade_avaliativa_fk FOREIGN KEY (atividade_avaliativa_id) REFERENCES atividade_avaliativa(id)
);


-- public.aula definition

-- Drop table

-- DROP TABLE public.aula;

CREATE TABLE public.aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id varchar(15) NOT NULL,
	disciplina_id varchar(20) NOT NULL,
	turma_id varchar(15) NOT NULL,
	tipo_calendario_id int8 NOT NULL,
	professor_rf varchar(15) NOT NULL,
	quantidade int4 NOT NULL,
	data_aula timestamp NOT NULL,
	recorrencia_aula int4 NOT NULL,
	tipo_aula int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	aula_pai_id int8 NULL,
	wf_aprovacao_id int8 NULL,
	status int4 NULL DEFAULT 1,
	aula_cj bool NOT NULL DEFAULT false,
	disciplina_compartilhada_id varchar(15) NULL,
	CONSTRAINT aula_pk PRIMARY KEY (id),
	CONSTRAINT aula_pai_fk FOREIGN KEY (aula_pai_id) REFERENCES aula(id),
	CONSTRAINT aula_tipo_calendario_fk FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id)
);
CREATE INDEX aula_aula_pai_idx ON public.aula USING btree (aula_pai_id);
CREATE INDEX aula_aula_tipo_calendario_fk_ix ON public.aula USING btree (tipo_calendario_id);
CREATE INDEX aula_data_aula_idx ON public.aula USING btree (data_aula);
CREATE INDEX aula_disciplina_idx ON public.aula USING btree (disciplina_id);
CREATE INDEX aula_turma_idx ON public.aula USING btree (turma_id);
CREATE INDEX aula_ue_idx ON public.aula USING btree (ue_id);
CREATE INDEX professor_rf_idx ON public.aula USING btree (professor_rf);


-- public.aula_prevista definition

-- Drop table

-- DROP TABLE public.aula_prevista;

CREATE TABLE public.aula_prevista (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_calendario_id int8 NOT NULL,
	disciplina_id varchar(20) NOT NULL,
	turma_id varchar(15) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT aula_prevista_pk PRIMARY KEY (id),
	CONSTRAINT aula_prevista_tipo_calendario_fk FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id)
);


-- public.aula_prevista_bimestre definition

-- Drop table

-- DROP TABLE public.aula_prevista_bimestre;

CREATE TABLE public.aula_prevista_bimestre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_prevista_id int8 NOT NULL,
	aulas_previstas int4 NOT NULL,
	bimestre int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT aula_prevista_bimestre_pk PRIMARY KEY (id),
	CONSTRAINT aula_prevista_bimestre_aula_prevista_fk FOREIGN KEY (aula_prevista_id) REFERENCES aula_prevista(id)
);


-- public.classificacao_documento definition

-- Drop table

-- DROP TABLE public.classificacao_documento;

CREATE TABLE public.classificacao_documento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(10) NULL,
	tipo_documento_id int8 NOT NULL,
	CONSTRAINT classificacao_documento_pk PRIMARY KEY (id),
	CONSTRAINT classificacao_documento_tipo_documento_fk FOREIGN KEY (tipo_documento_id) REFERENCES tipo_documento(id)
);
CREATE INDEX classificacao_documento_tipo_documento_idx ON public.classificacao_documento USING btree (tipo_documento_id);


-- public.componente_curricular definition

-- Drop table

-- DROP TABLE public.componente_curricular;

CREATE TABLE public.componente_curricular (
	id int8 NOT NULL,
	componente_curricular_pai_id int8 NULL,
	grupo_matriz_id int8 NULL DEFAULT 1,
	area_conhecimento_id int8 NULL,
	descricao varchar(100) NULL,
	eh_regencia bool NOT NULL DEFAULT false,
	eh_compartilhada bool NOT NULL DEFAULT false,
	eh_territorio bool NOT NULL DEFAULT false,
	eh_base_nacional bool NOT NULL DEFAULT false,
	permite_registro_frequencia bool NOT NULL DEFAULT false,
	permite_lancamento_nota bool NOT NULL DEFAULT true,
	descricao_sgp varchar(100) NULL,
	CONSTRAINT componente_curricular_teste_pk PRIMARY KEY (id),
	CONSTRAINT componente_curricular_area_conhecimento_fk FOREIGN KEY (area_conhecimento_id) REFERENCES componente_curricular_area_conhecimento(id),
	CONSTRAINT componente_curricular_grupo_matriz_fk FOREIGN KEY (grupo_matriz_id) REFERENCES componente_curricular_grupo_matriz(id)
);
CREATE INDEX componente_curricular_area_conhecimento_idx ON public.componente_curricular USING btree (area_conhecimento_id);
CREATE INDEX componente_curricular_grupo_matriz_idx ON public.componente_curricular USING btree (grupo_matriz_id);


-- public.componente_curricular_grupo_area_ordenacao definition

-- Drop table

-- DROP TABLE public.componente_curricular_grupo_area_ordenacao;

CREATE TABLE public.componente_curricular_grupo_area_ordenacao (
	grupo_matriz_id int8 NOT NULL,
	area_conhecimento_id int8 NOT NULL,
	ordem int4 NULL,
	CONSTRAINT componente_curricular_grupo_area_ordenacao_pk PRIMARY KEY (grupo_matriz_id, area_conhecimento_id),
	CONSTRAINT cc_gp_ac_ordenacao_area_fk FOREIGN KEY (area_conhecimento_id) REFERENCES componente_curricular_area_conhecimento(id),
	CONSTRAINT cc_gp_ac_ordenacao_grupo_fk FOREIGN KEY (grupo_matriz_id) REFERENCES componente_curricular_grupo_matriz(id)
);
CREATE INDEX cc_gp_ac_ordenacao_area__idx ON public.componente_curricular_grupo_area_ordenacao USING btree (area_conhecimento_id);
CREATE INDEX cc_gp_ac_ordenacao_grupo_idx ON public.componente_curricular_grupo_area_ordenacao USING btree (grupo_matriz_id);


-- public.componente_curricular_regencia definition

-- Drop table

-- DROP TABLE public.componente_curricular_regencia;

CREATE TABLE public.componente_curricular_regencia (
	componente_curricular_id int8 NOT NULL,
	turno int8 NULL,
	ano int8 NULL,
	CONSTRAINT componente_curricular_regencia_cp_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id)
);
CREATE INDEX componente_curricular_regencia_cp_idx ON public.componente_curricular_regencia USING btree (componente_curricular_id);


-- public.componente_curriculo_cidade definition

-- Drop table

-- DROP TABLE public.componente_curriculo_cidade;

CREATE TABLE public.componente_curriculo_cidade (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo int8 NOT NULL,
	componente_curricular_id int8 NULL,
	CONSTRAINT componente_curriculo_cidade_pk PRIMARY KEY (id),
	CONSTRAINT componente_curriculo_cidade_un UNIQUE (codigo, componente_curricular_id),
	CONSTRAINT componente_curriculo_cidade_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id)
);
CREATE INDEX componente_curriculo_cidade_componente_curricular_idx ON public.componente_curriculo_cidade USING btree (componente_curricular_id);


-- public.comunidado_grupo definition

-- Drop table

-- DROP TABLE public.comunidado_grupo;

CREATE TABLE public.comunidado_grupo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	comunicado_id int4 NOT NULL,
	grupo_comunicado_id int4 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT comunidado_grupo_pk PRIMARY KEY (id),
	CONSTRAINT comunidado_grupo_comunicado_fk FOREIGN KEY (comunicado_id) REFERENCES comunicado(id),
	CONSTRAINT comunidado_grupo_grupo_comunicado_fk FOREIGN KEY (grupo_comunicado_id) REFERENCES grupo_comunicado(id)
);


-- public.conselho_classe_parecer_ano definition

-- Drop table

-- DROP TABLE public.conselho_classe_parecer_ano;

CREATE TABLE public.conselho_classe_parecer_ano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	parecer_id int8 NOT NULL,
	ano_turma int4 NOT NULL,
	modalidade int4 NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(20) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(20) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT conselho_classe_parecer_ano_pkey PRIMARY KEY (id),
	CONSTRAINT conselho_classe_parecer_ano_parecer_id_fkey FOREIGN KEY (parecer_id) REFERENCES conselho_classe_parecer(id)
);
CREATE INDEX conselho_classe_parecer_ano_ano_turma_idx ON public.conselho_classe_parecer_ano USING btree (ano_turma, modalidade);


-- public.diario_bordo definition

-- Drop table

-- DROP TABLE public.diario_bordo;

CREATE TABLE public.diario_bordo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	devolutiva_id int8 NULL,
	planejamento varchar NOT NULL,
	reflexoes_replanejamento varchar NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT diario_bordo_pk PRIMARY KEY (id),
	CONSTRAINT diario_bordo_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id),
	CONSTRAINT diario_bordo_devolutiva_fk FOREIGN KEY (devolutiva_id) REFERENCES devolutiva(id)
);
CREATE INDEX diario_bordo_aula_idx ON public.diario_bordo USING btree (aula_id);
CREATE INDEX diario_bordo_devolutiva_idx ON public.diario_bordo USING btree (devolutiva_id);


-- public.diario_bordo_observacao definition

-- Drop table

-- DROP TABLE public.diario_bordo_observacao;

CREATE TABLE public.diario_bordo_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	observacao varchar NOT NULL,
	diario_bordo_id int8 NULL,
	usuario_id int8 NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(10) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(10) NULL,
	CONSTRAINT diario_bordo_observacao_pk PRIMARY KEY (id),
	CONSTRAINT diario_bordo_observacao_diario_bordo_fk FOREIGN KEY (diario_bordo_id) REFERENCES diario_bordo(id),
	CONSTRAINT diario_bordo_observacao_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);
CREATE INDEX diario_bordo_observacoes_idx ON public.diario_bordo_observacao USING btree (diario_bordo_id);


-- public.evento definition

-- Drop table

-- DROP TABLE public.evento;

CREATE TABLE public.evento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) NOT NULL,
	descricao varchar(500) NULL,
	data_inicio date NOT NULL,
	data_fim date NULL,
	letivo int4 NOT NULL,
	feriado_id int8 NULL,
	tipo_calendario_id int8 NOT NULL,
	tipo_evento_id int8 NOT NULL,
	dre_id varchar(15) NULL,
	ue_id varchar(15) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	evento_pai_id int8 NULL,
	migrado bool NOT NULL DEFAULT false,
	wf_aprovacao_id int8 NULL,
	status int4 NOT NULL DEFAULT 1,
	tipo_perfil_cadastro int4 NULL,
	CONSTRAINT evento_pk PRIMARY KEY (id),
	CONSTRAINT evento_fk FOREIGN KEY (evento_pai_id) REFERENCES evento(id),
	CONSTRAINT evento_wf_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id),
	CONSTRAINT feriado_fk FOREIGN KEY (feriado_id) REFERENCES feriado_calendario(id),
	CONSTRAINT tipo_calendario_fk FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id)
);
CREATE INDEX evento_criado_rf_idx ON public.evento USING btree (criado_rf);
CREATE INDEX evento_data_fim_idx ON public.evento USING btree (data_fim);
CREATE INDEX evento_data_inicio_idx ON public.evento USING btree (data_inicio);
CREATE INDEX evento_dre_idx ON public.evento USING btree (dre_id);
CREATE INDEX evento_excluido_idx ON public.evento USING btree (excluido);
CREATE INDEX evento_tipo_calendario_idx ON public.evento USING btree (tipo_calendario_id);
CREATE INDEX evento_ue_idx ON public.evento USING btree (ue_id);
CREATE INDEX evento_wf_aprovacao_idx ON public.evento USING btree (wf_aprovacao_id);


-- public.frequencia_aluno definition

-- Drop table

-- DROP TABLE public.frequencia_aluno;

CREATE TABLE public.frequencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno varchar(100) NOT NULL,
	tipo int4 NOT NULL,
	disciplina_id varchar(20) NOT NULL,
	periodo_inicio timestamp NOT NULL,
	periodo_fim timestamp NOT NULL,
	bimestre int4 NOT NULL,
	total_aulas int4 NOT NULL,
	total_ausencias int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	total_compensacoes int4 NOT NULL DEFAULT 0,
	turma_id varchar(15) NULL,
	periodo_escolar_id int8 NULL,
	CONSTRAINT frequencia_aluno_un UNIQUE (codigo_aluno, tipo, disciplina_id, periodo_inicio, periodo_fim, turma_id),
	CONSTRAINT frequencia_aluno_periodo_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id)
);
CREATE INDEX frequencia_aluno_periodo_idx ON public.frequencia_aluno USING btree (periodo_escolar_id);


-- public.grade_disciplina definition

-- Drop table

-- DROP TABLE public.grade_disciplina;

CREATE TABLE public.grade_disciplina (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	grade_id int8 NOT NULL,
	ano int2 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	quantidade_aulas int2 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT grade_disciplina_pk PRIMARY KEY (id),
	CONSTRAINT grade_disciplina_grade_id_fk FOREIGN KEY (grade_id) REFERENCES grade(id) ON DELETE CASCADE
);
CREATE INDEX grade_disciplina_ano_idx ON public.grade_disciplina USING btree (ano);
CREATE INDEX grade_disciplina_componente_curricular_idx ON public.grade_disciplina USING btree (componente_curricular_id);
CREATE INDEX grade_disciplina_grade_idx ON public.grade_disciplina USING btree (grade_id);


-- public.grade_filtro definition

-- Drop table

-- DROP TABLE public.grade_filtro;

CREATE TABLE public.grade_filtro (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	grade_id int8 NOT NULL,
	tipo_escola int2 NULL,
	modalidade int4 NOT NULL,
	duracao_turno int2 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT grade_filtro_pk PRIMARY KEY (id),
	CONSTRAINT grade_filtro_grade_id_fk FOREIGN KEY (grade_id) REFERENCES grade(id) ON DELETE CASCADE
);
CREATE INDEX grade_filtro_grade_idx ON public.grade_filtro USING btree (grade_id);


-- public.historico_email_usuario definition

-- Drop table

-- DROP TABLE public.historico_email_usuario;

CREATE TABLE public.historico_email_usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	email varchar(100) NOT NULL,
	tipo_acao int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT historico_email_usuario_pk PRIMARY KEY (id),
	CONSTRAINT historico_email_usuario_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);


-- public.historico_nota definition

-- Drop table

-- DROP TABLE public.historico_nota;

CREATE TABLE public.historico_nota (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	nota_anterior numeric(5,2) NULL,
	nota_nova numeric(5,2) NULL,
	conceito_anterior_id int8 NULL,
	conceito_novo_id int8 NULL,
	CONSTRAINT historico_nota_pk PRIMARY KEY (id),
	CONSTRAINT historico_nota_conceito_anterior_fk FOREIGN KEY (conceito_anterior_id) REFERENCES conceito_valores(id),
	CONSTRAINT historico_nota_conceito_novo_fk FOREIGN KEY (conceito_novo_id) REFERENCES conceito_valores(id)
);
CREATE INDEX historico_nota_conceito_anterior_idx ON public.historico_nota USING btree (conceito_anterior_id);
CREATE INDEX historico_nota_conceito_novo_idx ON public.historico_nota USING btree (conceito_novo_id);


-- public.itinerancia definition

-- Drop table

-- DROP TABLE public.itinerancia;

CREATE TABLE public.itinerancia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	data_visita timestamp NOT NULL,
	data_retorno_verificacao timestamp NULL,
	situacao int4 NOT NULL DEFAULT 1,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	ano_letivo int4 NOT NULL,
	evento_id int8 NULL,
	CONSTRAINT itinerancia_pk PRIMARY KEY (id),
	CONSTRAINT itinerancia_evento_fk FOREIGN KEY (evento_id) REFERENCES evento(id)
);
CREATE INDEX itinerancia_evento_idx ON public.itinerancia USING btree (evento_id);


-- public.itinerancia_evento definition

-- Drop table

-- DROP TABLE public.itinerancia_evento;

CREATE TABLE public.itinerancia_evento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	itinerancia_id int8 NOT NULL,
	evento_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT itinerancia_evento_pk PRIMARY KEY (id),
	CONSTRAINT itinerancia_evento_evento_fk FOREIGN KEY (evento_id) REFERENCES evento(id),
	CONSTRAINT itinerancia_evento_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id)
);
CREATE INDEX itinerancia_evento_evento_idx ON public.itinerancia_evento USING btree (evento_id);
CREATE INDEX itinerancia_evento_itinerancia_idx ON public.itinerancia_evento USING btree (itinerancia_id);


-- public.itinerancia_objetivo definition

-- Drop table

-- DROP TABLE public.itinerancia_objetivo;

CREATE TABLE public.itinerancia_objetivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	itinerancia_base_id int8 NOT NULL,
	itinerancia_id int8 NOT NULL,
	descricao varchar NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT itinerancia_objetivo_pk PRIMARY KEY (id),
	CONSTRAINT itinerancia_objetivo_base_fk FOREIGN KEY (itinerancia_base_id) REFERENCES itinerancia_objetivo_base(id),
	CONSTRAINT itinerancia_objetivo_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id)
);


-- public.matriz_saber_plano definition

-- Drop table

-- DROP TABLE public.matriz_saber_plano;

CREATE TABLE public.matriz_saber_plano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_id int8 NOT NULL,
	matriz_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT matriz_saber_plano_pk PRIMARY KEY (id),
	CONSTRAINT matriz_saber_plano_un UNIQUE (plano_id, matriz_id),
	CONSTRAINT matriz_id_fk FOREIGN KEY (matriz_id) REFERENCES matriz_saber(id)
);


-- public.notas_conceito definition

-- Drop table

-- DROP TABLE public.notas_conceito;

CREATE TABLE public.notas_conceito (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	atividade_avaliativa int8 NOT NULL,
	aluno_id varchar(20) NOT NULL,
	nota numeric(5,2) NULL,
	conceito int8 NULL,
	tipo_nota int8 NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	disciplina_id varchar(15) NOT NULL,
	CONSTRAINT notas_conceito_pk PRIMARY KEY (id),
	CONSTRAINT notas_conceito_tipo_nota_id_fk FOREIGN KEY (tipo_nota) REFERENCES notas_tipo_valor(id) ON DELETE CASCADE
);
CREATE INDEX notas_conceito_aluno_idx ON public.notas_conceito USING btree (aluno_id);
CREATE INDEX notas_conceito_avaliacao_idx ON public.notas_conceito USING btree (atividade_avaliativa);
CREATE INDEX notas_conceito_tipo_nota_idx ON public.notas_conceito USING btree (tipo_nota);
CREATE INDEX notas_disciplina_id_idx ON public.notas_conceito USING btree (disciplina_id);


-- public.notas_conceitos_ciclos_parametos definition

-- Drop table

-- DROP TABLE public.notas_conceitos_ciclos_parametos;

CREATE TABLE public.notas_conceitos_ciclos_parametos (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ciclo int4 NOT NULL,
	tipo_nota int8 NOT NULL,
	qtd_minima_avaliacao int4 NOT NULL,
	percentual_alerta int4 NOT NULL,
	ativo bool NOT NULL DEFAULT true,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT notas_conceitos_ciclos_parametos_pk PRIMARY KEY (id),
	CONSTRAINT notas_conceitos_ciclos_parametos_ciclo_id_fk FOREIGN KEY (ciclo) REFERENCES tipo_ciclo(id) ON DELETE CASCADE,
	CONSTRAINT notas_conceitos_ciclos_parametos_tipo_nota_id_fk FOREIGN KEY (tipo_nota) REFERENCES notas_tipo_valor(id) ON DELETE CASCADE
);
CREATE INDEX notas_conceitos_ciclos_parametos_ciclo_idx ON public.notas_conceitos_ciclos_parametos USING btree (ciclo);


-- public.notificacao definition

-- Drop table

-- DROP TABLE public.notificacao;

CREATE TABLE public.notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	titulo varchar(200) NOT NULL,
	mensagem varchar NOT NULL,
	status int4 NOT NULL,
	categoria int4 NOT NULL,
	tipo int4 NOT NULL,
	ue_id varchar(15) NULL,
	dre_id varchar(15) NULL,
	ano int4 NULL,
	codigo int8 NOT NULL,
	turma_id varchar(15) NULL,
	excluida bool NOT NULL,
	usuario_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT notificacao_pk PRIMARY KEY (id),
	CONSTRAINT notificacao_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id),
	CONSTRAINT usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);
CREATE INDEX evento__ue_idx ON public.notificacao USING btree (ue_id);
CREATE INDEX ix_usuario_id ON public.notificacao USING btree (usuario_id);
CREATE INDEX notificacao_ano_idx ON public.notificacao USING btree (ano);
CREATE INDEX notificacao_codigo_year_ix ON public.notificacao USING btree (codigo DESC, date_part('year'::text, criado_em));
CREATE INDEX notificacao_dre_idx ON public.notificacao USING btree (dre_id);
CREATE INDEX notificacao_titulo_idx ON public.notificacao USING btree (lower(f_unaccent((titulo)::text)) text_pattern_ops);
CREATE INDEX notificacao_turma_idx ON public.notificacao USING btree (turma_id);
CREATE INDEX notificacao_ue_idx ON public.notificacao USING btree (ue_id);


-- public.notificacao_aula definition

-- Drop table

-- DROP TABLE public.notificacao_aula;

CREATE TABLE public.notificacao_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	notificacao_id int8 NOT NULL,
	aula_id int8 NOT NULL,
	CONSTRAINT notificacao_aula_pk PRIMARY KEY (id),
	CONSTRAINT notificacao_aula_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id),
	CONSTRAINT notificacao_aula_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id)
);
CREATE INDEX notificacao_aula_aula_idx ON public.notificacao_aula USING btree (aula_id);
CREATE INDEX notificacao_aula_notificacao_idx ON public.notificacao_aula USING btree (notificacao_id);


-- public.notificacao_frequencia definition

-- Drop table

-- DROP TABLE public.notificacao_frequencia;

CREATE TABLE public.notificacao_frequencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo int4 NOT NULL,
	notificacao_codigo int8 NOT NULL,
	disciplina_codigo varchar(15) NOT NULL,
	aula_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT notificacao_frequencia_pk PRIMARY KEY (id),
	CONSTRAINT notificacao_frequencia_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id)
);
CREATE INDEX notificacao_frequencia_aula_id_idx ON public.notificacao_frequencia USING btree (aula_id);
CREATE INDEX notificacao_frequencia_disciplina_idx ON public.notificacao_frequencia USING btree (disciplina_codigo);
CREATE INDEX notificacao_frequencia_notificacao_idx ON public.notificacao_frequencia USING btree (notificacao_codigo);
CREATE INDEX notificacao_frequencia_tipo_idx ON public.notificacao_frequencia USING btree (tipo);


-- public.objetivo_aprendizagem_plano definition

-- Drop table

-- DROP TABLE public.objetivo_aprendizagem_plano;

CREATE TABLE public.objetivo_aprendizagem_plano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_id int8 NOT NULL,
	objetivo_aprendizagem_jurema_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_aprendizagem_plano_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_aprendizagem_plano_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular_jurema(id),
	CONSTRAINT plano_id_fk FOREIGN KEY (plano_id) REFERENCES plano_anual(id)
);
CREATE INDEX objetivo_aprendizagem_plano_plano_id_ix ON public.objetivo_aprendizagem_plano USING btree (plano_id);


-- public.pendencia_aula definition

-- Drop table

-- DROP TABLE public.pendencia_aula;

CREATE TABLE public.pendencia_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	pendencia_id int8 NOT NULL,
	motivo varchar(100) NULL,
	CONSTRAINT pendencia_aula_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_aula_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id)
);
CREATE INDEX pendencia_aula_aula_idx ON public.pendencia_aula USING btree (aula_id);
CREATE INDEX pendencia_aula_pendenci_idx ON public.pendencia_aula USING btree (pendencia_id);


-- public.pendencia_usuario definition

-- Drop table

-- DROP TABLE public.pendencia_usuario;

CREATE TABLE public.pendencia_usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	pendencia_id int8 NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	CONSTRAINT pendencia_usuario_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_usuario_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id),
	CONSTRAINT pendencia_usuario_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);
CREATE INDEX pendencia_usuario_pendencia_idx ON public.pendencia_usuario USING btree (pendencia_id);
CREATE INDEX pendencia_usuario_usuario_idx ON public.pendencia_usuario USING btree (usuario_id);


-- public.perfil_evento_tipo definition

-- Drop table

-- DROP TABLE public.perfil_evento_tipo;

CREATE TABLE public.perfil_evento_tipo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_perfil uuid NOT NULL,
	evento_tipo_id int8 NULL,
	excluido bool NOT NULL DEFAULT false,
	exclusivo bool NOT NULL DEFAULT true,
	permite_cadastro bool NULL DEFAULT false,
	CONSTRAINT perfil_evento_tipo_pk PRIMARY KEY (id),
	CONSTRAINT perfil_evento_tipo_tipo_fk FOREIGN KEY (evento_tipo_id) REFERENCES evento_tipo(id)
);
CREATE INDEX perfil_evento_tipo_perfil_idx ON public.perfil_evento_tipo USING btree (codigo_perfil);
CREATE INDEX perfil_evento_tipo_tipo_idx ON public.perfil_evento_tipo USING btree (evento_tipo_id);


-- public.plano_aula definition

-- Drop table

-- DROP TABLE public.plano_aula;

CREATE TABLE public.plano_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	descricao varchar NULL,
	desenvolvimento_aula varchar NOT NULL,
	recuperacao_aula varchar NULL,
	licao_casa varchar NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT plano_aula_pk PRIMARY KEY (id),
	CONSTRAINT plano_aula_aula_id_fk FOREIGN KEY (aula_id) REFERENCES aula(id) ON DELETE CASCADE
);
CREATE INDEX plano_aula_aula_idx ON public.plano_aula USING btree (aula_id);


-- public.questao definition

-- Drop table

-- DROP TABLE public.questao;

CREATE TABLE public.questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questionario_id int8 NOT NULL,
	ordem int4 NOT NULL,
	nome varchar(200) NOT NULL,
	observacao varchar(200) NULL,
	obrigatorio bool NOT NULL DEFAULT false,
	tipo int4 NOT NULL,
	opcionais varchar(100) NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	somente_leitura bool NOT NULL DEFAULT false,
	CONSTRAINT questao_pk PRIMARY KEY (id),
	CONSTRAINT questao_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id)
);
CREATE INDEX questao_questionario_idx ON public.questao USING btree (questionario_id);


-- public.recuperacao_paralela_eixo definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_eixo;

CREATE TABLE public.recuperacao_paralela_eixo (
	id int8 NOT NULL,
	descricao varchar(200) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	dt_inicio timestamp NOT NULL,
	dt_fim timestamp NULL,
	recuperacao_paralela_periodo_id int8 NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT eixo_pk PRIMARY KEY (id),
	CONSTRAINT eixo_recuperacao_paralela_periodo_fk FOREIGN KEY (recuperacao_paralela_periodo_id) REFERENCES recuperacao_paralela_periodo(id)
);


-- public.recuperacao_paralela_objetivo definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_objetivo;

CREATE TABLE public.recuperacao_paralela_objetivo (
	id int8 NOT NULL,
	eixo_id int8 NOT NULL,
	nome varchar(200) NOT NULL,
	descricao varchar(600) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	ordem int8 NOT NULL,
	ehespecifico bool NOT NULL DEFAULT false,
	dt_inicio timestamp NOT NULL,
	dt_fim timestamp NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	pagina int4 NULL,
	CONSTRAINT objetivo_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_eixo_fk FOREIGN KEY (eixo_id) REFERENCES recuperacao_paralela_eixo(id)
);


-- public.recuperacao_paralela_objetivo_desenvolvimento_plano definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_objetivo_desenvolvimento_plano;

CREATE TABLE public.recuperacao_paralela_objetivo_desenvolvimento_plano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_id int8 NOT NULL,
	objetivo_desenvolvimento_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_desenvolvimento_plano_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_desenvolvimento_un UNIQUE (plano_id, objetivo_desenvolvimento_id),
	CONSTRAINT objetivo_desenvolvimento_id_fk FOREIGN KEY (objetivo_desenvolvimento_id) REFERENCES recuperacao_paralela_objetivo_desenvolvimento(id)
);


-- public.recuperacao_paralela_objetivo_resposta definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_objetivo_resposta;

CREATE TABLE public.recuperacao_paralela_objetivo_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	objetivo_id int8 NOT NULL,
	resposta_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_resposta_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES recuperacao_paralela_resposta(id)
);


-- public.recuperacao_paralela_periodo_objetivo_resposta definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela_periodo_objetivo_resposta;

CREATE TABLE public.recuperacao_paralela_periodo_objetivo_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	recuperacao_paralela_id int8 NOT NULL,
	objetivo_id int8 NOT NULL,
	resposta_id int8 NOT NULL,
	periodo_recuperacao_paralela_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_pk PRIMARY KEY (id),
	CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_objetivo_fk FOREIGN KEY (objetivo_id) REFERENCES recuperacao_paralela_objetivo(id),
	CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_periodo_fk FOREIGN KEY (periodo_recuperacao_paralela_id) REFERENCES recuperacao_paralela_periodo(id),
	CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES recuperacao_paralela_resposta(id)
);


-- public.registro_frequencia definition

-- Drop table

-- DROP TABLE public.registro_frequencia;

CREATE TABLE public.registro_frequencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT registro_frequencia_pk PRIMARY KEY (id),
	CONSTRAINT registro_frequencia_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id)
);
CREATE INDEX registro_frequencia_aula_id_idx ON public.registro_frequencia USING btree (aula_id);


-- public.relatorio_correlacao definition

-- Drop table

-- DROP TABLE public.relatorio_correlacao;

CREATE TABLE public.relatorio_correlacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo uuid NOT NULL,
	tipo_relatorio int4 NOT NULL,
	usuario_solicitante_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	tipo_formato int4 NOT NULL,
	CONSTRAINT relatorio_correlacao_pk PRIMARY KEY (id),
	CONSTRAINT relatorio_correlacao_usuario_fk FOREIGN KEY (usuario_solicitante_id) REFERENCES usuario(id)
);
CREATE INDEX codigo_correlacao_idx ON public.relatorio_correlacao USING btree (codigo);


-- public.relatorio_correlacao_jasper definition

-- Drop table

-- DROP TABLE public.relatorio_correlacao_jasper;

CREATE TABLE public.relatorio_correlacao_jasper (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	jsession_id varchar(200) NOT NULL,
	request_id uuid NOT NULL,
	export_id uuid NOT NULL,
	relatorio_correlacao_id int8 NOT NULL,
	CONSTRAINT relatorio_correlacao_jasper_pk PRIMARY KEY (id),
	CONSTRAINT relatorio_correlacao_fk FOREIGN KEY (relatorio_correlacao_id) REFERENCES relatorio_correlacao(id)
);


-- public.secao_encaminhamento_aee definition

-- Drop table

-- DROP TABLE public.secao_encaminhamento_aee;

CREATE TABLE public.secao_encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questionario_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	nome varchar NOT NULL,
	etapa int4 NOT NULL,
	ordem int4 NOT NULL,
	CONSTRAINT secao_encaminhamento_aee_pk PRIMARY KEY (id),
	CONSTRAINT secao_encaminhamento_aee_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id)
);
CREATE INDEX secao_encaminhamento_aee_questionario_idx ON public.secao_encaminhamento_aee USING btree (questionario_id);


-- public.tipo_ciclo_ano definition

-- Drop table

-- DROP TABLE public.tipo_ciclo_ano;

CREATE TABLE public.tipo_ciclo_ano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_ciclo_id int8 NOT NULL,
	modalidade int8 NOT NULL,
	ano varchar(10) NOT NULL,
	CONSTRAINT tipo_ciclo_ano_pk PRIMARY KEY (id),
	CONSTRAINT tipo_ciclo_ano_un UNIQUE (tipo_ciclo_id, ano),
	CONSTRAINT tipo_ciclo_id_fk FOREIGN KEY (tipo_ciclo_id) REFERENCES tipo_ciclo(id)
);


-- public.ue definition

-- Drop table

-- DROP TABLE public.ue;

CREATE TABLE public.ue (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id varchar(15) NULL,
	dre_id int8 NULL,
	nome varchar(200) NULL,
	tipo_escola int2 NULL,
	data_atualizacao date NOT NULL,
	CONSTRAINT ue_pk PRIMARY KEY (id),
	CONSTRAINT ue_dre_id_fk FOREIGN KEY (dre_id) REFERENCES dre(id) ON DELETE CASCADE
);
CREATE INDEX ue_dre_idx ON public.ue USING btree (dre_id);
CREATE INDEX ue_ue_id_idx ON public.ue USING btree (ue_id);
CREATE INDEX ue_ue_idx ON public.ue USING btree (ue_id);


-- public.wf_aprovacao_itinerancia definition

-- Drop table

-- DROP TABLE public.wf_aprovacao_itinerancia;

CREATE TABLE public.wf_aprovacao_itinerancia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_id int8 NOT NULL,
	itinerancia_id int8 NOT NULL,
	status_aprovacao bool NULL,
	CONSTRAINT wf_aprovacao_itinerancia_pk PRIMARY KEY (id),
	CONSTRAINT wf_aprovacao_itinerancia_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id),
	CONSTRAINT wf_aprovacao_itinerancia_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id)
);
CREATE INDEX wf_aprovacao_itinerancia_idx ON public.wf_aprovacao_itinerancia USING btree (wf_aprovacao_id);
CREATE INDEX wf_aprovacao_itinerancia_itinerancia_idx ON public.wf_aprovacao_itinerancia USING btree (itinerancia_id);


-- public.wf_aprovacao_nivel definition

-- Drop table

-- DROP TABLE public.wf_aprovacao_nivel;

CREATE TABLE public.wf_aprovacao_nivel (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	status int4 NOT NULL,
	cargo int4 NULL,
	nivel int4 NOT NULL,
	wf_aprovacao_id int8 NOT NULL,
	observacao varchar(100) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT wf_aprova_nivel_nivel_pk PRIMARY KEY (id),
	CONSTRAINT wf_aprovacao_nivel_wf_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id)
);


-- public.wf_aprovacao_nivel_notificacao definition

-- Drop table

-- DROP TABLE public.wf_aprovacao_nivel_notificacao;

CREATE TABLE public.wf_aprovacao_nivel_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_nivel_id int8 NOT NULL,
	notificacao_id int8 NOT NULL,
	CONSTRAINT wf_aprovacao_nivel_mensagem_pk PRIMARY KEY (wf_aprovacao_nivel_id, notificacao_id),
	CONSTRAINT wf_aprovacao_nivel_mensagem_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id),
	CONSTRAINT wf_aprovacao_nivel_notificacao_wf_fk FOREIGN KEY (wf_aprovacao_nivel_id) REFERENCES wf_aprovacao_nivel(id)
);


-- public.wf_aprovacao_nivel_usuario definition

-- Drop table

-- DROP TABLE public.wf_aprovacao_nivel_usuario;

CREATE TABLE public.wf_aprovacao_nivel_usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_nivel_id int8 NOT NULL,
	usuario_id int8 NOT NULL,
	CONSTRAINT wf_aprovacao_nivel_usuario_pk PRIMARY KEY (wf_aprovacao_nivel_id, usuario_id),
	CONSTRAINT wf_aprovacao_nivel_mensagem_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id),
	CONSTRAINT wf_aprovacao_nivel_usuario_wf_fk FOREIGN KEY (wf_aprovacao_nivel_id) REFERENCES wf_aprovacao_nivel(id)
);


-- public.anotacao_frequencia_aluno definition

-- Drop table

-- DROP TABLE public.anotacao_frequencia_aluno;

CREATE TABLE public.anotacao_frequencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	motivo_ausencia_id int8 NULL,
	anotacao varchar NULL,
	aula_id int8 NOT NULL,
	codigo_aluno varchar(15) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT anotacao_frequencia_aluno_pk PRIMARY KEY (id),
	CONSTRAINT anotacao_frequencia_aluno_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id),
	CONSTRAINT anotacao_frequencia_aluno_motivo_ausencia_fk FOREIGN KEY (motivo_ausencia_id) REFERENCES motivo_ausencia(id)
);
CREATE INDEX anotacao_frequencia_aluno_aluno_idx ON public.anotacao_frequencia_aluno USING btree (codigo_aluno);
CREATE INDEX anotacao_frequencia_aluno_aula_idx ON public.anotacao_frequencia_aluno USING btree (aula_id);


-- public.devolutiva_diario_bordo_notificacao definition

-- Drop table

-- DROP TABLE public.devolutiva_diario_bordo_notificacao;

CREATE TABLE public.devolutiva_diario_bordo_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	devolutiva_id int8 NOT NULL,
	notificacao_id int8 NOT NULL,
	CONSTRAINT devolutiva_diario_bordo_notificacao_diario_bordo_devolutiva_fk FOREIGN KEY (devolutiva_id) REFERENCES devolutiva(id),
	CONSTRAINT devolutiva_diario_bordo_notificacao_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id)
);
CREATE INDEX devolutiva_diario_bordo_notificacao_devolutiva_idx ON public.devolutiva_diario_bordo_notificacao USING btree (devolutiva_id);
CREATE INDEX devolutiva_diario_bordo_notificacao_notificacao_idx ON public.devolutiva_diario_bordo_notificacao USING btree (notificacao_id);


-- public.diario_bordo_observacao_notificacao definition

-- Drop table

-- DROP TABLE public.diario_bordo_observacao_notificacao;

CREATE TABLE public.diario_bordo_observacao_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	observacao_id int8 NOT NULL,
	notificacao_id int8 NOT NULL,
	CONSTRAINT diario_bordo_observacao_notificacao_pk PRIMARY KEY (id),
	CONSTRAINT diario_bordo_observacao_notificacao_diario_bordo_observacao_fk FOREIGN KEY (observacao_id) REFERENCES diario_bordo_observacao(id),
	CONSTRAINT diario_bordo_observacao_notificacao_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id)
);
CREATE INDEX diario_bordo_observacao_notificacao_diario_bordo_observacao_idx ON public.diario_bordo_observacao_notificacao USING btree (observacao_id);
CREATE INDEX diario_bordo_observacao_notificacao_notificacao_idx ON public.diario_bordo_observacao_notificacao USING btree (notificacao_id);


-- public.documento definition

-- Drop table

-- DROP TABLE public.documento;

CREATE TABLE public.documento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	arquivo_id int8 NULL,
	ue_id int8 NOT NULL,
	ano_letivo int8 NULL,
	classificacao_documento_id int8 NOT NULL,
	CONSTRAINT documento_pk PRIMARY KEY (id),
	CONSTRAINT documento_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id),
	CONSTRAINT documento_classificacao_documento_fk FOREIGN KEY (classificacao_documento_id) REFERENCES classificacao_documento(id),
	CONSTRAINT documento_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id),
	CONSTRAINT documento_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);
CREATE INDEX documento_arquivo_idx ON public.documento USING btree (arquivo_id);
CREATE INDEX documento_classificacao_documento_idx ON public.documento USING btree (classificacao_documento_id);
CREATE INDEX documento_ue_idx ON public.documento USING btree (ue_id);
CREATE INDEX documento_usuario_idx ON public.documento USING btree (usuario_id);


-- public.fechamento_reabertura definition

-- Drop table

-- DROP TABLE public.fechamento_reabertura;

CREATE TABLE public.fechamento_reabertura (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar NOT NULL,
	inicio timestamp NOT NULL,
	fim timestamp NOT NULL,
	tipo_calendario_id int8 NOT NULL,
	dre_id int8 NULL,
	ue_id int8 NULL,
	wf_aprovacao_id int8 NULL,
	status int4 NOT NULL,
	migrado bool NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT fechamento_reabertura_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_reabertura_dre_fk FOREIGN KEY (dre_id) REFERENCES dre(id),
	CONSTRAINT fechamento_reabertura_tipo_calendario_fk FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id),
	CONSTRAINT fechamento_reabertura_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id),
	CONSTRAINT fechamento_reabertura_wf_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id)
);
CREATE INDEX fechamento_reabertura_dre_idx ON public.fechamento_reabertura USING btree (dre_id);
CREATE INDEX fechamento_reabertura_fim_idx ON public.fechamento_reabertura USING btree (fim);
CREATE INDEX fechamento_reabertura_inicio_idx ON public.fechamento_reabertura USING btree (inicio);
CREATE INDEX fechamento_reabertura_tipo_calendario_idx ON public.fechamento_reabertura USING btree (tipo_calendario_id);


-- public.fechamento_reabertura_bimestre definition

-- Drop table

-- DROP TABLE public.fechamento_reabertura_bimestre;

CREATE TABLE public.fechamento_reabertura_bimestre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_reabertura_id int8 NOT NULL,
	bimestre int4 NOT NULL,
	criado_em timestamp NOT NULL DEFAULT now(),
	criado_por varchar(200) NOT NULL DEFAULT ''::character varying,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL DEFAULT ''::character varying,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_reabertura_bimestre_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_reabertura_bimestre_fechamento_reabertura_fk FOREIGN KEY (fechamento_reabertura_id) REFERENCES fechamento_reabertura(id)
);
CREATE INDEX fechamento_reabertura_bimestre_fech_reab_bi_idx ON public.fechamento_reabertura_bimestre USING btree (fechamento_reabertura_id);


-- public.fechamento_reabertura_notificacao definition

-- Drop table

-- DROP TABLE public.fechamento_reabertura_notificacao;

CREATE TABLE public.fechamento_reabertura_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_reabertura_id int8 NOT NULL,
	notificacao_id int8 NOT NULL,
	CONSTRAINT fechamento_reabertura_notificacao_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_reabertura_notificacao_fechamento_fk FOREIGN KEY (fechamento_reabertura_id) REFERENCES fechamento_reabertura(id),
	CONSTRAINT fechamento_reabertura_notificacao_noticacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id)
);
CREATE INDEX fechamento_reabertura_notificacao_fechamento_idx ON public.fechamento_reabertura_notificacao USING btree (fechamento_reabertura_id);
CREATE INDEX fechamento_reabertura_notificacao_noticacao_idx ON public.fechamento_reabertura_notificacao USING btree (notificacao_id);


-- public.itinerancia_questao definition

-- Drop table

-- DROP TABLE public.itinerancia_questao;

CREATE TABLE public.itinerancia_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_id int8 NOT NULL,
	itinerancia_id int8 NOT NULL,
	resposta varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT itinerancia_questao_pk PRIMARY KEY (id),
	CONSTRAINT itinerancia_aluno_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id),
	CONSTRAINT itinerancia_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id),
	CONSTRAINT itinerancia_questao_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id)
);


-- public.itinerancia_ue definition

-- Drop table

-- DROP TABLE public.itinerancia_ue;

CREATE TABLE public.itinerancia_ue (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id int8 NOT NULL,
	itinerancia_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT itinerancia_ue_pk PRIMARY KEY (id),
	CONSTRAINT itinerancia_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id),
	CONSTRAINT itinerancia_ue_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id)
);


-- public.objetivo_aprendizagem_aula definition

-- Drop table

-- DROP TABLE public.objetivo_aprendizagem_aula;

CREATE TABLE public.objetivo_aprendizagem_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aula_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	objetivo_aprendizagem_id int8 NULL,
	componente_curricular_id int8 NULL,
	CONSTRAINT objetivo_aprendizagem_aula_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_aprendizagem_aula_objetivo_aprendizagem_fk FOREIGN KEY (objetivo_aprendizagem_id) REFERENCES objetivo_aprendizagem(id),
	CONSTRAINT objetivo_aprendizagem_aula_plano_id_fk FOREIGN KEY (plano_aula_id) REFERENCES plano_aula(id) ON DELETE CASCADE
);
CREATE INDEX objetivo_aprendizagem_aula_plano_idx ON public.objetivo_aprendizagem_aula USING btree (plano_aula_id);


-- public.opcao_resposta definition

-- Drop table

-- DROP TABLE public.opcao_resposta;

CREATE TABLE public.opcao_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_id int8 NOT NULL,
	ordem int4 NOT NULL,
	nome varchar(200) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	observacao varchar NULL,
	CONSTRAINT opcao_resposta_pk PRIMARY KEY (id),
	CONSTRAINT opcao_resposta_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id)
);
CREATE INDEX opcao_resposta_questao_idx ON public.opcao_resposta USING btree (questao_id);


-- public.pendencia_calendario_ue definition

-- Drop table

-- DROP TABLE public.pendencia_calendario_ue;

CREATE TABLE public.pendencia_calendario_ue (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id int8 NOT NULL,
	tipo_calendario_id int8 NOT NULL,
	pendencia_id int8 NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	CONSTRAINT pendencia_calendario_ue_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_calendario_ue_calendario_fk FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id),
	CONSTRAINT pendencia_calendario_ue_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id),
	CONSTRAINT pendencia_calendario_ue_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id)
);
CREATE INDEX pendencia_calendario_ue_calendario_idx ON public.pendencia_calendario_ue USING btree (tipo_calendario_id);
CREATE INDEX pendencia_calendario_ue_pendencia_idx ON public.pendencia_calendario_ue USING btree (pendencia_id);
CREATE INDEX pendencia_calendario_ue_ue_idx ON public.pendencia_calendario_ue USING btree (ue_id);


-- public.pendencia_parametro_evento definition

-- Drop table

-- DROP TABLE public.pendencia_parametro_evento;

CREATE TABLE public.pendencia_parametro_evento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	parametro_sistema_id int8 NOT NULL,
	pendencia_calendario_ue_id int8 NOT NULL,
	quantidade_eventos int4 NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	CONSTRAINT pendencia_parametro_evento_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_parametro_evento_calendario_ue_fk FOREIGN KEY (pendencia_calendario_ue_id) REFERENCES pendencia_calendario_ue(id),
	CONSTRAINT pendencia_parametro_evento_parametro_fk FOREIGN KEY (parametro_sistema_id) REFERENCES parametros_sistema(id)
);
CREATE INDEX pendencia_parametro_evento_calendario_ue_idx ON public.pendencia_parametro_evento USING btree (pendencia_calendario_ue_id);
CREATE INDEX pendencia_parametro_evento_parametro_idx ON public.pendencia_parametro_evento USING btree (parametro_sistema_id);


-- public.periodo_fechamento definition

-- Drop table

-- DROP TABLE public.periodo_fechamento;

CREATE TABLE public.periodo_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id int8 NULL,
	dre_id int8 NULL,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_dre_fk FOREIGN KEY (dre_id) REFERENCES dre(id),
	CONSTRAINT fechamento_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id)
);
CREATE INDEX fechamento_dre_idx ON public.periodo_fechamento USING btree (dre_id);
CREATE INDEX fechamento_ue_idx ON public.periodo_fechamento USING btree (ue_id);


-- public.periodo_fechamento_bimestre definition

-- Drop table

-- DROP TABLE public.periodo_fechamento_bimestre;

CREATE TABLE public.periodo_fechamento_bimestre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	periodo_escolar_id int8 NOT NULL,
	periodo_fechamento_id int8 NOT NULL,
	inicio_fechamento date NOT NULL,
	final_fechamento date NOT NULL,
	CONSTRAINT fechamento_bimestre_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_fk FOREIGN KEY (periodo_fechamento_id) REFERENCES periodo_fechamento(id),
	CONSTRAINT periodo_escolar_fechamento_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id)
);


-- public.registro_ausencia_aluno definition

-- Drop table

-- DROP TABLE public.registro_ausencia_aluno;

CREATE TABLE public.registro_ausencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno varchar(7) NOT NULL,
	numero_aula int4 NOT NULL,
	registro_frequencia_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT registro_ausencia_aluno_pk PRIMARY KEY (id),
	CONSTRAINT registro_ausencia_aluno_fk FOREIGN KEY (registro_frequencia_id) REFERENCES registro_frequencia(id)
);
CREATE INDEX registro_ausencia_aluno_excluido_idx ON public.registro_ausencia_aluno USING btree (excluido);
CREATE INDEX registro_ausencia_aluno_registro_frequencia_id_idx ON public.registro_ausencia_aluno USING btree (registro_frequencia_id);


-- public.turma definition

-- Drop table

-- DROP TABLE public.turma;

CREATE TABLE public.turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id varchar(15) NULL,
	ue_id int8 NULL,
	nome varchar(20) NULL,
	ano bpchar(1) NULL,
	ano_letivo int4 NULL,
	modalidade_codigo int4 NULL,
	semestre int4 NULL,
	qt_duracao_aula int2 NULL,
	tipo_turno int2 NULL,
	data_atualizacao date NOT NULL,
	historica bool NOT NULL DEFAULT false,
	dt_fim_eol date NULL,
	ensino_especial bool NULL DEFAULT false,
	etapa_eja int4 NULL,
	data_inicio date NULL,
	serie_ensino varchar(40) NULL,
	tipo_turma int4 NOT NULL DEFAULT 0,
	CONSTRAINT turma_pk PRIMARY KEY (id),
	CONSTRAINT turma_ue_id_fk FOREIGN KEY (ue_id) REFERENCES ue(id) ON DELETE CASCADE
);
CREATE INDEX turma_historica_idx ON public.turma USING btree (historica, dt_fim_eol);
CREATE INDEX turma_modalidade_idx ON public.turma USING btree (modalidade_codigo);
CREATE INDEX turma_turma_id_idx ON public.turma USING btree (turma_id);
CREATE INDEX turma_ue_idx ON public.turma USING btree (ue_id);


-- public.acompanhamento_aluno definition

-- Drop table

-- DROP TABLE public.acompanhamento_aluno;

CREATE TABLE public.acompanhamento_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT acompanhamento_aluno_pk PRIMARY KEY (id),
	CONSTRAINT acompanhamento_aluno_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX acompanhamento_aluno_aluno_idx ON public.acompanhamento_aluno USING btree (aluno_codigo);
CREATE INDEX acompanhamento_aluno_turma_idx ON public.acompanhamento_aluno USING btree (turma_id);


-- public.acompanhamento_aluno_semestre definition

-- Drop table

-- DROP TABLE public.acompanhamento_aluno_semestre;

CREATE TABLE public.acompanhamento_aluno_semestre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	acompanhamento_aluno_id int8 NOT NULL,
	semestre int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	observacoes varchar NULL,
	CONSTRAINT acompanhamento_aluno_semestre_pk PRIMARY KEY (id),
	CONSTRAINT acompanhamento_aluno_semestre_acompanhamento_fk FOREIGN KEY (acompanhamento_aluno_id) REFERENCES acompanhamento_aluno(id)
);
CREATE INDEX acompanhamento_aluno_semestre_acompanhamento_idx ON public.acompanhamento_aluno_semestre USING btree (acompanhamento_aluno_id);


-- public.acompanhamento_turma definition

-- Drop table

-- DROP TABLE public.acompanhamento_turma;

CREATE TABLE public.acompanhamento_turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	semestre int4 NOT NULL,
	apanhado_geral varchar NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT acompanhamento_turma_pk PRIMARY KEY (id),
	CONSTRAINT acompanhamento_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX acompanhamento_turma_idx ON public.acompanhamento_turma USING btree (turma_id);


-- public.carta_intencoes definition

-- Drop table

-- DROP TABLE public.carta_intencoes;

CREATE TABLE public.carta_intencoes (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	periodo_escolar_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	planejamento varchar NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT carta_intencoes_pk PRIMARY KEY (id),
	CONSTRAINT periodo_escolar_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id),
	CONSTRAINT turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX carta_intencoes_observacoes_turma_idx ON public.carta_intencoes USING btree (turma_id);
CREATE INDEX carta_intencoes_periodo_escolar_idx ON public.carta_intencoes USING btree (periodo_escolar_id);
CREATE INDEX carta_intencoes_turma_idx ON public.carta_intencoes USING btree (turma_id);


-- public.carta_intencoes_observacao definition

-- Drop table

-- DROP TABLE public.carta_intencoes_observacao;

CREATE TABLE public.carta_intencoes_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	observacao varchar NOT NULL,
	turma_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	usuario_id int8 NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(10) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(10) NULL,
	CONSTRAINT carta_intencoes_observacao_pk PRIMARY KEY (id),
	CONSTRAINT carta_intencoes_observacao_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX carta_intencoes_observacao_componente_curricular_idx ON public.carta_intencoes_observacao USING btree (componente_curricular_id);
CREATE INDEX carta_intencoes_observacao_turma_idx ON public.carta_intencoes_observacao USING btree (turma_id);


-- public.carta_intencoes_observacao_notificacao definition

-- Drop table

-- DROP TABLE public.carta_intencoes_observacao_notificacao;

CREATE TABLE public.carta_intencoes_observacao_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	notificacao_id int8 NOT NULL,
	observacao_id int8 NOT NULL,
	CONSTRAINT carta_intencoes_observacao_notificacao_pk PRIMARY KEY (id),
	CONSTRAINT carta_intencoes_observacao_notificacao_carta_intencoes_observac FOREIGN KEY (observacao_id) REFERENCES carta_intencoes_observacao(id),
	CONSTRAINT carta_intencoes_observacao_notificacao_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id)
);
CREATE INDEX carta_intencoes_observacao_notificacao_carta_intencoes_observac ON public.carta_intencoes_observacao_notificacao USING btree (observacao_id);
CREATE INDEX carta_intencoes_observacao_notificacao_notificacao_idx ON public.carta_intencoes_observacao_notificacao USING btree (notificacao_id);


-- public.compensacao_ausencia definition

-- Drop table

-- DROP TABLE public.compensacao_ausencia;

CREATE TABLE public.compensacao_ausencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	bimestre int4 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	turma_id int8 NOT NULL,
	nome varchar NOT NULL,
	descricao varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	ano_letivo int4 NOT NULL DEFAULT 0,
	CONSTRAINT compensacao_ausencia_pk PRIMARY KEY (id),
	CONSTRAINT compensacao_ausencia_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX compensacao_ausencia_ano_letivo_idx ON public.compensacao_ausencia USING btree (ano_letivo);
CREATE INDEX compensacao_ausencia_disciplina_idx ON public.compensacao_ausencia USING btree (disciplina_id);
CREATE INDEX compensacao_ausencia_turma_idx ON public.compensacao_ausencia USING btree (turma_id);


-- public.compensacao_ausencia_aluno definition

-- Drop table

-- DROP TABLE public.compensacao_ausencia_aluno;

CREATE TABLE public.compensacao_ausencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	compensacao_ausencia_id int8 NOT NULL,
	codigo_aluno varchar(100) NOT NULL,
	qtd_faltas_compensadas int4 NOT NULL,
	notificado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT compensacao_ausencia_aluno_pk PRIMARY KEY (id),
	CONSTRAINT compensacao_ausencia_aluno_compensacao_fk FOREIGN KEY (compensacao_ausencia_id) REFERENCES compensacao_ausencia(id)
);
CREATE INDEX compensacao_ausencia_aluno_codigo_aluno_idx ON public.compensacao_ausencia_aluno USING btree (codigo_aluno);
CREATE INDEX compensacao_ausencia_aluno_compensacao_ausencia_idx ON public.compensacao_ausencia_aluno USING btree (compensacao_ausencia_id);


-- public.compensacao_ausencia_disciplina_regencia definition

-- Drop table

-- DROP TABLE public.compensacao_ausencia_disciplina_regencia;

CREATE TABLE public.compensacao_ausencia_disciplina_regencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	compensacao_ausencia_id int8 NOT NULL,
	disciplina_id varchar(100) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT compensacao_ausencia_disciplina_regencia_pk PRIMARY KEY (id),
	CONSTRAINT compensacao_ausencia_disciplina_regencia_compensacao_fk FOREIGN KEY (compensacao_ausencia_id) REFERENCES compensacao_ausencia(id)
);
CREATE INDEX compensacao_ausencia_disciplina_regencia_compensacao_ausencia_i ON public.compensacao_ausencia_disciplina_regencia USING btree (compensacao_ausencia_id);


-- public.encaminhamento_aee definition

-- Drop table

-- DROP TABLE public.encaminhamento_aee;

CREATE TABLE public.encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	situacao int4 NOT NULL DEFAULT 1,
	aluno_nome varchar NULL,
	aluno_numero int4 NOT NULL DEFAULT 0,
	motivo_encerramento varchar NULL,
	responsavel_id int8 NULL,
	CONSTRAINT encaminhamento_aee_pk PRIMARY KEY (id),
	CONSTRAINT encaminhamento_aee_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id),
	CONSTRAINT encaminhamento_aee_usuario_fk FOREIGN KEY (responsavel_id) REFERENCES usuario(id)
);
CREATE INDEX encaminhamento_aee_aluno_idx ON public.encaminhamento_aee USING btree (aluno_codigo);
CREATE INDEX encaminhamento_aee_turma_idx ON public.encaminhamento_aee USING btree (turma_id);
CREATE INDEX encaminhamento_aee_usuario_idx ON public.encaminhamento_aee USING btree (responsavel_id);


-- public.encaminhamento_aee_secao definition

-- Drop table

-- DROP TABLE public.encaminhamento_aee_secao;

CREATE TABLE public.encaminhamento_aee_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_aee_id int8 NOT NULL,
	secao_encaminhamento_id int8 NOT NULL,
	concluido bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_aee_secao_pk PRIMARY KEY (id),
	CONSTRAINT encaminhamento_aee_secao_encaminhamento_fk FOREIGN KEY (encaminhamento_aee_id) REFERENCES encaminhamento_aee(id),
	CONSTRAINT encaminhamento_aee_secao_secao_fk FOREIGN KEY (secao_encaminhamento_id) REFERENCES secao_encaminhamento_aee(id)
);
CREATE INDEX encaminhamento_aee_secao_encaminhamento_idx ON public.encaminhamento_aee_secao USING btree (encaminhamento_aee_id);
CREATE INDEX encaminhamento_aee_secao_secao_idx ON public.encaminhamento_aee_secao USING btree (secao_encaminhamento_id);


-- public.evento_fechamento definition

-- Drop table

-- DROP TABLE public.evento_fechamento;

CREATE TABLE public.evento_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	evento_id int8 NOT NULL,
	fechamento_id int8 NOT NULL,
	excluido bool NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT evento_fechamento_pk PRIMARY KEY (id),
	CONSTRAINT evento_fechamento_evento_fk FOREIGN KEY (evento_id) REFERENCES evento(id),
	CONSTRAINT evento_fechamento_fk FOREIGN KEY (fechamento_id) REFERENCES periodo_fechamento_bimestre(id)
);


-- public.fechamento_turma definition

-- Drop table

-- DROP TABLE public.fechamento_turma;

CREATE TABLE public.fechamento_turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	periodo_escolar_id int8 NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_turma_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_turma_periodo_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id),
	CONSTRAINT fechamento_turma_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX fechamento_turma_periodo_idx ON public.fechamento_turma USING btree (periodo_escolar_id);
CREATE INDEX fechamento_turma_turma_idx ON public.fechamento_turma USING btree (turma_id);


-- public.fechamento_turma_disciplina definition

-- Drop table

-- DROP TABLE public.fechamento_turma_disciplina;

CREATE TABLE public.fechamento_turma_disciplina (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	disciplina_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	situacao int4 NOT NULL,
	justificativa varchar(250) NULL,
	fechamento_turma_id int8 NOT NULL,
	CONSTRAINT fechamento_turma_disciplina_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_turma_disciplina_fechamento_fk FOREIGN KEY (fechamento_turma_id) REFERENCES fechamento_turma(id)
);
CREATE INDEX fechamento_turma_disciplina_disciplina_idx ON public.fechamento_turma_disciplina USING btree (disciplina_id);
CREATE INDEX fechamento_turma_disciplina_fechamento_idx ON public.fechamento_turma_disciplina USING btree (fechamento_turma_id);


-- public.itinerancia_aluno definition

-- Drop table

-- DROP TABLE public.itinerancia_aluno;

CREATE TABLE public.itinerancia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno varchar(100) NOT NULL,
	itinerancia_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	turma_id int4 NULL,
	CONSTRAINT itinerancia_aluno_pk PRIMARY KEY (id),
	CONSTRAINT itinerancia_aluno_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id),
	CONSTRAINT itinerancia_aluno_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX itinerancia_aluno_turma__idx ON public.itinerancia_aluno USING btree (turma_id);


-- public.itinerancia_aluno_questao definition

-- Drop table

-- DROP TABLE public.itinerancia_aluno_questao;

CREATE TABLE public.itinerancia_aluno_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_id int8 NOT NULL,
	itinerancia_aluno_id int8 NOT NULL,
	resposta varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT itinerancia_aluno_questao_pk PRIMARY KEY (id),
	CONSTRAINT itinerancia_aluno_itinerancia_fk FOREIGN KEY (itinerancia_aluno_id) REFERENCES itinerancia_aluno(id)
);


-- public.notificacao_compensacao_ausencia definition

-- Drop table

-- DROP TABLE public.notificacao_compensacao_ausencia;

CREATE TABLE public.notificacao_compensacao_ausencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	notificacao_id int8 NOT NULL,
	compensacao_ausencia_id int8 NOT NULL,
	CONSTRAINT notificacao_compensacao_ausencia_pk PRIMARY KEY (id),
	CONSTRAINT notificacao_compensacao_ausencia_compensacao_fk FOREIGN KEY (compensacao_ausencia_id) REFERENCES compensacao_ausencia(id),
	CONSTRAINT notificacao_compensacao_ausencia_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id)
);
CREATE INDEX notificacao_compensacao_ausencia_compensacao_ausencia_idx ON public.notificacao_compensacao_ausencia USING btree (compensacao_ausencia_id);
CREATE INDEX notificacao_compensacao_ausencia_notificacao_idx ON public.notificacao_compensacao_ausencia USING btree (notificacao_id);


-- public.ocorrencia definition

-- Drop table

-- DROP TABLE public.ocorrencia;

CREATE TABLE public.ocorrencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	titulo varchar(50) NOT NULL,
	data_ocorrencia timestamp NOT NULL,
	hora_ocorrencia time NULL,
	descricao varchar NOT NULL,
	ocorrencia_tipo_id int8 NOT NULL,
	turma_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT ocorrencia_pk PRIMARY KEY (id),
	CONSTRAINT ocorrencia_ocorrencia_tipo_fk FOREIGN KEY (ocorrencia_tipo_id) REFERENCES ocorrencia_tipo(id),
	CONSTRAINT ocorrencia_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX ocorrencia_data_ocorrencia_idx ON public.ocorrencia USING btree (data_ocorrencia);
CREATE INDEX ocorrencia_ocorrencia_tipo_idx ON public.ocorrencia USING btree (ocorrencia_tipo_id);
CREATE INDEX ocorrencia_titulo_idx ON public.ocorrencia USING btree (titulo);
CREATE INDEX ocorrencia_turma_idx ON public.ocorrencia USING btree (turma_id);


-- public.ocorrencia_aluno definition

-- Drop table

-- DROP TABLE public.ocorrencia_aluno;

CREATE TABLE public.ocorrencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno int8 NOT NULL,
	ocorrencia_id int8 NOT NULL,
	CONSTRAINT ocorrencia_aluno_pk PRIMARY KEY (id),
	CONSTRAINT ocorrencia_aluno_ocorrencia_fk FOREIGN KEY (ocorrencia_id) REFERENCES ocorrencia(id)
);
CREATE INDEX ocorrencia_aluno_codigo_aluno_idx ON public.ocorrencia_aluno USING btree (codigo_aluno);


-- public.opcao_questao_complementar definition

-- Drop table

-- DROP TABLE public.opcao_questao_complementar;

CREATE TABLE public.opcao_questao_complementar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	opcao_resposta_id int8 NOT NULL,
	questao_complementar_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT opcao_questao_complementar_pk PRIMARY KEY (id),
	CONSTRAINT opcao_questao_complementar_opcao_resposta_fk FOREIGN KEY (opcao_resposta_id) REFERENCES opcao_resposta(id),
	CONSTRAINT opcao_questao_complementar_questao_fk FOREIGN KEY (questao_complementar_id) REFERENCES questao(id)
);
CREATE INDEX opcao_questao_complementar_opcao_resposta_idx ON public.opcao_questao_complementar USING btree (opcao_resposta_id);
CREATE INDEX opcao_questao_complementar_questao_idx ON public.opcao_questao_complementar USING btree (questao_complementar_id);


-- public.pendencia_encaminhamento_aee definition

-- Drop table

-- DROP TABLE public.pendencia_encaminhamento_aee;

CREATE TABLE public.pendencia_encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_aee_id int8 NOT NULL,
	pendencia_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_encaminhamento_aee_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_encaminhamento_aee_fk FOREIGN KEY (encaminhamento_aee_id) REFERENCES encaminhamento_aee(id),
	CONSTRAINT pendencia_encaminhamento_aee_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id)
);
CREATE INDEX pendencia_encaminhamento_aee_pendencia_id_idx ON public.pendencia_encaminhamento_aee USING btree (pendencia_id);
CREATE INDEX pendencia_encaminhamento_encaminhamento_aee_id_idx ON public.pendencia_encaminhamento_aee USING btree (encaminhamento_aee_id);


-- public.pendencia_fechamento definition

-- Drop table

-- DROP TABLE public.pendencia_fechamento;

CREATE TABLE public.pendencia_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_disciplina_id int8 NOT NULL,
	pendencia_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_fechamento_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_fechamento_fechamento_fk FOREIGN KEY (fechamento_turma_disciplina_id) REFERENCES fechamento_turma_disciplina(id),
	CONSTRAINT pendencia_fechamento_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id)
);
CREATE INDEX pendencia_fechamento_fechamento_turma_id_idx ON public.pendencia_fechamento USING btree (fechamento_turma_disciplina_id);
CREATE INDEX pendencia_fechamento_pendencia_id_idx ON public.pendencia_fechamento USING btree (pendencia_id);


-- public.pendencia_professor definition

-- Drop table

-- DROP TABLE public.pendencia_professor;

CREATE TABLE public.pendencia_professor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	pendencia_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	turma_id int8 NOT NULL,
	professor_rf varchar(15) NOT NULL,
	periodo_escolar_id int8 NULL,
	CONSTRAINT pendencia_professor_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_professor_componente_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id),
	CONSTRAINT pendencia_professor_periodo_escolar_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id),
	CONSTRAINT pendencia_professor_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX pendencia_professor_componente_curricular_idx ON public.pendencia_professor USING btree (componente_curricular_id);
CREATE INDEX pendencia_professor_periodo_escolar_idx ON public.pendencia_professor USING btree (periodo_escolar_id);
CREATE INDEX pendencia_professor_professor_idx ON public.pendencia_professor USING btree (professor_rf);
CREATE INDEX pendencia_professor_turma_idx ON public.pendencia_professor USING btree (turma_id);


-- public.pendencia_registro_individual definition

-- Drop table

-- DROP TABLE public.pendencia_registro_individual;

CREATE TABLE public.pendencia_registro_individual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	pendencia_id int8 NOT NULL,
	turma_id int8 NOT NULL,
	CONSTRAINT pendencia_registro_individual_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_uk UNIQUE (pendencia_id),
	CONSTRAINT pendencia_registro_individual_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id),
	CONSTRAINT pendencia_registro_individual_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX pendencia_registro_individual_turma_idx ON public.pendencia_registro_individual USING btree (turma_id);


-- public.pendencia_registro_individual_aluno definition

-- Drop table

-- DROP TABLE public.pendencia_registro_individual_aluno;

CREATE TABLE public.pendencia_registro_individual_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	pendencia_registro_individual_id int8 NOT NULL,
	codigo_aluno int8 NOT NULL,
	situacao int4 NOT NULL,
	CONSTRAINT pendencia_registro_individual_aluno_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_reg_individual_aluno_pendencia_reg_individual_fk FOREIGN KEY (pendencia_registro_individual_id) REFERENCES pendencia_registro_individual(id)
);


-- public.planejamento_anual definition

-- Drop table

-- DROP TABLE public.planejamento_anual;

CREATE TABLE public.planejamento_anual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT planejamento_anual_pk PRIMARY KEY (id),
	CONSTRAINT planejamento_anual_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id),
	CONSTRAINT planejamento_anual_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX planejamento_anual_componente_curricular_idx ON public.planejamento_anual USING btree (componente_curricular_id);
CREATE INDEX planejamento_anual_turma_idx ON public.planejamento_anual USING btree (turma_id);


-- public.planejamento_anual_periodo_escolar definition

-- Drop table

-- DROP TABLE public.planejamento_anual_periodo_escolar;

CREATE TABLE public.planejamento_anual_periodo_escolar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	periodo_escolar_id int8 NOT NULL,
	planejamento_anual_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT planejamento_anual_periodo_escolar_pk PRIMARY KEY (id),
	CONSTRAINT planejamento_anual_periodo_escolar_periodo_escolar_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id),
	CONSTRAINT planejamento_anual_periodo_escolar_planejamento_anual_fk FOREIGN KEY (planejamento_anual_id) REFERENCES planejamento_anual(id)
);
CREATE INDEX planejamento_anual_periodo_escolar_periodo_escolar_idx ON public.planejamento_anual_periodo_escolar USING btree (periodo_escolar_id);
CREATE INDEX planejamento_anual_periodo_escolar_planejamento_anual_idx ON public.planejamento_anual_periodo_escolar USING btree (planejamento_anual_id);


-- public.plano_aee definition

-- Drop table

-- DROP TABLE public.plano_aee;

CREATE TABLE public.plano_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	aluno_nome varchar NOT NULL,
	aluno_numero int4 NOT NULL,
	situacao int4 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	parecer_coordenacao varchar NULL,
	parecer_paai varchar NULL,
	responsavel_id int8 NULL,
	CONSTRAINT plano_aee_pk PRIMARY KEY (id),
	CONSTRAINT plano_aee_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id),
	CONSTRAINT plano_aee_usuario_fk FOREIGN KEY (responsavel_id) REFERENCES usuario(id)
);
CREATE INDEX plano_aee_aluno_idx ON public.plano_aee USING btree (aluno_codigo);
CREATE INDEX plano_aee_turma_idx ON public.plano_aee USING btree (turma_id);
CREATE INDEX plano_aee_usuario_idx ON public.plano_aee USING btree (responsavel_id);


-- public.plano_aee_observacao definition

-- Drop table

-- DROP TABLE public.plano_aee_observacao;

CREATE TABLE public.plano_aee_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_id int8 NOT NULL,
	observacao varchar NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_observacao_pk PRIMARY KEY (id),
	CONSTRAINT plano_aee_observacao_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id)
);
CREATE INDEX plano_aee_observacao_plano_idx ON public.plano_aee_observacao USING btree (plano_aee_id);


-- public.plano_aee_versao definition

-- Drop table

-- DROP TABLE public.plano_aee_versao;

CREATE TABLE public.plano_aee_versao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_id int8 NOT NULL,
	numero int4 NOT NULL DEFAULT 1,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_versao_pk PRIMARY KEY (id),
	CONSTRAINT plano_aee_versao_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id)
);
CREATE INDEX plano_aee_versao_numero_idx ON public.plano_aee_versao USING btree (numero);
CREATE INDEX plano_aee_versao_plano_idx ON public.plano_aee_versao USING btree (plano_aee_id);


-- public.questao_encaminhamento_aee definition

-- Drop table

-- DROP TABLE public.questao_encaminhamento_aee;

CREATE TABLE public.questao_encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_aee_secao_id int8 NOT NULL,
	questao_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT questao_encaminhamento_aee_pk PRIMARY KEY (id),
	CONSTRAINT questao_encaminhamento_aee_encaminhamento_fk FOREIGN KEY (encaminhamento_aee_secao_id) REFERENCES encaminhamento_aee_secao(id),
	CONSTRAINT questao_encaminhamento_aee_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id)
);
CREATE INDEX questao_encaminhamento_aee_questao_idx ON public.questao_encaminhamento_aee USING btree (questao_id);
CREATE INDEX questao_encaminhamento_aee_secao_idx ON public.questao_encaminhamento_aee USING btree (encaminhamento_aee_secao_id);


-- public.recuperacao_paralela definition

-- Drop table

-- DROP TABLE public.recuperacao_paralela;

CREATE TABLE public.recuperacao_paralela (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aluno_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	ano_letivo int4 NOT NULL,
	turma_id int8 NOT NULL,
	turma_recuperacao_paralela_id int8 NOT NULL,
	CONSTRAINT recuperacao_paralela_pk PRIMARY KEY (id),
	CONSTRAINT recuperacao_paralela_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id),
	CONSTRAINT recuperacao_paralela_turma_rp_fk FOREIGN KEY (turma_recuperacao_paralela_id) REFERENCES turma(id)
);
CREATE INDEX recuperacao_paralela_turma_idx ON public.recuperacao_paralela USING btree (turma_id);
CREATE INDEX recuperacao_paralela_turma_rp_idx ON public.recuperacao_paralela USING btree (turma_recuperacao_paralela_id);


-- public.registro_individual definition

-- Drop table

-- DROP TABLE public.registro_individual;

CREATE TABLE public.registro_individual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	aluno_codigo int8 NULL,
	componente_curricular_id int8 NOT NULL,
	data_registro timestamp NOT NULL,
	registro varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT registro_individual_pk PRIMARY KEY (id),
	CONSTRAINT registro_individual_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id),
	CONSTRAINT registro_individual_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX registro_individual_aluno_codigo_idx ON public.registro_individual USING btree (aluno_codigo);
CREATE INDEX registro_individual_componente_curricular_idx ON public.registro_individual USING btree (componente_curricular_id);
CREATE INDEX registro_individual_turma_idx ON public.registro_individual USING btree (turma_id);


-- public.relatorio_semestral_turma_pap definition

-- Drop table

-- DROP TABLE public.relatorio_semestral_turma_pap;

CREATE TABLE public.relatorio_semestral_turma_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	semestre int4 NOT NULL,
	CONSTRAINT relatorio_semestral_turma_pap_pk PRIMARY KEY (id),
	CONSTRAINT relatorio_semestral_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX relatorio_semestral_turma_pap_idx ON public.relatorio_semestral_turma_pap USING btree (turma_id);


-- public.resposta_encaminhamento_aee definition

-- Drop table

-- DROP TABLE public.resposta_encaminhamento_aee;

CREATE TABLE public.resposta_encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_encaminhamento_id int8 NOT NULL,
	resposta_id int8 NULL,
	arquivo_id int8 NULL,
	texto varchar NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT resposta_encaminhamento_aee_pk PRIMARY KEY (id),
	CONSTRAINT resposta_encaminhamento_aee_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id),
	CONSTRAINT resposta_encaminhamento_aee_questao_fk FOREIGN KEY (questao_encaminhamento_id) REFERENCES questao_encaminhamento_aee(id),
	CONSTRAINT resposta_encaminhamento_aee_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id)
);
CREATE INDEX resposta_encaminhamento_aee_arquivo_idx ON public.resposta_encaminhamento_aee USING btree (arquivo_id);
CREATE INDEX resposta_encaminhamento_aee_questao_idx ON public.resposta_encaminhamento_aee USING btree (questao_encaminhamento_id);
CREATE INDEX resposta_encaminhamento_aee_resposta_idx ON public.resposta_encaminhamento_aee USING btree (resposta_id);


-- public.acompanhamento_aluno_foto definition

-- Drop table

-- DROP TABLE public.acompanhamento_aluno_foto;

CREATE TABLE public.acompanhamento_aluno_foto (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	acompanhamento_aluno_semestre_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,
	miniatura_id int8 NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT acompanhamento_aluno_foto_pk PRIMARY KEY (id),
	CONSTRAINT acompanhamento_aluno_foto_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id),
	CONSTRAINT acompanhamento_aluno_foto_miniatura_fk FOREIGN KEY (miniatura_id) REFERENCES acompanhamento_aluno_foto(id),
	CONSTRAINT acompanhamento_aluno_foto_semestre_fk FOREIGN KEY (acompanhamento_aluno_semestre_id) REFERENCES acompanhamento_aluno_semestre(id)
);
CREATE INDEX acompanhamento_aluno_foto_arquivo_idx ON public.acompanhamento_aluno_foto USING btree (arquivo_id);
CREATE INDEX acompanhamento_aluno_foto_miniatura_idx ON public.acompanhamento_aluno_foto USING btree (miniatura_id);
CREATE INDEX acompanhamento_aluno_foto_semestre_idx ON public.acompanhamento_aluno_foto USING btree (acompanhamento_aluno_semestre_id);


-- public.anotacao_aluno_fechamento definition

-- Drop table

-- DROP TABLE public.anotacao_aluno_fechamento;

CREATE TABLE public.anotacao_aluno_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_disciplina_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	anotacao varchar NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT anotacao_aluno_fechamento_pk PRIMARY KEY (id),
	CONSTRAINT anotacao_aluno_fechamento_fechamento_fk FOREIGN KEY (fechamento_turma_disciplina_id) REFERENCES fechamento_turma_disciplina(id)
);
CREATE INDEX anotacao_aluno_fechamento_aluno_idx ON public.anotacao_aluno_fechamento USING btree (aluno_codigo);
CREATE INDEX anotacao_aluno_fechamento_fechamento_idx ON public.anotacao_aluno_fechamento USING btree (fechamento_turma_disciplina_id);


-- public.conselho_classe definition

-- Drop table

-- DROP TABLE public.conselho_classe;

CREATE TABLE public.conselho_classe (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	situacao int4 NOT NULL DEFAULT 2,
	CONSTRAINT conselho_classe_pk PRIMARY KEY (id),
	CONSTRAINT conselho_classe_fechamento_fk FOREIGN KEY (fechamento_turma_id) REFERENCES fechamento_turma(id)
);
CREATE INDEX conselho_classe_fechamento_idx ON public.conselho_classe USING btree (fechamento_turma_id);


-- public.conselho_classe_aluno definition

-- Drop table

-- DROP TABLE public.conselho_classe_aluno;

CREATE TABLE public.conselho_classe_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	conselho_classe_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	recomendacoes_aluno varchar NULL,
	recomendacoes_familia varchar NULL,
	anotacoes_pedagogicas varchar NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	conselho_classe_parecer_id int8 NULL,
	CONSTRAINT conselho_classe_aluno_pk PRIMARY KEY (id),
	CONSTRAINT conselho_classe_aluno_conselho_fk FOREIGN KEY (conselho_classe_id) REFERENCES conselho_classe(id),
	CONSTRAINT conselho_classe_aluno_parecer_fk FOREIGN KEY (conselho_classe_parecer_id) REFERENCES conselho_classe_parecer(id)
);
CREATE INDEX conselho_classe_aluno_conselho_idx ON public.conselho_classe_aluno USING btree (conselho_classe_id);


-- public.conselho_classe_aluno_turma_complementar definition

-- Drop table

-- DROP TABLE public.conselho_classe_aluno_turma_complementar;

CREATE TABLE public.conselho_classe_aluno_turma_complementar (
	conselho_classe_aluno_id int8 NOT NULL,
	turma_id int8 NOT NULL,
	CONSTRAINT conselho_classe_aluno_turma_complementar_pk PRIMARY KEY (conselho_classe_aluno_id, turma_id),
	CONSTRAINT conselho_classe_aluno_turma_complementar_conselho_classe_aluno_ FOREIGN KEY (conselho_classe_aluno_id) REFERENCES conselho_classe_aluno(id),
	CONSTRAINT conselho_classe_aluno_turma_complementar_turma_id_fk FOREIGN KEY (turma_id) REFERENCES turma(id)
);
CREATE INDEX conselho_classe_aluno_turma_complementar_conselho_classe_aluno_ ON public.conselho_classe_aluno_turma_complementar USING btree (conselho_classe_aluno_id);
CREATE INDEX conselho_classe_aluno_turma_complementar_turma_id_idx ON public.conselho_classe_aluno_turma_complementar USING btree (turma_id);


-- public.conselho_classe_nota definition

-- Drop table

-- DROP TABLE public.conselho_classe_nota;

CREATE TABLE public.conselho_classe_nota (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	conselho_classe_aluno_id int8 NOT NULL,
	componente_curricular_codigo int8 NOT NULL,
	nota numeric(5,2) NULL,
	conceito_id int8 NULL,
	justificativa varchar NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT conselho_classe_nota_pk PRIMARY KEY (id),
	CONSTRAINT conselho_classe_nota_aluno_fk FOREIGN KEY (conselho_classe_aluno_id) REFERENCES conselho_classe_aluno(id),
	CONSTRAINT conselho_classe_nota_conceito_fk FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id)
);
CREATE INDEX conselho_classe_nota_aluno_idx ON public.conselho_classe_nota USING btree (conselho_classe_aluno_id);


-- public.fechamento_aluno definition

-- Drop table

-- DROP TABLE public.fechamento_aluno;

CREATE TABLE public.fechamento_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_disciplina_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	anotacao varchar NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_aluno_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_aluno_fechamento_fk FOREIGN KEY (fechamento_turma_disciplina_id) REFERENCES fechamento_turma_disciplina(id)
);
CREATE INDEX fechamento_aluno_aluno_idx ON public.fechamento_aluno USING btree (aluno_codigo);
CREATE INDEX fechamento_aluno_fechamento_idx ON public.fechamento_aluno USING btree (fechamento_turma_disciplina_id);


-- public.fechamento_nota definition

-- Drop table

-- DROP TABLE public.fechamento_nota;

CREATE TABLE public.fechamento_nota (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	disciplina_id int8 NOT NULL,
	nota numeric(5,2) NULL,
	conceito_id int8 NULL,
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	sintese_id int8 NULL,
	fechamento_aluno_id int8 NOT NULL,
	CONSTRAINT nota_conceito_bimestre_pk PRIMARY KEY (id),
	CONSTRAINT fechamento_nota_aluno_fk FOREIGN KEY (fechamento_aluno_id) REFERENCES fechamento_aluno(id),
	CONSTRAINT nota_conceito_bimestre_sintese_fk FOREIGN KEY (sintese_id) REFERENCES sintese_valores(id)
);
CREATE INDEX fechamento_nota_aluno_idx ON public.fechamento_nota USING btree (fechamento_aluno_id);
CREATE INDEX nota_conceito_bimestre_disciplina_idx ON public.fechamento_nota USING btree (disciplina_id);


-- public.historico_nota_conselho_classe definition

-- Drop table

-- DROP TABLE public.historico_nota_conselho_classe;

CREATE TABLE public.historico_nota_conselho_classe (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	historico_nota_id int8 NOT NULL,
	conselho_classe_nota_id int8 NOT NULL,
	CONSTRAINT historico_nota_conselho_classe_pk PRIMARY KEY (id),
	CONSTRAINT historico_nota_conselho_classe_conselho_classe_nota_fk FOREIGN KEY (conselho_classe_nota_id) REFERENCES conselho_classe_nota(id),
	CONSTRAINT historico_nota_conselho_classe_historico_nota_fk FOREIGN KEY (historico_nota_id) REFERENCES historico_nota(id)
);
CREATE INDEX historico_nota_conselho_classe_conselho_classe_nota_idx ON public.historico_nota_conselho_classe USING btree (conselho_classe_nota_id);


-- public.historico_nota_fechamento definition

-- Drop table

-- DROP TABLE public.historico_nota_fechamento;

CREATE TABLE public.historico_nota_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	historico_nota_id int8 NOT NULL,
	fechamento_nota_id int8 NOT NULL,
	wf_aprovacao_id int8 NULL,
	CONSTRAINT historico_nota_fechamento_pk PRIMARY KEY (id),
	CONSTRAINT historico_nota_fechamento_fechamento_nota_fk FOREIGN KEY (fechamento_nota_id) REFERENCES fechamento_nota(id),
	CONSTRAINT historico_nota_fechamento_historico_nota_fk FOREIGN KEY (historico_nota_id) REFERENCES historico_nota(id),
	CONSTRAINT historico_nota_fechamento_wf_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id)
);
CREATE INDEX historico_nota_conselho_classe_historico_nota_idx ON public.historico_nota_fechamento USING btree (historico_nota_id);
CREATE INDEX historico_nota_fechamento_fechamento_nota_idx ON public.historico_nota_fechamento USING btree (fechamento_nota_id);
CREATE INDEX historico_nota_fechamento_historico_nota_idx ON public.historico_nota_fechamento USING btree (historico_nota_id);
CREATE INDEX historico_nota_fechamento_wf_aprovacao_idx ON public.historico_nota_fechamento USING btree (wf_aprovacao_id);


-- public.notificacao_plano_aee definition

-- Drop table

-- DROP TABLE public.notificacao_plano_aee;

CREATE TABLE public.notificacao_plano_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo int4 NOT NULL,
	notificacao_id int8 NOT NULL,
	plano_aee_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT notificacao_plano_aee_pk PRIMARY KEY (id),
	CONSTRAINT notificacao_plano_aee_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id),
	CONSTRAINT notificacao_plano_aee_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id)
);
CREATE INDEX notificacao_idx ON public.notificacao_plano_aee USING btree (notificacao_id);
CREATE INDEX notificacao_plano_aee_idx ON public.notificacao_plano_aee USING btree (plano_aee_id);


-- public.notificacao_plano_aee_observacao definition

-- Drop table

-- DROP TABLE public.notificacao_plano_aee_observacao;

CREATE TABLE public.notificacao_plano_aee_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_observacao_id int8 NOT NULL,
	notificacao_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT notificacao_plano_aee_observacao_pk PRIMARY KEY (id),
	CONSTRAINT notificacao_plano_aee_observacao_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id),
	CONSTRAINT notificacao_plano_aee_observacao_observacao_fk FOREIGN KEY (plano_aee_observacao_id) REFERENCES plano_aee_observacao(id)
);
CREATE INDEX notificacao_plano_aee_observacao_notificacao_idx ON public.notificacao_plano_aee_observacao USING btree (notificacao_id);
CREATE INDEX notificacao_plano_aee_observacao_observacao_idx ON public.notificacao_plano_aee_observacao USING btree (plano_aee_observacao_id);


-- public.pendencia_plano_aee definition

-- Drop table

-- DROP TABLE public.pendencia_plano_aee;

CREATE TABLE public.pendencia_plano_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_id int8 NOT NULL,
	pendencia_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_plano_aee_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_plano_aee_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id),
	CONSTRAINT pendencia_plano_aee_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id)
);
CREATE INDEX pendencia_plano_pendencia_idx ON public.pendencia_plano_aee USING btree (pendencia_id);
CREATE INDEX pendencia_plano_plano_idx ON public.pendencia_plano_aee USING btree (plano_aee_id);


-- public.planejamento_anual_componente definition

-- Drop table

-- DROP TABLE public.planejamento_anual_componente;

CREATE TABLE public.planejamento_anual_componente (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	planejamento_anual_periodo_escolar_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	descricao varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT planejamento_anual_componente_pk PRIMARY KEY (id),
	CONSTRAINT planejamento_anual_componente_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id),
	CONSTRAINT planejamento_anual_componente_planejamento_anual_periodo_escola FOREIGN KEY (planejamento_anual_periodo_escolar_id) REFERENCES planejamento_anual_periodo_escolar(id)
);
CREATE INDEX planejamento_anual_componente_componente_componente_curricular_ ON public.planejamento_anual_componente USING btree (componente_curricular_id);
CREATE INDEX planejamento_anual_componente_componente_planejamento_anual_per ON public.planejamento_anual_componente USING btree (planejamento_anual_periodo_escolar_id);


-- public.planejamento_anual_objetivos_aprendizagem definition

-- Drop table

-- DROP TABLE public.planejamento_anual_objetivos_aprendizagem;

CREATE TABLE public.planejamento_anual_objetivos_aprendizagem (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	planejamento_anual_componente_id int8 NOT NULL,
	objetivo_aprendizagem_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT planejamento_anual_objetivos_aprendizagem_pk PRIMARY KEY (id),
	CONSTRAINT planejamento_anual_objetivos_aprendizagem_objetivos_aprendizage FOREIGN KEY (objetivo_aprendizagem_id) REFERENCES objetivo_aprendizagem(id),
	CONSTRAINT planejamento_anual_objetivos_aprendizagem_planejamento_anual_fk FOREIGN KEY (planejamento_anual_componente_id) REFERENCES planejamento_anual_componente(id)
);
CREATE INDEX planejamento_anual_objetivos_aprendizagem_objetivo_aprendizagem ON public.planejamento_anual_objetivos_aprendizagem USING btree (objetivo_aprendizagem_id);
CREATE INDEX planejamento_anual_objetivos_aprendizagem_planejamento_anual_co ON public.planejamento_anual_objetivos_aprendizagem USING btree (planejamento_anual_componente_id);


-- public.plano_aee_questao definition

-- Drop table

-- DROP TABLE public.plano_aee_questao;

CREATE TABLE public.plano_aee_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_versao_id int8 NOT NULL,
	questao_id int8 NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_questao_pk PRIMARY KEY (id),
	CONSTRAINT plano_aee_questao_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id),
	CONSTRAINT plano_aee_questao_versao_fk FOREIGN KEY (plano_aee_versao_id) REFERENCES plano_aee_versao(id)
);
CREATE INDEX plano_aee_questao_questao_idx ON public.plano_aee_questao USING btree (questao_id);
CREATE INDEX plano_aee_questao_versao_idx ON public.plano_aee_questao USING btree (plano_aee_versao_id);


-- public.plano_aee_reestruturacao definition

-- Drop table

-- DROP TABLE public.plano_aee_reestruturacao;

CREATE TABLE public.plano_aee_reestruturacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_versao_id int8 NOT NULL,
	semestre int4 NOT NULL,
	descricao varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_reestruturacao_pk PRIMARY KEY (id),
	CONSTRAINT plano_aee_reestruturacao_versao_fk FOREIGN KEY (plano_aee_versao_id) REFERENCES plano_aee_versao(id)
);
CREATE INDEX plano_aee_reestruturacao_versao_idx ON public.plano_aee_reestruturacao USING btree (plano_aee_versao_id);


-- public.plano_aee_resposta definition

-- Drop table

-- DROP TABLE public.plano_aee_resposta;

CREATE TABLE public.plano_aee_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_questao_id int8 NOT NULL,
	resposta_id int8 NULL,
	arquivo_id int8 NULL,
	texto varchar NULL,
	periodo_inicio timestamp NULL,
	periodo_fim timestamp NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_resposta_pk PRIMARY KEY (id),
	CONSTRAINT plano_aee_resposta_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id),
	CONSTRAINT plano_aee_resposta_questao_fk FOREIGN KEY (plano_questao_id) REFERENCES plano_aee_questao(id),
	CONSTRAINT plano_aee_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id)
);
CREATE INDEX plano_aee_resposta_arquivo_idx ON public.plano_aee_resposta USING btree (arquivo_id);
CREATE INDEX plano_aee_resposta_questao_idx ON public.plano_aee_resposta USING btree (plano_questao_id);
CREATE INDEX plano_aee_resposta_resposta_idx ON public.plano_aee_resposta USING btree (resposta_id);


-- public.relatorio_semestral_pap_aluno definition

-- Drop table

-- DROP TABLE public.relatorio_semestral_pap_aluno;

CREATE TABLE public.relatorio_semestral_pap_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	relatorio_semestral_turma_pap_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT relatorio_semestral_pap_aluno_pk PRIMARY KEY (id),
	CONSTRAINT relatorio_semestral_pap_aluno_relatorio_fk FOREIGN KEY (relatorio_semestral_turma_pap_id) REFERENCES relatorio_semestral_turma_pap(id)
);
CREATE INDEX relatorio_semestral_pap_aluno_aluno_idx ON public.relatorio_semestral_pap_aluno USING btree (aluno_codigo);
CREATE INDEX relatorio_semestral_pap_aluno_relatorio_idx ON public.relatorio_semestral_pap_aluno USING btree (relatorio_semestral_turma_pap_id);


-- public.relatorio_semestral_pap_aluno_secao definition

-- Drop table

-- DROP TABLE public.relatorio_semestral_pap_aluno_secao;

CREATE TABLE public.relatorio_semestral_pap_aluno_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	relatorio_semestral_pap_aluno_id int8 NOT NULL,
	secao_relatorio_semestral_pap_id int8 NOT NULL,
	valor varchar NOT NULL,
	CONSTRAINT relatorio_semestral_pap_aluno_secao_pk PRIMARY KEY (id),
	CONSTRAINT relatorio_semestral_pap_aluno_secao_aluno_fk FOREIGN KEY (relatorio_semestral_pap_aluno_id) REFERENCES relatorio_semestral_pap_aluno(id),
	CONSTRAINT relatorio_semestral_pap_aluno_secao_secao_fk FOREIGN KEY (secao_relatorio_semestral_pap_id) REFERENCES secao_relatorio_semestral_pap(id)
);
CREATE INDEX relatorio_semestral_pap_aluno_secao_aluno_idx ON public.relatorio_semestral_pap_aluno_secao USING btree (relatorio_semestral_pap_aluno_id);
CREATE INDEX relatorio_semestral_pap_aluno_secao_secao_idx ON public.relatorio_semestral_pap_aluno_secao USING btree (secao_relatorio_semestral_pap_id);


-- public.wf_aprovacao_nota_fechamento definition

-- Drop table

-- DROP TABLE public.wf_aprovacao_nota_fechamento;

CREATE TABLE public.wf_aprovacao_nota_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_id int8 NOT NULL,
	fechamento_nota_id int8 NOT NULL,
	nota numeric(5,2) NULL,
	conceito_id int8 NULL,
	CONSTRAINT wf_aprovacao_nota_fechamento_pk PRIMARY KEY (id),
	CONSTRAINT wf_aprovacao_nota_fechamento_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id),
	CONSTRAINT wf_aprovacao_nota_fechamento_conceito_fk FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id),
	CONSTRAINT wf_aprovacao_nota_fechamento_nota_fk FOREIGN KEY (fechamento_nota_id) REFERENCES fechamento_nota(id)
);
CREATE INDEX wf_aprovacao_nota_fechamento_aprovacao_idx ON public.wf_aprovacao_nota_fechamento USING btree (wf_aprovacao_id);
CREATE INDEX wf_aprovacao_nota_fechamento_nota_idx ON public.wf_aprovacao_nota_fechamento USING btree (fechamento_nota_id);


-- public.pg_stat_statements source

CREATE OR REPLACE VIEW public.pg_stat_statements
AS SELECT pg_stat_statements.userid,
    pg_stat_statements.dbid,
    pg_stat_statements.queryid,
    pg_stat_statements.query,
    pg_stat_statements.calls,
    pg_stat_statements.total_time,
    pg_stat_statements.min_time,
    pg_stat_statements.max_time,
    pg_stat_statements.mean_time,
    pg_stat_statements.stddev_time,
    pg_stat_statements.rows,
    pg_stat_statements.shared_blks_hit,
    pg_stat_statements.shared_blks_read,
    pg_stat_statements.shared_blks_dirtied,
    pg_stat_statements.shared_blks_written,
    pg_stat_statements.local_blks_hit,
    pg_stat_statements.local_blks_read,
    pg_stat_statements.local_blks_dirtied,
    pg_stat_statements.local_blks_written,
    pg_stat_statements.temp_blks_read,
    pg_stat_statements.temp_blks_written,
    pg_stat_statements.blk_read_time,
    pg_stat_statements.blk_write_time
   FROM pg_stat_statements(true) pg_stat_statements(userid, dbid, queryid, query, calls, total_time, min_time, max_time, mean_time, stddev_time, rows, shared_blks_hit, shared_blks_read, shared_blks_dirtied, shared_blks_written, local_blks_hit, local_blks_read, local_blks_dirtied, local_blks_written, temp_blks_read, temp_blks_written, blk_read_time, blk_write_time);


-- public.v_abrangencia source

CREATE OR REPLACE VIEW public.v_abrangencia
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
   FROM abrangencia a
     LEFT JOIN v_abrangencia_cadeia_dres dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON turma.turma_id = a.turma_id
  WHERE a.historico = false AND (COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) = false OR COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) IS NULL);


-- public.v_abrangencia_cadeia_dres source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_dres
AS SELECT v_abrangencia_cadeia_turmas.dre_id,
    v_abrangencia_cadeia_turmas.dre_codigo,
    v_abrangencia_cadeia_turmas.dre_abreviacao,
    v_abrangencia_cadeia_turmas.dre_nome,
    v_abrangencia_cadeia_turmas.ue_id,
    v_abrangencia_cadeia_turmas.ue_codigo,
    v_abrangencia_cadeia_turmas.ue_nome,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.turma_ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.turma_nome,
    v_abrangencia_cadeia_turmas.turma_semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula,
    v_abrangencia_cadeia_turmas.tipo_turno,
    v_abrangencia_cadeia_turmas.turma_codigo,
    v_abrangencia_cadeia_turmas.turma_historica,
    v_abrangencia_cadeia_turmas.dt_fim_turma
   FROM v_abrangencia_cadeia_turmas
UNION ALL
 SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    NULL::bigint AS ue_id,
    NULL::character varying AS ue_codigo,
    NULL::character varying AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres;


-- public.v_abrangencia_cadeia_dres_somente source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_dres_somente
AS SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    NULL::bigint AS ue_id,
    NULL::character varying AS ue_codigo,
    NULL::character varying AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres;


-- public.v_abrangencia_cadeia_turmas source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_turmas
AS SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id AS ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    ab_turma.id AS turma_id,
    ab_turma.ano AS turma_ano,
    ab_turma.ano_letivo AS turma_ano_letivo,
    ab_turma.modalidade_codigo,
    ab_turma.nome AS turma_nome,
    ab_turma.semestre AS turma_semestre,
    ab_turma.qt_duracao_aula,
    ab_turma.tipo_turno,
    ab_turma.turma_id AS turma_codigo,
    ab_turma.historica AS turma_historica,
    ab_turma.dt_fim_eol AS dt_fim_turma,
    ab_turma.ensino_especial,
    ab_turma.tipo_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id
     JOIN turma ab_turma ON ab_turma.ue_id = ab_ues.id;


-- public.v_abrangencia_cadeia_ues source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_ues
AS SELECT v_abrangencia_cadeia_turmas.dre_id,
    v_abrangencia_cadeia_turmas.dre_codigo,
    v_abrangencia_cadeia_turmas.dre_abreviacao,
    v_abrangencia_cadeia_turmas.dre_nome,
    v_abrangencia_cadeia_turmas.ue_id,
    v_abrangencia_cadeia_turmas.ue_codigo,
    v_abrangencia_cadeia_turmas.ue_nome,
    v_abrangencia_cadeia_turmas.turma_id,
    v_abrangencia_cadeia_turmas.turma_ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.turma_nome,
    v_abrangencia_cadeia_turmas.turma_semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula,
    v_abrangencia_cadeia_turmas.tipo_turno,
    v_abrangencia_cadeia_turmas.turma_codigo,
    v_abrangencia_cadeia_turmas.turma_historica,
    v_abrangencia_cadeia_turmas.dt_fim_turma
   FROM v_abrangencia_cadeia_turmas
UNION ALL
 SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id AS ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id;


-- public.v_abrangencia_cadeia_ues_somente source

CREATE OR REPLACE VIEW public.v_abrangencia_cadeia_ues_somente
AS SELECT ab_dres.id AS dre_id,
    ab_dres.dre_id AS dre_codigo,
    ab_dres.abreviacao AS dre_abreviacao,
    ab_dres.nome AS dre_nome,
    ab_ues.id AS ue_id,
    ab_ues.ue_id AS ue_codigo,
    ab_ues.nome AS ue_nome,
    NULL::bigint AS turma_id,
    NULL::bpchar AS turma_ano,
    NULL::integer AS turma_ano_letivo,
    NULL::integer AS modalidade_codigo,
    NULL::character varying AS turma_nome,
    NULL::integer AS turma_semestre,
    NULL::smallint AS qt_duracao_aula,
    NULL::smallint AS tipo_turno,
    NULL::character varying AS turma_codigo,
    NULL::boolean AS turma_historica,
    NULL::date AS dt_fim_turma
   FROM dre ab_dres
     JOIN ue ab_ues ON ab_ues.dre_id = ab_dres.id;


-- public.v_abrangencia_historica source

CREATE OR REPLACE VIEW public.v_abrangencia_historica
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    u.login,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id,
    COALESCE(turma.dt_fim_turma, ue.dt_fim_turma, dre.dt_fim_turma) AS dt_fim_turma,
    a.dt_fim_vinculo
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
     LEFT JOIN v_abrangencia_cadeia_dres_somente dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues_somente ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON a.dre_id = turma.dre_id OR a.ue_id = turma.ue_id OR a.turma_id = turma.turma_id
  WHERE a.historico = true OR dre.turma_historica = true;


-- public.v_abrangencia_nivel_dre source

CREATE OR REPLACE VIEW public.v_abrangencia_nivel_dre
AS SELECT DISTINCT a.dre_id,
    a.perfil AS perfil_id,
    a.historico,
    u.login,
    a.ue_id,
    a.turma_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
  WHERE a.dre_id IS NOT NULL AND a.ue_id IS NULL AND a.turma_id IS NULL
  ORDER BY a.dre_id;


-- public.v_abrangencia_nivel_especifico source

CREATE OR REPLACE VIEW public.v_abrangencia_nivel_especifico
AS SELECT DISTINCT a.dre_id,
    a.ue_id,
    a.turma_id,
    a.perfil AS perfil_id,
    a.historico,
    u.login
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
  WHERE a.dre_id IS NOT NULL AND a.ue_id IS NOT NULL AND a.turma_id IS NOT NULL
  ORDER BY a.dre_id, a.ue_id, a.turma_id;


-- public.v_abrangencia_nivel_turma source

CREATE OR REPLACE VIEW public.v_abrangencia_nivel_turma
AS SELECT DISTINCT a.turma_id,
    a.perfil AS perfil_id,
    a.historico,
    u.login,
    a.ue_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
  WHERE a.turma_id IS NOT NULL
  ORDER BY a.turma_id;


-- public.v_abrangencia_nivel_ue source

CREATE OR REPLACE VIEW public.v_abrangencia_nivel_ue
AS SELECT DISTINCT a.ue_id,
    a.perfil AS perfil_id,
    a.historico,
    u.login,
    a.turma_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
  WHERE a.ue_id IS NOT NULL AND a.dre_id IS NULL AND a.turma_id IS NULL
  ORDER BY a.ue_id;


-- public.v_abrangencia_sintetica source

CREATE OR REPLACE VIEW public.v_abrangencia_sintetica
AS SELECT a.id,
    a.usuario_id,
    u.login,
    a.dre_id,
    dre.dre_id AS codigo_dre,
    a.ue_id,
    ue.ue_id AS codigo_ue,
    a.turma_id,
    turma.turma_id AS codigo_turma,
    a.perfil,
    a.historico
   FROM abrangencia a
     JOIN usuario u ON u.id = a.usuario_id
     LEFT JOIN dre dre ON dre.id = a.dre_id
     LEFT JOIN ue ue ON ue.id = a.ue_id
     LEFT JOIN turma turma ON turma.id = a.turma_id;


-- public.v_abrangencia_usuario source

CREATE OR REPLACE VIEW public.v_abrangencia_usuario
AS SELECT COALESCE(turma.dre_codigo, ue.dre_codigo, dre.dre_codigo) AS dre_codigo,
    COALESCE(turma.dre_abreviacao, ue.dre_abreviacao, dre.dre_abreviacao) AS dre_abreviacao,
    COALESCE(turma.dre_nome, ue.dre_nome, dre.dre_nome) AS dre_nome,
    a.usuario_id,
    a.perfil AS usuario_perfil,
    u.login,
    COALESCE(turma.ue_codigo, ue.ue_codigo, dre.ue_codigo) AS ue_codigo,
    COALESCE(turma.ue_nome, ue.ue_nome, dre.ue_nome) AS ue_nome,
    COALESCE(turma.turma_ano, ue.turma_ano, dre.turma_ano) AS turma_ano,
    COALESCE(turma.turma_ano_letivo, ue.turma_ano_letivo, dre.turma_ano_letivo) AS turma_ano_letivo,
    COALESCE(turma.modalidade_codigo, ue.modalidade_codigo, dre.modalidade_codigo) AS modalidade_codigo,
    COALESCE(turma.turma_nome, ue.turma_nome, dre.turma_nome) AS turma_nome,
    COALESCE(turma.turma_semestre, ue.turma_semestre, dre.turma_semestre) AS turma_semestre,
    COALESCE(turma.qt_duracao_aula, ue.qt_duracao_aula, dre.qt_duracao_aula) AS qt_duracao_aula,
    COALESCE(turma.tipo_turno, ue.tipo_turno, dre.tipo_turno) AS tipo_turno,
    COALESCE(turma.turma_codigo, ue.turma_codigo, dre.turma_codigo) AS turma_id
   FROM abrangencia a
     JOIN usuario u ON a.usuario_id = u.id
     LEFT JOIN v_abrangencia_cadeia_dres_somente dre ON dre.dre_id = a.dre_id
     LEFT JOIN v_abrangencia_cadeia_ues_somente ue ON ue.ue_id = a.ue_id
     LEFT JOIN v_abrangencia_cadeia_turmas turma ON a.dre_id = turma.dre_id OR a.ue_id = turma.ue_id OR turma.turma_id = a.turma_id
  WHERE a.historico = false AND (COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) = false OR COALESCE(turma.turma_historica, ue.turma_historica, dre.turma_historica) IS NULL);


-- public.v_estrutura_abrangencia_dres source

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_dres
AS SELECT v_abrangencia_cadeia_turmas.dre_abreviacao AS abreviacao,
    v_abrangencia_cadeia_turmas.dre_codigo AS codigo,
    v_abrangencia_cadeia_turmas.dre_nome AS nome,
    v_abrangencia_cadeia_turmas.modalidade_codigo,
    v_abrangencia_cadeia_turmas.dre_id
   FROM v_abrangencia_cadeia_turmas;


-- public.v_estrutura_abrangencia_turmas source

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_turmas
AS SELECT v_abrangencia_cadeia_turmas.turma_ano AS ano,
    v_abrangencia_cadeia_turmas.turma_ano_letivo AS anoletivo,
    v_abrangencia_cadeia_turmas.turma_codigo AS codigo,
    v_abrangencia_cadeia_turmas.modalidade_codigo AS codigomodalidade,
    v_abrangencia_cadeia_turmas.turma_nome AS nome,
    v_abrangencia_cadeia_turmas.turma_semestre AS semestre,
    v_abrangencia_cadeia_turmas.qt_duracao_aula AS qtduracaoaula,
    v_abrangencia_cadeia_turmas.tipo_turno AS tipoturno,
    v_abrangencia_cadeia_turmas.ensino_especial AS ensinoespecial,
    v_abrangencia_cadeia_turmas.turma_id
   FROM v_abrangencia_cadeia_turmas;


-- public.v_estrutura_abrangencia_turmas_tipos source

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_turmas_tipos
AS SELECT act.turma_ano AS ano,
    act.turma_ano_letivo AS anoletivo,
    act.turma_codigo AS codigo,
    act.modalidade_codigo AS codigomodalidade,
    act.turma_nome AS nome,
    act.turma_semestre AS semestre,
    act.qt_duracao_aula AS qtduracaoaula,
    act.tipo_turno AS tipoturno,
    act.ensino_especial AS ensinoespecial,
    act.turma_id,
    act.tipo_turma AS tipoturma
   FROM v_abrangencia_cadeia_turmas act;


-- public.v_estrutura_abrangencia_ues source

CREATE OR REPLACE VIEW public.v_estrutura_abrangencia_ues
AS SELECT act.ue_codigo AS codigo,
    act.ue_nome AS nome,
    ue.tipo_escola AS tipoescola,
    act.modalidade_codigo,
    act.ue_id
   FROM v_abrangencia_cadeia_turmas act
     JOIN ue ON act.ue_id = act.ue_id;


-- public.v_estrutura_eventos source

CREATE OR REPLACE VIEW public.v_estrutura_eventos
AS SELECT e.id AS eventoid,
    e.nome,
    e.descricao AS descricaoevento,
    e.data_inicio,
    e.data_fim,
    e.dre_id,
    e.letivo,
    e.feriado_id,
    e.tipo_calendario_id,
    e.tipo_evento_id,
    e.ue_id,
    e.criado_em,
    e.criado_por,
    e.alterado_em,
    e.alterado_por,
    e.criado_rf,
    e.alterado_rf,
    e.status,
    et.id AS tipoeventoid,
    et.ativo,
    et.tipo_data,
    et.descricao AS descricaotipoevento,
    et.excluido,
    et.local_ocorrencia
   FROM evento e
     JOIN evento_tipo et ON e.tipo_evento_id = et.id;


-- public.v_estrutura_eventos_calendario source

CREATE OR REPLACE VIEW public.v_estrutura_eventos_calendario
AS SELECT evento.id,
    evento.data_inicio AS data_evento,
    '(in�cio)'::text AS iniciofimdesc,
    evento.nome,
    'aaaa'::text AS tipoevento
   FROM evento;


-- public.v_estrutura_eventos_calendario_dias_com_eventos_no_mes source

CREATE OR REPLACE VIEW public.v_estrutura_eventos_calendario_dias_com_eventos_no_mes
AS SELECT date_part('day'::text, CURRENT_DATE) AS dia,
    'AAAA'::text AS tipoevento;


-- public.v_estrutura_eventos_listar source

CREATE OR REPLACE VIEW public.v_estrutura_eventos_listar
AS SELECT e.id AS eventoid,
    e.nome,
    e.descricao AS descricaoevento,
    e.data_inicio,
    e.data_fim,
    e.dre_id,
    e.letivo,
    e.feriado_id,
    e.tipo_calendario_id,
    e.tipo_evento_id,
    e.ue_id,
    e.criado_em,
    e.criado_por,
    e.alterado_em,
    e.alterado_por,
    e.criado_rf,
    e.alterado_rf,
    e.status,
    et.id AS tipoeventoid,
    et.ativo,
    et.tipo_data,
    et.descricao AS descricaotipoevento,
    et.excluido,
    0 AS total_registros
   FROM evento e
     JOIN evento_tipo et ON e.tipo_evento_id = et.id;



CREATE OR REPLACE FUNCTION public.f_abrangencia_anos_letivos(p_login character varying, p_perfil_id uuid, p_historico boolean)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico
	 
union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico

union

select distinct act.turma_ano_letivo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false));	  
$function$
;

CREATE OR REPLACE FUNCTION public.f_abrangencia_dres(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_dres
 LANGUAGE sql
AS $function$
select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo,
                act.dre_id as id 
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and		  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
	 
union 

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo,
                act.dre_id
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union 

select distinct act.dre_abreviacao,
				act.dre_codigo,
                act.dre_nome,
                act.modalidade_codigo,
                act.dre_id
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$
;

CREATE OR REPLACE FUNCTION public.f_abrangencia_modalidades(p_login character varying, p_perfil_id uuid, p_historico boolean, p_ano_letivo integer, p_ignorar_modalidades integer[] DEFAULT NULL::integer[])
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.modalidade_codigo
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and      
	  act.turma_ano_letivo = p_ano_letivo
	  and (p_ignorar_modalidades is null or not(act.modalidade_codigo = ANY(p_ignorar_modalidades)))
	 
union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and      	  
	  act.turma_ano_letivo = p_ano_letivo
	  and (p_ignorar_modalidades is null or not(act.modalidade_codigo = ANY(p_ignorar_modalidades)))

union

select distinct act.modalidade_codigo
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and	        
	  act.turma_ano_letivo = p_ano_letivo
	 and (p_ignorar_modalidades is null or not(act.modalidade_codigo = ANY(p_ignorar_modalidades)));
$function$
;

CREATE OR REPLACE FUNCTION public.f_abrangencia_semestres(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF integer
 LANGUAGE sql
AS $function$
select distinct act.turma_semestre
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and      
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
	 
union

select distinct act.turma_semestre
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union

select distinct act.turma_semestre
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$
;

CREATE OR REPLACE FUNCTION public.f_abrangencia_turmas(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ue_codigo character varying DEFAULT NULL::character varying, p_ano_letivo integer DEFAULT 0)
 RETURNS SETOF v_estrutura_abrangencia_turmas
 LANGUAGE sql
AS $function$
select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno,
				act.ensino_especial,
				act.turma_id 
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
	  (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
	 
union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno,
				act.ensino_especial ,
				act.turma_id
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and	 
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))

union

select distinct act.turma_ano,
				act.turma_ano_letivo,
				act.turma_codigo,
				act.modalidade_codigo,
				act.turma_nome,
				act.turma_semestre,
				act.qt_duracao_aula,
				act.tipo_turno,
				act.ensino_especial,
				act.turma_id
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 			
	  a.perfil_id = p_perfil_id and	  
	  a.historico = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_ue_codigo is null or (p_ue_codigo is not null and act.ue_codigo = p_ue_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo));
$function$
;

CREATE OR REPLACE FUNCTION public.f_abrangencia_turmas_tipos(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_ue_codigo character varying DEFAULT NULL::character varying, p_ano_letivo integer DEFAULT 0, p_tipos_turma integer[] DEFAULT NULL::integer[])
 RETURNS SETOF v_estrutura_abrangencia_turmas_tipos
 LANGUAGE sql
AS $function$
select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma
from v_abrangencia_nivel_dre a
    inner join v_abrangencia_cadeia_turmas act on a.dre_id = act.dre_id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and act.turma_historica = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
union
select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma
from v_abrangencia_nivel_ue a
    inner join v_abrangencia_cadeia_turmas act on a.ue_id = act.ue_id
    inner join ue on act.ue_id = ue.id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and act.turma_historica = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
union
select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma
from v_abrangencia_nivel_turma a
    inner join v_abrangencia_cadeia_turmas act on a.turma_id = act.turma_id
    inner join ue on act.ue_id = ue.id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and a.historico = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    ) $function$
;

CREATE OR REPLACE FUNCTION public.f_abrangencia_ues(p_login character varying, p_perfil_id uuid, p_historico boolean, p_modalidade_codigo integer DEFAULT 0, p_turma_semestre integer DEFAULT 0, p_dre_codigo character varying DEFAULT NULL::character varying, p_ano_letivo integer DEFAULT 0, p_ignorar_tipos_ue integer[] DEFAULT NULL::integer[])
 RETURNS SETOF v_estrutura_abrangencia_ues
 LANGUAGE sql
AS $function$
select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo,
                act.ue_id 
	from v_abrangencia_nivel_dre a
		inner join v_abrangencia_cadeia_turmas act
			on a.dre_id = act.dre_id
		inner join ue
			on act.ue_id = ue.id		
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and	  
	  act.turma_historica = p_historico and
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
	  (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
      and (p_ignorar_tipos_ue is null or not(ue.tipo_escola = ANY(p_ignorar_tipos_ue)))

union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo,
                act.ue_id
	from v_abrangencia_nivel_ue a
		inner join v_abrangencia_cadeia_turmas act
			on a.ue_id = act.ue_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  act.turma_historica = p_historico and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
      and (p_ignorar_tipos_ue is null or not(ue.tipo_escola = ANY(p_ignorar_tipos_ue)))

union

select distinct act.ue_codigo,
				act.ue_nome,
                ue.tipo_escola,
                act.modalidade_codigo,
                act.ue_id
	from v_abrangencia_nivel_turma a
		inner join v_abrangencia_cadeia_turmas act
			on a.turma_id = act.turma_id
		inner join ue
			on act.ue_id = ue.id
where a.login = p_login and 
	  a.perfil_id = p_perfil_id and
	  ((p_historico = true and a.historico = true) or
	   (p_historico = false and a.historico  = false and act.turma_historica = false)) and	  
      (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and act.modalidade_codigo = p_modalidade_codigo)) and
      (p_turma_semestre = 0 or (p_turma_semestre <> 0 and act.turma_semestre = p_turma_semestre)) and
      (p_dre_codigo is null or (p_dre_codigo is not null and act.dre_codigo = p_dre_codigo)) and
      (p_ano_letivo = 0 or (p_ano_letivo <> 0 and act.turma_ano_letivo = p_ano_letivo))
      and (p_ignorar_tipos_ue is null or not(ue.tipo_escola = ANY(p_ignorar_tipos_ue)))

$function$
;

CREATE OR REPLACE FUNCTION public.f_cria_constraint_se_nao_existir(tabela_nome text, constraint_nome text, constraint_sql text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
  begin
    -- Look for our constraint
    if not exists (select constraint_name
                   from information_schema.constraint_column_usage
                   where table_name = tabela_nome  and constraint_name = constraint_nome) then
        execute 'ALTER TABLE ' || tabela_nome || ' ADD CONSTRAINT ' || constraint_nome || ' ' || constraint_sql;
    end if;
end;
$function$
;

CREATE OR REPLACE FUNCTION public.f_cria_fk_se_nao_existir(tabela_nome text, constraint_nome text, fk_sql text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
  begin
    -- Look for our FK
    if not exists (select constraint_name
                   from information_schema.table_constraints
                   where table_name = tabela_nome  and constraint_name = constraint_nome) then
        execute 'ALTER TABLE ' || tabela_nome || ' ADD CONSTRAINT ' || constraint_nome || ' ' || fk_sql;
    end if;
end;
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos(p_login character varying, p_perfil_id uuid, p_historico boolean, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_desconsidera_local_dre boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date)
 RETURNS SETOF v_estrutura_eventos
 LANGUAGE sql
AS $function$
	select e.id,
		   e.nome,
		   e.descricao,
		   e.data_inicio,
		   e.data_fim,
		   e.dre_id,
		   e.letivo,
		   e.feriado_id,
		   e.tipo_calendario_id,
		   e.tipo_evento_id,
		   e.ue_id,
		   e.criado_em,
		   e.criado_por,
	       e.alterado_em,
	       e.alterado_por,
	       e.criado_rf,
		   e.alterado_rf,
		   e.status,	
		   et.id,
		   et.ativo,
		   et.tipo_data,
		   et.descricao,
		   et.excluido,
		   et.local_ocorrencia		   
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
			left join f_abrangencia_dres(p_login, p_perfil_id, p_historico) ad
				on e.dre_id = ad.codigo 
				-- modalidade 1 (fundamental/medio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (medio)
				-- modalidade 2 (EJA) do tipo de calendario, considera modalidade 3 (EJA)
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				-- para DREs considera local da ocorrencia 2 (DRE) e 5 (Todos)
				and et.local_ocorrencia in (2,3,5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorrencia 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1,3,4, 5)
	where not e.excluido 
		and not et.excluido 
		and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo) 
		and e.tipo_calendario_id = p_tipo_calendario_id		
		-- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		and ((p_dre_id is null and ((e.dre_id is null and e.ue_id is null) or e.dre_id in (select codigo from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or (p_dre_id is not null and ((e.dre_id is null and e.ue_id is null) or e.dre_id = p_dre_id)))
		and ((p_ue_id is null and (e.ue_id is null or e.ue_id in (select codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or (p_ue_id is not null and (e.ue_id is null or e.ue_id = p_ue_id)))
		-- caso desconsidere 6 (liberacao excepcional) e 15 (reposicao de recesso)
		and (p_desconsidera_liberacao_excep_reposicao_recesso = true or (p_desconsidera_liberacao_excep_reposicao_recesso = false and et.codigo not in (6, 15)))
		and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
		and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_dias_com_eventos_no_mes(p_login character varying, p_perfil_id uuid, p_historico boolean, p_mes integer, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario_dias_com_eventos_no_mes
 LANGUAGE sql
AS $function$ 	
select lista.dia,
	   lista.tipoEvento
from (

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_data_inicio_fim(p_login, p_perfil_id, p_historico, p_mes, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)

union 

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_rf_criador(p_login, p_mes, p_tipo_calendario_id, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)) lista
order by 1;
 $function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_eventos_do_dia(p_login character varying, p_perfil_id uuid, p_historico boolean, p_dia integer, p_mes integer, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	
	select id,
		   data_evento,
		   iniciofimdesc,
		   nome,
		   tipoevento
		from f_eventos_calendario_por_data_inicio_fim(p_login, p_perfil_id, p_historico, p_mes, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)
	where extract(day from data_evento) = p_dia
	
	union
	
	select id,
		   data_evento,
		   iniciofimdesc,
		   nome,
		   tipoevento
		from f_eventos_calendario_por_rf_criador(p_login, p_mes, p_tipo_calendario_id, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)
	where extract(day from data_evento) = p_dia;
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_por_data_inicio_fim(p_login character varying, p_perfil_id uuid, p_historico boolean, p_mes integer, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	select distinct e.id,
					e.data_inicio,
					case
						when data_inicio = data_fim then ''
						else '(inicio)'
					end descricao_inicio_fim,
					e.nome,
					case
						when e.dre_id is not null and e.ue_id is null then 'DRE'
				      	when e.dre_id is not null and e.ue_id is not null then 'UE'
						else 'SME'
					end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
			left join f_abrangencia_dres(p_login, p_perfil_id, p_historico) ad
				on e.dre_id = ad.codigo 
				-- modalidade 1 (fundamental/medio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (medio)
				-- modalidade 2 (EJA) do tipo de calendario, considera modalidade 3 (EJA)
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				-- para DREs considera local da ocorrencia 2 (DRE) e 5 (Todos)
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				-- para UEs considera local da ocorrencia 1 (UE) e 4 (SME/UE) e 5 (Todos)
				and et.local_ocorrencia in (1, 4, 5)
	where et.ativo 
		and not et.excluido
		and not e.excluido
		and extract(month from e.data_inicio) = p_mes
		and extract(year from e.data_inicio) = tc.ano_letivo	
		and e.tipo_calendario_id = p_tipo_calendario_id
		-- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and ((p_dre_id is null and ((e.dre_id is null and e.ue_id is null) or e.dre_id in (select codigo from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or (p_dre_id is not null and ((e.dre_id is null and e.ue_id is null) or e.dre_id = p_dre_id)))
		and ((p_ue_id is null and (e.ue_id is null or e.ue_id in (select codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or (p_ue_id is not null and (e.ue_id is null or e.ue_id = p_ue_id)))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and not (e.dre_id is null and e.ue_id is null)))
		and (-- tem perfil exclusivo pro tipo evento
			exists(select 1 from perfil_evento_tipo pet where not pet.excluido and pet.codigo_perfil = p_perfil_id and pet.evento_tipo_id = et.id and pet.exclusivo)
			-- nao tem tipo exclusivo para o perfil
		 or (not exists(select 1 from perfil_evento_tipo pet where not pet.excluido and pet.codigo_perfil = p_perfil_id and pet.exclusivo)
		 -- tem perfil com acesso tambem ao tipo de evento, nao exclusivo para esse tipo
		  and (exists (select 1 from perfil_evento_tipo pet where not pet.excluido and pet.codigo_perfil = p_perfil_id and pet.evento_tipo_id = et.id and not pet.exclusivo)
		 -- nao tem restricao de acesso para o tipo de evento
		 or not exists (select 1 from perfil_evento_tipo pet where not pet.excluido and pet.evento_tipo_id = et.id)))
		)

	union

	select distinct e.id,
					e.data_fim,
					'(fim)',
					e.nome,
					case
						when e.dre_id is not null and e.ue_id is null then 'DRE'
				      	when e.dre_id is not null and e.ue_id is not null then 'UE'
						else 'SME'
					end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
			left join f_abrangencia_dres(p_login, p_perfil_id, p_historico) ad
				on e.dre_id = ad.codigo 
				and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and ad.modalidade_codigo = 3))
				and et.local_ocorrencia in (2, 5)
			left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
				on e.ue_id = au.codigo
				and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or (tc.modalidade = 2 and au.modalidade_codigo = 3))
				and et.local_ocorrencia in (1, 4, 5)
	where e.data_inicio <> e.data_fim
		and et.ativo 
		and not et.excluido
		and not e.excluido
		and extract(month from e.data_fim) = p_mes
		and extract(year from e.data_inicio) = tc.ano_letivo
		and e.tipo_calendario_id = p_tipo_calendario_id
		-- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
		and ((p_considera_pendente_aprovacao = true and e.status in (1,2)) or (p_considera_pendente_aprovacao = false and e.status = 1)) 
		and ((p_dre_id is null and ((e.dre_id is null and e.ue_id is null) or e.dre_id in (select codigo from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or (p_dre_id is not null and ((e.dre_id is null and e.ue_id is null) or e.dre_id = p_dre_id)))
		and ((p_ue_id is null and (e.ue_id is null or e.ue_id in (select codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or (p_ue_id is not null and (e.ue_id is null or e.ue_id = p_ue_id)))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false  or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere evento SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and not (e.dre_id is null and e.ue_id is null)));	
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_calendario_por_rf_criador(p_login character varying, p_mes integer, p_tipo_calendario_id bigint, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario
 LANGUAGE sql
AS $function$
	select e.id,
		   e.data_inicio,
		   case 
		      when e.data_inicio = e.data_fim then ''
		      else '(inicio)'
		   end descricao_incio_fim,		   
		   e.nome,		   
		   case 
		      when e.dre_id is not null and e.ue_id is null then 'DRE'
			  when e.dre_id is not null and e.ue_id is not null then 'UE'
		      else 'SME'
		   end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id		
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
	where et.ativo
		and not et.excluido
		and not e.excluido
		-- considera somente pendente de aprovacao
		and e.status = 2
		and e.criado_rf = p_login
		and extract(month from e.data_inicio) = p_mes
		and extract(year from e.data_inicio) = tc.ano_letivo
		and e.tipo_calendario_id = p_tipo_calendario_id
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere eventos SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and not (e.dre_id is null and e.ue_id is null)))
		
	union
	
	select e.id,
		   e.data_inicio,
		   '(fim)' descricao_incio_fim,		   
		   e.nome,		   
		   case 
		      when e.dre_id is not null and e.ue_id is null then 'DRE'
			  when e.dre_id is not null and e.ue_id is not null then 'UE'
		      else 'SME'
		   end tipoEvento
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id		
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
	where e.data_inicio <> e.data_fim
		and et.ativo
		and not et.excluido
		and not e.excluido
		-- considera somente pendente de aprovacao
		and e.status = 2
		and e.criado_rf = p_login
		and extract(month from e.data_fim) = p_mes
		and extract(year from e.data_fim) = tc.ano_letivo
		and e.tipo_calendario_id = p_tipo_calendario_id
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		-- caso desconsidere o local do evento 2 (DRE)
		and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
		-- caso desconsidere eventos SME
		and (p_desconsidera_evento_sme = false or (p_desconsidera_evento_sme = true and not (e.dre_id is null and e.ue_id is null)));;
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_listar(p_login character varying, p_perfil_id uuid, p_historico boolean, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_desconsidera_local_dre boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date, p_qtde_registros_ignorados integer DEFAULT 0, p_qtde_registros integer DEFAULT 0, p_tipo_evento_id bigint DEFAULT NULL::bigint, p_nome_evento character varying DEFAULT NULL::character varying)
 RETURNS SETOF v_estrutura_eventos_listar
 LANGUAGE plpgsql
AS $function$
	declare 
		total_registros_obtido int4;
	begin
		total_registros_obtido := int4((select count(0) from f_eventos_listar_sem_paginacao(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_id, p_ue_id, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim, p_tipo_evento_id, p_nome_evento)));
		
		if (p_qtde_registros_ignorados > 0 and p_qtde_registros > 0) then
			return query select eventoid,
								nome,
								descricaoevento,
								data_inicio,
								data_fim,
								dre_id,
								letivo,
								feriado_id,
								tipo_calendario_id,
								tipo_evento_id,
								ue_id,
								criado_em,
								criado_por,
							    alterado_em,
							    alterado_por,
							    criado_rf,
								alterado_rf,
								status,	
								tipoeventoid,
								ativo,
								tipo_data,
								descricaotipoevento,
								excluido,
								total_registros_obtido
							from f_eventos_listar_sem_paginacao(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_id, p_ue_id, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim, p_tipo_evento_id, p_nome_evento)
							offset p_qtde_registros_ignorados rows fetch next p_qtde_registros rows only;
		else
			return query select eventoid,
								nome,
								descricaoevento,
								data_inicio,
								data_fim,
								dre_id,
								letivo,
								feriado_id,
								tipo_calendario_id,
								tipo_evento_id,
								ue_id,
								criado_em,
								criado_por,
							    alterado_em,
							    alterado_por,
							    criado_rf,
								alterado_rf,
								status,	
								tipoeventoid,
								ativo,
								tipo_data,
								descricaotipoevento,
								excluido,
								total_registros_obtido
							from f_eventos_listar_sem_paginacao(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_id, p_ue_id, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim, p_tipo_evento_id, p_nome_evento);
		end if;
	END;
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_listar_sem_paginacao(p_login character varying, p_perfil_id uuid, p_historico boolean, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_desconsidera_local_dre boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date, p_tipo_evento_id bigint DEFAULT NULL::bigint, p_nome_evento character varying DEFAULT NULL::character varying)
 RETURNS SETOF v_estrutura_eventos_listar
 LANGUAGE sql
AS $function$
	select eventoid,
		   nome,
		   descricaoevento,
		   data_inicio,
		   data_fim,
		   dre_id,
		   letivo,
		   feriado_id,
		   tipo_calendario_id,
		   tipo_evento_id,
		   ue_id,
		   criado_em,
		   criado_por,
	       alterado_em,
	       alterado_por,
	       criado_rf,
		   alterado_rf,
		   status,	
		   tipoeventoid,
		   ativo,
		   tipo_data,
		   descricaotipoevento,
		   excluido,
		   int4(0) total_registros
		from (
			select eventoid,
				   nome,
				   descricaoevento,
				   data_inicio,
				   data_fim,
				   dre_id,
				   letivo,
				   feriado_id,
				   tipo_calendario_id,
				   tipo_evento_id,
				   ue_id,
				   criado_em,
				   criado_por,
			       alterado_em,
			       alterado_por,
			       criado_rf,
				   alterado_rf,
				   status,	
				   tipoeventoid,
				   ativo,
				   tipo_data,
				   descricaotipoevento,
				   excluido				   				   
				from f_eventos(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_desconsidera_local_dre, p_dre_id, p_ue_id, p_desconsidera_liberacao_excep_reposicao_recesso, p_data_inicio, p_data_fim)
			
			union
			
			select eventoid,
				   nome,
				   descricaoevento,
				   data_inicio,
				   data_fim,
				   dre_id,
				   letivo,
				   feriado_id,
				   tipo_calendario_id,
				   tipo_evento_id,
				   ue_id,
				   criado_em,
				   criado_por,
			       alterado_em,
			       alterado_por,
			       criado_rf,
				   alterado_rf,
				   status,	
				   tipoeventoid,
				   ativo,
				   tipo_data,
				   descricaotipoevento,
				   excluido
				from f_eventos_por_rf_criador(p_login, p_tipo_calendario_id, p_dre_id, p_ue_id, p_data_inicio, p_data_fim)) lista
	where (p_tipo_evento_id is null or (p_tipo_evento_id is not null and tipo_evento_id = p_tipo_evento_id)) and
  		  (p_nome_evento is null or (p_nome_evento is not null and upper(nome) like upper('%' || p_nome_evento || '%')));
$function$
;

CREATE OR REPLACE FUNCTION public.f_eventos_por_rf_criador(p_login character varying, p_tipo_calendario_id bigint, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date)
 RETURNS SETOF v_estrutura_eventos
 LANGUAGE sql
AS $function$
	select e.id,
		   e.nome,
		   e.descricao,
		   e.data_inicio,
		   e.data_fim,
		   e.dre_id,
		   e.letivo,
		   e.feriado_id,
		   e.tipo_calendario_id,
		   e.tipo_evento_id,
		   e.ue_id,
		   e.criado_em,
		   e.criado_por,
	       e.alterado_em,
	       e.alterado_por,
	       e.criado_rf,
		   e.alterado_rf,
		   e.status,	
		   et.id,
		   et.ativo,
		   et.tipo_data,
		   et.descricao,
		   et.excluido,
		   et.local_ocorrencia
		from evento e
			inner join evento_tipo et
				on e.tipo_evento_id = et.id		
			inner join tipo_calendario tc
				on e.tipo_calendario_id = tc.id
	where not et.excluido
		and not e.excluido
		-- considera somente pendente de aprova��o
		and e.status = 2
		and e.criado_rf = p_login		
		and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo)
		and e.tipo_calendario_id = p_tipo_calendario_id
		and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
		and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
		and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
		and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$function$
;

CREATE OR REPLACE FUNCTION public.f_unaccent(text)
 RETURNS text
 LANGUAGE sql
 IMMUTABLE
AS $function$
SELECT public.unaccent('public.unaccent', $1)  -- schema-qualify function and dictionary
$function$
;

CREATE OR REPLACE FUNCTION public.migrate_plano_anual_not_migrado()
 RETURNS void
 LANGUAGE plpgsql
AS $function$
declare 
	turmaId  bigint;
    bimestre integer;
    componenteCurricularId bigint;
    planos record;
	 
begin
	for planos in 
		select pa.turma_id, pa.bimestre, cc.codigo_eol
		  from plano_anual pa
		 inner join objetivo_aprendizagem_plano oap on oap.plano_id = pa.id
		 inner join componente_curricular_jurema cc on cc.id = oap.componente_curricular_id
		 where not migrado
	loop
		raise notice 'Turma % - Bimestre % - Componente %', turmaId, bimestre, componenteCurricularId;
	end loop;
	
end;
$function$
;

CREATE OR REPLACE FUNCTION public.pg_stat_statements(showtext boolean, OUT userid oid, OUT dbid oid, OUT queryid bigint, OUT query text, OUT calls bigint, OUT total_time double precision, OUT min_time double precision, OUT max_time double precision, OUT mean_time double precision, OUT stddev_time double precision, OUT rows bigint, OUT shared_blks_hit bigint, OUT shared_blks_read bigint, OUT shared_blks_dirtied bigint, OUT shared_blks_written bigint, OUT local_blks_hit bigint, OUT local_blks_read bigint, OUT local_blks_dirtied bigint, OUT local_blks_written bigint, OUT temp_blks_read bigint, OUT temp_blks_written bigint, OUT blk_read_time double precision, OUT blk_write_time double precision)
 RETURNS SETOF record
 LANGUAGE c
 PARALLEL SAFE STRICT
AS '$libdir/pg_stat_statements', $function$pg_stat_statements_1_3$function$
;

CREATE OR REPLACE FUNCTION public.pg_stat_statements_reset()
 RETURNS void
 LANGUAGE c
 PARALLEL SAFE
AS '$libdir/pg_stat_statements', $function$pg_stat_statements_reset$function$
;

CREATE OR REPLACE FUNCTION public.postgres_fdw_handler()
 RETURNS fdw_handler
 LANGUAGE c
 STRICT
AS '$libdir/postgres_fdw', $function$postgres_fdw_handler$function$
;

CREATE OR REPLACE FUNCTION public.postgres_fdw_validator(text[], oid)
 RETURNS void
 LANGUAGE c
 STRICT
AS '$libdir/postgres_fdw', $function$postgres_fdw_validator$function$
;

CREATE OR REPLACE FUNCTION public.unaccent(text)
 RETURNS text
 LANGUAGE c
 STABLE PARALLEL SAFE STRICT
AS '$libdir/unaccent', $function$unaccent_dict$function$
;

CREATE OR REPLACE FUNCTION public.unaccent(regdictionary, text)
 RETURNS text
 LANGUAGE c
 STABLE PARALLEL SAFE STRICT
AS '$libdir/unaccent', $function$unaccent_dict$function$
;

CREATE OR REPLACE FUNCTION public.unaccent_init(internal)
 RETURNS internal
 LANGUAGE c
 PARALLEL SAFE
AS '$libdir/unaccent', $function$unaccent_init$function$
;

CREATE OR REPLACE FUNCTION public.unaccent_lexize(internal, internal, internal, internal)
 RETURNS internal
 LANGUAGE c
 PARALLEL SAFE
AS '$libdir/unaccent', $function$unaccent_lexize$function$
;
