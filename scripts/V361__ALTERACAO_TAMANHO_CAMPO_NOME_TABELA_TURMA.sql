update pg_catalog.pg_attribute
set atttypmod = 24
where attrelid = 'turma'::regclass
  and attname = 'nome'
  and atttypmod < 24;
 
update pg_catalog.pg_attribute
set atttypmod = 24
where attrelid = 'v_abrangencia_cadeia_turmas'::regclass
  and attname = 'turma_nome'
  and atttypmod < 24;