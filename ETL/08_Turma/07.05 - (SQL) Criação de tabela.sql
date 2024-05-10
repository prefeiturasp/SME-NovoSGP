USE [Manutencao]
CREATE TABLE #ues (
			id					INT				NOT NULL,
			ue_id				varchar(15)			NULL,
			dre_id				INT					NULL,
			nome				varchar(200)		NULL,
			tipo_escola			INT					NULL,
			data_atualizacao	DATE			    NULL,
			CONSTRAINT ues_pk PRIMARY KEY (id)
			);