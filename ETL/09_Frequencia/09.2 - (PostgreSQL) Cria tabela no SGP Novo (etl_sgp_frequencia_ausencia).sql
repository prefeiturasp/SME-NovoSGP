CREATE TABLE public.etl_sgp_frequencia_ausencia 
(
	id							int8		  NOT NULL GENERATED ALWAYS AS IDENTITY,
	taa_tud_id					int8		  NOT NULL,
	taa_tau_id					int8		  NOT NULL,
	taa_alu_id					int8		  NOT NULL,
	taa_mtu_id					int8		  NOT NULL,
	taa_mtd_id					int8		  NOT NULL,
	taa_criado_em				timestamp	  NOT NULL,
	taa_alterado_em				timestamp		  NULL,
	taa_alterado_por            varchar (200)	  NULL,
	taa_usu_idDocenteAlteracao  varchar (200)	  NULL,
	tau_tpc_id					int8		  NOT NULL,
	tau_tau_data				date		  NOT NULL,
	TAU_tau_numeroAulas         int8              NULL,
	tau_criado_em				timestamp	  NOT NULL,
	tau_alterado_em				timestamp		  NULL,
	tau_criado_por              varchar (200) NOT NULL,
	tau_alterado_por            varchar (200)	  NULL,
	tau_criado_rf               varchar (200) NOT NULL,
	tau_alterado_rf             varchar (200)	  NULL,
	tau_usu_id					varchar (200)	  NULL,
	tau_usu_iddocentealteracao  varchar (200)	  NULL,
	esc_esc_id					int8		  NOT NULL,
	tur_tur_id				    int8	 	  NOT NULL,
	codigo_aluno                varchar (50)  NOT NULL
);

