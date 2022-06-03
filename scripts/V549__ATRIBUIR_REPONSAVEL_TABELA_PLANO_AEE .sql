do $$
declare 
	REC_PLANO_AEE RECORD;
	usuarioId int8;
begin
	
	raise notice '*INICIANDO ATRIBUIÇÃO DE RESPONSÁVEL PARA PLANO AEE*';
	
	for REC_PLANO_AEE in select id, criado_rf from plano_aee where responsavel_id is null
	loop
		select id into usuarioId from usuario where rf_codigo = REC_PLANO_AEE.criado_rf; 
		
		update plano_aee set responsavel_id = usuarioId where id = REC_PLANO_AEE.id;
	end loop;
	
	raise notice '*FINAL ATRIBUIÇÃO DE RESPONSÁVEL PARA PLANO AEE*';
end $$;