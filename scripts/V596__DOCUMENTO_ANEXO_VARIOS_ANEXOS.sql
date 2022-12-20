
drop table if exists documento_arquivo;

CREATE table documento_arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	documento_id int8 NOT NULL,
	arquivo_id int8 NOT NULL,	
	CONSTRAINT documento_arquivo_pk PRIMARY KEY (id)
);

ALTER TABLE documento_arquivo ADD CONSTRAINT documento_arquivo_documento_fk FOREIGN KEY (documento_id) REFERENCES documento(id);

ALTER TABLE documento_arquivo ADD CONSTRAINT documento_arquivo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);

--> Movendo arquivo_id para tabela de detalhe
insert into documento_arquivo (documento_id,arquivo_id)
select id, arquivo_id from documento where arquivo_id is not null;

--Removendo coluna arquivo em documento
alter table documento drop column arquivo_id;