delete from parametros_sistema where tipo = 82;
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação de registros pedagógicos', '', 2021, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação de registros pedagógicos', '', 2020, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação de registros pedagógicos', '', 2019, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação de registros pedagógicos', '', 2018, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação de registros pedagógicos', '', 2017, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação registros pedagógicos', '', 2016, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação registros pedagógicos', '', 2015, true, now(), 'SISTEMA', '0');
insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
values('ExecucaoConsolidacaoRegistrosPedagogicos', 82, 'Data da última execução da rotina de consolidação registros pedagógicos', '', 2014, true, now(), 'SISTEMA', '0');

DROP TABLE if exists public.consolidacao_registros_pedagogicos;

CREATE TABLE public.consolidacao_registros_pedagogicos (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
    componente_curricular_id int8 NOT NULL,
	ano_letivo int4 NOT NULL,
    nome_professor varchar(200),
    rf_professor varchar(200),
    quantidade_aulas int4 NOT NULL DEFAULT 0,
    frequencias_pendentes int4 NOT NULL DEFAULT 0,
    data_ultima_frequencia timestamp NOT NULL,
    data_ultimo_diariobordo timestamp,
    data_ultimo_planoaula timestamp, 
    diario_bordo_pendentes int4 DEFAULT 0,
    planos_aula_pendentes int4 DEFAULT 0,

	CONSTRAINT consolidacao_registros_pedagogicos_pk PRIMARY KEY (id)
);
ALTER TABLE public.consolidacao_registros_pedagogicos ADD CONSTRAINT consolidacao_registros_pedagogicos_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
ALTER TABLE public.consolidacao_registros_pedagogicos ADD CONSTRAINT consolidacao_registros_pedagogicos_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id);
CREATE INDEX consolidacao_registros_pedagogicos_turma_idx ON public.consolidacao_registros_pedagogicos USING btree (turma_id);
CREATE INDEX consolidacao_registros_pedagogicos_componente_curricular_idx ON public.consolidacao_registros_pedagogicos USING btree (componente_curricular_id);
CREATE INDEX consolidacao_registros_pedagogicos_ano_idx ON public.consolidacao_registros_pedagogicos USING btree (ano_letivo);