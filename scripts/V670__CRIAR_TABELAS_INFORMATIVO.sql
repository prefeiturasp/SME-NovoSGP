CREATE table IF NOT EXISTS public.informativo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	dre_id int8 not null,
	ue_id int8 not null,
	titulo varchar(100) NOT NULL,
	texto varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT informativo_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists informativo_dre_idx ON public.informativo USING btree (dre_id);
ALTER TABLE public.informativo DROP CONSTRAINT if exists informativo_dre_fk;
ALTER TABLE public.informativo ADD CONSTRAINT informativo_dre_fk FOREIGN KEY (dre_id) REFERENCES dre(id);

CREATE INDEX if not exists informativo_ue_idx ON public.informativo USING btree (ue_id);
ALTER TABLE public.informativo DROP CONSTRAINT if exists informativo_ue_fk;
ALTER TABLE public.informativo ADD CONSTRAINT informativo_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);


CREATE table IF NOT EXISTS public.informativo_perfil(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	informativo_id int8 not null,
	codigo_perfil int8 not null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT informativo_perfil_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists informativo_perfil_id_idx ON public.informativo_perfil USING btree (informativo_id);
ALTER TABLE public.informativo_perfil DROP CONSTRAINT if exists informativo_perfil_id_fk;
ALTER TABLE public.informativo_perfil ADD CONSTRAINT informativo_perfil_id_fk FOREIGN KEY (informativo_id) REFERENCES informativo(id);