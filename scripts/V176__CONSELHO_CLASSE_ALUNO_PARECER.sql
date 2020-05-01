ALTER TABLE public.conselho_classe_aluno add column if not exists conselho_classe_parecer_id int8 null;
ALTER TABLE public.conselho_classe_aluno ADD CONSTRAINT conselho_classe_aluno_parecer_fk FOREIGN KEY (conselho_classe_parecer_id) REFERENCES conselho_classe_parecer(id);

alter table public.conselho_classe_parecer add column if not exists nota bool not null default false;
update public.conselho_classe_parecer set nota = true where id in (1,4);
update public.conselho_classe_parecer set conselho = true where id in (4);
