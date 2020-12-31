using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarOcorrenciaCommandHandler : IRequestHandler<AlterarOcorrenciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;

        public AlterarOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
        }

        public async Task<AuditoriaDto> Handle(AlterarOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            var ocorrencia = await repositorioOcorrencia.ObterPorIdAsync(request.Id);
            if (ocorrencia == null)
                throw new NegocioException($"Ocorrencia {request.Id} não encontrada!");

            MapearAlteracoes(ocorrencia, request);

            await repositorioOcorrencia.SalvarAsync(ocorrencia);

            return (AuditoriaDto)ocorrencia;
        }

        private void MapearAlteracoes(Ocorrencia entidade, AlterarOcorrenciaCommand request)
        {
            entidade.DataOcorrencia = request.DataOcorrencia;
            entidade.HoraOcorrencia = TimeSpan.Parse(request.HoraOcorrencia);
            entidade.Titulo = request.Titulo;
            entidade.Descricao = request.Descricao;
            entidade.OcorrenciaTipoId = request.OcorrenciaTipoId;
        }
    }
}

