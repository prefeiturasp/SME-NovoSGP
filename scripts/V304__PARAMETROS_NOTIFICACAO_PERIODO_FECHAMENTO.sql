insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoPeriodoFechamentoUe','Notificar UEs sobre Periodos de Fechamento não cadastrados X dias antes do inicio do Período da SME','7', 2020, true, now(),'Carga Inicial','Carga Inicial',46
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 46
      and nome = 'DiasNotificacaoPeriodoFechamentoUe');

insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoPeriodoFechamentoDre','Notificar DREs sobre Periodos de Fechamento não cadastrados X dias antes do inicio do Período da SME','3', 2020, true, now(),'Carga Inicial','Carga Inicial',47
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 47
      and nome = 'DiasNotificacaoPeriodoFechamentoDre');
