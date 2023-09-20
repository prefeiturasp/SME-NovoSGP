using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class BoletimEscolaAquiUseCase : IBoletimEscolaAquiUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDreConsulta repositorioDre;
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioUsuario repositorioUsuario;
        public BoletimEscolaAquiUseCase(IMediator mediator,
                              IRepositorioUeConsulta repositorioUe,
                              IRepositorioDreConsulta repositorioDre,
                              IRepositorioTurmaConsulta repositorioTurma,
                              IRepositorioUsuario repositorioUsuario)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }
        public async Task<bool> Executar(FiltroRelatorioEscolaAquiDto relatorioBoletimEscolaAquiDto)
        {
            int usuarioLogadoId = 1;
            if (repositorioDre.ObterPorCodigo(relatorioBoletimEscolaAquiDto.DreCodigo).EhNulo())
                throw new NegocioException("Não foi possível encontrar a DRE");

            if (repositorioUe.ObterPorCodigo(relatorioBoletimEscolaAquiDto.UeCodigo).EhNulo())
                throw new NegocioException("Não foi possível encontrar a UE");

            if (!string.IsNullOrEmpty(relatorioBoletimEscolaAquiDto.TurmaCodigo))
            {
                int codigoTurma;
                if (int.TryParse(relatorioBoletimEscolaAquiDto.TurmaCodigo, out codigoTurma) && codigoTurma <= 0)
                    relatorioBoletimEscolaAquiDto.TurmaCodigo = String.Empty;
                else if ((await repositorioTurma.ObterPorCodigo(relatorioBoletimEscolaAquiDto.TurmaCodigo)).EhNulo())
                    throw new NegocioException("Não foi possível encontrar a turma");
            }

            var usuarioLogado = repositorioUsuario.ObterPorId(usuarioLogadoId);

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.BoletimDetalhadoApp, relatorioBoletimEscolaAquiDto, usuarioLogado, RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosBoletimDetalhadoEscolaAqui, notificarErroUsuario: true));
        }
    }
}
