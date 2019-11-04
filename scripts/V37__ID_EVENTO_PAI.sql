ALTER TABLE if exists evento ADD if not exists evento_pai_id bigint NULL;

select f_cria_fk_se_nao_existir('evento', 'evento_fk', 'FOREIGN KEY (evento_pai_id) REFERENCES evento(id)');
