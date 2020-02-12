alter table public.processo_executando drop column if exists bimestre;
alter table public.processo_executando add column bimestre int4 not null;
