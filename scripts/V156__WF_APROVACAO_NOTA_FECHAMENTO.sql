CREATE TABLE public.wf_aprovacao_nota_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_id int8 NOT NULL,
	fechamento_nota_id int8 not null,

	nota numeric(5,2) NULL,
	conceito_id int8 NULL,

	CONSTRAINT wf_aprovacao_nota_fechamento_pk PRIMARY KEY (id)
);

ALTER TABLE public.wf_aprovacao_nota_fechamento ADD CONSTRAINT wf_aprovacao_nota_fechamento_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id);
ALTER TABLE public.wf_aprovacao_nota_fechamento ADD CONSTRAINT wf_aprovacao_nota_fechamento_nota_fk FOREIGN KEY (fechamento_nota_id) REFERENCES fechamento_nota(id);
ALTER TABLE public.wf_aprovacao_nota_fechamento ADD CONSTRAINT wf_aprovacao_nota_fechamento_conceito_fk FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id);

CREATE INDEX wf_aprovacao_nota_fechamento_aprovacao_idx ON public.wf_aprovacao_nota_fechamento USING btree (wf_aprovacao_id);
CREATE INDEX wf_aprovacao_nota_fechamento_nota_idx ON public.wf_aprovacao_nota_fechamento USING btree (fechamento_nota_id);
