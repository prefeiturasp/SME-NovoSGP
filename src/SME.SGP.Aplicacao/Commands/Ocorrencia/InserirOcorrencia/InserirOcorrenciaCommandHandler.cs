using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaCommandHandler : IRequestHandler<InserirOcorrenciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo;
        private readonly IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public InserirOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia, IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo,
            IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno, IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaTipo));
            this.repositorioOcorrenciaAluno = repositorioOcorrenciaAluno ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> Handle(InserirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
                    if (turma is null)
                        throw new NegocioException("A turma da ocorrência informada não foi encontrada.");

                    var ocorrenciaTipo = await repositorioOcorrenciaTipo.ObterPorIdAsync(request.OcorrenciaTipoId);
                    if (ocorrenciaTipo is null)
                        throw new NegocioException("O tipo da ocorrência informado não foi encontrado.");

                    var ocorrencia = new Ocorrencia(request.DataOcorrencia, 
                                                    request.HoraOcorrencia,
                                                    request.Titulo,
                                                    await MoverArquivos(request.Descricao),
                                                    ocorrenciaTipo,
                                                    turma);
                    ocorrencia.Id = await repositorioOcorrencia.SalvarAsync(ocorrencia);

                    ocorrencia.AdiconarAlunos(request.CodigosAlunos);
                    foreach (var ocorrenciaAluno in ocorrencia.Alunos)
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
        private async Task<string> MoverArquivos(string descricao)
        {
            var caminho = string.Empty;

            if (!string.IsNullOrEmpty(descricao))
                caminho = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.Ocorrencia, string.Empty, descricao));

            return caminho;
        }
    }
}