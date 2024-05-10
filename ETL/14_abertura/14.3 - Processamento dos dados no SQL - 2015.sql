USE GestaoPedagogica
GO
insert into [manutencao].[dbo].etl_sgp_periodo_reabertura

select 
ev.evt_nome										as descricao,
--c.cal_descricao  							    ,
ev.evt_dataInicio								as inicio, 
ev.evt_dataFim								    as fim,
CASE  
/*2019*/ WHEN (CHARINDEX('EJA',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao)) = 0 THEN 10
			ELSE CASE WHEN (CHARINDEX('1° Semestre',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao)) = 0 THEN 12
					  WHEN (CHARINDEX('2° Semestre',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao)) = 0 THEN 11	
/*2018*/			  WHEN (CHARINDEX('EJA',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 7 
					  WHEN (CHARINDEX('1° Semestre',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 9 	
					  WHEN (CHARINDEX('2° Semestre',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 8 	
/*2017*/		      WHEN (CHARINDEX('EJA',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 4 
					  WHEN  (CHARINDEX('1° Semestre',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 6 	
					  WHEN  (CHARINDEX('2° Semestre',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 5 	
/*2016*/		      WHEN (CHARINDEX('EJA',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2015',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 3 
/*2015*/		      WHEN (CHARINDEX('EJA',c.cal_descricao) + CHARINDEX('2014',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 2
/*2014*/		      WHEN (CHARINDEX('EJA',c.cal_descricao) + CHARINDEX('2016',c.cal_descricao) + CHARINDEX('2017',c.cal_descricao) + CHARINDEX('2018',c.cal_descricao) + CHARINDEX('2019',c.cal_descricao))  = 0 THEN 1
		 END
END											as tipo_calendario_id,
VCE.cd_unidade_administrativa_referencia	as [dre_id_eol],
UNI.uni_codigo								as [ue_id_eol],
--''										as wf_aprovacao_id,
ev.evt_situacao								as status,
--'TRUE'   								    as migrado,
ev.evt_dataCriacao							as criado_em,
--'ETL'										as criado_por,
--''										as alterado_em,
--'Migrado - Não informado no legado.'		as alterado_por,
--'0'										as criado_rf,
--''										as alterado_rf,
--'FALSE'									as excluido
ev.tpc_id									as bimestre
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
		 and YEAR(evt_dataInicio) = 2015
--		 and YEAR(evt_dataInicio) = c.cal_ano
		 /*and (evt_nome not like ('%retifica%') AND
			  evt_nome not like ('%regulariza%') AND
              evt_nome not like ('%final') AND
			  evt_nome not like ('%corre%')) */
	      and((tpc_id = 1 and (evt_dataInicio not between '2015-04-01' and '2015-05-30'))
		  or (tpc_id = 2 and (evt_dataInicio not between '2015-06-01' and '2015-07-30'))
		  or (tpc_id = 3 and (evt_dataInicio not between '2015-09-01' and '2015-10-30'))
		  or (tpc_id = 4 and (evt_dataInicio not between '2015-11-01' and '2015-12-22')))
--		  and es.esc_codigo in ('019455','094765')
		  order by tipo_calendario_id

--
USE Manutencao
GO
SELECT * FROM etl_sgp_periodo_reabertura