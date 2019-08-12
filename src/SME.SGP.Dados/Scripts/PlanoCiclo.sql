create table if not exists public.plano_ciclo (
  descricao varchar not null,
  id int8 not null generated always as identity,
  constraint plano_ciclo_pk primary key (id)
);
create table if not exists public.matriz_saber (
  descricao varchar(100) not null,
  id int8 not null generated always as identity,
  constraint matriz_saber_pk primary key (id)
);
create table if not exists public.matriz_saber_plano (
  id int8 not null generated always as identity,
  plano_id int8 not null,
  matriz_id int8 not null,
  constraint matriz_saber_plano_pk primary key (id),
  constraint matriz_saber_plano_un unique (plano_id, matriz_id)
);
alter table
  if exists public.matriz_saber_plano drop constraint if exists matriz_id_fk;
alter table
  if exists public.matriz_saber_plano
add
  constraint matriz_id_fk foreign key (matriz_id) references matriz_saber(id);
alter table
  if exists public.matriz_saber_plano drop constraint if exists plano_id_fk;
alter table
  if exists public.matriz_saber_plano
add
  constraint plano_id_fk foreign key (plano_id) references plano_ciclo(id);
create table if not exists public.objetivo_desenvolvimento (
    descricao varchar(100) not null,
    id int8 not null generated always as identity,
    constraint objetivo_desenvolvimento_pk primary key (id)
  );
create table if not exists public.objetivo_desenvolvimento_plano (
    id int8 not null generated always as identity,
    plano_id int8 not null,
    objetivo_desenvolvimento_id int8 not null,
    constraint objetivo_desenvolvimento_plano_pk primary key (id),
    constraint objetivo_desenvolvimento_un unique (plano_id, objetivo_desenvolvimento_id)
  );
alter table
  if exists public.objetivo_desenvolvimento_plano drop constraint if exists objetivo_desenvolvimento_id_fk;
alter table
  if exists public.objetivo_desenvolvimento_plano
add
  constraint objetivo_desenvolvimento_id_fk foreign key (objetivo_desenvolvimento_id) references objetivo_desenvolvimento(id);
alter table
  if exists public.objetivo_desenvolvimento_plano drop constraint if exists plano_id_fk;
alter table
  if exists public.objetivo_desenvolvimento_plano
add
  constraint plano_id_fk foreign key (plano_id) references plano_ciclo(id);
  /*Inserts Matriz Saber
                    -----------------------
                    -----------------------
                    -----------------------
                    -----------------------
                    */
insert into
  public.matriz_saber (descricao)
select
  ('Pensamento Científico, Crítico e Criativo')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Pensamento Científico, Crítico e Criativo'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Resolução de Problemas')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Resolução de Problemas'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Comunicação')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Comunicação'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Autoconhecimento e Autocuidado')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Autoconhecimento e Autocuidado'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Autonomia e Determinação')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Autonomia e Determinação'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Abertura à Diversidade')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Abertura à Diversidade'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Resposabilidade e Participação')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Resposabilidade e Participação'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Empatia e Colaboração')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Empatia e Colaboração'
  );
insert into
  public.matriz_saber (descricao)
select
  ('Repertório Cultural')
where
  not exists(
    select
      1
    from
      public.matriz_saber
    where
      descricao = 'Repertório Cultural'
  );
  /*Inserts Objetivos desenvolvimento
                    -----------------------
                    -----------------------
                    -----------------------
                    -----------------------
                    */
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Erradicação da Pobreza')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Erradicação da Pobreza'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Fome zero e agricultura sustentável')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Fome zero e agricultura sustentável'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Saúde e Bem Estar')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Saúde e Bem Estar'
  );;
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Educação de Qualidade')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Educação de Qualidade'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Igualdade de Gênero')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Igualdade de Gênero'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Água Potável e Saneamento')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Água Potável e Saneamento'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Energia Limpa e Acessível')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Energia Limpa e Acessível'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Trabalho decente e crescimento econômico')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Trabalho decente e crescimento econômico'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Indústria, inovação e infraestrutura')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Indústria, inovação e infraestrutura'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Redução das desigualdades')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Redução das desigualdades'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Cidades e comunidades sustentáveis')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Cidades e comunidades sustentáveis'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Consumo e produção responsáveis')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Consumo e produção responsáveis'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Ação contra a mudança global do clima')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Ação contra a mudança global do clima'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Vida na água')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Vida na água'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Vida terrestre')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Vida terrestre'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Paz, justiça e instituições eficazes')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Paz, justiça e instituições eficazes'
  );
insert into
  public.objetivo_desenvolvimento (descricao)
select
  ('Parcerias e meios de implementação')
where
  not exists(
    select
      1
    from
      public.objetivo_desenvolvimento
    where
      descricao = 'Parcerias e meios de implementação'
  );