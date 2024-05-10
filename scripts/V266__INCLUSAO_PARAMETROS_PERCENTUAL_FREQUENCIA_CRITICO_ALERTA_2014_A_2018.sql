insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select 'PercentualFrequenciaCritico', 4, 'Percentual de frequência para definir aluno em situação crítica', '75', 2014, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 4 and
				 	    ano = 2014) union
select 'PercentualFrequenciaCritico', 4, 'Percentual de frequência para definir aluno em situação crítica', '75', 2015, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 4 and
				 	    ano = 2015) union
select 'PercentualFrequenciaCritico', 4, 'Percentual de frequência para definir aluno em situação crítica', '75', 2016, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null 
where not exists (select 1
				  from parametros_sistema
				  where tipo = 4 and
				 	    ano = 2016) union
select 'PercentualFrequenciaCritico', 4, 'Percentual de frequência para definir aluno em situação crítica', '75', 2017, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null 
where not exists (select 1
				  from parametros_sistema
				  where tipo = 4 and
				 	    ano = 2017) union
select 'PercentualFrequenciaCritico', 4, 'Percentual de frequência para definir aluno em situação crítica', '75', 2018, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 4 and
				 	    ano = 2018) union
select 'PercentualFrequenciaAlerta', 3, 'Percentual de frequência para definir aluno em situação de alerta', '80', 2014, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 3 and
				 	    ano = 2014) union
select 'PercentualFrequenciaAlerta', 3, 'Percentual de frequência para definir aluno em situação de alerta', '80', 2015, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 3 and
				 	    ano = 2015) union
select 'PercentualFrequenciaAlerta', 3, 'Percentual de frequência para definir aluno em situação de alerta', '80', 2016, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 3 and
				 	    ano = 2016) union
select 'PercentualFrequenciaAlerta', 3, 'Percentual de frequência para definir aluno em situação de alerta', '80', 2017, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 3 and
				 	    ano = 2017) union
select 'PercentualFrequenciaAlerta', 3, 'Percentual de frequência para definir aluno em situação de alerta', '80', 2018, true, current_date, 'Carga Inicial', null::timestamp(6), null, 'Carga Inicial', null
where not exists (select 1
				  from parametros_sistema
				  where tipo = 3 and
				 	    ano = 2018);
				 	   



