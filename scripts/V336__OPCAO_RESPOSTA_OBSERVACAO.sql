alter table opcao_resposta drop if exists observacao;
alter table opcao_resposta 
	add observacao varchar null;
	