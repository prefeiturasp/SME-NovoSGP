insert into  
	public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PAPInicioAnoLetivo','Ano Letivo inicial para o PAP.','2020', null, now(),'Carga Inicial','Carga Inicial', 28
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 28);
