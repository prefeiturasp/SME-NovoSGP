insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'MunicipioAtendimentoHistoricoEscolar','Município impresso na data do relatório de histórico escolar','São Paulo', null, now(),'Carga Inicial','Carga Inicial',25
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 25);
