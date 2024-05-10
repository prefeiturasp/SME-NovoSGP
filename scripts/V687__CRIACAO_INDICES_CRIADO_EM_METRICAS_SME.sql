create index IF NOT EXISTS idx_aula_criado_em on aula (criado_em);
create index IF NOT EXISTS idx_registro_frequencia_criado_em on registro_frequencia (criado_em); 
create index IF NOT EXISTS idx_plano_aula_criado_em on plano_aula (criado_em);
create index IF NOT EXISTS idx_diario_bordo_criado_em on diario_bordo (criado_em);
create index IF NOT EXISTS idx_devolutiva_criado_em on devolutiva (criado_em);
create index IF NOT EXISTS idx_plano_aee_criado_em on plano_aee (criado_em);
create index IF NOT EXISTS idx_encaminhamento_aee_criado_em on encaminhamento_aee (criado_em);