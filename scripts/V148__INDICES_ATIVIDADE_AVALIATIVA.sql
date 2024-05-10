create index if not exists ix_filter_atividade_avaliativa on atividade_avaliativa (excluido, data_avaliacao, dre_id, ue_id, turma_id, professor_rf);

create index if not exists ix_atividade_avaliativa_id on atividade_avaliativa_disciplina(atividade_avaliativa_id);

create index if not exists ix_usuario_id on notificacao (usuario_id);