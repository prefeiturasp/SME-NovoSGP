CREATE TABLE IF NOT EXISTS public.wf_aprova_nivel
(
    id bigint NOT NULL generated always as identity,
    usuario_id varchar(15) NOT NULL,
    status int NOT NULL,
    descricao varchar(100) NULL,    
	escola_id varchar(15) NULL,
    dre_id varchar(15) NULL,
	ano int NULL,
	turma_id varchar(15) NULL,
	chave varchar(38) NULL,
	nivel int NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT wf_aprova_niveis_pk PRIMARY KEY (id)

);

CREATE INDEX wf_aprova_nivel_usuario_idx ON public.wf_aprova_nivel (usuario_id);
CREATE INDEX wf_aprova_nivel_chave_idx ON public.wf_aprova_nivel (chave);

