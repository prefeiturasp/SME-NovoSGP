--> Criar perfil manual ABAE
insert into prioridade_perfil (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por, criado_rf)
select (select Max(ordem)+1 from prioridade_perfil), 2, 'ABAE', 'EA741BF4-47EA-486D-8B88-5327521BCFC5', current_timestamp, 'Carga', 'Carga'
where not exists (select 1 from prioridade_perfil where nome_perfil = 'ABAE')