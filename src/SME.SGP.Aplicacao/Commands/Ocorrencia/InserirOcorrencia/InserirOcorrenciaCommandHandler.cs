using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IUnitOfWork unitOfWork;

        public InserirOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia, IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo,
            IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno, IRepositorioTurma repositorioTurma, IUnitOfWork unitOfWork)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.repositorioOcorrenciaAluno = repositorioOcorrenciaAluno ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
        }

        public async Task<AuditoriaDto> Handle(InserirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var turma = await repositorioTurma.ObterPorId(request.TurmaId);
                if(turma is null)
                    throw new NegocioException("A turma da ocorrência informada não foi encontrada.");

                var ocorrenciaTipo = await repositorioOcorrenciaTipo.ObterPorIdAsync(request.OcorrenciaTipoId);
                if (ocorrenciaTipo is null)
                    throw new NegocioException("O tipo da ocorrência informado não foi encontrado.");

                var ocorrencia = new Ocorrencia(request.DataOcorrencia, request.HoraOcorrencia, request.Titulo, request.Descricao, ocorrenciaTipo, turma);
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
