CREATE TABLE if not exists public.configuracao_email (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	email_remetente varchar(100) NOT NULL,
	nome_remetente varchar(100) NOT NULL,
	servidor_smtp varchar(100) NOT NULL,
	usuario varchar(50) NOT NULL,
	senha varchar(50) NOT NULL,
	porta int NOT NULL,
	usar_tls bool NOT NULL DEFAULT false,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200) ,
    CONSTRAINT configuracao_email_pk PRIMARY KEY (id)
);


insert
	into
	public.configuracao_email ( 
	email_remetente,
	nome_remetente,
	servidor_smtp,
	usuario,
	senha,
	porta,
	usar_tls,
	criado_em,
	criado_por,
	criado_rf)
select  
'sgp-nao_responder@sme.prefeitura.sp.gov.br',
'Institucional - ASCOM',
'smtp.office365.com',
'sgp-nao_responder@sme.prefeitura.sp.gov.br',
'Noh27146',
587,
false,
now(),
'Carga',
'Carga'
where
	not exists(
	select
		1
	from
		public.configuracao_email);