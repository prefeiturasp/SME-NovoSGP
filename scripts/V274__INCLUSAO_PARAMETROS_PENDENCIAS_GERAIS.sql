insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'ConselhoClasse','Conselho de Classe','4', 2020, true, now(),'Carga Inicial','Carga Inicial',30
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 30);


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'ReuniaoAPM','Reunião de APM','5', 2020, true, now(),'Carga Inicial','Carga Inicial',31
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 31);


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'ReuniaoConselhoEscola','Reunião de Conselho de Escola','11', 2020, true, now(),'Carga Inicial','Carga Inicial',32
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 32);

insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'ReunioesPedagógicas','Reuniões Pedagógicas','11', 2020, true, now(),'Carga Inicial','Carga Inicial',33
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 33);

insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DataInicioGeracaoPendencia','Data de Inicio da geração de Pendências','01/03', 2020, true, now(),'Carga Inicial','Carga Inicial',34
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 34);
	



