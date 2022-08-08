using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoDeClasseNotaBimestresCacheCommandHandler : AtualizadorDeCache<ConselhoClasseAlunoNotasConceitosRetornoDto>, IRequestHandler<SalvarConselhoDeClasseNotaBimestresCacheCommand, bool>
    {
        private SalvarConselhoDeClasseNotaBimestresCacheCommand request;

        public SalvarConselhoDeClasseNotaBimestresCacheCommandHandler(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Handle(SalvarConselhoDeClasseNotaBimestresCacheCommand request, CancellationToken cancellationToken)
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
            return await mediator.Send(new ObterConselhoDeClasseNotaBimestresCacheQuery(request.ConselhoClasseId, request.CodigoAluno, request.Bimestre));
        }
    }
}
