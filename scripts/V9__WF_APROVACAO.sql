CREATE TABLE IF NOT EXISTS public.wf_aprovacao
(
    id bigint NOT NULL generated always as identity,
    ue_id varchar(15) NULL,
    dre_id varchar(15) NULL,
	ano int NULL,
	turma_id varchar(15) NULL,
	notificacao_mensagem varchar(500) NOT NULL,
	notificacao_titulo varchar(500) NOT NULL,
	notificacao_tipo int NOT NULL,
	notificacao_categoria int NOT NULL,
	criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT wf_aprova_niveis_pk PRIMARY KEY (id)

);



