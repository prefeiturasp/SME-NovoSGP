drop table if exists pendencia_plano_aee;

CREATE table public.pendencia_plano_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_id int8 not null,
	pendencia_id int8 not null,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_plano_aee_pk PRIMARY KEY (id)
);

ALTER TABLE public.pendencia_plano_aee ADD CONSTRAINT pendencia_plano_aee_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id);
ALTER TABLE public.pendencia_plano_aee ADD CONSTRAINT pendencia_plano_aee_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id);

CREATE INDEX pendencia_plano_plano_idx ON public.pendencia_plano_aee USING btree (plano_aee_id);
CREATE INDEX pendencia_plano_pendencia_idx ON public.pendencia_plano_aee USING btree (pendencia_id);
