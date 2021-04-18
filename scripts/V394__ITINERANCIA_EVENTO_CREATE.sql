drop table if exists public.itinerancia_evento;

CREATE TABLE public.itinerancia_evento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,

	itinerancia_id int8 not null,
	evento_id int8 not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT itinerancia_evento_pk PRIMARY KEY (id)
);

ALTER TABLE public.itinerancia_evento ADD CONSTRAINT itinerancia_evento_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id);
ALTER TABLE public.itinerancia_evento ADD CONSTRAINT itinerancia_evento_evento_fk FOREIGN KEY (evento_id) REFERENCES evento(id);

CREATE INDEX itinerancia_evento_itinerancia_idx ON public.itinerancia_evento USING btree (itinerancia_id);
CREATE INDEX itinerancia_evento_evento_idx ON public.itinerancia_evento USING btree (evento_id);

