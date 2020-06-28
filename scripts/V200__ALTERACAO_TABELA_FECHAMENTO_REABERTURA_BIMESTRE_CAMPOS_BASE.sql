alter table fechamento_reabertura_bimestre
add column if not exists criado_em timestamp(6) default now() not null,
add column if not exists criado_por varchar(200) default '' not null,
add column if not exists alterado_em timestamp(6) null,
add column if not exists alterado_por varchar(200) null,
add column if not exists criado_rf varchar(200) default '' not null,
add column if not exists alterado_rf varchar(200) null;	

