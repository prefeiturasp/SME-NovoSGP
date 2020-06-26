alter table processo_executando
	drop column if exists aula_id;

alter table processo_executando
  add column aula_id int8 null;
