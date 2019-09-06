CREATE TABLE if not exists public.componente_curricular (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_jurema int8 NOT NULL,
	codigo_eol int8 NOT NULL,
	descricao_eol varchar(100) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT componente_curricular_pk PRIMARY KEY (id),
	CONSTRAINT componente_curricular_un UNIQUE (codigo_jurema,codigo_eol)
);
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  1,
  139,
  'Arte',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 139
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  4,
  8,
  'Geografia',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 8
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  8,
  2,
  'Matemática',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 2
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  2,
  89,
  'Ciências',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 89
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  5,
  7,
  'História',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 7
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  7,
  1046,
  'Língua Inglesa',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1046
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  7,
  1072,
  'Língua Inglesa Ciclo I',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1072
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  7,
  1073,
  'Língua Inglesa Ciclo I Manhã',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1073
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  7,
  1074,
  'Língua Inglesa Ciclo I Tarde',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1074
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  7,
  1106,
  'Língua Inglesa Compartilhada',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1106
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  7,
  1122,
  'Língua Inglesa Compartilhada Manhã',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1122
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  7,
  1123,
  'Língua Inglesa Compartilhada Tarde',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1123
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  6,
  138,
  'Língua Portuguesa',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 138
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  3,
  6,
  'Educação Física',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 6
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  9,
  1060,
  'Informática - OIE',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1060
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  9,
  1070,
  'Informática (OIE) FI - Integral Manhã',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1070
  );
insert into
  public.componente_curricular (
    codigo_jurema,
    codigo_eol,
    descricao_eol,
    criado_em,
    criado_por,
    criado_rf
  )
select
  9,
  1071,
  'Informática (OIE) FI - Integral Tarde',
  now(),
  'Carga Inicial',
  'Carga Inicial'
where
  not exists(
    select
      1
    from
      public.componente_curricular
    where
      codigo_eol = 1071
  );


CREATE TABLE if not exists public.plano_anual (
  id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
  escola_id varchar(10) NOT NULL,
  turma_id int8 NOT NULL,
  ano int8 NOT NULL,
  bimestre int4 NOT NULL,
  descricao varchar NOT NULL,
  criado_em timestamp NOT NULL,
  criado_por varchar(200) NOT NULL,
  alterado_em timestamp NULL,
  alterado_por varchar(200) NULL,
  criado_rf varchar(200) NOT NULL,
  alterado_rf varchar(200) NULL,
  CONSTRAINT plano_anual_pk PRIMARY KEY (id),
  CONSTRAINT plano_anual_un UNIQUE (escola_id, turma_id, ano, bimestre)
);



CREATE TABLE if not exists public.objetivo_aprendizagem_plano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_id int8 NOT NULL,
	objetivo_aprendizagem_jurema_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_aprendizagem_plano_pk PRIMARY KEY (id),
	CONSTRAINT objetivo_aprendizagem_plano_fk FOREIGN KEY (componente_curricular_id) REFERENCES public.componente_curricular(id),
	CONSTRAINT plano_id_fk FOREIGN KEY (plano_id) REFERENCES public.plano_anual(id)
);

