INSERT INTO public.prioridade_perfil 
    (ordem, tipo, nome_perfil, codigo_perfil, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
SELECT 
    399, 
    2, 
    'Fonoaudiólogo', 
    '90e1e074-37d6-e911-abd6-f81654fe895d', 
    NOW(), 
    'Carga', 
    NULL, 
    NULL, 
    'Carga', 
    NULL
WHERE NOT EXISTS (
    SELECT 1 
    FROM public.prioridade_perfil pp
    WHERE pp.codigo_perfil = '90e1e074-37d6-e911-abd6-f81654fe895d'
);

-- o tipo é o mesmo do assistente social
-- a ordem menor é que define quem vai aparecer primeiro quando logar (perfil)

