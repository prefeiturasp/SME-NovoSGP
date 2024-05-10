alter table secao_encaminhamento_aee drop if exists nome_componente;
alter table secao_encaminhamento_aee add nome_componente varchar(50);

alter table secao_encaminhamento_naapa drop if exists nome_componente;
alter table secao_encaminhamento_naapa add nome_componente varchar(50);

alter table questao drop if exists nome_componente;
alter table questao add nome_componente varchar(50);
