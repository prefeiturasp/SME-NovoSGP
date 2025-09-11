using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioFrequenciaUseCase : IGerarRelatorioFrequenciaUseCase
    {
        private readonly IMediator mediator;

        public GerarRelatorioFrequenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(FiltroRelatorioFrequenciaDto filtro)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuario.EhNulo())
                throw new NegocioException("Não foi possível localizar o usuário.");

            if (filtro.CodigoDre == "-99")
                throw new NegocioException("Não é possível gerar esse relatório para todas as DREs");

            filtro.NomeUsuario = usuario.Nome;
            filtro.CodigoRf = usuario.CodigoRf;


            if (filtro.Bimestres.Any(c => c == -99))
            {
                filtro.Bimestres = new List<int>();
                switch (filtro.Modalidade)
                {
                    case Modalidade.Fundamental:
                    case Modalidade.EducacaoInfantil:
                    case Modalidade.Medio:
                        filtro.Bimestres.AddRange(new int[] { 0, 1, 2, 3, 4});
                        break;
                    case Modalidade.EJA:
                    case Modalidade.CELP:
                        filtro.Bimestres.AddRange(new int[] { 0, 1, 2});
                        break;
                }
            }

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Frequencia, filtro, usuario, formato:filtro.TipoFormatoRelatorio, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequencia));
        }
    }
}
