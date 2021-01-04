using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaCommandHandler : IRequestHandler<InserirOcorrenciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo;
        private readonly IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno;
        private readonly IUnitOfWork unitOfWork;

        public InserirOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia, IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo,
            IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno, IUnitOfWork unitOfWork)
        {
            this.repositorioOcorrencia = repositorioOcorrencia;
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo;
            this.repositorioOcorrenciaAluno = repositorioOcorrenciaAluno;
            this.unitOfWork = unitOfWork;
        }

        public async Task<AuditoriaDto> Handle(InserirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var ocorrenciaTipo = await repositorioOcorrenciaTipo.ObterPorIdAsync(request.OcorrenciaTipoId);
                if (ocorrenciaTipo is null)
                    throw new NegocioException("O tipo da ocorrência informado não foi encontrado.");

                var ocorrencia = new Ocorrencia(request.DataOcorrencia, request.HoraOcorrencia, request.Titulo, request.Descricao, ocorrenciaTipo);
                ocorrencia.Id = await repositorioOcorrencia.SalvarAsync(ocorrencia);

                ocorrencia.AdiconarAlunos(request.CodigosAlunos);
                foreach(var ocorrenciaAluno in ocorrencia.Alunos)
                {
                    await repositorioOcorrenciaAluno.SalvarAsync(ocorrenciaAluno);
                }

                unitOfWork.PersistirTransacao();
                return (AuditoriaDto)ocorrencia;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
