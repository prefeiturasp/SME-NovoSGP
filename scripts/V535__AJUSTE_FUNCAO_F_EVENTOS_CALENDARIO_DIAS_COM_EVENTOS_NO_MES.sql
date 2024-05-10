drop function if exists public.f_eventos_calendario_dias_com_eventos_no_mes;
CREATE OR REPLACE FUNCTION public.f_eventos_calendario_dias_com_eventos_no_mes(p_login character varying, p_perfil_id uuid, p_historico boolean, p_mes integer, p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false, p_dre_id character varying DEFAULT NULL::character varying, p_ue_id character varying DEFAULT NULL::character varying, p_desconsidera_local_dre boolean DEFAULT false, p_desconsidera_evento_sme boolean DEFAULT false)
 RETURNS SETOF v_estrutura_eventos_calendario_dias_com_eventos_no_mes
 LANGUAGE sql
AS $function$ 	
select lista.dia,
	   lista.tipoEvento
from (

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_data_inicio_fim(p_login, p_perfil_id, p_historico, p_mes, p_tipo_calendario_id, p_considera_pendente_aprovacao, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)
    WHERE extract(month from data_evento) = p_mes
union 

select distinct extract(day from data_evento) as dia,
				tipoEvento
	from f_eventos_calendario_por_rf_criador(p_login, p_mes, p_tipo_calendario_id, p_dre_id, p_ue_id, p_desconsidera_local_dre, p_desconsidera_evento_sme)
    WHERE extract(month from data_evento) = p_mes
    ) lista
order by 1;
 $function$
;