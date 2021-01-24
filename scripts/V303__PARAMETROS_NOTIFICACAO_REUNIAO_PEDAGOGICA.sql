insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoReuniaoPedagogica','Notificar evento de reunião pedagogica com X dias de antecedencia','3', 2020, true, now(),'Carga Inicial','Carga Inicial',45
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 45
      and nome = 'DiasNotificacaoReuniaoPedagogica');
