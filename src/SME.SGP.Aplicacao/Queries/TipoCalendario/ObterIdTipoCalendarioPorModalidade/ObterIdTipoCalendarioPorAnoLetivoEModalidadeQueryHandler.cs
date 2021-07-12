using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdTipoCalendarioPorAnoLetivoEModalidadeQueryHandler : IRequestHandler<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery, long>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterIdTipoCalendarioPorAnoLetivoEModalidadeQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<long> Handle(ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery request, CancellationToken cancellationToken)
        {
            ModalidadeTipoCalendario modalidade;

            switch (request.Modalidade)
            {
                case Modalidade.EducacaoInfantil:
                    modalidade = ModalidadeTipoCalendario.Infantil;
                    break;
                case Modalidade.EJA:
                    modalidade = ModalidadeTipoCalendario.EJA;
                    break;
                case Modalidade.Fundamental:
                case Modalidade.Medio:
                default:
                    modalidade = ModalidadeTipoCalendario.FundamentalMedio;
                    break;
            }

            return await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.AnoLetivo, modalidade, request.Semestre ?? 0);
        }
    }
}
