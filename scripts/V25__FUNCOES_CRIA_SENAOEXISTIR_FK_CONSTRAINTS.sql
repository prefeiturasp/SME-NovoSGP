CREATE OR REPLACE FUNCTION f_cria_constraint_se_nao_existir (tabela_nome text, constraint_nome text, constraint_sql text)
  RETURNS void
AS
$BODY$
  begin
    -- Look for our constraint
    if not exists (select constraint_name
                   from information_schema.constraint_column_usage
                   where table_name = tabela_nome  and constraint_name = constraint_nome) then
        execute 'ALTER TABLE ' || tabela_nome || ' ADD CONSTRAINT ' || constraint_nome || ' ' || constraint_sql;
    end if;
end;
$BODY$
LANGUAGE plpgsql VOLATILE;



CREATE OR REPLACE FUNCTION f_cria_fk_se_nao_existir (tabela_nome text, constraint_nome text, fk_sql text)
  RETURNS void
AS
$BODY$
  begin
    -- Look for our FK
    if not exists (select constraint_name
                   from information_schema.table_constraints
                   where table_name = tabela_nome  and constraint_name = constraint_nome) then
        execute 'ALTER TABLE ' || tabela_nome || ' ADD CONSTRAINT ' || constraint_nome || ' ' || fk_sql;
    end if;
end;
$BODY$
LANGUAGE plpgsql VOLATILE;


