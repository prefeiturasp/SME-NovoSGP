CREATE TABLE if not exists public.parametros_sistema (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	nome varchar(50) NOT NULL,
    descricao varchar(200) NOT NULL,
	valor varchar(100) NOT NULL,
	ano int NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200) ,
    CONSTRAINT parametros_sistema_pk PRIMARY KEY (id)
);