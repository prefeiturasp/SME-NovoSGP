 insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
 select 'GerarNotificacaoAlteracaoEmAtividadeAvaliativa', 113, 'Gerar notificação de alteração em atividade avaliativa', '', 2024, true, CURRENT_DATE, 'SISTEMA', 'SISTEMA' 
 where not exists (select 1 from public.parametros_sistema where ano = 2024 and tipo = 113);
 
 insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
 select 'GerarNotificacaoCadastroDeCompensacaoDeAusencia', 114, 'Gerar notificação de cadastro de compensação de ausência', '', 2024, true, CURRENT_DATE, 'SISTEMA', 'SISTEMA' 
 where not exists (select 1 from public.parametros_sistema where ano = 2024 and tipo = 114);
 
 insert into public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
 select 'GerarNotificacaoPendenciaFechamento', 115, 'Gerar notificação de pendência de fechamento', '', 2024, true, CURRENT_DATE, 'SISTEMA', 'SISTEMA' 
 where not exists (select 1 from public.parametros_sistema where ano = 2024 and tipo = 115);