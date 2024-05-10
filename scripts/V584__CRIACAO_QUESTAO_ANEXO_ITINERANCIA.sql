do $$
begin
	--INSET QUESTAO ITINERANCIA (ANEXO)
	insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf) values(
	(select id from public.questionario where tipo = 3), 2, 'Anexos', false, 6, now(),'Carga Inicial','Carga Inicial'); 
end $$
	