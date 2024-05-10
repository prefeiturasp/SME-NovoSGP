USE [Manutencao]
DROP TABLE ETL_TURMAS;
CREATE TABLE ETL_TURMAS
		(CodEscola							varchar(6),
         CodTurma							int, 
		 AnoLetivo							smallint, 
		 Modalidade							varchar(15),
         Semestre							smallint, 
		 CodModalidade						smallint, 
		 CodDre								varchar(6),
         Dre								varchar(60),
         DreAbrev							varchar(60),
         UE									varchar(60),
         UEAbrev							varchar(60),
         NomeTurma							varchar(15),
         Ano								varchar(18),
         TipoUE								varchar(25),
         CodTipoUE							int, 
		 CodTipoEscola						int, 
		 TipoEscola							varchar(12),
         DuracaoTurno						smallint, 
		 TipoTurno							int,
		 data_atualizacao					datetime,
		 dt_fim_eol							datetime )