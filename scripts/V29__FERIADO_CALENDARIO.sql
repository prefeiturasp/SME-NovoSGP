CREATE TABLE IF NOT EXISTS public.feriado_calendario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	
	nome varchar(50) not null,
    abrangencia int not null,
	data_feriado timestamp not null,
	tipo int not null,
    ativo boolean not null default true,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido boolean not null default false
);

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Confraternização universal',
	1,
	'1900-01-01',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-01-01' );


insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Aniversário de São Paulo',
	3,
	'1900-01-25',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-01-25' );

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Tiradentes',
	1,
	'1900-04-21',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-04-21' );

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Dia do trabalhador',
	1,
	'1900-05-01',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-05-01' );

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Revolução constitucionalista',
	2,
	'1900-07-09',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-07-09' );

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Independência',
	1,
	'1900-09-07',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-09-07' );

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Nossa Senhora Aparecida',
	1,
	'1900-10-12',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-10-12' );


insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Finados',
	1,
	'1900-11-02',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-11-02' );

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Proclamação da república',
	1,
	'1900-11-15',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-11-15' );

insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Consciência Negra (Municipal)',
	3,
	'1900-11-20',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-11-20' );


insert
	into
	public.feriado_calendario ( 
    nome,
	abrangencia,
	data_feriado,
    tipo,
    ativo,
	criado_em,
	criado_por,
	criado_rf )
select
	'Natal',
	1,
	'1900-12-25',
    1,
    true,
	now(),
	'Carga Inicial',
	'Carga Inicial'
where
	not exists(
	select
		1
	from
		public.feriado_calendario
	where
		data_feriado = '1900-12-25' );