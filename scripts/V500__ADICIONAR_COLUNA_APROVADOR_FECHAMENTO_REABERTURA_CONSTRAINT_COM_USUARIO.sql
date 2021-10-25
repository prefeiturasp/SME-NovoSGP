--> Adicionando a coluna aprovador_id
ALTER TABLE fechamento_reabertura ADD COLUMN IF NOT EXISTS aprovador_id int8 null;
 
--> Adicionando a referência (constraint) com tabela de usuarios
DO 
$$ BEGIN
IF NOT EXISTS (select constraint_name 
               from information_schema.table_constraints 
               where upper(table_name) =  upper('fechamento_reabertura')  
               and upper(constraint_name) = upper('fechamento_reabertura_usuario_fk'))
THEN
   ALTER TABLE fechamento_reabertura ADD CONSTRAINT fechamento_reabertura_usuario_fk FOREIGN KEY (aprovador_id) REFERENCES public.usuario(id);
ELSE raise NOTICE 'Constraint fechamento_reabertura_usuario_fk já existe';   

END IF;
END
$$;