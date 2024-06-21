using MediatR;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaCommandHandler : IRequestHandler<InserirOcorrenciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo;
        private readonly IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno;
        private readonly IRepositorioOcorrenciaServidor repositorioOcorrenciaServidor;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public InserirOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia, IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo,
            IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno,IRepositorioOcorrenciaServidor ocorrenciaServidor, 
            IMediator mediator, IUnitOfWork unitOfWork,IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaTipo));
            this.repositorioOcorrenciaAluno = repositorioOcorrenciaAluno ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaAluno));
            this.repositorioOcorrenciaServidor = ocorrenciaServidor ?? throw new ArgumentNullException(nameof(ocorrenciaServidor));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<AuditoriaDto> Handle(InserirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    if (request.DataOcorrencia > DateTimeExtension.HorarioBrasilia())
                        throw new NegocioException(MensagemNegocioOcorrencia.Data_da_ocorrencia_nao_pode_ser_futura);

                    var ocorrenciaTipo = await repositorioOcorrenciaTipo.ObterPorIdAsync(request.OcorrenciaTipoId);
                    if (ocorrenciaTipo is null)
                        throw new NegocioException("O tipo da ocorrência informado não foi encontrado.");

                    var ocorrencia = new Ocorrencia(request.DataOcorrencia,
                                                    request.HoraOcorrencia,
                                                    request.Titulo,
                                                    TratativaReplaceTempParaArquivoPorRegex(request.Descricao),
                                                    ocorrenciaTipo,
                                                    request.TurmaId,
                                                    request.UeId);

                    ocorrencia.Id = await repositorioOcorrencia.SalvarAsync(ocorrencia);

                    ocorrencia.AdiconarAlunos(request.CodigosAlunos);
                    foreach (var ocorrenciaAluno in ocorrencia.Alunos)
                    {
                        await repositorioOcorrenciaAluno.SalvarAsync(ocorrenciaAluno);
                    }
                    
                    ocorrencia.AdicionarServidores(request.CodigosServidores);
                    foreach (var ocorrenciaServidor in ocorrencia.Servidores)
                    {
                        await repositorioOcorrenciaServidor.SalvarAsync(ocorrenciaServidor);
                    }
                    
                    unitOfWork.PersistirTransacao();
                    await MoverArquivos(request);
                    return (AuditoriaDto)ocorrencia;
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
        private async Task MoverArquivos(InserirOcorrenciaCommand novo)
        {
            if (!string.IsNullOrEmpty(novo.Descricao))
            {
                await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.Ocorrencia, string.Empty, novo.Descricao));
            }
        }
        private string TratativaReplaceTempParaArquivoPorRegex(string descricao)
        {
            string pattern = $@"\b{configuracaoArmazenamentoOptions.Value.BucketTemp}\b";
            return Regex.Replace(descricao, pattern, configuracaoArmazenamentoOptions.Value.BucketArquivos);
        }
    }
}