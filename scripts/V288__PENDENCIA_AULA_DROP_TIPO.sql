alter table pendencia_aula drop column tipo;
alter table pendencia_aula alter pendencia_id set not null;
alter table pendencia_aula add if not exists motivo varchar(100) null;