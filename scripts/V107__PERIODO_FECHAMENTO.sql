CREATE TABLE IF NOT EXISTS public.periodo_fechamento
(
    id bigint NOT NULL generated always as identity,
    ue_id varchar(15) NULL,
    dre_id varchar(15) NULL,
    tipo_calendario_id bigint NOT NULL,    
	periodo_fechamento_bimestre_id bigint NOT NULL,
	excluido BOOLEAN NOT NULL,
    criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT periodo_fechamento_pk PRIMARY KEY (id)
);




CREATE TABLE IF NOT EXISTS public.periodo_fechamento_bimestre
(
    id bigint NOT NULL generated always as identity,
    periodo_escolar_id bigint NOT NULL,
	inicio_fechamento date NOT NULL,
	final_fechamento date NOT null,
	CONSTRAINT periodo_fechamento_bimestre_pk PRIMARY KEY (id)
);
select f_cria_constraint_se_nao_existir ('periodo_escolar', 'periodo_escolar_pk', 'PRIMARY KEY (id)');

select f_cria_fk_se_nao_existir('periodo_fechamento_bimestre', 'periodo_escolar_fechamento_fk', 'FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar (id)');

select f_cria_fk_se_nao_existir('periodo_fechamento', 'periodo_fechamento_bimestre_fk', 'FOREIGN KEY (periodo_fechamento_bimestre_id) REFERENCES periodo_fechamento_bimestre (id)');

select f_cria_fk_se_nao_existir('periodo_fechamento', 'periodo_fechamento_calendario_fk', 'FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario (id)');

CREATE INDEX periodo_fechamento_ue_idx ON public.periodo_fechamento (ue_id);
CREATE INDEX periodo_fechamento_dre_idx ON public.periodo_fechamento (dre_id);
CREATE INDEX periodo_fechamento_calendario_idx ON public.periodo_fechamento (tipo_calendario_id);

