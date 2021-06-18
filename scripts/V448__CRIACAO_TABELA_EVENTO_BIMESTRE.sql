DELETE FROM evento WHERE tipo_evento_id = 7;


CREATE TABLE IF NOT EXISTS public.evento_bimestre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	
	evento_id int8 NOT NULL,	
    
	bimestre int8 NOT NULL,
	criado_em timestamp, 
	criado_por varchar(200),
    alterado_em timestamp,
    alterado_por varchar(200),
 	criado_rf varchar (200),
    alterado_rf varchar (200),
	
	
	CONSTRAINT evento_bimestre_pk PRIMARY KEY (id)
);


ALTER TABLE public.evento_bimestre ADD CONSTRAINT evento_bimestre_evento_fk FOREIGN KEY (evento_id) REFERENCES public.evento(id);