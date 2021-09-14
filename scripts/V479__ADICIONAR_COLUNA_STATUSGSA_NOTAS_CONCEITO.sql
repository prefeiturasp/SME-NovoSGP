ALTER TABLE Public.notas_conceito
    ADD COLUMN IF NOT EXISTS STATUS_GSA int default 1;