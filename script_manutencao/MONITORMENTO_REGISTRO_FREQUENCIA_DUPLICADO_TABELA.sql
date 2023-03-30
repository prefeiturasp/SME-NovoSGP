CREATE TABLE IF NOT EXISTS monitoramento.registro_frequencia_duplicado  (
	aula_id  int8 NOT NULL,	
	quantidade int4 NOT NULL,
	primeiro_registro timestamp NOT NULL,
	ultimo_registro timestamp NOT NULL,
	ultimo_id int8 NOT NULL
);