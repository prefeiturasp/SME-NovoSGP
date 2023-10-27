--> Criar perfil manual Coordenador CELP
insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por, criado_rf)
select (select Max(ordem)+1 from prioridade_perfil), 2, 'Coordenador CELP', '32C01A4F-B251-4A0F-933D-5B61C8B5DDBF', current_timestamp, 'Carga', 'Carga'
where not exists (select 1 from prioridade_perfil where nome_perfil = 'Coordenador CELP')