do $$
declare 
	questionarioId bigint;	
begin
	select questionario_id 
		into questionarioId
	from secao_encaminhamento_naapa;

	update secao_encaminhamento_naapa set nome = 'Informações' where questionario_id = questionarioId and ordem = 1;
end $$
	