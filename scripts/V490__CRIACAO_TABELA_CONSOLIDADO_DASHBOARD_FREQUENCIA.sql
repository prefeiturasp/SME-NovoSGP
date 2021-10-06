-- DROP TABLE public.consolidado_dashboard_frequencia;

CREATE TABLE IF NOT EXISTS public.consolidado_dashboard_frequencia
(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	
	turma_nome varchar (15) null,	
	turma_id int8 NOT NULL,
    turma_ano char (1) NOT NULL,
    data_aula timestamp NOT NULL,    
    modalidade_codigo INT4 NOT NULL,
	data_inicio_semana timestamp NULL,
	data_fim_semana timestamp NULL,
	mes int4 NULL,
	tipo int4 NOT NULL,	
    ano_letivo int4 NOT NULL,
    dre_id int8 NOT NULL,
    ue_id int8 NOT NULL, 
	dre_abreviacao varchar (15) NOT NULL,
	quantidade_presencas int8 NOT NULL,   
	quantidade_ausencias int8 NOT NULL,
	quantidade_remotos int8 NOT NULL,
	criado_em timestamp NOT NULL, 
	
	CONSTRAINT consolidado_dashboard_frequencia_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists consolidado_dashboard_frequencia_turma_id_ix ON public.consolidado_dashboard_frequencia USING btree (turma_id);

CREATE INDEX if not exists consolidado_dashboard_frequencia_dre_id_ix ON public.consolidado_dashboard_frequencia USING btree (dre_id);

CREATE INDEX if not exists consolidado_dashboard_frequencia_ue_id_ix ON public.consolidado_dashboard_frequencia USING btree (ue_id);
