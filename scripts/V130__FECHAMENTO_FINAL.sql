create table if not exists public.fechamento_final ( id int8 not null generated always as identity,
aluno_codigo varchar (15) not null,
disciplina_codigo varchar (15) not null,
conceito_id int8 null,
turma_id int8 not null,
migrado boolean default false,
nota numeric (5,
2) null,
criado_em timestamp not null,
criado_por varchar (200) not null,
alterado_em timestamp ,
alterado_por varchar (200),
criado_rf varchar (200) not null,
alterado_rf varchar (200),
eh_regencia boolean default false,
constraint fechamento_final_pk primary key (id) );

select f_cria_fk_se_nao_existir('fechamento_final', 'fechamento_final_turma_fk', 'FOREIGN KEY (turma_id) REFERENCES turma(id)');
select f_cria_fk_se_nao_existir('fechamento_final', 'fechamento_final_conceito_fk', 'FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id)');


CREATE INDEX fechamento_final_aluno_idx ON public.fechamento_final USING btree
(aluno_codigo);
CREATE INDEX fechamento_final_disciplina_idx ON public.fechamento_final USING btree
(disciplina_codigo);

