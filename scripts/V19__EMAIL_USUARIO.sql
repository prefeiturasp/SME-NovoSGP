ALTER TABLE PUBLIC.usuario add column nome varchar(100) null;
ALTER TABLE PUBLIC.usuario add column email varchar(100) null;
ALTER TABLE PUBLIC.usuario add column expiracao_recuperacao_senha timestamp null;
ALTER TABLE PUBLIC.usuario add column token_recuperacao_senha uuid null;
