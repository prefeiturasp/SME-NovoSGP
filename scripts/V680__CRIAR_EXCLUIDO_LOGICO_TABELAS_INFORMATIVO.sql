alter table public.informativo_notificacao 
add column if not exists excluido bool not null default false;

alter table public.informativo 
add column if not exists excluido bool not null default false;

alter table public.informativo_perfil 
add column if not exists excluido bool not null default false;