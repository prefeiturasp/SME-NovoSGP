insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeDiasAlteracaoNotaFinal','Quantidade de dias para gerar notificação caso a nota final seja alterada','30','2020',now(),'Carga Inicial','Carga Inicial',19
where
	not exists(
	select
		1
	from
		public.parametros_sistema 
	where
		tipo = 19 and ano = '2020');