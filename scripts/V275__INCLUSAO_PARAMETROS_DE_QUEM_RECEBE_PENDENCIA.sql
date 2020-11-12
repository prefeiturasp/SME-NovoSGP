insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaAulasDiasNaoLetivos','Usuarios que recebem Notificação de pendências dias não letivos','Professor', 2020, true, now(),'Carga Inicial','Carga Inicial',35
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 35);

	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaAulasDiasNaoLetivos','Tipo de Cargo que recebem Notificação de pendências dias não letivos','CP', 2020, true, now(),'Carga Inicial','Carga Inicial',35
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 35);
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Tipo de Cargo que recebem Notificação de pendências dias letivos insuficientes','CP', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36);
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Tipo de Cargo que recebem Notificação de pendências dias letivos insuficientes','AD', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36);
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Tipo de Cargo que recebem Notificação de pendências dias letivos insuficientes','Diretor', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36);
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Tipo de Cargo que recebem Notificação de pendências dias letivos insuficientes','ADM UE', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36);