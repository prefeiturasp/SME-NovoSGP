CREATE OR REPLACE PROCEDURE public.monitoramento_registro_frequencia_aluno_duplicado_por_ue(IN p_ueid bigint)
 LANGUAGE plpgsql
AS $procedure$
BEGIN
	insert into monitoramento.registro_frequencia_aluno_duplicado (registro_frequencia_id, aula_id, numero_aula, aluno_codigo, quantidade, primeiro_registro, ultimo_registro, ultimo_id)	
			(
			 select rfa.registro_frequencia_id, 
					rfa.aula_id,		
					rfa.numero_aula,		
					rfa.codigo_aluno, 				
					count(rfa.id) as quantidade,
					min(rfa.criado_em) as primeiro_registro,
					max(rfa.criado_em) as ultimo_registro,
					max(rfa.id) as ultimo_id 
					from registro_frequencia_aluno rfa 
					join aula a on a.id = rfa.aula_id  
					join turma t on t.turma_id = a.turma_id 		
			where t.ano_letivo = extract(year from NOW())	
			  and t.ue_id = p_ueid	
			group by rfa.registro_frequencia_id,
					 rfa.aula_id,		 
 					 rfa.numero_aula,
					 rfa.codigo_aluno
			having count(rfa.id) > 1
	    );
END;
$procedure$
;