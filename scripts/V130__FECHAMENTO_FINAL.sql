CREATE table
if not exists public.fechamento_final
(
                id int8 NOT NULL GENERATED ALWAYS AS identity,
                aluno_codigo varchar
(15) NOT NULL,
                disciplina_codigo varchar
(15) NOT NULL,
                conceito_id int8  NULL,
                turma_id int8 NOT NULL,
                migrado boolean default false,
                nota numeric
(5,2) NULL,
                                
                criado_em timestamp  NOT NULL,
                criado_por varchar
(200) NOT NULL,
                alterado_em timestamp ,
                alterado_por varchar
(200),
                criado_rf varchar
(200)  NOT NULL,
                alterado_rf varchar
(200),                
    
                CONSTRAINT fechamento_final_pk PRIMARY KEY
(id)        
);

select f_cria_fk_se_nao_existir('fechamento_final', 'fechamento_final_turma_fk', 'FOREIGN KEY (turma_id) REFERENCES turma(id)');
select f_cria_fk_se_nao_existir('fechamento_final', 'fechamento_final_conceito_fk', 'FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id)');


CREATE INDEX fechamento_final_aluno_idx ON public.fechamento_final USING btree
(aluno_codigo);
CREATE INDEX fechamento_final_disciplina_idx ON public.fechamento_final USING btree
(disciplina_codigo);

