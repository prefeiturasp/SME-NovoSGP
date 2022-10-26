using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class BoletimUseCase : IBoletimUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public BoletimUseCase(IMediator mediator,
                              IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(FiltroRelatorioBoletimDto filtroRelatorioBoletimDto)
        {
            bool existeUe = await mediator
                .Send(new ValidaSeExisteUePorCodigoQuery(filtroRelatorioBoletimDto.UeCodigo));

            bool existeDre = await mediator
                .Send(new ValidaSeExisteDrePorCodigoQuery(filtroRelatorioBoletimDto.DreCodigo));

            if (!existeDre)
                throw new NegocioException("Não foi possível encontrar a DRE");

            if (!existeUe)
                throw new NegocioException("Não foi possível encontrar a UE");

            if (!string.IsNullOrEmpty(filtroRelatorioBoletimDto.TurmaCodigo))
            {
                int codigoTurma;
                if (int.TryParse(filtroRelatorioBoletimDto.TurmaCodigo, out codigoTurma) && codigoTurma <= 0)
                    filtroRelatorioBoletimDto.TurmaCodigo = String.Empty;
                else if (await mediator.Send(new ObterTurmaPorCodigoQuery(filtroRelatorioBoletimDto.TurmaCodigo)) == null)
                    throw new NegocioException("Não foi possível encontrar a turma");
            }

            unitOfWork.IniciarTransacao();

            var usuarioLogado = await mediator
                .Send(new ObterUsuarioLogadoQuery());

            filtroRelatorioBoletimDto.Usuario = usuarioLogado;

            if (filtroRelatorioBoletimDto.AlunosCodigo != null && !filtroRelatorioBoletimDto.AlunosCodigo.Any())
            {
                filtroRelatorioBoletimDto.AlunosCodigo = (await mediator
                    .Send(new ObterAlunosPorTurmaQuery(filtroRelatorioBoletimDto.TurmaCodigo, filtroRelatorioBoletimDto.ConsideraInativo)))
                    .Select(a => a.CodigoAluno)
                    .ToArray();
            }

            bool retorno;

            if (filtroRelatorioBoletimDto.Modelo == ModeloBoletim.Detalhado)
            {
                retorno = await mediator
                    .Send(new GerarRelatorioCommand(TipoRelatorio.BoletimDetalhado, filtroRelatorioBoletimDto, usuarioLogado, RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosBoletimDetalhado));
            }
            else
            {
                var rotaBoletim = !string.IsNullOrEmpty(filtroRelatorioBoletimDto.TurmaCodigo) ?
                    RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosBoletimTurma :
                    RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosBoletim;

                retorno = await mediator
                    .Send(new GerarRelatorioCommand(TipoRelatorio.Boletim, filtroRelatorioBoletimDto, usuarioLogado, rotaBoletim));
            }

            unitOfWork.PersistirTransacao();
            return retorno;
        }
    }
}
