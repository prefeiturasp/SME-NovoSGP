CREATE TABLE IF NOT EXISTS public.tipo_ciclo
(
    id bigint NOT NULL,
    descricao character varying COLLATE pg_catalog."default" NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT tipo_ciclo_pk PRIMARY KEY (id),
    CONSTRAINT tipo_ciclo_un UNIQUE (descricao)

)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.tipo_ciclo
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public.tipo_ciclo_ano
(
    id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    tipo_ciclo_id bigint NOT NULL,
    ano integer NOT NULL,
    CONSTRAINT tipo_ciclo_ano_pk PRIMARY KEY (id),
    CONSTRAINT tipo_ciclo_ano_un UNIQUE (tipo_ciclo_id, ano)
,
    CONSTRAINT tipo_ciclo_id_fk FOREIGN KEY (tipo_ciclo_id)
        REFERENCES public.tipo_ciclo (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.tipo_ciclo_ano
    OWNER to postgres;

insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(1,'Alfabetização',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(2,'Interdisciplinar',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(3,'Autoral',now(),'Carga inicial','Carga inicial');

insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (1,1);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (1,2);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (1,3);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (2,4);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (2,5);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (2,6);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (3,7);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (3,8);
insert into tipo_ciclo_ano (tipo_ciclo_id, ano) values (3,9);