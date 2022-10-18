INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'ExecucaoConsolidacaoDiariosBordo',81,'Data da última execução da rotina de consolidação de diarios de bordo','',2022,true,current_timestamp,'SISTEMA',null,null,'0',null
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2022 and tipo = 81
    );