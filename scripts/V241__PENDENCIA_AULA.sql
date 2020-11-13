DROP TABLE if exists public.pendencia_aula;

CREATE TABLE public.pendencia_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 not null,
	tipo int8 not null, 
    CONSTRAINT pendencia_aula_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'pendencia_aula',
		'pendencia_aula_aula_fk',
		'FOREIGN KEY (aula_id) REFERENCES aula (id)'
	);

CREATE INDEX pendencia_aula_aula_idx ON public.pendencia_aula USING btree (aula_id);