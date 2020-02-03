CREATE table if not exists public.fechamento_reabertura_notificacao (
                id int8 NOT NULL GENERATED ALWAYS AS identity,
                fechamento_reabertura_id int8 not null,
                notificacao_id int8 not null,
    CONSTRAINT fechamento_reabertura_notificacao_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('fechamento_reabertura_notificacao', 'fechamento_reabertura_notificacao_fechamento_fk', 'FOREIGN KEY (fechamento_reabertura_id) REFERENCES fechamento_reabertura(id)');
select f_cria_fk_se_nao_existir('fechamento_reabertura_notificacao', 'fechamento_reabertura_notificacao_noticacao_fk', 'FOREIGN KEY (notificacao_id) REFERENCES notificacao(id)');

CREATE index if not exists fechamento_reabertura_notificacao_fechamento_idx ON public.fechamento_reabertura_notificacao USING btree (fechamento_reabertura_id);
CREATE INDEX if not exists fechamento_reabertura_notificacao_noticacao_idx ON public.fechamento_reabertura_notificacao USING btree (notificacao_id);



