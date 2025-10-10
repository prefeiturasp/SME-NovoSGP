-- Remove a constraint atual
ALTER TABLE painel_educacional_consolidacao_ideb 
DROP CONSTRAINT IF EXISTS uq_painel_ideb_ano_etapa_faixa;

-- Adiciona a nova constraint incluindo codigo_dre e codigo_ue
ALTER TABLE painel_educacional_consolidacao_ideb 
ADD CONSTRAINT uq_painel_ideb_ano_etapa_faixa_dre_ue 
UNIQUE (ano_letivo, etapa, faixa, codigo_dre, codigo_ue);