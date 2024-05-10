insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'CompensacaoAusenciaPercentualRegenciaClasse','Percentual de frequência onde a compensação de ausência considera abaixo do limite para regência de classe','75',null,now(),'Carga Inicial','Carga Inicial',10
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 10 );

		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'CompensacaoAusenciaPercentualFund2','Percentual de frequência onde a compensação de ausência considera abaixo do limite para Fund2','50',null,now(),'Carga Inicial','Carga Inicial',11
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 11 );

		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeMaximaCompensacaoAusencia','Quantidade máxima de ausências que um registro de compensação de ausência pode compensar','10',null,now(),'Carga Inicial','Carga Inicial',12
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 12 );