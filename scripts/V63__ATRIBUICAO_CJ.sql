CREATE TABLE if NOT EXISTS public.atribuicao_cj (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	nome varchar(30) NOT NULL,
	descricao varchar(200) NOT NULL,  
    situacao boolean not null default true,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    CONSTRAINT tipo_avaliacao_pk PRIMARY KEY (id)
);