DROP TABLE plano_ciclo cascade;

CREATE TABLE plano_ciclo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar NOT NULL,
	ano int8 NOT NULL,
	ciclo_id int8 NOT NULL,
	escola_id varchar(10) NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_ciclo_pk PRIMARY KEY (id),
	CONSTRAINT plano_ciclo_un UNIQUE (ano, ciclo_id, escola_id)
);