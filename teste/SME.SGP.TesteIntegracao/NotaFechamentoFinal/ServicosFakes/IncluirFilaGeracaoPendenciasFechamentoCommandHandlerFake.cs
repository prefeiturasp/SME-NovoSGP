using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes
{
    public class IncluirFilaGeracaoPendenciasFechamentoCommandHandlerFake: IRequestHandler<IncluirFilaGeracaoPendenciasFechamentoCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaGeracaoPendenciasFechamentoCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaGeracaoPendenciasFechamentoCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new GerarPendenciasFechamentoCommand(request.ComponenteCurricularId
                , request.TurmaCodigo
                , request.TurmaNome
                , request.PeriodoEscolarInicio
                , request.PeriodoEscolarFim
                , request.Bimestre
                , request.Usuario.Id
                , request.Usuario.CodigoRf
                , request.FechamentoTurmaDisciplinaId
                , request.Justificativa
                , request.CriadoRF
                , request.TurmaId
                , request.ComponenteSemNota
                , request.RegistraFrequencia));
            
            return true;
        }
    }
}
