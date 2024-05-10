do $$
declare 
  _classificacaoDocumentoId integer :=0 ;
  _classificacaoDocumentoCPedId integer :=0;
begin 
	_classificacaoDocumentoId := (select case when cd.id != (select id from classificacao_documento where descricao = 'Carta Pedagógica' order by id limit 1) 
											  then cd.id 
											  else 0 
											  end as Id 
											  from classificacao_documento cd 
										     	where cd.descricao = 'Carta Pedagógica' 
										      order by cd.id desc limit 1);
										     
	--Valor Correto, caso tenha duplicidade na base que rodar o script.
    _classificacaoDocumentoCPedId := (select id from classificacao_documento where descricao = 'Carta Pedagógica' order by id limit 1);
										     
	
	if _classificacaoDocumentoId <> 0 then 
		-- Modifica as classificações com vínculo incorreto
			update documento 
			set classificacao_documento_id = _classificacaoDocumentoCPedId, 
			alterado_em = current_timestamp,
			alterado_por = 'Sistema',
			alterado_rf = '0'
			where id in (select d.id from documento d
			inner join classificacao_documento cd on cd.id = d.classificacao_documento_id 
			where cd.id = _classificacaoDocumentoId);
			
		-- Exclui a classificação incorreta - Carta pedagógica duplicada.
			delete from classificacao_documento cd where cd.id = _classificacaoDocumentoId;

	end if;
end $$