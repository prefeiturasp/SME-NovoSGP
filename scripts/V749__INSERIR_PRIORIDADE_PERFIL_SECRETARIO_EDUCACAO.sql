insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por, criado_rf)
select 189, 2, 'Secretário de Educação', '89e1e074-37d6-e911-abd6-f81654fe895d', current_timestamp, 'Carga', 'Carga'
where not exists (select 1 from prioridade_perfil where nome_perfil= 'Secretário de Educação');