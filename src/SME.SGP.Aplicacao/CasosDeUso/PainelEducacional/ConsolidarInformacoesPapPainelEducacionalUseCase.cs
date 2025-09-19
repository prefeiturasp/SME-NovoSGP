using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesPapPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesPapPainelEducacionalUseCase
    {
        public ConsolidarInformacoesPapPainelEducacionalUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var indicadoresPap = await mediator.Send(new ObterIndicadoresPapQuery());

            if (indicadoresPap?.Any() != true)
                return false;

            var entidades = indicadoresPap.Select(MapearParaEntidade).ToList();

            return await mediator.Send(new SalvarConsolidacaoPapCommand(entidades));
        }
        private ConsolidacaoInformacoesPap MapearParaEntidade(PainelEducacionalInformacoesPapDto dto)
        {
            return new ConsolidacaoInformacoesPap(
                id: 0,
                tipoPap: dto.TipoPap,
                dreCodigo: dto.DreCodigo,
                ueCodigo: dto.UeCodigo,
                dreNome: dto.DreNome,
                ueNome: dto.UeNome,
                quantidadeTurmas: dto.QuantidadeTurmas,
                quantidadeEstudantes: dto.QuantidadeEstudantes,
                quantidadeEstudantesComFrequenciaInferiorLimite: dto.QuantidadeEstudantesComFrequenciaInferiorLimite,
                dificuldadeAprendizagemTop1: dto.QuantidadeEstudantesDificuldadeTop1,
                dificuldadeAprendizagemTop2: dto.QuantidadeEstudantesDificuldadeTop2,
                outrasDificuldadesAprendizagem: dto.OutrasDificuldadesAprendizagem,
                nomeDificuldadeTop1: dto.NomeDificuldadeTop1,
                nomeDificuldadeTop2: dto.NomeDificuldadeTop2
            );
        }

    }
}
