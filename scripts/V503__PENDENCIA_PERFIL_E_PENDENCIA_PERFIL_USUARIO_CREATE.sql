DROP TABLE IF EXISTS public.pendencia_perfil;
DROP TABLE IF EXISTS public.pendencia_perfil_usuario;

CREATE TABLE public.pendencia_perfil (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    cargo int4 NOT NULL,
    nivel int4 NOT NULL, 
    pendencia_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_perfil_pk PRIMARY KEY (id)
);

CREATE TABLE public.pendencia_perfil_usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	pendencia_perfil_id int8 NOT NULL,
    usuario_id int8 NOT NULL,
	CONSTRAINT pendencia_perfil_usuario_pk PRIMARY KEY (id)
);

CREATE INDEX pendencia_perfil_pendencia_idx ON public.pendencia_perfil USING btree (pendencia_id);
ALTER TABLE public.pendencia_perfil ADD CONSTRAINT pendencia_perfil_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id);
CREATE INDEX pendencia_perfil_pendenciaperfil_idx ON public.pendencia_perfil_usuario USING btree (pendencia_perfil_id);
ALTER TABLE public.pendencia_perfil_usuario ADD CONSTRAINT pendencia_perfil_pendencia_fk FOREIGN KEY (pendencia_perfil_id) REFERENCES pendencia_perfil(id);
CREATE INDEX pendencia_perfil_usuario_idx ON public.pendencia_perfil_usuario USING btree (usuario_id);
ALTER TABLE public.pendencia_perfil_usuario ADD CONSTRAINT pendencia_perfil_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id);

