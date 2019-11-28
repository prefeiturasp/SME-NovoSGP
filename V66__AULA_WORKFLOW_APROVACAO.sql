ALTER TABLE aula ADD COLUMN wf_aprovacao_id  int8; 
ALTER TABLE aula ADD COLUMN status  int4;  
alter table aula alter column status SET default 1 ;
update aula set status = 1;
