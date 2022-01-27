insert  into
    public.parametros_sistema (nome,descricao,ativo,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
    'CJInfantilPodeEditarAulaTitular','Parametro para o CJ da educação infantil permitir editar registros de aula titular', false,'', 2021, now(),'Sistema','Sistema',84
where
    not exists(
            select 	1
            from public.parametros_sistema
            where tipo = 84 and ano = 2021);

insert  into
    public.parametros_sistema (nome,descricao,ativo,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
    'CJInfantilPodeEditarAulaTitular','Parametro para o CJ da educação infantil permitir editar registros de aula titular', true,'', 2022, now(),'Sistema','Sistema',84
where
    not exists(
            select 	1
            from public.parametros_sistema
            where tipo = 84 and ano = 2022);
