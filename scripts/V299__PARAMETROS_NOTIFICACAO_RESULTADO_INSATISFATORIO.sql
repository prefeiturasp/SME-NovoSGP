
insert into  
	public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'DiasNotificacaoResultadoInsatisfatorio','Dias para Geração de notificação para Resultado Instatisfatório de notas','7', 2020, now(),'Notificações','Notificações', 40
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 40);
