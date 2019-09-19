CREATE TABLE IF NOT EXISTS public.usuario
(
    id bigint NOT NULL generated always as identity,
    rf_codigo varchar(12)  NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT usuario_pk PRIMARY KEY (id)

);

CREATE INDEX usuario_codigo_rf_idx ON public.usuario (rf_codigo);

