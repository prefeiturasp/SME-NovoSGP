CREATE TABLE IF NOT EXISTS public.tipo_ciclo
(
    id int8 NOT NULL,
    descricao varchar(200) NOT NULL,
    criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200) ,
    CONSTRAINT tipo_ciclo_pk PRIMARY KEY (id),
    CONSTRAINT tipo_ciclo_un UNIQUE (descricao)

);

CREATE TABLE IF NOT EXISTS public.tipo_ciclo_ano
(
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    tipo_ciclo_id int8 NOT NULL,
    modalidade int8 NOT NULL,
    ano varchar(10) NOT NULL,
    CONSTRAINT tipo_ciclo_ano_pk PRIMARY KEY (id),
    CONSTRAINT tipo_ciclo_ano_un UNIQUE (tipo_ciclo_id, ano),
    CONSTRAINT tipo_ciclo_id_fk FOREIGN KEY (tipo_ciclo_id)
        REFERENCES public.tipo_ciclo (id)
);



insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	1,'Alfabetização',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'Alfabetização' );


insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	2,'Interdisciplinar',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'Interdisciplinar' );


insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	3,'Autoral',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'Autoral' );

insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	4,'Médio',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'Médio' );

insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	5,'EJA - Alfabetização',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'EJA - Alfabetização' );

insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	6,'EJA - Básica',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'EJA - Básica' );


insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	7,'EJA - Complementar',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'EJA - Complementar' );

insert 
    into 
    public.tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) 
select
	8,'EJA - Final',now(),'Carga Inicial','Carga Inicial'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo
	where
		descricao = 'EJA - Final' );





insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	1,5,'1'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =1 and modalidade = 5 and ANO = '1');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	1,5,'2'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =1 and modalidade = 5 and ANO = '2');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	1,5,'3'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =1 and modalidade = 5 and ANO = '3');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	2,5,'4'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =2 and modalidade = 5 and ANO = '4');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	2,5,'5'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =2 and modalidade = 5 and ANO = '5');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	2,5,'6'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =2 and modalidade = 5 and ANO = '6');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	3,5,'7'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =3 and modalidade = 5 and ANO = '7');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	3,5,'8'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =3 and modalidade = 5 and ANO = '8');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	3,5,'9'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =3 and modalidade = 5 and ANO = '9');





insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	4,6,'1'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =4 and modalidade = 6 and ANO = '1');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	4,6,'2'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =4 and modalidade = 6 and ANO = '2');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	4,6,'3'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =4 and modalidade = 6 and ANO = '3');




insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	5,3,'1'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =5 and modalidade = 3 and ANO = '1');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	6,3,'2'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =6 and modalidade = 3 and ANO = '2');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	7,3,'3'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =7 and modalidade = 3 and ANO = '3');
insert 
    into public.tipo_ciclo_ano (tipo_ciclo_id,modalidade,ano) 
    select
	8,3,'4'
where
	not exists(
	select
		1
	from
		public.tipo_ciclo_ano
	where
		tipo_ciclo_id =8 and modalidade = 3 and ANO = '4');