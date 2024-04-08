 insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
 select 'DiasExclusaoNotificacoesLidasDeAlerta ', 110, 'Dias de exclusão das notificações lidas de alerta', '5', 2024, true, CURRENT_DATE, 'SISTEMA', 'SISTEMA' 
 where not exists (
  select 1 from public.parametros_sistema where ano = 2024 and tipo = 110
 );
 
 insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
 select 'DiasExclusaoNotificacoesLidasDeAviso', 111, 'Dias de exclusão das notificações lidas de aviso', '2', 2024, true, CURRENT_DATE, 'SISTEMA', 'SISTEMA' 
 where not exists (
  select 1 from public.parametros_sistema where ano = 2024 and tipo = 111
 );
 
 insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
 select 'DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta', 112, 'Dias de exclusão das notificações não lidas de aviso e alerta', '30', 2024, true, CURRENT_DATE, 'SISTEMA', 'SISTEMA' 
 where not exists (
  select 1 from public.parametros_sistema where ano = 2024 and tipo = 112
 );