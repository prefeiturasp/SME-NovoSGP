CREATE TABLE IF NOT EXISTS public.tipo_ciclo
(
    id int8 NOT NULL,
    descricao varchar(100) NOT NULL,
    criado_em timestamp NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp,
    alterado_por varchar(200),
    criado_rf varchar(200) NOT NULL,
    alterado_rf varchar(200),
    CONSTRAINT tipo_ciclo_pk PRIMARY KEY (id),
    CONSTRAINT tipo_ciclo_un UNIQUE (descricao)
);


CREATE TABLE IF NOT EXISTS public.tipo_ciclo_ano
(
    id bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    tipo_ciclo_id bigint NOT NULL,
    etapa_id bigint NOT NULL,
    ano varchar(4) NOT NULL,
    CONSTRAINT tipo_ciclo_ano_pk PRIMARY KEY (id),
    CONSTRAINT tipo_ciclo_ano_un UNIQUE (tipo_ciclo_id, etapa_id, ano),
    CONSTRAINT tipo_ciclo_id_fk FOREIGN KEY (tipo_ciclo_id)
    REFERENCES public.tipo_ciclo (id) 
);

insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(1,'Alfabetização',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(2,'Interdisciplinar',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(3,'Autoral',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(4,'Médio',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(5,'Básica',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(6,'Complementar',now(),'Carga inicial','Carga inicial');
insert into tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) values(7,'Final',now(),'Carga inicial','Carga inicial');

insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (1,5,1);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (1,5,2);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (1,5,3);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (2,5,4);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (2,5,5);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (2,5,6);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (3,5,7);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (3,5,8);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (3,5,9);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (4,6,1);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (4,6,2);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (4,6,3);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (1,3,1);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (1,3,2);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (5,3,1);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (5,3,2);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (6,3,1);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (6,3,2);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (7,3,1);
insert into tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) values (7,3,2);
