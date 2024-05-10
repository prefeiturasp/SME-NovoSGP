insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'RecuperacaoParalelaFrequente','Percentual de frequência onde a recuperação paralela considera acima do limite para alunos frequentes','75',null,now(),'Carga Inicial','Carga Inicial',13
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'RecuperacaoParalelaFrequente' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'RecuperacaoParalelaPoucoFrequente','Percentual de frequência onde a recuperação paralela considera abaixo do limite para alunos poruco frequentes','75',null,now(),'Carga Inicial','Carga Inicial',13
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'RecuperacaoParalelaPoucoFrequente' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'RecuperacaoParalelaNaoComparece','Percentual de frequência onde a recuperação paralela considera baixo do limite para alunos não frequentes','50',null,now(),'Carga Inicial','Carga Inicial',13
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'RecuperacaoParalelaNaoComparece' );