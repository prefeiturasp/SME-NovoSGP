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


		insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	1301,
	'REG CLASSE SURDOCEGUEIRA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1301
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	1301,
	'REG CLASSE SURDOCEGUEIRA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1301
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	1301,
	'REG CLASSE SURDOCEGUEIRA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1301
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	1301,
	'REG CLASSE SURDOCEGUEIRA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1301
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	1301,
	'REG CLASSE SURDOCEGUEIRA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1301
		and codigo_jurema = 8);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	508,
	'REGENCIA CLAS.F I' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 508
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	508,
	'REGENCIA CLAS.F I' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 508
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	508,
	'REGENCIA CLAS.F I' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 508
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	508,
	'REGENCIA CLAS.F I' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 508
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	508,
	'REGENCIA CLAS.F I' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 508
		and codigo_jurema = 8);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	1105,
	'REG CLASSE CICLO ALFAB / INTERD 5HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1105
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	1105,
	'REG CLASSE CICLO ALFAB / INTERD 5HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1105
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	1105,
	'REG CLASSE CICLO ALFAB / INTERD 5HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1105
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	1105,
	'REG CLASSE CICLO ALFAB / INTERD 5HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1105
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	1105,
	'REG CLASSE CICLO ALFAB / INTERD 5HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1105
		and codigo_jurema = 8);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	1115,
	'REG CLASSE ESPECIAL DIURNO' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1115
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	1115,
	'REG CLASSE ESPECIAL DIURNO' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1115
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	1115,
	'REG CLASSE ESPECIAL DIURNO' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1115
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	1115,
	'REG CLASSE ESPECIAL DIURNO' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1115
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	1115,
	'REG CLASSE ESPECIAL DIURNO' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1115
		and codigo_jurema = 8);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	1121,
	'REG CLASSE ALFAB_INTEGRAL TARDE' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1121
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	1121,
	'REG CLASSE ALFAB_INTEGRAL TARDE' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1121
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	1121,
	'REG CLASSE ALFAB_INTEGRAL TARDE' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1121
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	1121,
	'REG CLASSE ALFAB_INTEGRAL TARDE' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1121
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	1121,
	'REG CLASSE ALFAB_INTEGRAL TARDE' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1121
		and codigo_jurema = 8);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	1124,
	'REG CLASSE ALFAB_INTEGRAL MANHA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1124
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	1124,
	'REG CLASSE ALFAB_INTEGRAL MANHA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1124
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	1124,
	'REG CLASSE ALFAB_INTEGRAL MANHA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1124
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	1124,
	'REG CLASSE ALFAB_INTEGRAL MANHA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1124
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	1124,
	'REG CLASSE ALFAB_INTEGRAL MANHA' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1124
		and codigo_jurema = 8);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	1213,
	'REG CLASSE SP INTEGRAL 1A5 ANOS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1213
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	1213,
	'REG CLASSE SP INTEGRAL 1A5 ANOS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1213
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	1213,
	'REG CLASSE SP INTEGRAL 1A5 ANOS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1213
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	1213,
	'REG CLASSE SP INTEGRAL 1A5 ANOS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1213
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	1213,
	'REG CLASSE SP INTEGRAL 1A5 ANOS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1213
		and codigo_jurema = 8);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	2,
	1112,
	'REG CLASSE CICLO ALFAB / INTERD 4HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1112
		and codigo_jurema = 2);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	4,
	1112,
	'REG CLASSE CICLO ALFAB / INTERD 4HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1112
		and codigo_jurema = 4);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	5,
	1112,
	'REG CLASSE CICLO ALFAB / INTERD 4HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1112
		and codigo_jurema = 5);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	6,
	1112,
	'REG CLASSE CICLO ALFAB / INTERD 4HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1112
		and codigo_jurema = 6);

insert
	into
	public.componente_curricular (codigo_jurema,
	codigo_eol,
	descricao_eol,
	criado_em,
	criado_por,
	criado_rf
)select
	8,
	1112,
	'REG CLASSE CICLO ALFAB / INTERD 4HRS' ,
	now(),
	'Carga',
	'Carga'
where
	not exists(
	select
		1
	from
		public.componente_curricular
	where
		codigo_eol = 1112
		and codigo_jurema = 8);
