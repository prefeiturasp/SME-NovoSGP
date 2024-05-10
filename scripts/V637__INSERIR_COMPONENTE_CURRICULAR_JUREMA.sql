DO $$
BEGIN
IF (select count(*) from componente_curricular cc where cc.id in (1322, 1770)) = 2 THEN
	insert into componente_curricular_jurema (codigo_jurema, codigo_eol, descricao_eol, criado_em, criado_por, criado_rf) 
	values (14, 1322, 'PAP - RECUPERACAO DE APRENDIZAGENS', CURRENT_TIMESTAMP, 'Carga', 'Carga'),
		   (14, 1770, 'PAP PROJETO COLABORATIVO', CURRENT_TIMESTAMP, 'Carga', 'Carga');

	insert into componente_curriculo_cidade(codigo, componente_curricular_id)
	select ccj.codigo_jurema, ccj.codigo_eol 
	from componente_curricular_jurema ccj
	where ccj.codigo_eol in (1322, 1770);
END if;
END $$;