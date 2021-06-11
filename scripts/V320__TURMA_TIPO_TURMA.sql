alter table turma drop if exists tipo_turma;

alter table turma add tipo_turma int4 not null default 0;