CREATE TABLE public.registro_poa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_rf varchar(20) NOT NULL,
	mes int2 NOT NULL,
	titulo varchar(50) NOT NULL,
	descricao varchar(10000) NOT NULL,
	dre_id varchar(50) NOT NULL,
	ue_id varchar(50) NOT NULL,
	excluido boolean NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(30) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(30) NULL
);

CREATE INDEX registro_poa_codigo_rf_idx ON public.registro_poa USING btree (codigo_rf);
CREATE INDEX registro_poa_Titulo_idx ON public.registro_poa USING btree (titulo);
CREATE INDEX registro_poa_dre_id_idx ON public.registro_poa USING btree (dre_id);
CREATE INDEX registro_poa_ue_id_idx ON public.registro_poa USING btree (ue_id);