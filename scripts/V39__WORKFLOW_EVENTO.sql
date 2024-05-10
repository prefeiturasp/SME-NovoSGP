alter table if exists public.wf_aprovacao add column if not exists tipo int not null default 1;

alter table if exists public.evento add column if not exists wf_aprovacao_id bigint null;

select f_cria_fk_se_nao_existir('evento', 'evento_wf_aprovacao_fk', 'FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id)');

CREATE index if not exists evento_wf_aprovacao_idx ON public.evento(wf_aprovacao_id);

alter table if exists public.evento add column if not exists status int not null default 1;


