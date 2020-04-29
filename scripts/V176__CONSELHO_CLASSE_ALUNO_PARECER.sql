ALTER TABLE public.conselho_classe_aluno drop column if exists conselho_classe_parecer_id;

ALTER TABLE public.conselho_classe_aluno add column conselho_classe_parecer_id int8 null;
ALTER TABLE public.conselho_classe_aluno ADD CONSTRAINT conselho_classe_aluno_parecer_fk FOREIGN KEY (conselho_classe_parecer_id) REFERENCES conselho_classe_parecer(id);
