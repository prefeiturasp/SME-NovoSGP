DROP TABLE if exists public.ocorrencia_servidor;

CREATE TABLE public.ocorrencia_servidor (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	rf_codigo varchar(12) NOT NULL,
	ocorrencia_id int8 NOT null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	
    CONSTRAINT ocorrencia_servidor_pk PRIMARY KEY (id)
);

ALTER TABLE public.ocorrencia_servidor ADD CONSTRAINT ocorrencia_servidor_fk FOREIGN KEY (ocorrencia_id) REFERENCES ocorrencia(id);
CREATE INDEX ocorrencia_servidor_ocorrencia_idx ON public.ocorrencia_servidor USING btree (ocorrencia_id);
CREATE INDEX ocorrencia_servidor_rf_codigo_idx ON public.ocorrencia_servidor USING btree (rf_codigo);