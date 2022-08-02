using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao.Cache
{
    public class ConselhoDeClasseNotaPosConselhoBimestresCache : ServicoDeGerenciamentoDeCache<ConselhoClasseAlunoNotasConceitosRetornoDto>
    {
        private GravarConselhoClasseCommad request;

        public ConselhoDeClasseNotaPosConselhoBimestresCache(GravarConselhoClasseCommad request,IMediator mediator):base(mediator)
        {
            this.request = request;
        }

        protected override string ObtenhaChave()
        {
            return $"NotaConceitoBimestre-{request.ConselhoClasseId}-${request.CodigoAluno}-{request.Bimestre}";
        }

        protected override async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> ObtenhaObjetoAtualizado()
        {
            var retorno = await ObtenhaObjeto();

            if (retorno != null)
            {
                var conselhoClasseComponenteFrequenciaDtos = retorno!.NotasConceitos.FirstOrDefault()!.ComponentesCurriculares.FirstOrDefault(x => x.CodigoComponenteCurricular == request.ConselhoClasseNotaDto.CodigoComponenteCurricular);
                conselhoClasseComponenteFrequenciaDtos!.NotaPosConselho.Nota = request.ConselhoClasseNotaDto.Conceito ?? request.ConselhoClasseNotaDto.Nota;

                return retorno;
            }

            return null;
        }
    }
}
