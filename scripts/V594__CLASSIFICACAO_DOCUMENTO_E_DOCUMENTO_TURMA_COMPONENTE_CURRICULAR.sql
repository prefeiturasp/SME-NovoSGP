--> Aumentando o tamanho do campo 'descricao'
ALTER TABLE classificacao_documento alter COLUMN descricao type varchar(50);

--> Criar o campo para indicar que a classificação permite mais de um registro.
ALTER TABLE classificacao_documento alter COLUMN descricao type varchar(50);
ALTER TABLE classificacao_documento ADD COLUMN IF NOT EXISTS ehRegistroMultiplo bool NOT NULL DEFAULT false;

--> Carta Pedagógica
insert into classificacao_documento (descricao, tipo_documento_id, ehRegistroMultiplo)
select 'Carta Pedagógica', 2, true
where not exists(select 1 from public.ocorrencia_tipo where descricao = 'Carta Pedagógica');

--> Documentos da turma
insert into classificacao_documento (descricao, tipo_documento_id, ehRegistroMultiplo)
select 'Documentos da Turma', 2, true
where not exists(select 1 from public.ocorrencia_tipo where descricao = 'Documentos da Turma');

--> Criando campo turma_id
ALTER TABLE documento ADD column turma_id int8 NULL;
ALTER TABLE documento ADD CONSTRAINT documento_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

--> Criando campo componente_curricular_id
ALTER TABLE documento ADD column componente_curricular_id int8 NULL;
ALTER TABLE documento ADD CONSTRAINT documento_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id);

--> Removendo identity classificacao_documento
ALTER TABLE classificacao_documento ALTER COLUMN id DROP IDENTITY IF EXISTS;

--> Removendo identity tipo_documento
ALTER TABLE tipo_documento ALTER COLUMN id DROP IDENTITY IF EXISTS;