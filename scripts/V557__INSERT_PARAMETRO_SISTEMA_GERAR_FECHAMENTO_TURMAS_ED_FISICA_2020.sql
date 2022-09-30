INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'GerarFechamentoTurmasEdFisica2020',103,'Permitir Gerar Fechamento Para Turmas de Ed. Física 2020','false',false,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE tipo = 103
    );