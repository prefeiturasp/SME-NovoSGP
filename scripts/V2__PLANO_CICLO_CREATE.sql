create table if not exists public.plano_ciclo ( descricao varchar not null,
id int8 not null generated always as identity,
ano int8 not null,
ciclo_id int8 not null,
escola_id int8 not null,
migrado boolean default false,
criado_em timestamp not null,
criado_por varchar (200) not null,
alterado_em timestamp null,
alterado_por varchar (200) null,
criado_rf varchar(200) not null,
alterado_rf varchar(200) null,
constraint plano_ciclo_pk primary key (id),
constraint plano_ciclo_un unique (ano,
ciclo_id,
escola_id) );

create table if not exists public.matriz_saber ( descricao varchar(100) not null,
id int8 not null generated always as identity,
criado_em timestamp not null,
criado_por varchar (200) not null,
alterado_em timestamp null,
alterado_por varchar (200) null,
criado_rf varchar(200) not null,
alterado_rf varchar(200) null,
constraint matriz_saber_pk primary key (id) );

create table if not exists public.matriz_saber_plano ( id int8 not null generated always as identity,
plano_id int8 not null,
matriz_id int8 not null,
criado_em timestamp not null,
criado_por varchar (200) not null,
alterado_em timestamp null,
alterado_por varchar (200) null,
criado_rf varchar(200) not null,
alterado_rf varchar(200) null,
constraint matriz_saber_plano_pk primary key (id),
constraint matriz_saber_plano_un unique (plano_id,
matriz_id) );

alter table if exists public.matriz_saber_plano drop constraint if exists matriz_id_fk;

alter table if exists public.matriz_saber_plano add constraint matriz_id_fk foreign key (matriz_id) references matriz_saber(id);

alter table if exists public.matriz_saber_plano drop constraint if exists plano_id_fk;

alter table if exists public.matriz_saber_plano add constraint plano_id_fk foreign key (plano_id) references plano_ciclo(id);

create table if not exists public.objetivo_desenvolvimento ( descricao varchar(100) not null,
id int8 not null generated always as identity,
criado_em timestamp not null,
criado_por varchar (200) not null,
alterado_em timestamp null,
alterado_por varchar (200) null,
criado_rf varchar(200) not null,
alterado_rf varchar(200) null,
constraint objetivo_desenvolvimento_pk primary key (id) );

create table if not exists public.objetivo_desenvolvimento_plano ( id int8 not null generated always as identity,
plano_id int8 not null,
objetivo_desenvolvimento_id int8 not null,
criado_em timestamp not null,
criado_por varchar (200) not null,
alterado_em timestamp null,
alterado_por varchar (200) null,
criado_rf varchar(200) not null,
alterado_rf varchar(200) null,
constraint objetivo_desenvolvimento_plano_pk primary key (id),
constraint objetivo_desenvolvimento_un unique (plano_id,
objetivo_desenvolvimento_id) );

alter table if exists public.objetivo_desenvolvimento_plano drop constraint if exists objetivo_desenvolvimento_id_fk;

alter table if exists public.objetivo_desenvolvimento_plano add constraint objetivo_desenvolvimento_id_fk foreign key (objetivo_desenvolvimento_id) references objetivo_desenvolvimento(id);

alter table if exists public.objetivo_desenvolvimento_plano drop constraint if exists plano_id_fk;

alter table if exists public.objetivo_desenvolvimento_plano add constraint plano_id_fk foreign key (plano_id) references plano_ciclo(id);

/*Inserts Matriz Saber
                    -----------------------
                    -----------------------
                    -----------------------
                    -----------------------
                    */
insert
	into
	public.matriz_saber (descricao,criado_em,criado_por, criado_rf)
select
	'Pensamento Científico, Crítico e Criativo',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Pensamento Científico, Crítico e Criativo' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Resolução de Problemas',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Resolução de Problemas' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Comunicação',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Comunicação' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Autoconhecimento e Autocuidado',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Autoconhecimento e Autocuidado' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Autonomia e Determinação',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Autonomia e Determinação' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Abertura à Diversidade',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Abertura à Diversidade' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Resposabilidade e Participação',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Resposabilidade e Participação' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Empatia e Colaboração',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Empatia e Colaboração' );

insert
	into
	public.matriz_saber (descricao,criado_em,criado_por,criado_rf)
select
	'Repertório Cultural',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.matriz_saber
	where
		descricao = 'Repertório Cultural' );

/*Inserts Objetivos desenvolvimento
                    -----------------------
                    -----------------------
                    -----------------------
                    -----------------------
                    */
insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Erradicação da Pobreza',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Erradicação da Pobreza' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Fome zero e agricultura sustentável',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Fome zero e agricultura sustentável' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Saúde e Bem Estar',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Saúde e Bem Estar' );


insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Educação de Qualidade',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Educação de Qualidade' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Igualdade de Gênero',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Igualdade de Gênero' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Água Potável e Saneamento',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Água Potável e Saneamento' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Energia Limpa e Acessível',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Energia Limpa e Acessível' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Trabalho decente e crescimento econômico',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Trabalho decente e crescimento econômico' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Indústria, inovação e infraestrutura',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Indústria, inovação e infraestrutura' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Redução das desigualdades',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Redução das desigualdades' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Cidades e comunidades sustentáveis',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Cidades e comunidades sustentáveis' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Consumo e produção responsáveis',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Consumo e produção responsáveis' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Ação contra a mudança global do clima',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Ação contra a mudança global do clima' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Vida na água',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Vida na água' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Vida terrestre',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Vida terrestre' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Paz, justiça e instituições eficazes',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Paz, justiça e instituições eficazes' );

insert
	into
	public.objetivo_desenvolvimento (descricao,criado_em,criado_por,criado_rf)
select
	'Parcerias e meios de implementação',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.objetivo_desenvolvimento
	where
		descricao = 'Parcerias e meios de implementação' );