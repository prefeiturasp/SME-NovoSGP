
alter table atribuicao_cj
alter column professor_rf type varchar(11) using professor_rf::varchar(11);


alter table atribuicao_esporadica
alter column professor_rf type varchar(11) using professor_rf::varchar(11);