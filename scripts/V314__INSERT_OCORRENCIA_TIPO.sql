alter table public.ocorrencia_tipo alter column descricao type VARCHAR(50);

insert into public.ocorrencia_tipo 
(alterado_em, alterado_por, alterado_rf, criado_em , criado_por , criado_rf , descricao , excluido )
values
(null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Incidente (Brigas, desentendimentos)', false),
(null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Acidente (quedas, machucados)', false),
(null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Alimentação', false),
(null, null, null, CURRENT_TIMESTAMP, 'Carga inicial', 'Carga inicial', 'Como chegou à escola?', false)