-- PLANO AULA
DROP TABLE if exists public.plano_aula;
CREATE TABLE public.plano_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 not null,
	descricao varchar(500) NULL,
	desenvolvimento_aula varchar(500) NOT NULL,
	recuperacao_aula varchar(500) NULL,
	licao_casa varchar(500) NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aula_pk PRIMARY KEY (id),
	CONSTRAINT plano_aula_aula_id_fk FOREIGN KEY (aula_id) REFERENCES aula(id) ON DELETE CASCADE
);
CREATE INDEX plano_aula_aula_idx ON public.plano_aula USING btree (aula_id);

-- OBJETIVO APRENDIZAGEM AULA
DROP TABLE if exists public.objetivo_aprendizagem_aula;
CREATE TABLE public.objetivo_aprendizagem_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aula_id int8 not null,
	objetivo_aprendizagem_plano_id int8 not null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_aprendizagem_aula_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_aprendizagem_aula_plano_id_fk FOREIGN KEY (plano_aula_id) REFERENCES plano_aula(id) ON DELETE cascade,
	CONSTRAINT objetivo_aprendizagem_aula_obj_apr_plano_id_fk FOREIGN KEY (objetivo_aprendizagem_plano_id) REFERENCES objetivo_aprendizagem_plano(id) ON DELETE CASCADE
);
CREATE INDEX objetivo_aprendizagem_aula_plano_idx ON public.objetivo_aprendizagem_aula USING btree (plano_aula_id);
CREATE INDEX objetivo_aprendizagem_aula_obj_apr_idx ON public.objetivo_aprendizagem_aula USING btree (objetivo_aprendizagem_plano_id);
