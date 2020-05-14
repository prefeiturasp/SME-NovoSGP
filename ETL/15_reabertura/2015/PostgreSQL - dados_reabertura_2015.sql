--2015 Reabertura 
---
truncate etl_sgp_fechamento_reabertura;

select * from etl_sgp_fechamento_reabertura;
select * from fechamento_reabertura;
select * from fechamento_reabertura_bimestre;

select * from tipo_calendario
where ano_letivo in (2014,2015,2016,2017,2018,2019)

--- INSERT fechamento_reabertura

insert into fechamento_reabertura
(descricao,inicio,fim,tipo_calendario_id,dre_id,ue_id,wf_aprovacao_id,status,migrado,
criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf,excluido)
select 
	   fr.descricao,
	   fr.inicio,
	   fr.fim,
	   fr.tipo_calendario_id,
	   dre.id						as dre_id,
	   ue.id						as ue_id,
	   null							as wf_aprovacao_id,
	   1							as status,
	   true							as migrado,
	   fr.criado_em,
	   'ETL'						as criado_por,
	   null							as alterado_em,
	   null							as alterado_por,
	   '0'							as criado_rf,
	   null							as alterado_rf,
	   false						as excluido
from etl_sgp_fechamento_reabertura fr
inner join ue ue
on ue.ue_id = fr.ue_id_eol
inner join dre dre
on dre.dre_id = fr.dre_id_eol;


-- INSERT  fechamento_reabertura_bimestre
insert into fechamento_reabertura_bimestre
(fechamento_reabertura_id,bimestre)
select 
fr.id as fechamento_reabertura_id,
etl.bimestre
from fechamento_reabertura fr
inner join ue ue
on ue.id = fr.ue_id
inner join dre dre
on dre.id = fr.dre_id
inner join etl_sgp_fechamento_reabertura etl
on etl.descricao = fr.descricao
and etl.inicio = fr.inicio
and etl.fim = fr.fim
and etl.tipo_calendario_id = fr.tipo_calendario_id
and dre.id = fr.dre_id
and ue.id = fr.ue_id
and etl.criado_em = fr.criado_em;
