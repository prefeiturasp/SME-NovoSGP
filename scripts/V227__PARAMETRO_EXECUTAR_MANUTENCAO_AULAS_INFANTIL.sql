insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'ExecutarManutencaoAulasInfantil','Executar manutenção nas aulas infantis','1', null,true, now(),'Carga Inicial','Carga Inicial',26
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 26);
