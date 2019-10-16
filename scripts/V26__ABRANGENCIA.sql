--INÍCIO DRES
create table if not exists public.abrangencia_dres
(
 	id bigint NOT NULL generated always as identity,
 	usuario_id bigint not null ,
 	dre_id varchar(15) not null,
 	abreviacao varchar(10) not null,
 	nome varchar(100) not null,
	perfil uuid not null,
 	
 	CONSTRAINT abrangencia_dres_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('abrangencia_dres', 'abrangencia_dres_usario_fk', 'FOREIGN KEY (usuario_id) REFERENCES usuario(id)');

CREATE index if not exists abrangencia_dres_usuario_idx ON public.abrangencia_dres (usuario_id);
CREATE index if not exists abrangencia_dres_dre_id_idx ON public.abrangencia_dres (dre_id);

--FIM DRES

--INÍCIO UES
create table if not exists public.abrangencia_ues
(
 	id bigint NOT NULL generated always as identity,
 	ue_id varchar(15) not null,
 	abrangencia_dres_id bigint not null,
 	nome varchar(200) not null, 	
 	
 	CONSTRAINT abrangencia_ues_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('abrangencia_ues', 'abrangencia_ues_abrangencia_dres_id_fk', 'FOREIGN KEY (abrangencia_dres_id) REFERENCES abrangencia_dres(id) ON DELETE CASCADE');

CREATE index if not exists abrangencia_ues_ue_idx ON public.abrangencia_ues (ue_id);
CREATE index if not exists abrangencia_ues_abrangecia_dres_idx ON public.abrangencia_ues (abrangencia_dres_id);

--FIM UES

--INÍCIO TURMAS

create table if not exists public.abrangencia_turmas
(
 	id bigint NOT NULL generated always as identity,
 	turma_id varchar(15) not null,
 	abrangencia_ues_id bigint not null,
 	nome varchar(10) not null, 	
 	ano char not null,
 	ano_letivo int not null,
 	modalidade_codigo varchar(5) not null,
	semestre int not null,
	
 	CONSTRAINT abrangencia_turmas_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('abrangencia_turmas', 'abrangencia_turmas_abrangencia_ues_id_fk', 'FOREIGN KEY (abrangencia_ues_id) REFERENCES abrangencia_ues(id) ON DELETE CASCADE');

CREATE index if not exists abrangencia_turmas_ue_idx ON public.abrangencia_turmas (turma_id);
CREATE index if not exists abrangencia_turmas_abrangecia_ues_idx ON public.abrangencia_turmas (abrangencia_ues_id);

--FIM TURMAS