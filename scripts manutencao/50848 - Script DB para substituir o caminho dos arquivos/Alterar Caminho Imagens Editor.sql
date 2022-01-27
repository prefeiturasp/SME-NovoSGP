-- Plano ciclo

select '/Arquivos/Editor/' as pasta_anterior, 
concat('/arquivos/plano/ciclo/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		concat(a.codigo,substring(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(descricao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from plano_ciclo
		)
union all	
-- Frequência/Anotações do estudante

select '/Arquivos/Editor/' as pasta_anterior, 
concat('/arquivos/aluno/frequencia/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		concat(a.codigo,substring(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(anotacao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from anotacao_frequencia_aluno
		)
union all		

-- Planos anuais

select '/Arquivos/Editor/' as pasta_anterior, 
concat('/arquivos/planejamento/anual/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		concat(a.codigo,substring(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(descricao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from planejamento_anual_componente
		)
union all		
-- Diario de bordo

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/diario/bordo/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(planejamento, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from diario_bordo
		)
union all		
-- Devolutivas

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/devolutiva/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(descricao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from devolutiva
		) 
union all		

-- Compensacao ausencia

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/compensacao/ausencia/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(descricao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from compensacao_ausencia
		) 
union all		
-- Registro individual

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/registro/individual/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(registro, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from registro_individual
		)
union all		
		
-- Territorio do saber

-- imagem desenvolvimento

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/planejamento/anual/territorio_saber/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(desenvolvimento, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from plano_anual_territorio_saber
		) 
union all
-- imagem reflexao

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/planejamento/anual/territorio_saber/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(reflexao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from plano_anual_territorio_saber
		)
union all		
		
-- Carta de intenções

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/carta/intencoes/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(planejamento, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from carta_intencoes
		)
union all		

-- Registro POA

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/regsitro/poa/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(descricao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from registro_poa
		)
union all		
		
-- Conselho de classe

-- imagens recomendacoes aluno

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/conselho_classe/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(recomendacoes_aluno, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from conselho_classe_aluno
		) 
union all	
-- imagens recomendacoes familia

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/conselho_classe/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(recomendacoes_familia, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from conselho_classe_aluno
		)
union all		

-- imagens anotacoes pedagogicas

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/conselho_classe/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(anotacoes_pedagogicas, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from conselho_classe_aluno
		)
union all		
-- Ocorrencia


SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/aluno/ocorrencia/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(descricao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from ocorrencia
		)
union all		

-- relatorio semestral pap

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/relatorio/semestral_pap/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(valor, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from relatorio_semestral_pap_aluno_secao
		)
union all		

-- Plano aula

-- imagens descricao

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/planejamento/aula/descricao/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(descricao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from plano_aula
		)
union all		

-- imagens desenvolvimento aula

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/planejamento/aula/desenvolvimento/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(desenvolvimento_aula, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from plano_aula
		)
union all		
-- imagens recuperacao aula

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/planejamento/aula/recuperacao/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(recuperacao_aula, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from plano_aula
		) 
union all		

-- imagens licao casa

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/planejamento/aula/licao_casa/',
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(licao_casa, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from plano_aula
		) 
union all		

-- Fechamento/Anotações

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/fechamento/aluno/anotacao/' ,
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(anotacao, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from anotacao_aluno_fechamento
		) 
union all

-- Relatório de acompanhamento da aprendizagem

-- imagens PERCURSO COLETIVO
SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/turma/acompanhamento/' ,
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(apanhado_geral, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from ACOMPANHAMENTO_TURMA
		) 
union all


-- imagens PERCURSO INDIVIDUAL

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/aluno/acompanhamento/' ,
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(PERCURSO_INDIVIDUAL, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from ACOMPANHAMENTO_ALUNO_SEMESTRE
		) 
union all
-- imagens observações

SELECT '/Arquivos/Editor/' AS PASTA_ANTERIOR, 
CONCAT('/arquivos/aluno/acompanhamento/' ,
        EXTRACT(YEAR FROM a.CRIADO_EM),
        '/',
        EXTRACT(MONTH FROM a.CRIADO_EM),'/') as pasta_destino,
		CONCAT(A.CODIGO,SUBSTRING(nome, '\.[0-9a-zA-Z]+')) as nome
from arquivo a where a.codigo  in (
			select uuid(cast(regexp_matches(observacoes, '[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}', 'gi')  as varchar)) 
			from ACOMPANHAMENTO_ALUNO_SEMESTRE
		) 