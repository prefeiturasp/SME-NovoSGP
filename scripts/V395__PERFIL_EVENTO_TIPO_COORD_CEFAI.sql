alter table perfil_evento_tipo drop column if exists exclusivo;
alter table perfil_evento_tipo add exclusivo bool not null default true;

insert into perfil_evento_tipo (codigo_perfil, evento_tipo_id, exclusivo) 
 select '4be1e074-37d6-e911-abd6-f81654fe895d', (select id from evento_tipo where codigo = 28), false
 where not exists(select id from perfil_evento_tipo where codigo_perfil = '4be1e074-37d6-e911-abd6-f81654fe895d');

insert into perfil_evento_tipo (codigo_perfil, evento_tipo_id, exclusivo) 
 select'44e1e074-37d6-e911-abd6-f81654fe895d', (select id from evento_tipo where codigo = 28), false
 where not exists(select id from perfil_evento_tipo where codigo_perfil = '44e1e074-37d6-e911-abd6-f81654fe895d');

insert into perfil_evento_tipo (codigo_perfil, evento_tipo_id, exclusivo) 
 select '45e1e074-37d6-e911-abd6-f81654fe895d', (select id from evento_tipo where codigo = 28), false
 where not exists(select id from perfil_evento_tipo where codigo_perfil = '45e1e074-37d6-e911-abd6-f81654fe895d');

insert into perfil_evento_tipo (codigo_perfil, evento_tipo_id, exclusivo) 
 select '46e1e074-37d6-e911-abd6-f81654fe895d', (select id from evento_tipo where codigo = 28), false
 where not exists(select id from perfil_evento_tipo where codigo_perfil = '46e1e074-37d6-e911-abd6-f81654fe895d');
