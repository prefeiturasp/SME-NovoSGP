insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoFechamentoPendente1','Quantidade de Dias antes do Fechamento para notificar UEs com percentual de turmas fechadas insuficientes','7', 2020, true, now(),'Carga Inicial','Carga Inicial',41
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 41
      and nome = 'DiasNotificacaoFechamentoPendente1');

insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoFechamentoPendente2','Quantidade de Dias antes do Fechamento para notificar UEs com percentual de turmas fechadas insuficientes','3', 2020, true, now(),'Carga Inicial','Carga Inicial',41
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 41
      and nome = 'DiasNotificacaoFechamentoPendente2');

insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFechamentosInsuficientesNotificacao','Percentual de turmas com fechamento não realizados para notificar Supervisor da UE','50', 2020, true, now(),'Carga Inicial','Carga Inicial',42
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 42
      and nome = 'PercentualFechamentosInsuficientesNotificacao');
