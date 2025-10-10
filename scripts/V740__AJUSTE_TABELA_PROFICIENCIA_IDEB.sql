ALTER TABLE proficiencia_ideb RENAME COLUMN codigo_escola TO codigo_eol_escola;
ALTER TABLE proficiencia_ideb RENAME COLUMN serie TO serie_ano;

ALTER TABLE proficiencia_ideb ALTER COLUMN criado_rf DROP NOT NULL;
ALTER TABLE proficiencia_ideb ALTER COLUMN alterado_em DROP NOT NULL;
ALTER TABLE proficiencia_ideb ALTER COLUMN criado_por DROP NOT NULL;
ALTER TABLE proficiencia_ideb ALTER COLUMN criado_em DROP NOT NULL;