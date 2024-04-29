

insert into componente_curricular_jurema (codigo_jurema, 
										  codigo_eol, 
										  descricao_eol, 
										  criado_em,
										  criado_por,
										  alterado_em,
										  alterado_por,
										  criado_rf,
										  alterado_rf)
select codigo_jurema,
	   1113 codigo_eol,
	   'REG CLASSE EJA ETAPA ALFAB',
	   current_date,
	   'Carga',
	   null,
	   null,
	   'Carga',
	   null
	from componente_curricular_jurema
where codigo_eol = 1105 and
	not exists (select 1
				from componente_curricular_jurema 
				where codigo_eol = 1113);
				
insert into componente_curricular_jurema (codigo_jurema, 
										  codigo_eol, 
										  descricao_eol, 
										  criado_em,
										  criado_por,
										  alterado_em,
										  alterado_por,
										  criado_rf,
										  alterado_rf)
select codigo_jurema,
	   1114 codigo_eol,
	   'REG CLASSE EJA ETAPA BASICA',
	   current_date,
	   'Carga',
	   null,
	   null,
	   'Carga',
	   null
	from componente_curricular_jurema
where codigo_eol = 1105 and
	not exists (select 1
				from componente_curricular_jurema 
				where codigo_eol = 1114);			
				
insert into componente_curricular_jurema (codigo_jurema, 
										  codigo_eol, 
										  descricao_eol, 
										  criado_em,
										  criado_por,
										  alterado_em,
										  alterado_por,
										  criado_rf,
										  alterado_rf)
select codigo_jurema,
	   1125 codigo_eol,
	   'REG CLASSE EJA EE',
	   current_date,
	   'Carga',
	   null,
	   null,
	   'Carga',
	   null
	from componente_curricular_jurema
where codigo_eol = 1105 and
	not exists (select 1
				from componente_curricular_jurema 
				where codigo_eol = 1125);		
				
				
insert into componente_curriculo_cidade(codigo, componente_curricular_id)
    select ccj.codigo_jurema, ccj.codigo_eol 
      from componente_curricular_jurema ccj
      where ccj.codigo_eol in (1113, 1114, 1125)
      and not exists (select 1 from componente_curriculo_cidade 
      				  where codigo = ccj.codigo_jurema 
      				  		and componente_curricular_id = ccj.codigo_eol);				