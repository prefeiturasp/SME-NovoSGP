CREATE table IF NOT EXISTS public.idep(
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int8 NOT NULL,
    serie_ano int8 NOT NULL,
    codigo_eol_escola varchar(200) NOT NULL,
    nota numeric(5,2) NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT idep_pk PRIMARY KEY (id)
);
 