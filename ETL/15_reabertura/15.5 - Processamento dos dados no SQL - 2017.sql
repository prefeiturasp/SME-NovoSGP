USE Manutencao
GO
truncate table etl_sgp_periodo_fechamento

USE GestaoPedagogica
GO
insert into [manutencao].[dbo].etl_sgp_periodo_fechamento
select 
--c.cal_descricao  							    as descricao,
ev.esc_id										as esc_id,	
ev.tpc_id										as tpc_id,
VCE.cd_unidade_administrativa_referencia		as [dre_id_eol],
UNI.uni_codigo									as [ue_id_eol],
ev.evt_nome										as nome,
cast(ev.evt_dataInicio as datetime)				as dataInicio,
cast(ev.evt_dataFim as datetime)      			as dataFim,
ev.evt_dataCriacao								as dataCriacao,
ev.evt_dataAlteracao							as dataAlteracao,
CASE 
	WHEN CHARINDEX('EJA',c.cal_descricao) = 0 THEN 4 -- não é EJA
	 	 ELSE CASE when tpc_id = 1 then 5
		           when tpc_id = 2 then 6
			  END
END												as tipo
from ACA_Evento ev
inner join ACA_CalendarioEvento				ce
            on ev.evt_id = ce.evt_id
inner join ACA_CalendarioAnual				c
            on c.cal_id = ce.cal_id
inner join ESC_Escola						es
            on es.esc_id = ev.esc_id
left join ESC_UnidadeEscola AS UNI (NOLOCK)
            on UNI.uni_id = ev.uni_id
            and UNI.esc_id = es.esc_id
left join [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_cadastro_unidade_educacao] AS vce (NOLOCK)
            on vce.cd_unidade_educacao = UNI.uni_codigo COLLATE Latin1_General_CI_AS
where    tev_id = 1
         and evt_situacao = 1
		 and c.cal_situacao = 1
		 and YEAR(evt_dataInicio) = 2017
		 and YEAR(evt_dataInicio) = c.cal_ano
		 and (evt_nome not like ('%retifica%') AND
			  evt_nome not like ('%regulariza%') AND
              evt_nome not like ('%final') AND
	--		  evt_nome not like ('Fech%')AND
			  evt_nome not like ('%corre%')) 
	      and((tpc_id = 1 and (evt_dataInicio between '2017-04-01' and '2017-05-30'))
		  or (tpc_id = 2 and (evt_dataInicio between '2017-06-01' and '2017-07-30'))
		  or (tpc_id = 3 and (evt_dataInicio between '2017-09-01' and '2017-10-30'))
		  or (tpc_id = 4 and (evt_dataInicio between '2017-11-01' and '2017-12-22')))
--		  and es.esc_codigo in ('019455','094765')
		 order by c.cal_descricao,ev.tpc_id

--
USE Manutencao
GO
SELECT * FROM etl_sgp_periodo_fechamento

