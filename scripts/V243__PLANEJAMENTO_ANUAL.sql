--Area do conhecimento
CREATE TABLE if not exists public.componente_curricular_area_conhecimento (
	id int8 NOT NULL,
	nome varchar(200) NOT null,
	CONSTRAINT componente_curricular_area_conhecimento_pk PRIMARY KEY (id)
);

INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(1, 'Linguagens');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(2, 'Matemática');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(3, 'Ciências da Natureza');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(4, 'Ciências Humanas');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(5, 'Língua Estrangeira Moderna');
INSERT INTO public.componente_curricular_area_conhecimento
(id, nome)
VALUES(6, 'Língua Brasileira de Sinais');



--Grupo Matriz

CREATE TABLE if not exists public.componente_curricular_grupo_matriz (
	id int8 NOT null,
	nome varchar(200) NOT null,
	CONSTRAINT componente_curricular_grupo_matriz_pk PRIMARY KEY (id)
);

INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(1, 'Base Nacional Comum');
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(2, 'Diversificada');
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(3, 'Enriquecimento Curricular');
INSERT INTO public.componente_curricular_grupo_matriz
(id, nome)
VALUES(4, 'Integral');


ALTER TABLE public.componente_curricular RENAME TO componente_curricular_jurema;

--Componente Curricular
CREATE TABLE if not exists public.componente_curricular(
	id int8 NOT NULL,
	componente_curricular_pai_id int8 NULL,
	grupo_matriz_id int8 NULL,
	area_conhecimento_id int8 null,
	descricao varchar(100) NULL,
	eh_regencia bool NOT NULL DEFAULT false,
	eh_compartilhada bool NOT NULL DEFAULT false,
	eh_territorio bool NOT NULL DEFAULT false,
	eh_base_nacional bool NOT NULL DEFAULT false,
	permite_registro_frequencia bool NOT NULL DEFAULT false,
	permite_lancamento_nota bool NOT NULL DEFAULT true,
	CONSTRAINT componente_curricular_teste_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS componente_curricular_grupo_matriz_idx ON public.componente_curricular USING btree (grupo_matriz_id);

CREATE INDEX IF NOT EXISTS componente_curricular_area_conhecimento_idx ON public.componente_curricular USING btree (area_conhecimento_id);

select
	f_cria_fk_se_nao_existir(
		'componente_curricular',
		'componente_curricular_area_conhecimento_fk',
		'FOREIGN KEY (area_conhecimento_id) REFERENCES componente_curricular_area_conhecimento (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'componente_curricular',
		'componente_curricular_grupo_matriz_fk',
		'FOREIGN KEY (grupo_matriz_id) REFERENCES componente_curricular_grupo_matriz (id)'
	);


--Jurema/Curriculo

CREATE TABLE if not exists public.componente_curriculo_cidade(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo int8 NOT NULL,
	componente_curricular_id int8 null,
	CONSTRAINT componente_curriculo_cidade_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'componente_curriculo_cidade',
		'componente_curriculo_cidade_componente_curricular_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)'
	);

CREATE INDEX IF NOT EXISTS componente_curriculo_cidade_componente_curricular_idx ON public.componente_curriculo_cidade USING btree (componente_curricular_id);

ALTER TABLE public.componente_curriculo_cidade ADD CONSTRAINT componente_curriculo_cidade_un UNIQUE (codigo,componente_curricular_id);

	
	insert
	into
	componente_curriculo_cidade (codigo, componente_curricular_id)
select
	ccj.codigo_jurema,
	ccj.codigo_eol
from
	componente_curricular_jurema ccj
inner join componente_curricular cc on
	ccj.codigo_eol = cc.id;

--Plano Anual
CREATE TABLE if not exists public.planejamento_anual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual',
		'planejamento_anual_turma_fk',
		'FOREIGN KEY (turma_id) REFERENCES turma (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual',
		'planejamento_anual_componente_curricular_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)'
	);

CREATE INDEX IF NOT EXISTS planejamento_anual_turma_idx ON public.planejamento_anual USING btree (turma_id);

CREATE INDEX IF NOT EXISTS planejamento_anual_componente_curricular_idx ON public.planejamento_anual USING btree (componente_curricular_id);


CREATE TABLE if not exists public.planejamento_anual_periodo_escolar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	periodo_escolar_id int8 NOT NULL,
	planejamento_anual_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_periodo_escolar_pk PRIMARY KEY (id)
);
	
select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_periodo_escolar',
		'planejamento_anual_periodo_escolar_planejamento_anual_fk',
		'FOREIGN KEY (planejamento_anual_id) REFERENCES planejamento_anual (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_periodo_escolar',
		'planejamento_anual_periodo_escolar_periodo_escolar_fk',
		'FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar (id)'
	);

CREATE INDEX IF NOT EXISTS planejamento_anual_periodo_escolar_periodo_escolar_idx ON public.planejamento_anual_periodo_escolar USING btree (periodo_escolar_id);


CREATE INDEX IF NOT EXISTS planejamento_anual_periodo_escolar_planejamento_anual_idx ON public.planejamento_anual_periodo_escolar USING btree (planejamento_anual_id);


CREATE TABLE if not exists public.planejamento_anual_componente (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	planejamento_anual_periodo_escolar_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	descricao varchar NOT null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_componente_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_componente',
		'planejamento_anual_componente_planejamento_anual_periodo_escolar_fk',
		'FOREIGN KEY (planejamento_anual_periodo_escolar_id) REFERENCES planejamento_anual_periodo_escolar (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_componente',
		'planejamento_anual_componente_componente_curricular_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)'
	);
	

CREATE INDEX IF NOT EXISTS planejamento_anual_componente_componente_componente_curricular_idx ON public.planejamento_anual_componente USING btree (componente_curricular_id);


CREATE INDEX IF NOT EXISTS planejamento_anual_componente_componente_planejamento_anual_periodo_escolar_idx ON public.planejamento_anual_componente USING btree (planejamento_anual_periodo_escolar_id);


CREATE TABLE if not exists public.planejamento_anual_objetivos_aprendizagem(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	planejamento_anual_componente_id int8 NOT NULL,
	objetivo_aprendizagem_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT planejamento_anual_objetivos_aprendizagem_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_objetivos_aprendizagem',
		'planejamento_anual_objetivos_aprendizagem_planejamento_anual_fk',
		'FOREIGN KEY (planejamento_anual_componente_id) REFERENCES planejamento_anual_componente (id)'
	);
	

select
	f_cria_fk_se_nao_existir(
		'planejamento_anual_objetivos_aprendizagem',
		'planejamento_anual_objetivos_aprendizagem_objetivos_aprendizagem_fk',
		'FOREIGN KEY (objetivo_aprendizagem_id) REFERENCES objetivo_aprendizagem (id)'
	);
	

CREATE INDEX IF NOT EXISTS planejamento_anual_objetivos_aprendizagem_planejamento_anual_componente_idx ON public.planejamento_anual_objetivos_aprendizagem USING btree (planejamento_anual_componente_id);

CREATE INDEX IF NOT EXISTS planejamento_anual_objetivos_aprendizagem_objetivo_aprendizagem_idx ON public.planejamento_anual_objetivos_aprendizagem USING btree (objetivo_aprendizagem_id);
