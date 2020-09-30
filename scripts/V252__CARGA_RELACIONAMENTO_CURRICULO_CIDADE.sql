
delete from componente_curricular_jurema where codigo_eol in (1072, 1073, 1074);
  
delete from componente_curriculo_cidade;

insert into componente_curriculo_cidade(codigo, componente_curricular_id)
    select ccj.codigo_jurema, ccj.codigo_eol 
      from componente_curricular_jurema ccj;

