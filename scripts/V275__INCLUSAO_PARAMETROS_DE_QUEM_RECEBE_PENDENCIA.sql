insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaAulasDiasNaoLetivos','Gerar pendencia de aulas em dias não letivos para o professor','Professor', 2020, true, now(),'Carga Inicial','Carga Inicial',35;

	
insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaAulasDiasNaoLetivos','Gerar pendencia de aulas em dias não letivos para CP','CP', 2020, true, now(),'Carga Inicial','Carga Inicial',35;


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o CP','CP', 2020, true, now(),'Carga Inicial','Carga Inicial',36;


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o AD','AD', 2020, true, now(),'Carga Inicial','Carga Inicial',36;


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o Diretor','Diretor', 2020, true, now(),'Carga Inicial','Carga Inicial',36;


insert  into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'GerarPendenciaDiasLetivosInsuficientes','Gerar pendências de dias letivos insuficientes para o ADM UE','ADM UE', 2020, true, now(),'Carga Inicial','Carga Inicial',36;