insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFrequenciaCriticoBaseNacional','Percentual de frequ�ncia para definir aluno em situa��o cr�tica na Base Nacional Comum','50', null, now(),'Carga Inicial','Carga Inicial',20
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 20);
