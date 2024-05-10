delete from parametros_sistema where tipo in (52);

insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('DiasGeracaoNotificacoesPlanoAEEExpirado', 52, 'Dias após a expiração do plano para notificar o Coordenador CEFAI', '10', 2021, true, now(), 'SISTEMA', '0');
