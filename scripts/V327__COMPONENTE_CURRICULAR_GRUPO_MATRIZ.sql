alter table componente_curricular alter column grupo_matriz_id set default 1;
                   
update componente_curricular
   set grupo_matriz_id = 1
  where grupo_matriz_id is null;