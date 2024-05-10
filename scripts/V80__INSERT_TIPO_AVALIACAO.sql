insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Avaliação bimestral','Avaliação bimestral',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Avaliação bimestral' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Avaliação mensal','Avaliação mensal',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Avaliação mensal' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Chamada oral','Chamada oral',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Chamada oral' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Debate','Debate',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Debate' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Dramatização','Dramatização',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Dramatização' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Estudo de meio','Estudo de meio',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Estudo de meio' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Pesquisa','Pesquisa',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Pesquisa' );

insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Produção de texto','Produção de textoquisa',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Produção de texto' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Projeto escolar','Projeto escolar',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Projeto escolar' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Seminário','Seminário',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Seminário' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'TCA','TCA',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'TCA' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Teste','Teste',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Teste' );

insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Teste de múltipla escolha','Teste de múltipla escolha',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Teste de múltipla escolha' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Texto','Texto',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Texto' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Trabalho individual','Trabalho individual',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Trabalho individual' );
			
insert into public.tipo_avaliacao (nome,descricao,situacao,criado_em,criado_por, criado_rf,excluido)
select
	'Trabalho em grupo','Trabalho em grupo',true,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.tipo_avaliacao
		where
			nome = 'Trabalho em grupo' );