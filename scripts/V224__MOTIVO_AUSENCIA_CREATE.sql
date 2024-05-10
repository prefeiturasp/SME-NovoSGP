DROP TABLE IF EXISTS public.motivo_ausencia;

CREATE TABLE public.motivo_ausencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(150) NOT NULL,	
	
	CONSTRAINT motivo_ausencia_pap_pk PRIMARY KEY (id)
);

insert into public.motivo_ausencia(descricao) values 
  	('Atestado Médico do Aluno'),
  	('Atestado Médico de pessoa da Família'),
  	('Doença na Família, sem atestado'),
  	('Óbito de pessoa da Família'),
  	('Inexistência de pessoa para levar à escola'),
	('Enchente'),
	('Falta de transporte'),
	('Violência na área onde mora'),
	('Calamidade pública que atingiu a escola ou exigiu o uso do espaço como abrigamento'),
	('Escola fechada por situação de violência');