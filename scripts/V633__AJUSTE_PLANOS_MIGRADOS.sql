DO $$
	DECLARE
		situacao_plano INTEGER;
		novo_plano_aee_id INTEGER;
		codigo_aluno INTEGER;
	BEGIN
	
	FOR codigo_aluno IN SELECT DISTINCT cd_aluno FROM temp_planos_migrados LOOP
		
		UPDATE plano_aee
		SET 
			turma_id = turma_infantil,
			situacao = 7,
			alterado_em = current_date,
			alterado_por = 'Sistema',
			alterado_rf = 'Sistema'
		FROM (
			SELECT
				pa.id as id_plano,
				tpm.cd_aluno,
				t_inf.id as turma_infantil,
				pa.turma_id as turma_plano,
				pa.situacao
			FROM
				temp_planos_migrados tpm
				INNER JOIN plano_aee pa ON pa.aluno_codigo::int = tpm.cd_aluno
				INNER JOIN turma t_fund ON t_fund.turma_id::int = tpm.turma_fundamental
				INNER JOIN turma t_inf ON t_inf.turma_id::int = tpm.turma_infantil
			WHERE
				NOT pa.excluido  
				AND t_fund.id = pa.turma_id 
				AND pa.situacao NOT IN (3, 7)
				AND tpm.cd_aluno = codigo_aluno	
		) p WHERE p.id_plano = id
		RETURNING p.situacao INTO situacao_plano;

		INSERT INTO plano_aee (turma_id,aluno_codigo, aluno_nome,aluno_numero,situacao,excluido, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf,parecer_coordenacao, parecer_paai, responsavel_paai_id, responsavel_id)
		SELECT t_fund.id, tpm.cd_aluno, pa.aluno_nome, pa.aluno_numero, situacao_plano, pa.excluido, current_date,'Sistema', null, null, 'Sistema', null, pa.parecer_coordenacao ,pa.parecer_paai, pa.responsavel_paai_id, pa.responsavel_id
		FROM
			temp_planos_migrados tpm
			INNER JOIN plano_aee pa ON pa.aluno_codigo::int = tpm.cd_aluno
			INNER JOIN turma t_fund ON t_fund.turma_id::int = tpm.turma_fundamental
			INNER JOIN turma t_inf ON t_inf.turma_id::int = tpm.turma_infantil
		WHERE
			NOT pa.excluido  
			AND t_fund.id = pa.turma_id 
			AND pa.situacao NOT IN (3, 7)
			AND tpm.cd_aluno = codigo_aluno
		RETURNING id INTO novo_plano_aee_id;

		UPDATE plano_aee_versao
		SET 
			plano_aee_id = novo_plano_aee_id,
			numero = sequencia
		FROM (
			SELECT
				pav.id as id_versao,
				pav.plano_aee_id as id_plano,
				tpm.cd_aluno,
				pav.numero,
				pav.criado_em,
				ROW_NUMBER() OVER (PARTITION BY tpm.cd_aluno,pa.id, EXTRACT(YEAR FROM pav.criado_em) ORDER BY COALESCE(pav.criado_em)) sequencia
			FROM
				temp_planos_migrados tpm
				INNER JOIN plano_aee pa ON pa.aluno_codigo::int = tpm.cd_aluno
				INNER JOIN turma t_fund ON t_fund.turma_id::int = tpm.turma_fundamental
				INNER JOIN turma t_inf ON t_inf.turma_id::int = tpm.turma_infantil
				INNER JOIN plano_aee_versao pav ON pav.plano_aee_id = pa.id 
			WHERE
				NOT pa.excluido  
				AND t_fund.id = pa.turma_id 
				AND pa.situacao NOT IN (3, 7)
				AND EXTRACT(YEAR FROM pav.criado_em) >= EXTRACT(YEAR FROM CURRENT_DATE)
				AND tpm.cd_aluno = codigo_aluno
		) pv WHERE pv.id_versao = id;
   
		UPDATE plano_aee_observacao 
		SET plano_aee_id = novo_plano_aee_id
		WHERE 
			EXISTS 
			(
			SELECT pao.id AS plano_observacao_id
			FROM
				temp_planos_migrados tpm
				INNER JOIN plano_aee pa ON pa.aluno_codigo::int = tpm.cd_aluno
				INNER JOIN turma t_fund ON t_fund.turma_id::int = tpm.turma_fundamental
				INNER JOIN turma t_inf ON t_inf.turma_id::int = tpm.turma_infantil
				INNER JOIN plano_aee_observacao pao on pao.plano_aee_id = pa.id
			WHERE
				NOT pa.excluido  
				AND t_fund.id = pa.turma_id 
				AND pa.situacao NOT IN (3, 7)
				AND EXTRACT(YEAR FROM pao.criado_em) >= EXTRACT(YEAR FROM CURRENT_DATE)
				AND tpm.cd_aluno = codigo_aluno
				AND pao.id = plano_aee_observacao.id
			);
	
	END LOOP;	
END $$;
