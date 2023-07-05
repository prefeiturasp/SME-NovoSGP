CREATE table IF NOT EXISTS public.configuracao_relatorio_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY ,
	inicio_vigencia date not null,
	fim_vigencia date,
	tipo_periodicidade varchar(1) NOT NULL,--(S)emestre ou (B)imestre
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT configuracao_relatorio_pap_pk PRIMARY KEY (id)
);

CREATE table IF NOT EXISTS public.periodo_relatorio_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY ,
	configuracao_relatorio_pap_id int8 NOT NULL, 
	periodo int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT periodo_relatorio_pap_pk PRIMARY KEY (id)
);

CREATE INDEX periodo_relatorio_pap_config_idx ON public.periodo_relatorio_pap USING btree (configuracao_relatorio_pap_id);
ALTER TABLE public.periodo_relatorio_pap ADD CONSTRAINT periodo_relatorio_pap_config_fk 
FOREIGN KEY (configuracao_relatorio_pap_id) REFERENCES public.configuracao_relatorio_pap(id);


CREATE table IF NOT EXISTS public.periodo_escolar_relatorio_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY ,
	periodo_relatorio_pap_id int8 NOT NULL, 
	periodo_escolar_id int8 NOT NULL, 	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT periodo_escolar_relatorio_pap_pk PRIMARY KEY (id)
);

CREATE INDEX periodo_escolar_relatorio_pap_periodo_pap_idx ON public.periodo_escolar_relatorio_pap USING btree (periodo_relatorio_pap_id);
ALTER TABLE public.periodo_escolar_relatorio_pap ADD CONSTRAINT periodo_escolar_relatorio_pap_periodo_pap_fk 
FOREIGN KEY (periodo_relatorio_pap_id) REFERENCES public.periodo_relatorio_pap(id);

CREATE INDEX periodo_escolar_relatorio_pap_periodo_escolar_idx ON public.periodo_escolar_relatorio_pap USING btree (periodo_escolar_id);
ALTER TABLE public.periodo_escolar_relatorio_pap ADD CONSTRAINT periodo_escolar_relatorio_pap_periodo_escolar_fk 
FOREIGN KEY (periodo_escolar_id) REFERENCES public.periodo_escolar(id);


CREATE table public.secao_relatorio_periodico_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questionario_id int8 not null,
    configuracao_relatorio_pap_id int8 NOT NULL, 
	nome_componente varchar(50),
	nome varchar,
	ordem int4,
	etapa int4,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT secao_relatorio_periodico_pap_pk PRIMARY KEY (id)
);

CREATE INDEX secao_relatorio_periodico_pap_questionario_idx ON public.secao_relatorio_periodico_pap USING btree (questionario_id);
ALTER TABLE public.secao_relatorio_periodico_pap ADD CONSTRAINT secao_relatorio_periodico_pap_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id);

CREATE INDEX secao_relatorio_periodico_pap_config_idx ON public.secao_relatorio_periodico_pap USING btree (configuracao_relatorio_pap_id);
ALTER TABLE public.secao_relatorio_periodico_pap ADD CONSTRAINT secao_relatorio_periodico_pap_config_fk 
FOREIGN KEY (configuracao_relatorio_pap_id) REFERENCES public.configuracao_relatorio_pap(id);
