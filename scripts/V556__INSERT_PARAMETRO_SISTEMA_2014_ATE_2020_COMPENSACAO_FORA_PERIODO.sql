INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'PermiteCompensacaoForaPeriodo',83,'Permitir Compensação Fora do Período','false',2014,false,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2014 and tipo = 83
    );

INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'PermiteCompensacaoForaPeriodo',83,'Permitir Compensação Fora do Período','false',2015,false,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2015 and tipo = 83
    );

INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'PermiteCompensacaoForaPeriodo',83,'Permitir Compensação Fora do Período','false',2016,false,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2016 and tipo = 83
    );

INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'PermiteCompensacaoForaPeriodo',83,'Permitir Compensação Fora do Período','false',2017,false,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2017 and tipo = 83
    );

INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'PermiteCompensacaoForaPeriodo',83,'Permitir Compensação Fora do Período','false',2018,false,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2018 and tipo = 83
    );

INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'PermiteCompensacaoForaPeriodo',83,'Permitir Compensação Fora do Período','true',2019,true,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2019 and tipo = 83
    );

INSERT INTO public.parametros_sistema
    (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
SELECT 'PermiteCompensacaoForaPeriodo',83,'Permitir Compensação Fora do Período','true',2020,true,current_timestamp,'SISTEMA',null,'Sistema','0','0'
WHERE
    NOT EXISTS (
        SELECT ano FROM public.parametros_sistema WHERE ano = 2020 and tipo = 83
    );