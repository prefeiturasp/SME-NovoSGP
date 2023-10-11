do $$
declare
	questaoId bigint;
	ultimoId bigint;
begin
	
	SELECT id INTO questaoId FROM questao WHERE nome_componente = 'DIFIC_APRESENTADAS';	
    SELECT max(id)+1 INTO ultimoId FROM opcao_resposta WHERE questao_id = questaoId;
    
	INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	select questaoId, ultimoId, 'Registro migrado', NOW(), 'SISTEMA', '0'
	where not exists (select 1 from opcao_resposta where nome = 'Registro migrado');
	
end $$;	