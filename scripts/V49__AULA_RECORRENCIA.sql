ALTER table if exists public.aula ADD column if not exists aula_pai_id  int8 null;

select f_cria_fk_se_nao_existir('aula', 'aula_pai_fk', 'FOREIGN KEY (aula_pai_id) REFERENCES aula(id)');

