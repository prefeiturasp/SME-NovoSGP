CREATE TABLE IF NOT EXISTS public.aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id varchar(15) NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	turma_id varchar(15) NOT NULL,
	tipo_calendario_id varchar(15) NOT NULL,
	professor_id int8 NOT NULL,
	quantidade int4 NOT NULL,
	data_aula timestamp not null,
	recorrencia_aula int not null,
	tipo_aula int not null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido boolean not null default false,
	migrado boolean not null default false,

	CONSTRAINT aula_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('aula', 'aula_usuario_fk', 'FOREIGN KEY (professor_id) REFERENCES usuario(id)');

CREATE index if not exists aula_ue_idx ON public.aula (ue_id);
CREATE index if not exists aula_disciplina_idx ON public.aula (disciplina_id);
CREATE index if not exists aula_turma_idx ON public.aula (turma_id);