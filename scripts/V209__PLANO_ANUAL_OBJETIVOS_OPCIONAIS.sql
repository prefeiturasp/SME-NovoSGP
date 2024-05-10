alter table plano_anual
  add column if not exists objetivos_opcionais bool not null default false;
