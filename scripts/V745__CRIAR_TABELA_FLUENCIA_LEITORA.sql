CREATE table IF NOT EXISTS public.fluencia_leitora(
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY, 
    ano_letivo int8 NOT NULL,  
    codigo_eol_turma varchar(200) NOT NULL,
    codigo_eol_aluno varchar(200) NOT NULL,
    fluencia int8 NOT NULL NOT NULL,
    tipo_avaliacao int8 NOT NULL NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT fluencia_leitora_pk PRIMARY KEY (id)
);