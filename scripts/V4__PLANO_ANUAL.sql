CREATE TABLE if not exists public.plano_anual (
  id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
  escola_id int8 NOT NULL,
  turma_id int8 NOT NULL,
  ano int8 NOT NULL,
  bimestre int4 NOT NULL,
  descricao varchar NOT NULL,
  criado_em timestamp NOT NULL,
  criado_por varchar(200) NOT NULL,
  alterado_em timestamp NULL,
  alterado_por varchar(200) NULL,
  criado_rf varchar(200) NOT NULL,
  alterado_rf varchar(200) NULL,
  CONSTRAINT plano_anual_pk PRIMARY KEY (id),
  CONSTRAINT plano_anual_un UNIQUE (escola_id, turma_id, ano, bimestre)
);
CREATE TABLE if not exists public.objetivo_aprendizagem_plano (
  id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
  objetivo_aprendizagem_id int8 NOT NULL,
  plano_id int8 NOT NULL,
  criado_em timestamp NOT NULL,
  criado_por varchar(200) NOT NULL,
  alterado_em timestamp NULL,
  alterado_por varchar(200) NULL,
  criado_rf varchar(200) NOT NULL,
  alterado_rf varchar(200) NULL,
  CONSTRAINT objetivo_aprendizagem_plano_pk PRIMARY KEY (id),
  CONSTRAINT objetivo_aprendizagem_plano_un UNIQUE (objetivo_aprendizagem_id, plano_id),
  CONSTRAINT plano_id_fk FOREIGN KEY (plano_id) REFERENCES plano_anual(id)
);
CREATE TABLE if not exists public.disciplina_plano (
  id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
  disciplina_id int8 NOT NULL,
  plano_id int8 NOT NULL,
  criado_em timestamp NOT NULL,
  criado_por varchar(200) NOT NULL,
  alterado_em timestamp NULL,
  alterado_por varchar(200) NULL,
  criado_rf varchar(200) NOT NULL,
  alterado_rf varchar(200) NULL,
  CONSTRAINT disciplina_plano_pk PRIMARY KEY (id),
  CONSTRAINT disciplina_plano_un UNIQUE (disciplina_id, plano_id),
  CONSTRAINT plano_id_fk FOREIGN KEY (plano_id) REFERENCES plano_anual(id)
);