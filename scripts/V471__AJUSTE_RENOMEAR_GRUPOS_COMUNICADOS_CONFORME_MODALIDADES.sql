do $$
begin 
    if(SELECT EXISTS (
   SELECT FROM pg_tables
   WHERE  schemaname = 'public'
   AND    tablename  = 'grupo_comunicado'))  then
        update grupo_comunicado set nome = 'Ensino Fundamental' where nome = 'Fundamental';
        update grupo_comunicado set nome = 'Educação de Jovens e Adultos' where nome = 'EJA';
        update grupo_comunicado set nome = 'Educação Infantil' where nome = 'Infantil CEI';
        update grupo_comunicado set nome = 'Ensino Médio' where nome = 'Médio';
    end if;
end $$;
