using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarOcorrenciaCommandHandler : IRequestHandler<AlterarOcorrenciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo;
        private readonly IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public AlterarOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia, IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo,
            IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaTipo)); ;
            this.repositorioOcorrenciaAluno = repositorioOcorrenciaAluno ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaAluno)); ;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(AlterarOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var ocorrencia = await repositorioOcorrencia.ObterPorIdAsync(request.Id);
                    if (ocorrencia == null)
                        throw new NegocioException($"Ocorrencia {request.Id} não encontrada!");

                    var ocorrenciaTipo = await repositorioOcorrenciaTipo.ObterPorIdAsync(request.OcorrenciaTipoId);
                    if (ocorrenciaTipo is null)
                        throw new NegocioException("O tipo da ocorrência informado não foi encontrado.");

                    var descricaoAtual = ocorrencia.Descricao;
                    MapearAlteracoes(ocorrencia, request, ocorrenciaTipo);
                    await repositorioOcorrencia.SalvarAsync(ocorrencia);

                    var alunosParaSeremDeletados = ocorrencia.Alunos.Where(x => !request.CodigosAlunos.Contains(x.CodigoAluno)).Select(x => x.Id);
                    await repositorioOcorrenciaAluno.ExcluirAsync(alunosParaSeremDeletados);

                    var novosAlunos = request.CodigosAlunos.Where(x => !ocorrencia.Alunos.Any(y => y.CodigoAluno == x)).ToList();
                    ocorrencia.AdiconarAlunos(novosAlunos);
                    foreach (var novoAluno in novosAlunos)
                    {
                        var ocorrenciaAluno = ocorrencia.Alunos.FirstOrDefault(x => x.CodigoAluno == novoAluno);
                        await repositorioOcorrenciaAluno.SalvarAsync(ocorrenciaAluno);
                    }

                    unitOfWork.PersistirTransacao();
                    MoverRemoverExcluidos(request.Descricao, descricaoAtual);
                    return (AuditoriaDto)ocorrencia;
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
        private void MoverRemoverExcluidos(string novo, string atual)
        {
            if (!string.IsNullOrEmpty(novo))
            {
                var moverArquivo = mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.Ocorrencia, atual, novo));
            }
            if (!string.IsNullOrEmpty(atual))
            {
                var deletarArquivosNaoUtilziados = mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo, TipoArquivo.Ocorrencia.Name()));
            }
        }
        private void MapearAlteracoes(Ocorrencia entidade, AlterarOcorrenciaCommand request, OcorrenciaTipo ocorrenciaTipo)
        {
            entidade.DataOcorrencia = request.DataOcorrencia;
            entidade.SetHoraOcorrencia(request.HoraOcorrencia);
            entidade.Titulo = request.Titulo;
            entidade.Descricao = request.Descricao.Replace(ArquivoContants.PastaTemporaria, $"/{Path.Combine(TipoArquivo.Ocorrencia.Name(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString())}/");
            entidade.SetOcorrenciaTipo(ocorrenciaTipo);
        }
    }
}

