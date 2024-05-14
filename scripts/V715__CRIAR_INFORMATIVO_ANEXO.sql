drop table if exists informativo_anexo;

CREATE table informativo_anexo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	informativo_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,	
	CONSTRAINT informativo_anexo_pk PRIMARY KEY (id)
);

ALTER TABLE informativo_anexo ADD CONSTRAINT informativo_anexo_informe_fk FOREIGN KEY (informativo_id) REFERENCES informativo(id);

ALTER TABLE informativo_anexo ADD CONSTRAINT informativo_anexo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
