CREATE OR REPLACE FUNCTION create_constraint_if_not_exists (t_name text, c_name text, constraint_sql text)
  RETURNS void
AS
$BODY$
  begin
    -- Look for our constraint
    if not exists (select constraint_name
                   from information_schema.constraint_column_usage
                   where table_name = t_name  and constraint_name = c_name) then
        execute 'ALTER TABLE ' || t_name || ' ADD CONSTRAINT ' || c_name || ' ' || constraint_sql;
    end if;
end;
$BODY$
LANGUAGE plpgsql VOLATILE;



CREATE OR REPLACE FUNCTION create_fk_if_not_exists (t_name text, c_name text, constraint_sql text)
  RETURNS void
AS
$BODY$
  begin
    -- Look for our FK
    if not exists (select constraint_name
                   from information_schema.table_constraints
                   where table_name = t_name  and constraint_name = c_name) then
        execute 'ALTER TABLE ' || t_name || ' ADD CONSTRAINT ' || c_name || ' ' || constraint_sql;
    end if;
end;
$BODY$
LANGUAGE plpgsql VOLATILE;


