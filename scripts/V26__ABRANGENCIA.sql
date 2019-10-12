--INÍCIO DRES
create table if not exists public.abrangencia_dres
(
 	id bigint NOT NULL generated always as identity,
 	usuario_id bigint not null ,
 	dre_id varchar(15) not null,
 	abreviacao varchar(50) not null,
 	nome varchar(200) not null, 	
 	
 	CONSTRAINT abrangencia_dres_pk PRIMARY KEY (id)
);

select create_fk_if_not_exists('abrangencia_dres', 'abrangencia_dres_usario_fk', 'FOREIGN KEY (usuario_id) REFERENCES usuario(id)');

CREATE index if not exists abrangencia_dres_usuario_idx ON public.abrangencia_dres (usuario_id);
CREATE index if not exists abrangencia_dres_dre_id_idx ON public.abrangencia_dres (dre_id);

--FIM DRES

--INÍCIO UES


--FIM UES