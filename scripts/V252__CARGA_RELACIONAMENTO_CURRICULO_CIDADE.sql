insert into componente_curricular(id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao)
                           values(1046, null, null, null, 'LINGUA INGLESA'),
                                 (1018, null, null, null, 'INFORMATICA  (OIE)');

delete from componente_curricular_jurema where codigo_eol in (1072, 1073, 1074);
  
delete from componente_curriculo_cidade;

insert into componente_curriculo_cidade(codigo, componente_curricular_id)
    select ccj.codigo_jurema, ccj.codigo_eol 
      from componente_curricular_jurema ccj;

