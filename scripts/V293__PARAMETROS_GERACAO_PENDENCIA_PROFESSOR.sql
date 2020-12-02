insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasGeracaoPendenciaAvaliacaoProfessor','Quantidade de dias antes do encerramento do fechamento para gerar pendencia de avaliação para o professor','15', 2020, true, now(),'Carga Inicial','Carga Inicial',37
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 37
      and nome = 'DiasGeracaoPendenciaAvaliacaoProfessor');

insert into  
	public.parametros_sistema (nome,descricao,valor,ano,ativo,criado_em,criado_por, criado_rf,tipo)
select
	'DiasGeracaoPendenciaAvaliacaoCP','Quantidade de dias antes do encerramento do fechamento para gerar pendencia de avaliação para o CP','5', 2020, true, now(),'Carga Inicial','Carga Inicial',37
where
	not exists(
	select 	1
	from public.parametros_sistema 
	where tipo = 37
      and nome = 'DiasGeracaoPendenciaAvaliacaoCP');
