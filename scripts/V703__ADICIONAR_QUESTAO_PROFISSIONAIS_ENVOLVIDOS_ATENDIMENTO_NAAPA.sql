    do $$
declare 
	id_questionario_itinerancia_naapa int;
begin
	--INSET QUESTAO ITINERANCIA (PROFISSIONAIS ENVOLVIDOS)
	id_questionario_itinerancia_naapa := (select id from public.questionario where tipo = 5 and id in (select questionario_id from public.secao_encaminhamento_naapa where nome_componente = 'QUESTOES_ITINERANCIA'));
	update questao set ordem = 6 where questionario_id  = id_questionario_itinerancia_naapa and nome_componente = 'DESCRICAO_DO_ATENDIMENTO';
	update questao set ordem = 7 where questionario_id  = id_questionario_itinerancia_naapa and nome_componente = 'ANEXO_ITINERANCIA';
	insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf, nome_componente, dimensao) 
		select id_questionario_itinerancia_naapa,  5, 'Profissionais envolvidos', false, 23, now(),'SISTEMA','0','PROFISSIONAIS_ENVOLVIDOS_ATENDIMENTO',12 
	where not exists(select 1 from public.questao where questionario_id = id_questionario_itinerancia_naapa and tipo = 23);
end $$ 