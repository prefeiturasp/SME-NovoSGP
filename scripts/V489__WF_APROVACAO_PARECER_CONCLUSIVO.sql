DROP TABLE IF EXISTS public.wf_aprovacao_parecer_conclusivo;
CREATE TABLE public.wf_aprovacao_parecer_conclusivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	wf_aprovacao_id int8 NOT NULL,
	conselho_classe_aluno_id int8 not null,

	conselho_classe_parecer_id int8 NULL,

	CONSTRAINT wf_aprovacao_nota_conselho_pk PRIMARY KEY (id)
);

ALTER TABLE public.wf_aprovacao_parecer_conclusivo ADD CONSTRAINT wf_aprovacao_parecer_conclusivo_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id);
ALTER TABLE public.wf_aprovacao_parecer_conclusivo ADD CONSTRAINT wf_aprovacao_parecer_conclusivo_aluno_fk FOREIGN KEY (conselho_classe_aluno_id) REFERENCES conselho_classe_aluno(id);
ALTER TABLE public.wf_aprovacao_parecer_conclusivo ADD CONSTRAINT wf_aprovacao_parecer_conclusivo_parecer_fk FOREIGN KEY (conselho_classe_parecer_id) REFERENCES conselho_classe_parecer(id);

CREATE INDEX wf_aprovacao_parecer_conclusivo_aprovacao_idx ON public.wf_aprovacao_parecer_conclusivo USING btree (wf_aprovacao_id);
CREATE INDEX wf_aprovacao_parecer_conclusivo_aluno_idx ON public.wf_aprovacao_parecer_conclusivo USING btree (conselho_classe_aluno_id);
CREATE INDEX wf_aprovacao_parecer_conclusivo_parecer_idx ON public.wf_aprovacao_parecer_conclusivo USING btree (conselho_classe_parecer_id);
