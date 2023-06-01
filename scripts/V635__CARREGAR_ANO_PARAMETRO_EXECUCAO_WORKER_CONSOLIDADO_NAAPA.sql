update parametros_sistema 
	set ano = extract(year from now())
where tipo in (104, 105)
