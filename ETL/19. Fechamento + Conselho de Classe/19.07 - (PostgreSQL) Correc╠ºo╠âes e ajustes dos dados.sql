update etl_sgp_fechamento_cc 
   set recomendacoes_aluno = 'Migrado - N�o informado no legado.'
 where recomendacoes_aluno = 'NULL';

update etl_sgp_fechamento_cc 
   set recomendacoes_familia = 'Migrado - N�o informado no legado.'
 where recomendacoes_familia = 'NULL';

update etl_sgp_fechamento_cc 
   set anotacoes_pedagocidas = 'Migrado - N�o informado no legado.'
 where anotacoes_pedagocidas = 'NULL';

update etl_sgp_fechamento_cc 
   set justificativa = 'Migrado - N�o informado no legado.'
 where justificativa = 'NULL';

update etl_sgp_fechamento_cc 
   set recomendacoes_aluno = 'Migrado - N�o informado no legado.'
 where recomendacoes_aluno = NULL;

update etl_sgp_fechamento_cc 
   set recomendacoes_familia = 'Migrado - N�o informado no legado.'
 where recomendacoes_familia = NULL;

update etl_sgp_fechamento_cc 
   set anotacoes_pedagocidas = 'Migrado - N�o informado no legado.'
 where anotacoes_pedagocidas = NULL;

update etl_sgp_fechamento_cc 
   set justificativa = 'Migrado - N�o informado no legado.'
 where justificativa = NULL;

