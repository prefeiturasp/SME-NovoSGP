DROP TABLE if exists public.historico_email_usuario;

CREATE TABLE public.historico_email_usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	email varchar(100) NOT NULL,
	tipo_acao int4 NOT NULL,
	criado_em timestamp  NOT NULL,
    	criado_por varchar(200) NOT NULL,
    	alterado_em timestamp,
    	alterado_por varchar(200),
    	criado_rf varchar(200)  NOT NULL,
    	alterado_rf varchar(200),
	CONSTRAINT historico_email_usuario_pk PRIMARY KEY (id)
);

ALTER TABLE public.historico_email_usuario ADD CONSTRAINT historico_email_usuario_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id);
