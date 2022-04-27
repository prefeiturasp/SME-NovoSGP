DROP TABLE IF EXISTS public.conselho_classe_aluno_recomendacao;
CREATE TABLE public.conselho_classe_aluno_recomendacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	conselho_classe_aluno_id int8 NOT NULL,
	conselho_classe_recomendacao_id int8 NOT NULL,
	CONSTRAINT conselho_classe_aluno_recomendacao_pk PRIMARY KEY (id)
);
CREATE INDEX conselho_classe_aluno_recomendacao_aluno_idx ON public.conselho_classe_aluno_recomendacao USING btree (conselho_classe_aluno_id);
CREATE INDEX conselho_classe_aluno_recomendacao_recomendacao_idx ON public.conselho_classe_aluno_recomendacao USING btree (conselho_classe_recomendacao_id);

ALTER TABLE public.conselho_classe_aluno_recomendacao ADD CONSTRAINT conselho_classe_aluno_recomendacao_aluno_fk FOREIGN KEY (conselho_classe_aluno_id) REFERENCES public.conselho_classe_aluno(id);
ALTER TABLE public.conselho_classe_aluno_recomendacao ADD CONSTRAINT conselho_classe_aluno_recomendacao_recomendacao_fk FOREIGN KEY (conselho_classe_recomendacao_id) REFERENCES public.conselho_classe_recomendacao(id);