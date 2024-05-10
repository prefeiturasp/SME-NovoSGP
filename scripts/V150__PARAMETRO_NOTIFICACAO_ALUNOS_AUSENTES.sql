insert into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select 'QuantidadeDiasNotificaoCPAlunosAusentes','Quantidade de dias para notificar o CP sobre alunos ausentes','5',null,now(),'Carga Inicial','Carga Inicial', 14
where  not exists(
	select 1
	from public.parametros_sistema
	where tipo = 16 );

insert into 
    public.parametros_sistema (nome,descricao,valor,ano,criado_em,criado_por, criado_rf,tipo)
select 'QuantidadeDiasNotificaoDiretorAlunosAusentes','Quantidade de dias para notificar o Diretor sobre alunos ausentes','5',null,now(),'Carga Inicial','Carga Inicial', 14
where  not exists(
	select 1
	from public.parametros_sistema
	where tipo = 17 );

