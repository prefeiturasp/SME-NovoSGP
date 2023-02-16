insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
 select 'TiposUEIgnorarGeracaoPendencia ', 95, 'Tipos de UE ignoradas na geração de pendências no ano', '10,11,12,18', 2023, true, CURRENT_DATE, 'SISTEMA', 'SISTEMA' 
 where not exists (
  select tipo from public.parametros_sistema where ano = 2023 and tipo = 95
 )