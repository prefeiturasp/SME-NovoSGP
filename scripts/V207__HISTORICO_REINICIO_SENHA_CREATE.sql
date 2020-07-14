CREATE TABLE if not exists public.historico_reinicio_senha (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_rf varchar(200) NOT NULL,
	dre_codigo varchar(200) NOT NULL,
	ue_codigo varchar(200) NULL,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT historico_reinicio_senha_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists historico_reinicio_senha_usuario_idx ON public.historico_reinicio_senha (usuario_rf);
CREATE INDEX if not exists historico_reinicio_senha_dre_idx ON public.historico_reinicio_senha (dre_codigo);
CREATE INDEX if not exists historico_reinicio_senha_ue_idx ON public.historico_reinicio_senha (ue_codigo);
