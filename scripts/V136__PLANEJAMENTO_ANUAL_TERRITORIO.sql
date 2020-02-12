CREATE TABLE if not exists public.plano_anual_territorio_saber (
  id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
  escola_id varchar(10) NOT NULL,
  turma_id int8 NOT NULL,
  ano int8 NOT NULL,
  bimestre int4 NOT NULL,
  territorio_experiencia_id int8 not null,
  desenvolvimento varchar NOT NULL,
  reflexao varchar NOT NULL,
  criado_em timestamp NOT NULL,
  criado_por varchar(200) NOT NULL,
  alterado_em timestamp NULL,
  alterado_por varchar(200) NULL,
  criado_rf varchar(200) NOT NULL,
  alterado_rf varchar(200) NULL,
  CONSTRAINT plano_anual_territorio_saber_pk PRIMARY KEY (id),
  CONSTRAINT plano_anual_territorio_saber_un UNIQUE (escola_id, turma_id, ano, bimestre)
);
