update parametros_sistema
   set ano = 2020   
 where ano is null 
   and ativo = true
   and nome not in ('HabilitarServicosEmBackground',
					'MunicipioAtendimentoHistoricoEscolar',
					'DataUltimaAtualizacaoObjetivosJurema',
					'ExecutarManutencaoAulasInfantil',
					'PAPInicioAnoLetivo',
					'DataInicioSGP'
					);