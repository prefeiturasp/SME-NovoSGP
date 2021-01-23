alter table secao_encaminhamento_aee drop if exists etapa;
alter table secao_encaminhamento_aee drop if exists ordem;

alter table secao_encaminhamento_aee 
	add etapa int4 not null;
alter table secao_encaminhamento_aee 
	add ordem int4 not null;