drop table if exists pendencia_encaminhamento_aee;

CREATE table public.pendencia_encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_aee_id int8 not null,
	pendencia_id int8 not null,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_encaminhamento_aee_pk PRIMARY KEY (id)
);

CREATE INDEX pendencia_encaminhamento_encaminhamento_aee_id_idx ON public.pendencia_encaminhamento_aee USING btree (encaminhamento_aee_id);
CREATE INDEX pendencia_encaminhamento_aee_pendencia_id_idx ON public.pendencia_encaminhamento_aee USING btree (pendencia_id);

ALTER TABLE public.pendencia_encaminhamento_aee ADD CONSTRAINT pendencia_encaminhamento_aee_fk FOREIGN KEY (encaminhamento_aee_id) REFERENCES encaminhamento_aee(id);
ALTER TABLE public.pendencia_encaminhamento_aee ADD CONSTRAINT pendencia_encaminhamento_aee_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id);
