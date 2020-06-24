using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImpressaoConselhoClasseAlunoUseCase : IImpressaoConselhoClasseAlunoUseCase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public ImpressaoConselhoClasseAlunoUseCase(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(FiltroRelatorioConselhoClasseAlunoDto filtroRelatorioConselhoClasseAlunoDto)
        {

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario == null)
                throw new NegocioException("Não foi possível localizar o usuário.");

            filtroRelatorioConselhoClasseAlunoDto.Usuario = usuario;

            
            using (SentrySdk.Init(configuration.GetValue<string>("Sentry:DSN")))
            {
                SentrySdk.CaptureMessage("1 - ImpressaoConselhoClasseAlunoUseCase");                
            }

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ConselhoClasseAluno, filtroRelatorioConselhoClasseAlunoDto, usuario.Id));
        }
    }
}
