update etl_sgp_aula_plano_aula_dados_ok
   set PLA_descricao = NULL
 where PLA_descricao = 'NULL';

update etl_sgp_aula_plano_aula_dados_ok
   set PLA_recuperacao_aula = NULL
 where PLA_recuperacao_aula = 'NULL';

update etl_sgp_aula_plano_aula_dados_ok
   set PLA_licao_casa = NULL
 where PLA_licao_casa = 'NULL';

delete from etl_sgp_aula_plano_aula_dados_ok
 where PLA_descricao IS NULL
   and PLA_desenvolvimento_aula = 'Migrado - Não informado no legado.'
   and PLA_recuperacao_aula IS NULL
   and PLA_licao_casa IS NULL;