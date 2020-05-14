insert into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select 'PercentualAlunosInsuficientes','Percentual de alunos com nota/conceito insuficientes para exigência de justificativa','50',null,now(),'Carga Inicial','Carga Inicial', 15
where  not exists(
	select 1
	from public.parametros_sistema
	where tipo = 15 );