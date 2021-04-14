CREATE TABLE if not exists public.wf_aprovacao_itinerancia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_id int8 NOT NULL,
	itinerancia_id int8 not null,
	status_aprovacao boolean null,

	CONSTRAINT wf_aprovacao_itinerancia_pk PRIMARY KEY (id)
);

ALTER TABLE public.wf_aprovacao_itinerancia ADD CONSTRAINT wf_aprovacao_itinerancia_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id);
ALTER TABLE public.wf_aprovacao_itinerancia ADD CONSTRAINT wf_aprovacao_itinerancia_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id);

CREATE INDEX if not exists wf_aprovacao_itinerancia_idx ON public.wf_aprovacao_itinerancia USING btree (wf_aprovacao_id);
CREATE INDEX if not exists wf_aprovacao_itinerancia_itinerancia_idx ON public.wf_aprovacao_itinerancia USING btree (itinerancia_id);
