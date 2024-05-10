do $$
declare 
	parecer_conclusivo_continuidade_estudos int;
    parecer_conclusivo_retido_frequencia int;
begin	
	parecer_conclusivo_continuidade_estudos := (select id from conselho_classe_parecer where nome = 'Continuidade dos estudos' and fim_vigencia is null);
	parecer_conclusivo_retido_frequencia := (select id from conselho_classe_parecer where nome = 'Retido por frequência' and fim_vigencia is null);

	UPDATE conselho_classe_parecer_ano 
		SET fim_vigencia = '2022-12-31'
		WHERE ano_turma in (7,8) 
		and inicio_vigencia < '2023-01-01'
		and fim_vigencia is null;

	--Parecer anual continuidade estudos
	INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
			  (SELECT parecer_conclusivo_continuidade_estudos,
					  7,
					  5,
					  '2023-01-01',
					  'SISTEMA',
					  current_timestamp,
					  '0'
			   WHERE NOT EXISTS
				   (SELECT ccpa.id
					FROM conselho_classe_parecer_ano ccpa
					WHERE ccpa.parecer_id = parecer_conclusivo_continuidade_estudos
					  AND ccpa.ano_turma =  7  
	   				  AND ccpa.modalidade = 5
					  AND inicio_vigencia = '2023-01-01'));	
					 
	INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
			  (SELECT parecer_conclusivo_continuidade_estudos,
					  8,
					  5,
					  '2023-01-01',
					  'SISTEMA',
					  current_timestamp,
					  '0'
			   WHERE NOT EXISTS
				   (SELECT ccpa.id
					FROM conselho_classe_parecer_ano ccpa
					WHERE ccpa.parecer_id = parecer_conclusivo_continuidade_estudos
					  AND ccpa.ano_turma =  8
					  AND ccpa.modalidade = 5
					  AND inicio_vigencia = '2023-01-01'));		
					 
	--Parecer anual retido frequencia
	INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
			  (SELECT parecer_conclusivo_retido_frequencia,
					  7,
					  5,
					  '2023-01-01',
					  'SISTEMA',
					  current_timestamp,
					  '0'
			   WHERE NOT EXISTS
				   (SELECT ccpa.id
					FROM conselho_classe_parecer_ano ccpa
					WHERE ccpa.parecer_id = parecer_conclusivo_retido_frequencia
					  AND ccpa.ano_turma =  7  
	   				  AND ccpa.modalidade = 5
					  AND inicio_vigencia = '2023-01-01'));	
					 
	INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
			  (SELECT parecer_conclusivo_retido_frequencia,
					  8,
					  5,
					  '2023-01-01',
					  'SISTEMA',
					  current_timestamp,
					  '0'
			   WHERE NOT EXISTS
				   (SELECT ccpa.id
					FROM conselho_classe_parecer_ano ccpa
					WHERE ccpa.parecer_id = parecer_conclusivo_retido_frequencia
					  AND ccpa.ano_turma =  8
					  AND ccpa.modalidade = 5
					  AND inicio_vigencia = '2023-01-01'));							 
			
end $$ 