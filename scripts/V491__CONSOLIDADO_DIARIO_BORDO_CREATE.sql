delete from parametros_sistema where tipo = 81;
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2021, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2020, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2019, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2018, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2017, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2016, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2015, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoDiariosBordo', 81, 'Data da última execução da rotina de consolidação de diarios de bordo', '', 2014, true, now(), 'SISTEMA', '0');

DROP TABLE if exists public.consolidacao_diarios_bordo;

CREATE TABLE public.consolidacao_diarios_bordo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	ano_letivo int4 NOT NULL,
	quantidade_preenchidos int4 NOT NULL DEFAULT 0,
	quantidade_pendentes int4 NOT NULL DEFAULT 0,
	CONSTRAINT consolidacao_diarios_bordo_pk PRIMARY KEY (id)
);
ALTER TABLE public.consolidacao_diarios_bordo ADD CONSTRAINT consolidacao_diarios_bordo_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
CREATE INDEX consolidacao_diarios_bordo_turma_idx ON public.consolidacao_diarios_bordo USING btree (turma_id);
CREATE INDEX consolidacao_diarios_bordo_ano_idx ON public.consolidacao_diarios_bordo USING btree (ano_letivo);
