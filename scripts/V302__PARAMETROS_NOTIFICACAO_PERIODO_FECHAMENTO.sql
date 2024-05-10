
insert into  
	public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoPeriodoFechamentoInicio','Dias para Geração de notificação de início do Período de Fechamento','3', 2020, now(),'Notificações','Notificações', 43
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 43);


insert into  
	public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoPeriodoFechamentoFim','Dias para Geração de notificação de fim do Período de Fechamento','7', 2020, now(),'Notificações','Notificações', 44
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 44);
