SELECT 
	distinct 
	servidor.cd_registro_funcional, 
	null AS cd_servidor_classroom, 
	[dbo].[proc_gerar_email_funcionario](servidor.nm_pessoa, servidor.cd_registro_funcional) AS nm_email, 
	0 AS in_ativo, 
	'/Professores/Inativos' AS nm_organizacao
INTO #tempFuncionariosInativos
FROM   
	v_servidor_cotic servidor
INNER JOIN 
	v_cargo_base_cotic cargoServidor 
	on servidor.cd_servidor = cargoServidor.cd_servidor AND cargoServidor.dt_fim_nomeacao > '2020-07-01'
LEFT JOIN 
	lotacao_servidor lotacao 
	on cargoServidor.cd_cargo_base_servidor = lotacao.cd_cargo_base_servidor AND lotacao.dt_fim > '2020-07-01';

INSERT INTO servidor_classroom
SELECT
	temp.*
FROM
	#tempFuncionariosInativos temp
LEFT JOIN
	servidor_classroom serv
	ON temp.cd_registro_funcional = serv.cd_servidor_cotic
WHERE
	serv.cd_servidor_cotic IS NULL;


