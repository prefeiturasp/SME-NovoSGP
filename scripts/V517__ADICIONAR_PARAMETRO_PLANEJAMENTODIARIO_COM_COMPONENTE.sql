insert
	into
	public.parametros_sistema
(nome,
	tipo,
	descricao,
	valor,
	ano,
	ativo,
	criado_em,
	criado_por,
	alterado_em,
	alterado_por,
	criado_rf,
	alterado_rf)
select'ControlePlanejamentoDiarioInfantilComComponente',
85,
'Utilizar no Layout Para Relatorio de Planejamento Diario',
'2022',
2022,
true,
now(),
'SISTEMA',
null,
null,
'0',
null
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 85 and ano 2022);
