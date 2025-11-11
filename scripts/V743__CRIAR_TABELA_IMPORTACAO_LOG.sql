CREATE table IF NOT EXISTS public.importacao_log(
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    nome_arquivo varchar(200) NOT NULL,
    tipo_arquivo_importacao varchar(200) NOT NULL,
    data_inicio_processamento timestamp NOT NULL,
    data_fim_processamento timestamp NULL,
    total_registros int8 NULL,
    registros_processados int8 NULL,
    registros_com_falha int8 NULL,
    status_importacao varchar(200) NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT importacao_log_pk PRIMARY KEY (id)
);
 