insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
select 'UtilizarCacheAbrangencia', 116, 'Cache filtros abrangência. ativo liga/desliga; valor = TTL em min (minutos p/ perfis em geral, cj p/ perfil CJ). Mudanças valem em até 1 min', '{"minutos":5,"cj":1}', 2026, false, CURRENT_DATE, 'SISTEMA', 'SISTEMA'
where not exists (select 1 from public.parametros_sistema where tipo = 116);
