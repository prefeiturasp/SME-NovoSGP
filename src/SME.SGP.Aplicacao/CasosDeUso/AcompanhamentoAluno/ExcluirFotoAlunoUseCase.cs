using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoAlunoUseCase : AbstractUseCase, IExcluirFotoAlunoUseCase
    {
        public ExcluirFotoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(Guid codigoFoto)
        {
            var foto = await mediator.Send(new ObterFotoSemestreAlunoPorCodigoQuery(codigoFoto));

            if (foto == null)
                throw new NegocioException("Código da foto não licalizado");

            return await mediator.Send(new ExcluirFotoAcompanhamentoAlunoCommand(foto));
        }
    }
}
