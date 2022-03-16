insert  into
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
    'ProcessarDeadletter','parametro para ativar ou desativar o processamento das deadletter', 'true', 2021, now(),'Sistema','Sistema',101
where
    not exists(
            select 	1
            from public.parametros_sistema
            where tipo = 101);
