CREATE TABLE public.auditoria (
	data timestamp not null,
	entidade varchar(200) not null,
	chave bigint not null,
	usuario varchar(200) not null,
	acao varchar(1) not null
);
