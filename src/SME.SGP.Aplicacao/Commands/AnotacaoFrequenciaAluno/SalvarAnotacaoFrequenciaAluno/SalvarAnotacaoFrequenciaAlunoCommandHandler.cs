using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAnotacaoFrequenciaAlunoCommandHandler : IRequestHandler<SalvarAnotacaoFrequenciaAlunoCommand, AuditoriaDto>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;
        private readonly IRepositorioMotivoAusencia repositorioMotivoAusencia;

        public SalvarAnotacaoFrequenciaAlunoCommandHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno, IRepositorioMotivoAusencia repositorioMotivoAusencia)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new System.ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
            this.repositorioMotivoAusencia = repositorioMotivoAusencia ?? throw new System.ArgumentNullException(nameof(repositorioMotivoAusencia));
        }
        public async Task<AuditoriaDto> Handle(SalvarAnotacaoFrequenciaAlunoCommand request, CancellationToken cancellationToken)
        {
            var anotacao = new AnotacaoFrequenciaAluno(request.AulaId, request.Anotacao, request.CodigoAluno, request.MotivoAusenciaId);
            if (request.MotivoAusenciaId.HasValue)
            {
                var motivoAusencia = await repositorioMotivoAusencia.ObterPorIdAsync(request.MotivoAusenciaId.Value);
                if (motivoAusencia == null)
                {
                    throw new NegocioException("O motivo de ausência informado não foi localizado.");
                }
            }

            await repositorioAnotacaoFrequenciaAluno.SalvarAsync(anotacao);
            return (AuditoriaDto)anotacao;
        }
    }
}
