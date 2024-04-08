TRUNCATE TABLE consolidado_encaminhamento_naapa;
TRUNCATE TABLE consolidado_atendimento_naapa;

ALTER TABLE consolidado_encaminhamento_naapa ADD COLUMN IF NOT EXISTS modalidade_codigo int4 not null;
ALTER TABLE consolidado_atendimento_naapa ADD COLUMN IF NOT EXISTS modalidade_codigo int4 not null;
