﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase : AbstractUseCase, IObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase
    {
        public ObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ParametroQuantidadeUploadImagemDto> Executar(int param)
        {
            var parametroQuantidadeImagemPercursoColetivo = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.QuantidadeImagensPercursoTurma, param));
            var parametroQuantidadeImagemPercursoIndividual = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.QuantidadeImagensPercursoIndividualCrianca, param));

            var parametroQuantidadeUploadImagem = new ParametroQuantidadeUploadImagemDto();
            
            parametroQuantidadeUploadImagem.AdicionarValorQuantidadeImagemPercursoColetivo(parametroQuantidadeImagemPercursoColetivo?.Valor);
            parametroQuantidadeUploadImagem.AdicionarValorQuantidadeImagemPercursoIndividual(parametroQuantidadeImagemPercursoIndividual?.Valor);
        

            return parametroQuantidadeUploadImagem;
        }
    }
}
