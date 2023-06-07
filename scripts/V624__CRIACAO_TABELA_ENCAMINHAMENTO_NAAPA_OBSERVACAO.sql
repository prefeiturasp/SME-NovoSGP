CREATE TABLE if not exists encaminhamento_naapa_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_naapa_id int8 NOT NULL,
	observacao varchar NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_naapa_observacao_pk PRIMARY KEY (id)
);
CREATE INDEX IF NOT EXISTS encaminhamento_naapa_observacao_encaminhamento_idx ON encaminhamento_naapa_observacao USING btree (encaminhamento_naapa_id);

ALTER TABLE encaminhamento_naapa_observacao 
ADD CONSTRAINT  encaminhamento_naapa_observacao_encaminhamento_fk FOREIGN KEY (encaminhamento_naapa_id) 
REFERENCES public.encaminhamento_naapa(id);

