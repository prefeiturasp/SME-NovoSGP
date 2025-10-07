CREATE table IF NOT EXISTS public.importacao_log_erro(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	importacao_log_id int8 NOT NULL,
	linha_arquivo int8 NOT NULL,
	motivo_falha varchar(200) NOT NULL,
	criado_em timestamp without time zone NOT NULL,
    criado_por character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_em timestamp without time zone,
    alterado_por character varying(200) COLLATE pg_catalog."default",
    criado_rf character varying(200) COLLATE pg_catalog."default" NOT NULL,
    alterado_rf character varying(200) COLLATE pg_catalog."default",
	CONSTRAINT importacao_log_erro_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS importacao_log_erro_importacao_log_id_idx ON public.importacao_log_erro (importacao_log_id);
ALTER TABLE public.importacao_log_erro DROP CONSTRAINT IF EXISTS importacao_log_erro_importacao_log_id_fk;
ALTER TABLE public.importacao_log_erro ADD CONSTRAINT importacao_log_erro_importacao_log_id_fk FOREIGN KEY (importacao_log_id) REFERENCES public.importacao_log (id);


