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
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUsuario repositorioUsuario;
        public BoletimEscolaAquiUseCase(IMediator mediator,
                              IUnitOfWork unitOfWork,
                              IRepositorioUe repositorioUe,
                              IRepositorioDre repositorioDre,
                              IRepositorioTurma repositorioTurma,
                              IRepositorioUsuario repositorioUsuario)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }
        public async Task<bool> Executar(FiltroRelatorioBoletimEscolaAquiDto relatorioBoletimEscolaAquiDto)
        {
            int usuarioLogadoId = 1;
            if (repositorioDre.ObterPorCodigo(relatorioBoletimEscolaAquiDto.DreCodigo) == null)
                throw new NegocioException("Não foi possível encontrar a DRE");

            if (repositorioUe.ObterPorCodigo(relatorioBoletimEscolaAquiDto.UeCodigo) == null)
                throw new NegocioException("Não foi possível encontrar a UE");

            if (!string.IsNullOrEmpty(relatorioBoletimEscolaAquiDto.TurmaCodigo))
            {
                int codigoTurma;
                if (int.TryParse(relatorioBoletimEscolaAquiDto.TurmaCodigo, out codigoTurma) && codigoTurma <= 0)
                    relatorioBoletimEscolaAquiDto.TurmaCodigo = String.Empty;
                else if (await repositorioTurma.ObterPorCodigo(relatorioBoletimEscolaAquiDto.TurmaCodigo) == null)
                    throw new NegocioException("Não foi possível encontrar a turma");
            }

            unitOfWork.IniciarTransacao();
            var usuarioLogado = repositorioUsuario.ObterPorId(usuarioLogadoId);
            bool retorno;

            if (relatorioBoletimEscolaAquiDto.Modelo == (int)ModeloBoletim.Detalhado)
                retorno = await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.BoletimDetalhadoApp, relatorioBoletimEscolaAquiDto, usuarioLogado, RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosBoletimDetalhadoEscolaAqui, notificarErroUsuario: true));
            else
                retorno = await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Boletim, relatorioBoletimEscolaAquiDto, usuarioLogado, RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosBoletimEscolaAqui));

            unitOfWork.PersistirTransacao();
            return retorno;
        }
    }
}
