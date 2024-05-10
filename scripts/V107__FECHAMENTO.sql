CREATE TABLE IF NOT EXISTS public.fechamento
(
    id bigint NOT NULL generated always as identity,
    ue_id bigint NULL,
    dre_id bigint NULL,
    migrado bool NOT NULL DEFAULT false,
    criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_pk PRIMARY KEY (id)
);




CREATE TABLE IF NOT EXISTS public.fechamento_bimestre
(
    id bigint NOT NULL generated always as identity,
    periodo_escolar_id bigint NOT NULL,
	fechamento_id bigint NOT NULL,
	inicio_fechamento date NOT NULL,
	final_fechamento date NOT null,
	CONSTRAINT fechamento_bimestre_pk PRIMARY KEY (id)
);


select f_cria_constraint_se_nao_existir ('periodo_escolar', 'periodo_escolar_pk', 'PRIMARY KEY (id)');

select f_cria_fk_se_nao_existir('fechamento_bimestre', 'periodo_escolar_fechamento_fk', 'FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar (id)');

select f_cria_fk_se_nao_existir('fechamento_bimestre', 'fechamento_fk', 'FOREIGN KEY (fechamento_id) REFERENCES fechamento (id)');

select f_cria_fk_se_nao_existir('fechamento', 'fechamento_dre_fk', 'FOREIGN KEY (dre_id) REFERENCES dre (id)');
select f_cria_fk_se_nao_existir('fechamento', 'fechamento_ue_fk', 'FOREIGN KEY (ue_id) REFERENCES ue (id)');

CREATE INDEX if not exists fechamento_ue_idx ON public.fechamento (ue_id);
CREATE INDEX if not exists fechamento_dre_idx ON public.fechamento (dre_id);
