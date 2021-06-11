drop table if exists opcao_questao_complementar;

-- OPCAO_QUESTAO_COMPLEMENTAR
CREATE table public.opcao_questao_complementar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	opcao_resposta_id int8 not null,
	questao_complementar_id int8 not null,		
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT opcao_questao_complementar_pk PRIMARY KEY (id)
);

CREATE INDEX opcao_questao_complementar_opcao_resposta_idx ON public.opcao_questao_complementar USING btree (opcao_resposta_id);
ALTER TABLE public.opcao_questao_complementar ADD CONSTRAINT opcao_questao_complementar_opcao_resposta_fk FOREIGN KEY (opcao_resposta_id) REFERENCES opcao_resposta(id);

CREATE INDEX opcao_questao_complementar_questao_idx ON public.opcao_questao_complementar USING btree (questao_complementar_id);
ALTER TABLE public.opcao_questao_complementar ADD CONSTRAINT opcao_questao_complementar_questao_fk FOREIGN KEY (questao_complementar_id) REFERENCES questao(id);

-- CARGA DO RELACIONAMENTO QUESTÃO E OPÇÃO RESPOSTA
insert into opcao_questao_complementar
		   (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
     select id, questao_complementar_id, now(), 'SISTEMA', 0  
       from opcao_resposta 
      where questao_complementar_id is not null;

-- EXCLUIR QUESTAO_COMPLEMENTAR_ID DA TABELA OPCAO_RESPOSTA
alter table opcao_resposta drop if exists questao_complementar_id;