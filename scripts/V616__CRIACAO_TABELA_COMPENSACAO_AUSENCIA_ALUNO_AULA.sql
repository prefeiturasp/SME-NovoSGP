create table if not exists compensacao_ausencia_aluno_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	compensacao_ausencia_aluno_id int8 NOT NULL,
	registro_frequencia_aluno_id int8 NOT NULL,
	numero_aula int4,
	data_aula timestamp,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT compensacao_ausencia_aluno_aula_pk PRIMARY KEY (id)
);

CREATE index if not exists comp_ausencia_aluno_aula_comp_ausencia_aluno_id_ix ON compensacao_ausencia_aluno_aula USING btree (compensacao_ausencia_aluno_id);
CREATE index if not exists comp_ausencia_aluno_aula_registro_frequencia_aluno_id_ix ON compensacao_ausencia_aluno_aula USING btree (registro_frequencia_aluno_id);