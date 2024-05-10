CREATE TABLE public.auditoria
(
	id bigint not null
	generated always as identity,
	data timestamp not null,
	entidade varchar
	(200) not null,
	chave bigint not null,
	usuario varchar
	(200) not null,
	acao varchar
	(1) not null,
	rf varchar (200) not null
);
