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
	   1117 codigo_eol,
	   'REG CLASSE ESPECIAL NOTURNO',
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
				where codigo_eol = 1117);


