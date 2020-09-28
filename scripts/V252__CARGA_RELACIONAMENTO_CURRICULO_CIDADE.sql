delete from componente_curriculo_cidade 

insert into componente_curriculo_cidade(codigo, componente_curricular_id)
    select ccj.codigo_jurema, ccj.codigo_eol 
      from componente_curricular_jurema ccj 
     where codigo_eol not in (1073,1074,1072) 

select
	f_cria_fk_se_nao_existir(
		'componente_curriculo_cidade',
		'componente_curriculo_cidade_objetivo_aprendizagem_fk',
		'FOREIGN KEY (componente_curricular_id) REFERENCES objetivo_aprendizagem (componente_curricular_id)'
	);
