CREATE TABLE IF NOT EXISTS public.supervisor_escola_dre
(
    id bigint NOT null generated always as identity,
    supervisor_id varchar(15) not null,
    escola_id varchar(15) not null,
    dre_id varchar(15) not null,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT supervisor_escola_pk PRIMARY KEY (id),
    constraint supervisor_escola_dre_ck  UNIQUE (supervisor_id, escola_id, dre_id)
)