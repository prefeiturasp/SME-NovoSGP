DROP TABLE IF EXISTS public.wf_aprovacao_nota_conselho;
CREATE TABLE public.wf_aprovacao_nota_conselho (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_id int8 NOT NULL,
	conselho_classe_nota_id int8 not null,
	usuario_solicitante_id int8 not null,

	nota numeric(5,2) NULL,
	conceito_id int8 NULL,

	CONSTRAINT wf_aprovacao_nota_conselho_pk PRIMARY KEY (id)
);

ALTER TABLE public.wf_aprovacao_nota_conselho ADD CONSTRAINT wf_aprovacao_nota_conselho_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id);
ALTER TABLE public.wf_aprovacao_nota_conselho ADD CONSTRAINT wf_aprovacao_nota_conselho_nota_fk FOREIGN KEY (conselho_classe_nota_id) REFERENCES conselho_classe_nota(id);
ALTER TABLE public.wf_aprovacao_nota_conselho ADD CONSTRAINT wf_aprovacao_nota_conselho_conceito_fk FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id);
ALTER TABLE public.wf_aprovacao_nota_conselho ADD CONSTRAINT wf_aprovacao_nota_conselho_usuario_fk FOREIGN KEY (usuario_solicitante_id) REFERENCES usuario(id);

CREATE INDEX wf_aprovacao_nota_conselho_aprovacao_idx ON public.wf_aprovacao_nota_conselho USING btree (wf_aprovacao_id);
CREATE INDEX wf_aprovacao_nota_conselho_nota_idx ON public.wf_aprovacao_nota_conselho USING btree (conselho_classe_nota_id);
CREATE INDEX wf_aprovacao_nota_conselho_usuario_idx ON public.wf_aprovacao_nota_conselho USING btree (usuario_solicitante_id);