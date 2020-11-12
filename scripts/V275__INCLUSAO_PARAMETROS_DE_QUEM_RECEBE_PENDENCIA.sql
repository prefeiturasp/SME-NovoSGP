insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaAulasDiasNaoLetivos','Gerar pendencia de aulas em dias não letivos para o professor','Professor', 2020, true, now(),'Carga Inicial','Carga Inicial',35
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 35
      and valor = 'Professor');

	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaAulasDiasNaoLetivos','Gerar pendencia de aulas em dias não letivos para CP','CP', 2020, true, now(),'Carga Inicial','Carga Inicial',35
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 35
	  and valor = 'CP');
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o CP','CP', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36
	  and valor = 'CP');
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o AD','AD', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36
	  and valor = 'AD');
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o Diretor','Diretor', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36
	  and valor = 'Diretor');
	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o ADM UE','ADM UE', 2020, true, now(),'Carga Inicial','Carga Inicial',36
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 36
	  and valor = 'ADM UE');