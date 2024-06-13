
CREATE table if not exists public.informativo_modalidade (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	informativo_id int8 NOT NULL,
    modalidade_codigo int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT informativo_modalidade_pk PRIMARY KEY (id)
);  

ALTER TABLE informativo_modalidade ADD CONSTRAINT informativo_modalidade_informativo_fk FOREIGN KEY (informativo_id) REFERENCES informativo(id);