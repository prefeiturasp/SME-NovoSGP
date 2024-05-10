begin transaction;

alter table if exists evento_tipo add column IF NOT exists codigo int8;

create temp table tmp_evento_tipo(id int8, codigo int8);

insert into tmp_evento_tipo 
select row_number() over() as codigo, id  from evento_tipo ev
 order by ev.id ;


UPDATE evento_tipo t1
SET codigo = t2.codigo
from tmp_evento_tipo t2
where t1.id = t2.id;

drop table tmp_evento_tipo;

ALTER TABLE evento_tipo ALTER COLUMN codigo SET NOT NULL;

CREATE INDEX evento__ue_idx ON public.notificacao (ue_id);

end transaction;

 