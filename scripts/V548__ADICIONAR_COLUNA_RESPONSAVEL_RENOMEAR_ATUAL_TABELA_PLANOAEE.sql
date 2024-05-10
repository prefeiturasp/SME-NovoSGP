DO $$
BEGIN
  IF NOT EXISTS(SELECT 1
    FROM information_schema.columns
    WHERE table_name='plano_aee' and column_name='responsavel_paai_id')
  THEN
      ALTER TABLE if exists public.plano_aee RENAME COLUMN responsavel_id TO responsavel_paai_id;
	  ALTER TABLE if exists public.plano_aee ADD if not exists responsavel_id int8 NULL;
	  ALTER TABLE if exists public.plano_aee ADD CONSTRAINT plano_aee_usuario_id_paai_responsavel_fk FOREIGN KEY (responsavel_paai_id) REFERENCES usuario(id);
	  CREATE INDEX plano_aee_responsavel_id_idx ON public.plano_aee USING btree (responsavel_id);
	  CREATE INDEX plano_aee_id_paai_responsavel_idx ON public.plano_aee USING btree (responsavel_paai_id);
  END IF;
END $$;