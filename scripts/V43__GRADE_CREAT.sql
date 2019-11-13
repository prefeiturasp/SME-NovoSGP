/**	Grade **/
drop table if exists grade;
create table public.grade (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	

	nome varchar(200) NOT null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido boolean not null default false,
	
	CONSTRAINT grade_pk PRIMARY KEY (id)	
);

/**	Grade Filtro **/
drop table if exists grade_filtro;
create table public.grade_filtro (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	

	grade_id int8 NOT NULL,
	tipo_escola int2 null,
	modalidade int4 not null,
	duracao_turno int2 not null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido boolean not null default false,
	
	CONSTRAINT grade_filtro_pk PRIMARY KEY (id),
	CONSTRAINT grade_filtro_grade_id_fk FOREIGN KEY (grade_id) REFERENCES grade(id) ON DELETE CASCADE
);
CREATE INDEX grade_filtro_grade_idx ON public.grade_filtro USING btree (grade_id);

/**	Grade Disciplina **/
drop table if exists grade_disciplina;
create table public.grade_disciplina (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	

	grade_id int8 NOT NULL,
	ano int2 not null,
	componente_curricular_id int8 not null,
	quantidade_aulas int2 not null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido boolean not null default false,
	
	CONSTRAINT grade_disciplina_pk PRIMARY KEY (id),
	CONSTRAINT grade_disciplina_grade_id_fk FOREIGN KEY (grade_id) REFERENCES grade(id) ON DELETE CASCADE
);
CREATE INDEX grade_disciplina_grade_idx ON public.grade_disciplina USING btree (grade_id);
CREATE INDEX grade_disciplina_componente_curricular_idx ON public.grade_disciplina USING btree (componente_curricular_id);
CREATE INDEX grade_disciplina_ano_idx ON public.grade_disciplina USING btree (ano);
