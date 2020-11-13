CREATE TABLE public.componente_curricular_regencia (
	componente_curricular_id int8 NOT NULL,
	turno int8 NULL,
	ano int8 NULL	
);

INSERT INTO public.componente_curricular_regencia (componente_curricular_id,turno,ano) VALUES 
(2,NULL,NULL)
,(2,4,1)
,(2,4,2)
,(2,4,3)
,(2,4,4)
,(2,4,5)
,(6,4,1)
,(6,4,2)
,(7,NULL,NULL)
,(7,4,1);

INSERT INTO public.componente_curricular_regencia (componente_curricular_id,turno,ano) VALUES 
(7,4,2)
,(7,4,3)
,(7,4,4)
,(7,4,5)
,(8,NULL,NULL)
,(8,4,1)
,(8,4,2)
,(8,4,3)
,(8,4,4)
,(8,4,5);

INSERT INTO public.componente_curricular_regencia (componente_curricular_id,turno,ano) VALUES 
(89,NULL,NULL)
,(89,4,1)
,(89,4,2)
,(89,4,3)
,(89,4,4)
,(89,4,5)
,(138,NULL,NULL)
,(138,4,1)
,(138,4,2)
,(138,4,3);

INSERT INTO public.componente_curricular_regencia (componente_curricular_id,turno,ano) VALUES 
(138,4,4)
,(138,4,5)
,(139,4,1)
,(139,4,2)
,(139,4,3)
,(139,4,4)
,(139,4,5);

select
	f_cria_fk_se_nao_existir(
		'componente_curricular_regencia',
		'componente_curricular_regencia_cp_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)'
	);

CREATE INDEX IF NOT EXISTS componente_curricular_regencia_cp_idx ON public.componente_curricular_regencia USING btree (componente_curricular_id);