USE [Manutencao]

--) Update no campo data_atualizacao
UPDATE ETL_UES
	SET data_atualizacao= '2019-12-09'

--) Update no campo de ue_id
UPDATE ETL_UES
	SET ue_id = RIGHT('000000' + ue_id,6)