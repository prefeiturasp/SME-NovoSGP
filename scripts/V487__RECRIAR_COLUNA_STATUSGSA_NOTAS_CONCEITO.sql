ALTER TABLE notas_conceito
	DROP COLUMN IF EXISTS STATUS_GSA;

ALTER TABLE Public.notas_conceito
    ADD COLUMN IF NOT EXISTS STATUS_GSA int;