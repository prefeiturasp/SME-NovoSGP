insert into
  public.resposta (
  id,
  nome,
  descricao,
  sim,
  excluido,
  dt_inicio,
  dt_fim,
  criado_em,
  criado_por,
  alterado_em,
  alterado_por,
  criado_rf,
  alterado_rf
  )
select
  19,
  'Silábico alfabético',
  'Silábico alfabético',
  false,
  false,
  '2020-01-01 00:00:00.000',
  NULL,
  now(),
  'Carga Inicial',
  NULL,
  NULL,
  'Carga Inicial',
  NULL
where
  not exists(
    select
      1
    from
      public.resposta
    where
      nome = 'Silábico alfabético'
  );
  
insert into
  public.objetivo_resposta (
  objetivo_id, 
  resposta_id, 
  excluido, 
  criado_em, 
  criado_por, 
  alterado_em, 
  alterado_por, 
  criado_rf, 
  alterado_rf
  )
select
  5, 
  19, 
  false, 
  now(), 
  'Carga Inicial', 
  NULL, 
  NULL, 
  'Carga Inicial', 
  NULL
where
  not exists(
    select
      1
    from
      public.objetivo_resposta
    where
      objetivo_id = 5 and resposta_id = 19
  );