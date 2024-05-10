CREATE TABLE public.evento_tipo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(200) NOT NULL,
	local_ocorrencia int4 NOT NULL,
	concomitancia bool NOT NULL DEFAULT true,
	tipo_data int4 NOT NULL,
	dependencia bool NOT NULL DEFAULT false,
	letivo int4 NOT NULL,
	ativo bool NOT NULL DEFAULT true,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL
);

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Conselho de Classe',1,FALSE,3,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Conselho de Classe');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Evento DRE',2,FALSE,2,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Evento DRE');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Fechamento de Bimestre',4,TRUE,1,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Fechamento de Bimestre');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Feriado',3,FALSE,2,FALSE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Feriado');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Férias docentes',3,FALSE,2,FALSE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Férias docentes');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Liberação Excepcional',1,FALSE,3,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Liberação Excepcional');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Liberação do Boletim',4,FALSE,3,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Liberação do Boletim');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Organização SME',3,FALSE,2,FALSE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Organização SME');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Outros',5,FALSE,3,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Outros');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Projeto Político Pedagógico',4,FALSE,3,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Projeto Político Pedagógico');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Recesso',3,FALSE,2,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Recesso');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Recreio nas Férias',3,FALSE,2,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Recreio nas Férias');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reposição de Aula',1,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reposição de Aula');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reposição do Dia',1,FALSE,1,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reposição do Dia');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reposição no Recesso',4,TRUE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reposição no Recesso');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reunião Pedagógica',1,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reunião Pedagógica');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reunião de APM',1,FALSE,1,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reunião de APM');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reunião de Conselho de Escola',1,FALSE,1,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reunião de Conselho de Escola');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reunião de Responsáveis',3,FALSE,1,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reunião de Responsáveis');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Sondagem',3,FALSE,1,TRUE,2,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Sondagem');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Suspensão de Atividades',4,FALSE,2,FALSE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Suspensão de Atividades');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Formação',2,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Formação');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Curso',2,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Curso');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Encontro Mensal',2,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Encontro Mensal');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Seminário',2,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Seminário');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Palestra',2,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Palestra');

insert into public.evento_tipo (descricao,local_ocorrencia,dependencia,letivo,concomitancia,tipo_data,ativo,excluido,criado_em,criado_por,criado_rf) 
select 'Reunião',2,FALSE,2,TRUE,1,TRUE,FALSE,now(),'Carga Inicial',0 where not exists(select 1 from public.evento_tipo where descricao = 'Reunião');
