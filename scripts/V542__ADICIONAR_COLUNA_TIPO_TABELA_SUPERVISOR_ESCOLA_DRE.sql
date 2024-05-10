alter table supervisor_escola_dre add column if not exists tipo int;

update supervisor_escola_dre 
set tipo = 1;