
insert into  
	public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'DiasGeracaoPendenciaAusenciaFechamento','Dias para Geração de pendências de fechamento','15', 2020, now(),'Pendências','Pendências', 38
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 38);
