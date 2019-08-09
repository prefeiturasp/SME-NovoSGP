CREATE TABLE public.plano_ciclo (
	descricao varchar NOT NULL,
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	CONSTRAINT plano_ciclo_pk PRIMARY KEY (id)
);

CREATE TABLE public.matriz_saber (
	descricao varchar(100) NOT  NULL,
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	CONSTRAINT matriz_saber_pk PRIMARY KEY (id)
);

CREATE TABLE public.matriz_saber_plano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_id int8 NOT NULL,
	matriz_id int8 NOT NULL,
	CONSTRAINT matriz_saber_plano_pk PRIMARY KEY (id),
	CONSTRAINT matriz_saber_plano_un UNIQUE (plano_id, matriz_id)
);

ALTER TABLE public.matriz_saber_plano ADD CONSTRAINT matriz_id_fk FOREIGN KEY (matriz_id) REFERENCES matriz_saber(id);
ALTER TABLE public.matriz_saber_plano ADD CONSTRAINT plano_id_fk FOREIGN KEY (plano_id) REFERENCES plano_ciclo(id);

CREATE TABLE public.objetivo_desenvolvimento (
	descricao varchar(100) NOT NULL,
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	CONSTRAINT objetivo_desenvolvimento_pk PRIMARY KEY (id)
);

CREATE TABLE public.objetivo_desenvolvimento_plano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_id int8 NOT NULL,
	objetivo_desenvolvimento_id int8 NOT NULL,
	CONSTRAINT objetivo_desenvolvimento_plano_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_desenvolvimento_un UNIQUE (plano_id, objetivo_desenvolvimento_id)
);

ALTER TABLE public.objetivo_desenvolvimento_plano ADD CONSTRAINT objetivo_desenvolvimento_id_fk FOREIGN KEY (objetivo_desenvolvimento_id) REFERENCES objetivo_desenvolvimento(id);
ALTER TABLE public.objetivo_desenvolvimento_plano ADD CONSTRAINT plano_id_fk FOREIGN KEY (plano_id) REFERENCES plano_ciclo(id);

--Inserts Matriz Saber

insert
	into
	public.matriz_saber (id,descricao)
values('Pensamento Científico, Crítico e Criativo');

insert
	into
	public.matriz_saber (descricao)
values('Resolução de Problemas');

insert
	into
	public.matriz_saber (descricao)
values('Comunicação');

insert
	into
	public.matriz_saber (descricao)
values('Autoconhecimento e Autocuidado');

insert
	into
	public.matriz_saber (descricao)
values('Autonomia e Determinação');

insert
	into
	public.matriz_saber (descricao)
values('Abertura à Diversidade');

insert
	into
	public.matriz_saber (descricao)
values('Resposabilidade e Participação');

insert
	into
	public.matriz_saber (descricao)
values('Empatia e Colaboração');

insert
	into
	public.matriz_saber (descricao)
values('Repertório Cultural');

--Inserts Objetivos desenvolvimento

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Erradicação da Pobreza');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Fome zero e agricultura sustentável');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Saúde e Bem Estar');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Educação de Qualidade');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Igualdade de Gênero');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Água Potável e Saneamento');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Energia Limpa e Acessível');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Trabalho decente e crescimento econômico');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Indústria, inovação e infraestrutura');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Redução das desigualdades');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Cidades e comunidades sustentáveis');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Consumo e produção responsáveis');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Ação contra a mudança global do clima');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Vida na água');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Vida terrestre');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Paz, justiça e instituições eficazes');

insert
	into
	public.objetivo_desenvolvimento (descricao)
values('Parcerias e meios de implementação');