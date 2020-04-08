create table if not exists public.grupo_comunicado ( 
	id int8 not null generated always as identity,
	nome varchar(30),
	tipo_escola int8 null,
	tipo_ciclo_id int8 null,
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
select f_cria_fk_se_nao_existir('grupo_comunicado', 'grupo_comunicado_tipo_ciclo_fk', 'FOREIGN KEY (tipo_ciclo_id) REFERENCES tipo_ciclo(id)');



insert 
    into 
    public.grupo_comunicado (nome,criado_em,criado_por, criado_rf)
select
	'EMEBS',now(),'Carga Inicial','Carga Inicial'
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
    public.grupo_comunicado (nome,criado_em,criado_por, criado_rf)
select
	'CEI',now(),'Carga Inicial','Carga Inicial'
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
    public.grupo_comunicado (nome,criado_em,criado_por, criado_rf)
select
	'EMEI',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.grupo_comunicado
	where
		nome = 'EMEI' );

