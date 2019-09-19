CREATE TABLE IF NOT EXISTS public.notificacao
(
    id bigint NOT NULL generated always as identity,
    titulo varchar(50)  NOT NULL,
    mensagem varchar(500) NOT NULL,    
    status int NOT NULL,
	categoria int NOT NULL,
    tipo int NOT NULL,
    ue_id varchar(15) NULL,
    dre_id varchar(15) NULL,
	ano int NULL,		
	codigo bigint NOT NULL,
	turma_id varchar(15) NULL,    
	excluida BOOLEAN NOT NULL,
	usuario_id bigint NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
	CONSTRAINT notificacao_pk PRIMARY KEY (id),
	CONSTRAINT usuario_fk FOREIGN KEY (usuario_id) REFERENCES public.usuario (id)

);

CREATE INDEX notificacao_ue_idx ON public.notificacao (ue_id);
CREATE INDEX notificacao_dre_idx ON public.notificacao (dre_id);
CREATE INDEX notificacao_turma_idx ON public.notificacao (turma_id);
CREATE INDEX notificacao_ano_idx ON public.notificacao (ano);
CREATE INDEX notificacao_titulo_idx ON public.notificacao (lower(f_unaccent(titulo)) text_pattern_ops);

