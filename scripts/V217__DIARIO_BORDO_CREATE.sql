DROP TABLE IF EXISTS public.devolutiva_diario_bordo;
DROP TABLE IF EXISTS public.diario_bordo;

CREATE TABLE public.devolutiva_diario_bordo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar not null,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT devolutiva_diario_bordo_pk PRIMARY KEY (id)
);

CREATE TABLE public.diario_bordo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 not null,
	devolutiva_id int8 null,
	planejamento varchar not null,
	reflexoes_replanejamento varchar null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT diario_bordo_pk PRIMARY KEY (id)
);
CREATE INDEX diario_bordo_aula_idx ON public.diario_bordo USING btree (aula_id);
ALTER TABLE public.diario_bordo ADD CONSTRAINT diario_bordo_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id);
CREATE INDEX diario_bordo_devolutiva_idx ON public.diario_bordo USING btree (devolutiva_id);
ALTER TABLE public.diario_bordo ADD CONSTRAINT diario_bordo_devolutiva_fk FOREIGN KEY (devolutiva_id) REFERENCES devolutiva_diario_bordo(id);

