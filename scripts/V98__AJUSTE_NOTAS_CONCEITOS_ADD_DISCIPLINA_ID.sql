delete from public.notas_conceito where 1=1;
alter table public.notas_conceito add column disciplina_id varchar(15) NOT null;
CREATE INDEX if not exists notas_disciplina_id_idx ON public.notas_conceito (disciplina_id);