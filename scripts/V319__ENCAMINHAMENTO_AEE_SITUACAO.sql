alter table encaminhamento_aee drop if exists situacao;

alter table encaminhamento_aee 
	add situacao int4 not null default 1;