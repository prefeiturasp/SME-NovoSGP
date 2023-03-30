CREATE OR REPLACE PROCEDURE public.monitoramento_registro_frequencia_duplicado_por_dre(IN p_dreid bigint)
 LANGUAGE plpgsql
AS $procedure$
BEGIN
	insert into monitoramento.registro_frequencia_duplicado (aula_id, quantidade, primeiro_registro, ultimo_registro, ultimo_id)			
			(
			 select rf.aula_id, 				
					count(rf.id) as quantidade,
					min(rf.criado_em) as primeiro_registro,
					max(rf.criado_em) as ultimo_registro,
					max(rf.id) as ultimo_id 
					from registro_frequencia rf 
					join aula a on a.id = rf.aula_id  
					join turma t on t.turma_id = a.turma_id 
					join ue on ue.id = t.ue_id 
					join dre on dre.id = ue.dre_id 					
			where t.ano_letivo = extract(year from NOW())	
			  and dre.id = p_dreid	
			group by rf.aula_id
			having count(rf.id) > 1
	    );
END;
$procedure$
;