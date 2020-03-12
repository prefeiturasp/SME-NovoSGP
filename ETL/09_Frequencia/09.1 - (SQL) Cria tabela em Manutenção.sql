USE GestaoPedagogica
GO


DROP TABLE [Manutencao].[dbo].[ETL_SGP_FREQUENCIA_AUSENCIA]
GO


CREATE TABLE [Manutencao].[dbo].[ETL_SGP_FREQUENCIA_AUSENCIA]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[taa_tud_id] [int] NOT NULL,
	[taa_tau_id] [int] NOT NULL,
	[taa_alu_id] [int] NOT NULL,
	[taa_mtu_id] [int] NOT NULL,
	[taa_mtd_id] [int] NOT NULL,
	[taa_criado_em] [datetime] NOT NULL,
	[taa_alterado_em] [datetime] NULL,
	[taa_alterado_por] [varchar] (200) NULL,
	[taa_usu_idDocenteAlteracao] [varchar] (200) NULL,
	[tau_tpc_id] [int] NOT NULL,
	[tau_tau_data] [date] NOT NULL,
	[TAU_tau_numeroAulas] [int] NULL,
	[tau_criado_em] [datetime] NOT NULL,
	[tau_alterado_em] [datetime] NULL,
	[tau_criado_por] [varchar] (200) NOT NULL,
	[tau_alterado_por] [varchar] (200) NULL,
	[tau_criado_rf] [varchar] (200) NOT NULL,
	[tau_alterado_rf] [varchar] (200) NULL,
	[tau_usu_id] [varchar] (200) NULL,
	[tau_usu_iddocentealteracao] [varchar] (200) NULL,
	[esc_esc_id] [int] NOT NULL,
	[tur_tur_id] [int] NOT NULL,
	[Codigo_Aluno] [varchar] (50) NOT NULL
) ON [PRIMARY]
GO
