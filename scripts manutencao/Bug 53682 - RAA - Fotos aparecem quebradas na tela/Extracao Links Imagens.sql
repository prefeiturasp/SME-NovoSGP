drop table if exists tmp_acompanhamento_aluno;

select aas.* 
	, CONCAT('https://novosgp.sme.prefeitura.sp.gov.br/Arquivos/aluno/acompanhamento/',
        EXTRACT(YEAR FROM aas.CRIADO_EM),'/',
        EXTRACT(MONTH FROM aas.CRIADO_EM),'/',
	 	aas.Nome) as NomeCompleto
	, CONCAT('https://novosgp.sme.prefeitura.sp.gov.br/Arquivos/aluno/acompanhamento/',
        EXTRACT(YEAR FROM a.CRIADO_EM),'/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/',
	 	aas.Nome) as NomeCompletoAlternativo
	, CONCAT('https://novosgp.sme.prefeitura.sp.gov.br/Arquivos/registro/individual/',
        EXTRACT(YEAR FROM aas.CRIADO_EM),'/',
        EXTRACT(MONTH FROM aas.CRIADO_EM),'/',
	 	aas.Nome) as NomeCompletoAlternativo2
	, CONCAT('https://novosgp.sme.prefeitura.sp.gov.br/Arquivos/registro/individual/',
        EXTRACT(YEAR FROM a.CRIADO_EM),'/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/',
	 	aas.Nome) as NomeCompletoAlternativo3
into tmp_acompanhamento_aluno
  from arquivo a
 inner join (
select uuid(cast(regexp_matches(PERCURSO_INDIVIDUAL, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) as Codigo
	, CONCAT(uuid(cast(regexp_matches(PERCURSO_INDIVIDUAL, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar))
	, SUBSTRING(cast(regexp_matches(PERCURSO_INDIVIDUAL, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[0-9a-zA-Z]+', 'gi') as varchar), '\.[0-9a-zA-Z]+')) as Nome
	, aas.ID, aas.PERCURSO_INDIVIDUAL, aas.CRIADO_EM
from ACOMPANHAMENTO_ALUNO_SEMESTRE aas
where PERCURSO_INDIVIDUAL like '%https://novosgp.sme.prefeitura.sp.gov.br/Arquivos/aluno/acompanhamento/%'
) aas on aas.codigo = a.codigo
;