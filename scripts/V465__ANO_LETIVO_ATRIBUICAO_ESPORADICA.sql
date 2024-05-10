alter table public.atribuicao_esporadica add column
if not exists ano_letivo int4 null;

update atribuicao_esporadica set ano_letivo = date_part('year', data_inicio)
where not excluido;