ALTER TABLE atividade_avaliativa
ADD COLUMN migrado bool;

ALTER TABLE atividade_avaliativa
ALTER COLUMN descricao_avaliacao type varchar;