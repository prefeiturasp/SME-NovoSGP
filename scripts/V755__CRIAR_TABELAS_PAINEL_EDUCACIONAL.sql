 CREATE INDEX IF NOT EXISTS idx_cfam_turma_ano 
ON turma (ano_letivo);

CREATE INDEX IF NOT EXISTS idx_cfam_mes 
ON consolidacao_frequencia_aluno_mensal (mes);

CREATE INDEX IF NOT EXISTS idx_cfam_turma_id 
ON consolidacao_frequencia_aluno_mensal (turma_id);

CREATE INDEX IF NOT EXISTS idx_ue_dre_id 
ON ue (dre_id);