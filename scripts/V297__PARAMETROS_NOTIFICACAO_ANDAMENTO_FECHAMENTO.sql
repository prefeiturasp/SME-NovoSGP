insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoAndamentoFechamento1','Quantidade de Dias antes do Fechamento para notificar sobre o andamento do mesmo','15', 2020, true, now(),'Carga Inicial','Carga Inicial',39
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 39
      and nome = 'DiasNotificacaoAndamentoFechamento1');

insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoAndamentoFechamento2','Quantidade de Dias antes do Fechamento para notificar sobre o andamento do mesmo','7', 2020, true, now(),'Carga Inicial','Carga Inicial',39
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 39
      and nome = 'DiasNotificacaoAndamentoFechamento2');

insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoAndamentoFechamento3','Quantidade de Dias antes do Fechamento para notificar sobre o andamento do mesmo','3', 2020, true, now(),'Carga Inicial','Carga Inicial',39
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 39
      and nome = 'DiasNotificacaoAndamentoFechamento3');
