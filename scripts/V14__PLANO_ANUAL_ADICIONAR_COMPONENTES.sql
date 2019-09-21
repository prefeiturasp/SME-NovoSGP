 insert
	into
	public.componente_curricular ( codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf )
select
	3,
	1289,
	'Educação Física Integral Tarde',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1289 );


insert
	into
	public.componente_curricular ( codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf )
select
	3,
	1288,
	'Educação Física Integral Manhã',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1288 );
	
	
	
insert
	into
	public.componente_curricular ( codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf )
select
	7,
	9,
	'Inglês',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 9 );
	
	
		
insert
	into
	public.componente_curricular ( codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf )
select
	9,
	1018,
	'Informática  - OIE',
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1018 );
