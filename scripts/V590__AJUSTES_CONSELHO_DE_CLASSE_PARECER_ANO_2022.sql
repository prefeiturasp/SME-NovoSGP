do $$
declare
	_conselho_parecer_ano_promovidos_parecer_1 record;
	_conselho_parecer_ano_promovido record;
	_conselho_parecer_ano_promovido_nao_existente record;
	_conselho_parecer_ano_ret_freq_nao_existente record;
	_conselho_parecer_ano_cont_estudos record;
	_conselho_parecer_ano_cont_estudos_nao_existente record;
	_conselho_parecer_ano_ret_freq2_nao_existente record;
	parecer_conclusivo_promovido_novo int;
	parecer_conclusivo_promovido_ano_atipico int;
begin	
	UPDATE conselho_classe_parecer 
		SET fim_vigencia = '2021-12-31'
		WHERE id = 1;

	--Parecer Promovido 2022
	INSERT INTO public.conselho_classe_parecer (nome, aprovado, frequencia, nota, inicio_vigencia, fim_vigencia, criado_por, criado_em, criado_rf)
	  (SELECT 'Promovido',
			true,
			true,
			false,
			  '2022-01-01',
			  '2022-12-31',
			  'SISTEMA',
			  '2022-01-01',
			  '0'
	   WHERE NOT EXISTS
		   (SELECT ccp.id
			FROM conselho_classe_parecer ccp
			WHERE ccp.nome = 'Promovido'
			  AND ccp.inicio_vigencia = '2022-01-01'
			  AND ccp.fim_vigencia = '2022-12-31'));

	--Parecer Promovido 2023
	INSERT INTO public.conselho_classe_parecer (nome, aprovado, frequencia, nota, inicio_vigencia, fim_vigencia, criado_por, criado_em, criado_rf)
	  (SELECT 'Promovido',
			true,
			true,
			true,
			  '2023-01-01',
			  null,
			  'SISTEMA',
			  '2023-01-01',
			  '0'
	   WHERE NOT EXISTS
		   (SELECT ccp.id
			FROM conselho_classe_parecer ccp
			WHERE ccp.nome = 'Promovido'
			  AND ccp.inicio_vigencia = '2023-01-01'));
			  
	parecer_conclusivo_promovido_ano_atipico := (select id from conselho_classe_parecer where nome = 'Promovido' and inicio_vigencia = '2022-01-01'  and fim_vigencia = '2022-12-31');
	parecer_conclusivo_promovido_novo := (select id from conselho_classe_parecer where nome = 'Promovido' and inicio_vigencia = '2023-01-01');
		
	--Update pareceres ano vigentes que ainda apontavam para parecer Promovido Id 1	
	for _conselho_parecer_ano_promovidos_parecer_1 in 
		select ccp.nome, ccp.id as parecer_id, ccpa.id, ccpa.ano_turma, ccpa.modalidade, ccpa.etapa_eja, ccpa.inicio_vigencia, ccpa.fim_vigencia
			from conselho_classe_parecer_ano ccpa 
			inner join conselho_classe_parecer ccp on ccp.id = ccpa.parecer_id 
			where ccp.id in (1) --promovido até 2022
			and ccpa.fim_vigencia is null  and ccpa.inicio_vigencia <> '2023-01-01'
  	loop
  		
		UPDATE conselho_classe_parecer_ano SET fim_vigencia = '2021-12-31' 
			WHERE id = _conselho_parecer_ano_promovidos_parecer_1.id;
		
		INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
			  (SELECT parecer_conclusivo_promovido_novo,
					  _conselho_parecer_ano_promovidos_parecer_1.ano_turma,
					  _conselho_parecer_ano_promovidos_parecer_1.modalidade,
					  _conselho_parecer_ano_promovidos_parecer_1.etapa_eja,
					  '2023-01-01',
					  'SISTEMA',
					  '2023-01-01',
					  '0'
			   WHERE NOT EXISTS
				   (SELECT ccpa.id
					FROM conselho_classe_parecer_ano ccpa
					WHERE ccpa.parecer_id = parecer_conclusivo_promovido_novo
					  AND ccpa.modalidade = _conselho_parecer_ano_promovidos_parecer_1.modalidade
					  AND ccpa.etapa_eja = _conselho_parecer_ano_promovidos_parecer_1.etapa_eja
					  AND ccpa.ano_turma =  _conselho_parecer_ano_promovidos_parecer_1.ano_turma
					  AND inicio_vigencia = '2023-01-01'));	
			
		 	--raise notice 'Passou!'; 
  	end loop;
	
	

	--INATIVAÇÃO DE PARECERES ANO NÃO NECESSÁRIOS PARA 2022 E JOGADOS PARA 2023
	for _conselho_parecer_ano_promovido in 
		select ccp.nome, ccp.id as parecer_id, ccpa.id, ccpa.ano_turma, ccpa.modalidade, ccpa.etapa_eja, ccpa.inicio_vigencia, ccpa.fim_vigencia
			from conselho_classe_parecer_ano ccpa 
			inner join conselho_classe_parecer ccp on ccp.id = ccpa.parecer_id 
			where ccp.id not in (1, 5, parecer_conclusivo_promovido_novo) --diferente de promovido e retido por frequencia
			and ((ccpa.ano_turma = 9 and ccpa.modalidade = 5) or
				 (ccpa.ano_turma = 3 and ccpa.modalidade = 6) or 
				 (ccpa.ano_turma = 4 and ccpa.modalidade = 6) or
				 (ccpa.etapa_eja = 2 and ccpa.modalidade = 3 and ccpa.ano_turma = 4)
				)
			and ccpa.fim_vigencia is null  and ccpa.inicio_vigencia <> '2023-01-01'
  	loop
  		
		UPDATE conselho_classe_parecer_ano SET fim_vigencia = '2021-12-31' 
			WHERE id = _conselho_parecer_ano_promovido.id;
		
		INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
			  (SELECT _conselho_parecer_ano_promovido.parecer_id,
					  _conselho_parecer_ano_promovido.ano_turma,
					  _conselho_parecer_ano_promovido.modalidade,
					  _conselho_parecer_ano_promovido.etapa_eja,
					  '2023-01-01',
					  'SISTEMA',
					  '2023-01-01',
					  '0'
			   WHERE NOT EXISTS
				   (SELECT ccpa.id
					FROM conselho_classe_parecer_ano ccpa
					WHERE ccpa.parecer_id = _conselho_parecer_ano_promovido.parecer_id
					  AND ccpa.modalidade = _conselho_parecer_ano_promovido.modalidade
					  AND ccpa.etapa_eja = _conselho_parecer_ano_promovido.etapa_eja
					  AND ccpa.ano_turma =  _conselho_parecer_ano_promovido.ano_turma
					  AND inicio_vigencia = '2023-01-01'));	
			
		 	--raise notice 'Passou!'; 
  	end loop;
	
	--CRIAÇÃO DOS PARECERES ANO PROMOVIDO PARA 2022 CASO NAO EXISTA VIGENTE
	for _conselho_parecer_ano_promovido_nao_existente in 
		select distinct ccpa1.ano_turma, ccpa1.modalidade, ccpa1.etapa_eja
				from conselho_classe_parecer_ano ccpa1 
				 WHERE NOT EXISTS
								   (SELECT ccpa.id
									FROM conselho_classe_parecer_ano ccpa
									WHERE ccpa.parecer_id = parecer_conclusivo_promovido_ano_atipico
									  AND ccpa.modalidade = ccpa1.modalidade
									  AND ccpa.ano_turma =  ccpa1.ano_turma	
									  and (ccpa.etapa_eja = ccpa1.etapa_eja or (ccpa.etapa_eja is null and ccpa1.etapa_eja is null))
									 and ((ccpa.ano_turma = 9 and ccpa.modalidade = 5) or
											 (ccpa.ano_turma = 3 and ccpa.modalidade = 6) or 
											 (ccpa.ano_turma = 4 and ccpa.modalidade = 6) or
											 (ccpa.etapa_eja = 2 and ccpa.modalidade = 3 and ccpa.ano_turma = 4))
									AND ((ccpa.fim_vigencia is null and ccpa.inicio_vigencia <= '2022-01-01') or (ccpa.inicio_vigencia = '2022-01-01' and ccpa.fim_vigencia = '2022-12-31')))
				and ((ccpa1.ano_turma = 9 and ccpa1.modalidade = 5) or
											 (ccpa1.ano_turma = 3 and ccpa1.modalidade = 6) or 
											 (ccpa1.ano_turma = 4 and ccpa1.modalidade = 6) or
											 (ccpa1.etapa_eja = 2 and ccpa1.modalidade = 3 and ccpa1.ano_turma = 4))

  	loop
  		
		INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf, fim_vigencia)
			 values (parecer_conclusivo_promovido_ano_atipico,
					  _conselho_parecer_ano_promovido_nao_existente.ano_turma,
					  _conselho_parecer_ano_promovido_nao_existente.modalidade,
					  _conselho_parecer_ano_promovido_nao_existente.etapa_eja,
					  '2022-01-01',
					  'SISTEMA',
					  '2022-01-01',
					  '0',
					  '2022-12-31');		
			
		 	--raise notice 'Passou!'; 
  	end loop;
	
	--CRIAÇÃO DOS PARECERES ANO RETIDO FREQUENCIA PARA 2022 CASO NAO EXISTA VIGENTE
	for _conselho_parecer_ano_ret_freq_nao_existente in 
		select distinct ccpa1.ano_turma, ccpa1.modalidade, ccpa1.etapa_eja
				from conselho_classe_parecer_ano ccpa1 
				 WHERE NOT EXISTS
								   (SELECT ccpa.id
									FROM conselho_classe_parecer_ano ccpa
									WHERE ccpa.parecer_id = 5
									  AND ccpa.modalidade = ccpa1.modalidade
									  AND ccpa.ano_turma =  ccpa1.ano_turma	
									  and (ccpa.etapa_eja = ccpa1.etapa_eja or (ccpa.etapa_eja is null and ccpa1.etapa_eja is null))
									 and ((ccpa.ano_turma = 9 and ccpa.modalidade = 5) or
											 (ccpa.ano_turma = 3 and ccpa.modalidade = 6) or 
											 (ccpa.ano_turma = 4 and ccpa.modalidade = 6) or
											 (ccpa.etapa_eja = 2 and ccpa.modalidade = 3 and ccpa.ano_turma = 4))
									AND ((ccpa.fim_vigencia is null and ccpa.inicio_vigencia <= '2022-01-01') or (ccpa.inicio_vigencia = '2022-01-01' and ccpa.fim_vigencia = '2022-12-31')))
				and ((ccpa1.ano_turma = 9 and ccpa1.modalidade = 5) or
						(ccpa1.ano_turma = 3 and ccpa1.modalidade = 6) or 
						(ccpa1.ano_turma = 4 and ccpa1.modalidade = 6) or
						(ccpa1.etapa_eja = 2 and ccpa1.modalidade = 3 and ccpa1.ano_turma = 4))

  	loop
  		
		INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf, fim_vigencia)
			 values (5,
					  _conselho_parecer_ano_ret_freq_nao_existente.ano_turma,
					  _conselho_parecer_ano_ret_freq_nao_existente.modalidade,
					  _conselho_parecer_ano_ret_freq_nao_existente.etapa_eja,
					  '2022-01-01',
					  'SISTEMA',
					  '2022-01-01',
					  '0',
					  '2022-12-31');		
			
		 	--raise notice 'Passou!'; 
  	end loop;
	
	--INATIVAÇÃO DE PARECERES ANO NÃO NECESSÁRIOS PARA 2022 E JOGADOS PARA 2023
	for _conselho_parecer_ano_cont_estudos in 
		select ccp.nome, ccp.id as parecer_id, ccpa.id, ccpa.ano_turma, ccpa.modalidade, ccpa.etapa_eja, ccpa.inicio_vigencia, ccpa.fim_vigencia
			from conselho_classe_parecer_ano ccpa 
			inner join conselho_classe_parecer ccp on ccp.id = ccpa.parecer_id 
			where ccp.id not in (3, 5) --diferente de continuidade estudos e retido por frequencia
			and not ((ccpa.ano_turma = 9 and ccpa.modalidade = 5) or
				 (ccpa.ano_turma = 3 and ccpa.modalidade = 6) or 
				 (ccpa.ano_turma = 4 and ccpa.modalidade = 6) or
				 (ccpa.etapa_eja = 2 and ccpa.modalidade = 3 and ccpa.ano_turma = 4)
				)
			and ccpa.fim_vigencia is null and ccpa.inicio_vigencia <> '2023-01-01'
  	loop
  		
		UPDATE conselho_classe_parecer_ano SET fim_vigencia = '2021-12-31' 
			WHERE id = _conselho_parecer_ano_cont_estudos.id;
		
		INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
			  (SELECT _conselho_parecer_ano_cont_estudos.parecer_id,
					  _conselho_parecer_ano_cont_estudos.ano_turma,
					  _conselho_parecer_ano_cont_estudos.modalidade,
					  _conselho_parecer_ano_cont_estudos.etapa_eja,
					  '2023-01-01',
					  'SISTEMA',
					  '2023-01-01',
					  '0'
			   WHERE NOT EXISTS
				   (SELECT ccpa.id
					FROM conselho_classe_parecer_ano ccpa
					WHERE ccpa.parecer_id = _conselho_parecer_ano_cont_estudos.parecer_id
					  AND ccpa.modalidade = _conselho_parecer_ano_cont_estudos.modalidade
					  AND ccpa.etapa_eja = _conselho_parecer_ano_cont_estudos.etapa_eja
					  AND ccpa.ano_turma =  _conselho_parecer_ano_cont_estudos.ano_turma
					  AND inicio_vigencia = '2023-01-01'));		  	
			
		 	--raise notice 'Passou!'; 
  	end loop;
	
	--CRIAÇÃO DOS PARECERES ANO CONTINUIDADE DOS ESTUDOS PARA 2022 CASO NAO EXISTA VIGENTE	
	for _conselho_parecer_ano_cont_estudos_nao_existente in 
		select distinct ccpa1.ano_turma, ccpa1.modalidade, ccpa1.etapa_eja
				from conselho_classe_parecer_ano ccpa1 
				 WHERE NOT EXISTS
								   (SELECT ccpa.id
									FROM conselho_classe_parecer_ano ccpa
									WHERE ccpa.parecer_id = 3
									  AND ccpa.modalidade = ccpa1.modalidade
									  AND ccpa.ano_turma =  ccpa1.ano_turma	
									  and (ccpa.etapa_eja = ccpa1.etapa_eja or (ccpa.etapa_eja is null and ccpa1.etapa_eja is null))
									 and not ((ccpa.ano_turma = 9 and ccpa.modalidade = 5) or
											 (ccpa.ano_turma = 3 and ccpa.modalidade = 6) or 
											 (ccpa.ano_turma = 4 and ccpa.modalidade = 6) or
											 (ccpa.etapa_eja = 2 and ccpa.modalidade = 3 and ccpa.ano_turma = 4))
									AND ((ccpa.fim_vigencia is null and ccpa.inicio_vigencia <= '2022-01-01') or (ccpa.inicio_vigencia = '2022-01-01' and ccpa.fim_vigencia = '2022-12-31')))
				and not ((ccpa1.ano_turma = 9 and ccpa1.modalidade = 5) or
						(ccpa1.ano_turma = 3 and ccpa1.modalidade = 6) or 
						(ccpa1.ano_turma = 4 and ccpa1.modalidade = 6) or
						(ccpa1.etapa_eja = 2 and ccpa1.modalidade = 3 and ccpa1.ano_turma = 4))

  	loop
  		
		INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf, fim_vigencia)
			 values (3,
					  _conselho_parecer_ano_cont_estudos_nao_existente.ano_turma,
					  _conselho_parecer_ano_cont_estudos_nao_existente.modalidade,
					  _conselho_parecer_ano_cont_estudos_nao_existente.etapa_eja,
					  '2022-01-01',
					  'SISTEMA',
					  '2022-01-01',
					  '0',
					  '2022-12-31');		
			
		 	--raise notice 'Passou!'; 
  	end loop;
	
	--CRIAÇÃO DOS PARECERES ANO RETIDO FREQUENCIA PARA 2022 CASO NAO EXISTA VIGENTE	
	for _conselho_parecer_ano_ret_freq2_nao_existente in 
		select distinct ccpa1.ano_turma, ccpa1.modalidade, ccpa1.etapa_eja
				from conselho_classe_parecer_ano ccpa1 
				 WHERE NOT EXISTS
								   (SELECT ccpa.id
									FROM conselho_classe_parecer_ano ccpa
									WHERE ccpa.parecer_id = 5
									  AND ccpa.modalidade = ccpa1.modalidade
									  AND ccpa.ano_turma =  ccpa1.ano_turma	
									  and (ccpa.etapa_eja = ccpa1.etapa_eja or (ccpa.etapa_eja is null and ccpa1.etapa_eja is null))
									 and not ((ccpa.ano_turma = 9 and ccpa.modalidade = 5) or
											 (ccpa.ano_turma = 3 and ccpa.modalidade = 6) or 
											 (ccpa.ano_turma = 4 and ccpa.modalidade = 6) or
											 (ccpa.etapa_eja = 2 and ccpa.modalidade = 3 and ccpa.ano_turma = 4))
									AND ((ccpa.fim_vigencia is null and ccpa.inicio_vigencia <= '2022-01-01') or (ccpa.inicio_vigencia = '2022-01-01' and ccpa.fim_vigencia = '2022-12-31')))
				and not ((ccpa1.ano_turma = 9 and ccpa1.modalidade = 5) or
						(ccpa1.ano_turma = 3 and ccpa1.modalidade = 6) or 
						(ccpa1.ano_turma = 4 and ccpa1.modalidade = 6) or
						(ccpa1.etapa_eja = 2 and ccpa1.modalidade = 3 and ccpa1.ano_turma = 4))

  	loop
  		
		INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf, fim_vigencia)
			 values (5,
					  _conselho_parecer_ano_ret_freq2_nao_existente.ano_turma,
					  _conselho_parecer_ano_ret_freq2_nao_existente.modalidade,
					  _conselho_parecer_ano_ret_freq2_nao_existente.etapa_eja,
					  '2022-01-01',
					  'SISTEMA',
					  '2022-01-01',
					  '0',
					  '2022-12-31');		
			
		 	--raise notice 'Passou!'; 
  	end loop;
end $$ 