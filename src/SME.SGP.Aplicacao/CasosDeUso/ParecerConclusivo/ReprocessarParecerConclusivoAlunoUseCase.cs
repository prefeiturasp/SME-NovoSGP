using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarParecerConclusivoAlunoUseCase : AbstractUseCase, IReprocessarParecerConclusivoAlunoUseCase
    {
        private readonly IServicoConselhoClasse servicoConselhoClasse;

        public ReprocessarParecerConclusivoAlunoUseCase(IMediator mediator, IServicoConselhoClasse servicoConselhoClasse) : base(mediator)
        {
            this.servicoConselhoClasse = servicoConselhoClasse ?? throw new ArgumentNullException(nameof(servicoConselhoClasse));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registro = param.ObterObjetoMensagem<ConselhoClasseFechamentoAlunoDto>();
            await servicoConselhoClasse.GerarParecerConclusivoAlunoAsync(registro.ConselhoClasseId, registro.FechamentoTurmaId, registro.AlunoCodigo);

            return true;
        }
    }
}
