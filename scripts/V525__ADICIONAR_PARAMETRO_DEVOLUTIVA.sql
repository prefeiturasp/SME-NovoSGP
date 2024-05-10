insert
	into
	parametros_sistema
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
select'Devolutiva',
86,
'Utilizar no Layout Para Relatorio de Devolutiva',
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
		parametros_sistema
	where
		tipo = 86 and ano = 2022);
