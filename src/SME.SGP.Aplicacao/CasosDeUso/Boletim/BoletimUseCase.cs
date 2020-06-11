using MediatR;
using SME.SGP.Aplicacao.Commands.Relatorios.GerarRelatorio;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class BoletimUseCase : IBoletimUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioCicloEnsino repositorioCicloEnsino;
        private readonly IRepositorioTurma repositorioTurma;

        public BoletimUseCase(IMediator mediator,
                              IRepositorioUe repositorioUe,
                              IRepositorioCicloEnsino repositorioCicloEnsino,
                              IRepositorioTurma repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioCicloEnsino = repositorioCicloEnsino ?? throw new ArgumentNullException(nameof(repositorioCicloEnsino));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<bool> Executar(FiltroRelatorioBoletimDto filtroRelatorioBoletimDto)
        {
            if (repositorioUe.ObterPorCodigo(filtroRelatorioBoletimDto.CodigoUe) == null)
                throw new NegocioException("Não foi possível encontrar a Ue");

            if (filtroRelatorioBoletimDto.CodCicloEnsino.HasValue &&
                repositorioCicloEnsino.ObterPorId(filtroRelatorioBoletimDto.CodCicloEnsino.Value) == null)
                throw new NegocioException("Não foi possível encontrar o ciclo");

            if (!string.IsNullOrEmpty(filtroRelatorioBoletimDto.CodigoTurma) && 
                repositorioTurma.ObterPorCodigo(filtroRelatorioBoletimDto.CodigoTurma) == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Boletim, filtroRelatorioBoletimDto));
        }
    }
}
