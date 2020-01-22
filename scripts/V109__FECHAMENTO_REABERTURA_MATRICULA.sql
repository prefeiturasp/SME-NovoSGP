

CREATE table if not exists public.fechamento_reabertura (
                id int8 NOT NULL GENERATED ALWAYS AS identity,
                descricao varchar NOT NULL,
                inicio timestamp  NOT NULL,
                fim timestamp  NOT NULL,
                tipo_calendario_id int8 not null,
                dre_id int8 null,
                ue_id int8 null,
				wf_aprovacao_id  int8 null,
				status int NOT NULL,
                migrado boolean default false,
                
                criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT fechamento_reabertura_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('fechamento_reabertura', 'fechamento_reabertura_ue_fk', 'FOREIGN KEY (ue_id) REFERENCES ue(id)');
select f_cria_fk_se_nao_existir('fechamento_reabertura', 'fechamento_reabertura_dre_fk', 'FOREIGN KEY (dre_id) REFERENCES dre(id)');
select f_cria_fk_se_nao_existir('fechamento_reabertura', 'fechamento_reabertura_tipo_calendario_fk', 'FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id)');
select f_cria_fk_se_nao_existir('fechamento_reabertura', 'fechamento_reabertura_wf_aprovacao_fk', 'FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id)');

CREATE INDEX fechamento_reabertura_inicio_idx ON public.fechamento_reabertura USING btree (inicio);
CREATE INDEX fechamento_reabertura_fim_idx ON public.fechamento_reabertura USING btree (fim);
CREATE INDEX fechamento_reabertura_tipo_calendario_idx ON public.fechamento_reabertura USING btree (tipo_calendario_id);
CREATE INDEX fechamento_reabertura_dre_idx ON public.fechamento_reabertura USING btree (dre_id);


CREATE table if not exists public.fechamento_reabertura_bimestre (
                id int8 NOT NULL GENERATED ALWAYS AS identity,
                fechamento_reabertura_id int8 not null,
                bimestre int not null,         
    CONSTRAINT fechamento_reabertura_bimestre_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('fechamento_reabertura_bimestre', 'fechamento_reabertura_bimestre_fechamento_reabertura_fk', 'FOREIGN KEY (fechamento_reabertura_id) REFERENCES fechamento_reabertura(id)');

CREATE INDEX fechamento_reabertura_bimestre_fech_reab_bi_idx ON public.fechamento_reabertura_bimestre USING btree (fechamento_reabertura_id);


