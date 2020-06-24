using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class BoletimUseCase : IBoletimUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioCicloEnsino repositorioCicloEnsino;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTurma repositorioTurma;

        public BoletimUseCase(IMediator mediator,
                              IUnitOfWork unitOfWork,
                              IRepositorioUe repositorioUe,
                               IRepositorioDre repositorioDre,
                               IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                              IRepositorioCicloEnsino repositorioCicloEnsino,
                              IRepositorioTurma repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioCicloEnsino = repositorioCicloEnsino ?? throw new ArgumentNullException(nameof(repositorioCicloEnsino));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<bool> Executar(FiltroRelatorioBoletimDto filtroRelatorioBoletimDto)
        {
            if (repositorioDre.ObterPorCodigo(filtroRelatorioBoletimDto.DreCodigo) == null)
                throw new NegocioException("Não foi possível encontrar a DRE");

            if (repositorioUe.ObterPorCodigo(filtroRelatorioBoletimDto.UeCodigo) == null)
                throw new NegocioException("Não foi possível encontrar a UE");

            if (!string.IsNullOrEmpty(filtroRelatorioBoletimDto.TurmaCodigo))
            {
                int codigoTurma;
                if (int.TryParse(filtroRelatorioBoletimDto.TurmaCodigo, out codigoTurma) && codigoTurma <= 0)
                    filtroRelatorioBoletimDto.TurmaCodigo = String.Empty;
                else if (await repositorioTurma.ObterPorCodigo(filtroRelatorioBoletimDto.TurmaCodigo) == null)
                    throw new NegocioException("Não foi possível encontrar a turma");
            }


            unitOfWork.IniciarTransacao();
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtroRelatorioBoletimDto.Usuario = usuarioLogado;
            var retorno = await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Boletim, filtroRelatorioBoletimDto, usuarioLogado.Id));
            unitOfWork.PersistirTransacao();
            return retorno;
        }
    }
}
