CREATE table IF NOT EXISTS public.painel_educacional_visao_geral(
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_dre varchar(200) NOT NULL,
    codigo_ue varchar(200) NOT NULL,
    ano_letivo int8 NULL,
    indicador varchar(200) NOT NULL,
    serie varchar(200) NULL,
    valor numeric(5,2) NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT painel_educacional_visao_geral_pk PRIMARY KEY (id)
);
 