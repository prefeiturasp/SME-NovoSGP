begin transaction;

ALTER TABLE public.aula ALTER COLUMN tipo_calendario_id TYPE int8 USING tipo_calendario_id::int;

select f_cria_fk_se_nao_existir('aula', 'aula_tipo_calendario_fk', 'FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id)');

end transaction;