create table if not exists public.ciclo_ensino ( 
	id int8 not null generated always as identity,
	cod_ciclo_ensino_eol int8 not null,
	descricao varchar(60) not null,
	criado_em timestamp not null,
	criado_por varchar (200) not null,
	alterado_em timestamp ,
	alterado_por varchar (200),
	criado_rf varchar (200) not null,
	alterado_rf varchar (200),
constraint ciclo_ensino_pk primary key (id) );

create table if not exists public.tipo_escola ( 
	id int8 not null generated always as identity,
	cod_tipo_escola_eol int8 not null,
	descricao varchar(60) not null,
	criado_em timestamp not null,
	criado_por varchar (200) not null,
	alterado_em timestamp ,
	alterado_por varchar (200),
	criado_rf varchar (200) not null,
	alterado_rf varchar (200),
constraint tipo_escola_pk primary key (id) );

create table if not exists public.grupo_comunicado ( 
	id int8 not null generated always as identity,
	nome varchar(30),
	tipo_escola_id varchar(200) null,
	tipo_ciclo_id varchar(200) null,
	criado_em timestamp not null,
	criado_por varchar (200) not null,
	alterado_em timestamp ,
	alterado_por varchar (200),
	criado_rf varchar (200) not null,
	alterado_rf varchar (200),
constraint grupo_comunicado_pk primary key (id) );

create table if not exists public.comunidado_grupo ( 
	id int8 not null generated always as identity,
	comunicado_id int not null,
	grupo_comunicado_id int not null,
constraint comunidado_grupo_pk primary key (id) );


create table if not exists public.comunicado ( 
	id int8 not null generated always as identity,
	comunicado_grupo_id int not null,
	titulo varchar (30) not null,
	descricao varchar not null,
	data_envio timestamp not null,
	data_expiracao timestamp not null,
	criado_em timestamp not null,
	criado_por varchar (200) not null,
	alterado_em timestamp ,
	alterado_por varchar (200),
	criado_rf varchar (200) not null,
	alterado_rf varchar (200),
constraint comunicado_pk primary key (id) );

select f_cria_fk_se_nao_existir('comunidado_grupo', 'comunidado_grupo_comunicado_fk', 'FOREIGN KEY (comunicado_id) REFERENCES grupo_comunicado(id)');
select f_cria_fk_se_nao_existir('comunidado_grupo', 'comunidado_grupo_grupo_comunicado_fk', 'FOREIGN KEY (grupo_comunicado_id) REFERENCES comunicado(id)');



insert 
    into 
    public.grupo_comunicado (nome,tipo_escola_id,criado_em,criado_por, criado_rf)
select
	'EMEBS','4',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.grupo_comunicado
	where
		nome = 'EMEBS' );

insert 
    into 
    public.grupo_comunicado (nome,tipo_ciclo_id,criado_em,criado_por, criado_rf)
select
	'CEI','1,23',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.grupo_comunicado
	where
		nome = 'CEI' );

insert 
    into 
    public.grupo_comunicado (nome,tipo_ciclo_id,criado_em,criado_por, criado_rf)
select
	'EMEI','2,14',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.grupo_comunicado
	where
		nome = 'EMEI' );

insert 
    into 
    public.grupo_comunicado (nome,tipo_ciclo_id,criado_em,criado_por, criado_rf)
select
	'Fundamental','1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.grupo_comunicado
	where
		nome = 'Fundamental' );

insert 
    into 
    public.grupo_comunicado (nome,tipo_ciclo_id,criado_em,criado_por, criado_rf)
select
	'Médio','1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.grupo_comunicado
	where
		nome = 'Médio' );

insert 
    into 
    public.grupo_comunicado (nome,tipo_ciclo_id,criado_em,criado_por, criado_rf)
select
	'EJA','2,14',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.grupo_comunicado
	where
		nome = 'EJA' );


