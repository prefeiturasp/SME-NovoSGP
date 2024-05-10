using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterTurmaItinerarioEnsinoMedioQueryHandlerFake : IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>
    {
        private const string INVESTIGACAO_CIENTIFICA = "Investigação cientifica";
        private const string HISTORIA_DAS_TRANSFORMACOES_DO_TRABALHO = "História das transformações do trabalho";
        private const string CIDADANIA_LUTAS_RESISTENCIAS_E_CONQUISTAS = "Cidadania, lutas, resistências e conquistas";
        private const string O_HUMANO_E_O_SOCIAL_ATRAVES_DAS_LINGUAGENS = "O humano e o social através das linguagens";

        public ObterTurmaItinerarioEnsinoMedioQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<TurmaItinerarioEnsinoMedioDto>> Handle(ObterTurmaItinerarioEnsinoMedioQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<TurmaItinerarioEnsinoMedioDto>()
          {
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 9,
              Nome = INVESTIGACAO_CIENTIFICA,
              Serie = 2
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 10,
              Nome = HISTORIA_DAS_TRANSFORMACOES_DO_TRABALHO,
              Serie = 2
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 11,
              Nome = HISTORIA_DAS_TRANSFORMACOES_DO_TRABALHO,
              Serie = 3
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 12,
              Nome = CIDADANIA_LUTAS_RESISTENCIAS_E_CONQUISTAS,
              Serie = 2
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 13,
              Nome = CIDADANIA_LUTAS_RESISTENCIAS_E_CONQUISTAS,
              Serie = 3
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 14,
              Nome = INVESTIGACAO_CIENTIFICA,
              Serie = 3
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 15,
              Nome = INVESTIGACAO_CIENTIFICA,
              Serie = 2
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 16,
              Nome = O_HUMANO_E_O_SOCIAL_ATRAVES_DAS_LINGUAGENS,
              Serie = 2
            },
            new TurmaItinerarioEnsinoMedioDto()
            {
              Id = 17,
              Nome = O_HUMANO_E_O_SOCIAL_ATRAVES_DAS_LINGUAGENS,
              Serie = 3
            },
          });
        }

    }
}