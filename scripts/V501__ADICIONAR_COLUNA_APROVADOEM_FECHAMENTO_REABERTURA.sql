--> Adicionando a coluna aprovador_id
ALTER TABLE fechamento_reabertura ADD COLUMN IF NOT EXISTS aprovado_em timestamp null;
 