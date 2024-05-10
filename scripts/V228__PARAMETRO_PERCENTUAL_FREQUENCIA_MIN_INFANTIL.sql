insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFrequenciaMinimaInfantil','Percentual de frequência mínima da educação infantil para envio da notificação bimestral','60','2020',now(),'Carga Inicial','Carga Inicial',27
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 27 and ano = '2020' );
		

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFrequenciaMinimaInfantil','Percentual de frequência mínima da educação infantil para envio da notificação bimestral','60','2021',now(),'Carga Inicial','Carga Inicial',27
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 27 and ano = '2021' );