create index if not EXISTS atividade_avaliativa_disciplina_disciplina_id_ix 
	on atividade_avaliativa_disciplina (disciplina_id)
	where excluido = false;
    
create index if not exists objetivo_aprendizagem_plano_plano_id_ix 
	on objetivo_aprendizagem_plano(plano_id);

create index if not exists notificacao_frequencia_aula_id_idx 
	on notificacao_frequencia(aula_id);

create index if not exists notificacao_codigo_year_ix 
	on notificacao (codigo desc, EXTRACT(year FROM criado_em));