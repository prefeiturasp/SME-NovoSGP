do $$
declare 
	id_questionario_itinerancia_naapa int;
begin
	--INSET QUESTAO ITINERANCIA (ANEXO)
	id_questionario_itinerancia_naapa := (select id from public.questionario where tipo = 5 and id in (select questionario_id from public.secao_encaminhamento_naapa where nome_componente = 'QUESTOES_ITINERACIA'));
	insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf) 
		select id_questionario_itinerancia_naapa,  6, 'Anexos', false, 6, now(),'Carga Inicial','Carga Inicial' where not exists(select 1 from public.questao where questionario_id = id_questionario_itinerancia_naapa and tipo = 6);
end $$