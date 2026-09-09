using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    [ExcludeFromCodeCoverage]
    public class SalvarFotoEstudanteUseCase : AbstractUseCase, ISalvarFotoEstudanteUseCase
    {
        public SalvarFotoEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<Guid> Executar(EstudanteFotoDto dto)
        {
            try
            {
                return await mediator.Send(new SalvarFotoEstudanteCommand(dto.File, dto.AlunoCodigo));
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"=========== Erro ao salvar foto do Aluno SalvarFotoEstudanteUseCase: {e.Message}, {e.StackTrace?.ToString()}, {e.InnerException} , {e}", LogNivel.Critico, LogContexto.Geral));
                throw;
            }
        }
    }
}
