-- ADD colunas na tabela 'etl_abrangencia' 
alter table etl_abrangencia
add column historico varchar(4),
add column perfil varchar(50);
commit;