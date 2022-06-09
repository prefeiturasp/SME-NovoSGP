DO $$
BEGIN
  IF NOT EXISTS(SELECT 1
    FROM information_schema.columns
    WHERE table_name='plano_aee' and column_name='responsavel_paai_id')
  THEN
      ALTER TABLE if exists public.plano_aee RENAME COLUMN responsavel_id TO responsavel_paai_id;
  END IF;
 
  ALTER TABLE if exists public.plano_aee ADD if not exists responsavel_id int8 NULL;
 
  ALTER TABLE if exists public.plano_aee ADD CONSTRAINT plano_aee_usuario_id_responsavel_fk FOREIGN KEY (responsavel_id) REFERENCES usuario(id);

END $$;