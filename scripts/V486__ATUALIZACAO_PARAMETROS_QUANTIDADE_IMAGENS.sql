update parametros_sistema 
set nome = 'QuantidadeImagensPercursoIndividualCrianca',
descricao = 'Quantidade de Imagens permitiras no percurso individual da criança'
where id = 336;

update parametros_sistema set valor = 2 where id in (400, 401);