insert 
    into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select
	'QuantidadeDiasNotificarProfessor','Dias antes do fim do bimestre para notificar aulas previstas x criadas divergentes','7',null,now(),'Carga Inicial','Carga Inicial',9
where
	not exists(
	select
		1
	from
		public.parametros_sistema
	where
		tipo = 9 );