drop table public.parametros_sistema;

CREATE TABLE public.parametros_sistema (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(50) NOT NULL,
	tipo int4 not NULL,
	descricao varchar(200) NOT NULL,
	valor varchar(100) NOT NULL,
	ano int4 NULL,
	ativo bool NOT NULL DEFAULT true,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT parametros_sistema_pk PRIMARY KEY (id)
);

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2019',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2019' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2020',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2020' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2021',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2021' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2022',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2022' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2023',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2023' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2024',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2024' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2025',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2025' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2026',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2026' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2027',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2027' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2028',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2028' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2029',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2029' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2030',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2030' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2031',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2031' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2032',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2032' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2033',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2033' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2034',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2034' );
		

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2035',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2035' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2036',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2036' );
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2037',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2037' );
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2038',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2038' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'EjaDiasLetivos','Dias letivos minimo permitido para EJA','100','2039',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'EjaDiasLetivos' and ano = '2039' );


insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2019',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2019' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2020',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2020' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2021',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2021' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2022',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2022' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2023',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2023' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2024',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2024' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2025',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2025' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2026',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2026' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2027',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2027' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2028',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2028' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2029',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2029' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2030',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2030' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2031',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2031' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2032',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2032' );

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2033',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2033' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2034',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2034' );
		

insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2035',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2035' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2036',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2036' );
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2037',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2037' );
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2038',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2038' );
		
insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'FundamentalMedioDiasLetivos','Dias letivos minimo permitido para Fundamental e Médio','200','2039',now(),'Carga Inicial','Carga Inicial',1
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		nome = 'FundamentalMedioDiasLetivos' and ano = '2039' );
		
	
	
	
	
	
	
	insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFrequenciaAlerta','Percentual de frequência para definir aluno em situação de alerta','80','2019',now(),'Carga Inicial','Carga Inicial',3
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 3 and ano = '2019' );
	
		
	insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFrequenciaAlerta','Percentual de frequência para definir aluno em situação de alerta','80','2020',now(),'Carga Inicial','Carga Inicial',3
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 3 and ano = '2020' );
	
	
	
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFrequenciaCritico','Percentual de frequência para definir aluno em situação crítica','75','2019',now(),'Carga Inicial','Carga Inicial',4
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 4 and ano = '2019' );
	
	
	
	
		
	insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'PercentualFrequenciaCritico','Percentual de frequência para definir aluno em situação crítica','75','2020',now(),'Carga Inicial','Carga Inicial',4
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 4 and ano = '2020' );
	
	
	
	
	
	
		
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeAulasNotificarProfessor','Quantidade de aulas sem chamada para notificar o professor','3','2019',now(),'Carga Inicial','Carga Inicial',5
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 5 and ano = '2019' );
	
	
	
	
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeAulasNotificarProfessor','Quantidade de aulas sem chamada para notificar o professor','3','2020',now(),'Carga Inicial','Carga Inicial',5
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 5 and ano = '2020' );
	
	
	
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeAulasNotificarGestorUE','Quantidade de aulas sem chamada para notificar o gestor da UE','5','2019',now(),'Carga Inicial','Carga Inicial',6
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 6 and ano = '2019' );
		
	
			insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeAulasNotificarGestorUE','Quantidade de aulas sem chamada para notificar o gestor da UE','5','2020',now(),'Carga Inicial','Carga Inicial',6
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 6 and ano = '2020' );
		
	
	
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeAulasNotificarSupervisorUE','Quantidade de aulas sem chamada para notificar o supervisor da UE','15','2019',now(),'Carga Inicial','Carga Inicial',7
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 7 and ano = '2019' );
		
	
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeAulasNotificarSupervisorUE','Quantidade de aulas sem chamada para notificar o supervisor da UE','15','2020',now(),'Carga Inicial','Carga Inicial',7
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 7 and ano = '2020' );
		
	
		
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeDiasNotificarAlteracaoChamadaEfetivada','Quantidade de dias que deve notificar alterações em chamadas com intervalo superior a','30','2019',now(),'Carga Inicial','Carga Inicial',8
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 8 and ano = '2019' );
		
	
	
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeDiasNotificarAlteracaoChamadaEfetivada','Quantidade de dias que deve notificar alterações em chamadas com intervalo superior a','30','2020',now(),'Carga Inicial','Carga Inicial',8
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 8 and ano = '2020' );
		
	
	
	
		insert 
    into 
    public.parametros_sistema (nome,descricao,valor,criado_em,criado_por, criado_rf,tipo)
select
	'HabilitarServicosEmBackground','Habilita ou desabilita as execuções em background. Ex: Recorrência de eventos','1',now(),'Carga Inicial','Carga Inicial',100
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 100 );