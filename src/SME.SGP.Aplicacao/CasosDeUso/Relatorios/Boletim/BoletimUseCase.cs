using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class BoletimUseCase : IBoletimUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private const int DOIS_BOLETIM = 2;

        public BoletimUseCase(IMediator mediator,
                              IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(FiltroRelatorioBoletimDto filtroRelatorioBoletimDto)
        {
            if (filtroRelatorioBoletimDto.QuantidadeBoletimPorPagina <= 0)
                throw new NegocioException(MensagemNegocioBoletim.QUANTIDADE_BOLETIM_POR_PAGINAS);
            
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

            var usuarioLogado = await mediator
                .Send(ObterUsuarioLogadoQuery.Instance);

            filtroRelatorioBoletimDto.Usuario = usuarioLogado;

            if (filtroRelatorioBoletimDto.AlunosCodigo != null && !filtroRelatorioBoletimDto.AlunosCodigo.Any())
            {
                filtroRelatorioBoletimDto.AlunosCodigo = (await mediator
                    .Send(new ObterAlunosPorTurmaQuery(filtroRelatorioBoletimDto.TurmaCodigo, filtroRelatorioBoletimDto.ConsideraInativo)))
                    .Select(a => a.CodigoAluno)
                    .ToArray();
            }

            bool retorno;

            if (filtroRelatorioBoletimDto.QuantidadeBoletimPorPagina <= DOIS_BOLETIM)
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

            return retorno;
        }
    }
}
