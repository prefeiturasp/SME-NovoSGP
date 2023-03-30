CREATE TABLE IF NOT EXISTS monitoramento.registro_frequencia_aluno_duplicado  (
	registro_frequencia_id  int8 NOT NULL,	
	aula_id  int8 NOT NULL,	
	numero_aula int4 NOT NULL,	
	aluno_codigo varchar(15) NOT NULL,
	quantidade int4 NOT NULL,
	primeiro_registro timestamp NOT NULL,
	ultimo_registro timestamp NOT NULL,
	ultimo_id int8 NOT NULL
);