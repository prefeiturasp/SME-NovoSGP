insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaEIPre',
	60,
	'Habilita o tipo Remota para Frequências - Modalidade Infantil Pré-escola',
	'1',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaEIPre'
			and ano = '2021'
	);

insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaEICEI',
	61,
	'Habilita o tipo Remota para Frequências - Modalidade Infantil CEI',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaEICEI'
			and ano = '2021'
	);

insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaEJA',
	62,
	'Habilita o tipo Remota para Frequências - Modalidade EJA',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaEJA'
			and ano = '2021'
	);

insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaCIEJA',
	63,
	'Habilita o tipo Remota para Frequências - Modalidade CIEJA',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaCIEJA'
			and ano = '2021'
	);

insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaEF',
	64,
	'Habilita o tipo Remota para Frequências - Modalidade Fundamental',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaEF'
			and ano = '2021'
	);

insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaEM',
	65,
	'Habilita o tipo Remota para Frequências - Modalidade Médio',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaEM'
			and ano = '2021'
	);

insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaCMCT',
	66,
	'Habilita o tipo Remota para Frequências - Modalidade CMCT',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaCMCT'
			and ano = '2021'
	);

insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaMOVA',
	67,
	'Habilita o tipo Remota para Frequências - Modalidade MOVA',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaMOVA'
			and ano = '2021'
	);
	
insert into public.parametros_sistema (
		nome,
		tipo,
		descricao,
		valor,
		ano,
		criado_em,
		criado_por,
		criado_rf
	)
select 'HabilitaFrequenciaRemotaETEC',
	68,
	'Habilita o tipo Remota para Frequências - Modalidade ETEC',
	'0',
	'2021',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where not exists(
		select 1
		from public.parametros_sistema
		where nome = 'HabilitaFrequenciaRemotaETEC'
			and ano = '2021'
	);