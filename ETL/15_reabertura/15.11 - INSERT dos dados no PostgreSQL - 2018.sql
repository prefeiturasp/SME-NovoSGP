-- 2018 ABERTURA OK
-- tabelas: PERIODO_FECHAMENTO e PERIODO_FECHAMENTO_BIMESTRE
truncate etl_sgp_periodo_fechamento cascade;

UPDATE etl_sgp_periodo_fechamento
SET ue_id_eol = RIGHT('000000' || ue_id_eol,6);
----
insert into periodo_fechamento
(ue_id,dre_id,migrado,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
select distinct 
	   ue.id			as ue_id,
	   dre.id			as dre_id,
	   true 			as migrado,
	   datacriacao		as criado_em,
	   'ETL'			as criado_por,
	   dataalteracao	as alterado_em,
	   'ETL'			as alterado_por,
	   0				as criado_rf,
	   0				as alterado_rf
from etl_sgp_periodo_fechamento etl
inner join ue ue
on ue.ue_id = etl.ue_id_eol
inner join dre dre
on dre.dre_id = etl.dre_id_eol;

-- tabela: PERIODO_FECHAMENTO_BIMESTRE

insert into periodo_fechamento_bimestre
(periodo_escolar_id,periodo_fechamento_id,inicio_fechamento,final_fechamento)	
	select  
	case when etl.tipo = 7 then
							case etl.tpc_id   when 1 then 21
											  when 2 then 22
											  when 3 then 23
											  when 4 then 24
							end
 		 when etl.tipo = 8  then 
 		 					case etl.tpc_id   when 1 then 25
 		 									  when 2 then 26
 		 					end
 		 when etl.tipo = 9  then
 		 					case etl.tpc_id   when 1 then 27
 		 									  when 2 then 28
 		 					end
	    end						as periodo_escolar_id,
		per.id		 			as periodo_fechamento_id,
		etl.datainicio			as inicio_fechamento,
		etl.datafim				as final_fechamento
        from      etl_sgp_periodo_fechamento as etl
 	    inner join ue
			on ue.ue_id = etl.ue_id_eol   
		inner join dre
			on dre.dre_id = etl.dre_id_eol			
        inner join periodo_fechamento as per
           on  ue.id  = per.ue_id and    
               dre.id = per.dre_id and
               etl.datacriacao = per.criado_em
		inner join tipo_calendario tc
			on tc.id = etl.tipo
		inner join periodo_escolar pe
			on pe.tipo_calendario_id=tc.id and
			pe.bimestre = etl.tpc_id;