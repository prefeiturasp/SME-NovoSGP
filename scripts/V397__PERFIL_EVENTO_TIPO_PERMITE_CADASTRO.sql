alter table perfil_evento_tipo drop if exists permite_cadastro;
alter table perfil_evento_tipo add permite_cadastro bool default false;

update perfil_evento_tipo set permite_cadastro = true 
 where codigo_perfil in ('4ae1e074-37d6-e911-abd6-f81654fe895d', '4be1e074-37d6-e911-abd6-f81654fe895d');