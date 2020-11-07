insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'NotificarPendenciaDiasNaoLetivos','Usuarios que recebem Notificação de pendências dias não letivos','999,3379', 2020, true, now(),'Carga Inicial','Carga Inicial',35
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 35);


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'NotificarPendenciaDiasLetivosInsuficientes','Usuarios que recebem Notificação de pendências Calendário com dias letivos abaixo do permitido','3379,3085,3360', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36);


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'NotificarPendenciaEventoCalendario','Usuarios que recebem Notificação de Cadastro de eventos pendente','', 2020, true, now(),'Carga Inicial','Carga Inicial',37
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 37);