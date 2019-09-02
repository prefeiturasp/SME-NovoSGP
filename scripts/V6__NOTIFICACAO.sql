CREATE TABLE IF NOT EXISTS public.notificacao
(
    id bigint NOT NULL,
    titulo varchar(50)  NOT NULL,
    mensagem varchar(500) NOT NULL,
    usuario_id varchar(15) NOT NULL,
    status int NOT NULL,
    categoria int NOT NULL,
    escola_id varchar(15) NULL,
    dre_id varchar(15) NULL,
    pode_remover BOOLEAN NOT NULL,
    criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT notificacao_pk PRIMARY KEY (id)

);

CREATE INDEX notificacao_usuario_idx ON public.notificacao (usuario_id);
CREATE INDEX notificacao_escola_idx ON public.notificacao (escola_id);
CREATE INDEX notificacao_dre_idx ON public.notificacao (dre_id);

