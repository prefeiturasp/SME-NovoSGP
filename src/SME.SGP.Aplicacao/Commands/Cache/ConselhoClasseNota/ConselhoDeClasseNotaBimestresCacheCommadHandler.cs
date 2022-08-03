using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConselhoDeClasseNotaBimestresCacheCommadHandler : AtualizadorDeCache<ConselhoClasseAlunoNotasConceitosRetornoDto>, IRequestHandler<ConselhoDeClasseNotaBimestresCacheCommad, bool>
    {
        private ConselhoDeClasseNotaBimestresCacheCommad request;

        public ConselhoDeClasseNotaBimestresCacheCommadHandler(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Handle(ConselhoDeClasseNotaBimestresCacheCommad request, CancellationToken cancellationToken)
        {
            this.request = request;

            return await SalvaNoCache();
        }

        protected override ConselhoClasseAlunoNotasConceitosRetornoDto ObtenhaObjetoAtualizado(ConselhoClasseAlunoNotasConceitosRetornoDto objeto)
        {
            var conselhoClasseComponenteFrequenciaDtos = objeto!.NotasConceitos.FirstOrDefault()!.ComponentesCurriculares.FirstOrDefault(x => x.CodigoComponenteCurricular == request.ConselhoClasseNotaDto.CodigoComponenteCurricular);
                
            conselhoClasseComponenteFrequenciaDtos!.NotaPosConselho.Nota = request.ConselhoClasseNotaDto.Conceito ?? request.ConselhoClasseNotaDto.Nota;

            return objeto;
        }

        protected override async Task<ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>> ObtenhaValorCache()
        {
            return await mediator.Send(new ConselhoDeClasseNotaBimestresCacheQuery(request.ConselhoClasseId, request.CodigoAluno, request.Bimestre));
        }
    }
}
