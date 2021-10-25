--> Adicionando a coluna aprovador_id
ALTER TABLE fechamento_reabertura ADD COLUMN IF NOT EXISTS aprovador_id int8 null;
 
--> Adicionando a referência (constraint) com tabela de usuarios
select f_cria_constraint_se_nao_existir('fechamento_reabertura','fechamento_reabertura_usuario_fk','FOREIGN KEY (aprovador_id) REFERENCES public.usuario(id)')