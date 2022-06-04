DROP TABLE IF EXISTS public.suporte_usuario;
CREATE TABLE public.suporte_usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_administrador VARCHAR(50) NOT NULL,
	usuario_simulado VARCHAR(50) NOT NULL,
	data_acesso timestamp NOT NULL,
	token_acesso  VARCHAR(2500) NOT NULL,
	CONSTRAINT suporte_usuario_pk PRIMARY KEY (id)
);