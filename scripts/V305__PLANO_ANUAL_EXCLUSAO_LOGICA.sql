alter table public.planejamento_anual 
add column if not exists excluido bool not null default false;

alter table public.planejamento_anual_objetivos_aprendizagem 
add column if not exists excluido bool not null default false;

alter table public.planejamento_anual_componente 
add column if not exists excluido bool not null default false;

alter table public.planejamento_anual_periodo_escolar 
add column if not exists excluido bool not null default false;





