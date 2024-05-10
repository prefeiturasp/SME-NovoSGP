--2017
USE Manutencao
-- Drop table
-- DROP TABLE etl_sgp_periodo_reabertura;
truncate table etl_sgp_periodo_reabertura
CREATE TABLE etl_sgp_periodo_reabertura(
			id						int					NOT NULL IDENTITY(1,1),
			descricao				varchar(200)		NULL,
			inicio					datetime			NOT NULL,
			fim						datetime			NOT NULL,
			tipo_calendario_id		int					NOT NULL,
			dre_id_eol				varchar(200)		NULL,
			ue_id_eol				varchar(200)		NULL,
			status					int					NULL,
			criado_em				datetime			NOT NULL,
			bimestre				int					NULL
	)

USE GestaoPedagogica
insert into [manutencao].[dbo].etl_sgp_periodo_reabertura

select 
c.cal_descricao  							    as descricao,
ev.evt_dataInicio								as inicio, 
ev.evt_dataFim									as fim,
CASE 
	WHEN CHARINDEX('EJA',c.cal_descricao) = 0 THEN 5 -- não é EJA
	 	 ELSE CASE when tpc_id = 1 then 5
		           when tpc_id = 2 then 6
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
		 and YEAR(evt_dataInicio) = 2017
	--	 and YEAR(evt_dataInicio) = c.cal_ano
		 /*and (evt_nome not like ('%retifica%') AND
			  evt_nome not like ('%regulariza%') AND
              evt_nome not like ('%final') AND
			  evt_nome not like ('%corre%')) */
	      and((tpc_id = 1 and (evt_dataInicio not between '2017-04-01' and '2017-05-30'))
		  or (tpc_id = 2 and (evt_dataInicio not between '2017-06-01' and '2017-07-30'))
		  or (tpc_id = 3 and (evt_dataInicio not between '2017-09-01' and '2017-10-30'))
		  or (tpc_id = 4 and (evt_dataInicio not between '2017-11-01' and '2017-12-22')))
		  and es.esc_codigo in ('019455','094765')
		  order by c.cal_descricao,ev.tpc_id
--

select * from ESC_Escola
where esc_codigo = '019455'


USE Manutencao
GO
SELECT * FROM etl_sgp_periodo_reabertura 
