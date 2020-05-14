alter table public.frequencia_aluno drop column if exists turma_id;
alter table public.frequencia_aluno add column turma_id varchar(15);
