using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoNotaPorTurmaQueryHandler : IRequestHandler<ObterTipoNotaPorTurmaQuery, TipoNota>
    {
        private readonly IServicoEol servicoEol;
        private readonly IRepositorioNotaTipoValor repositorioNotaTipoValor;
        private readonly IRepositorioCiclo repositorioCiclo;

        public ObterTipoNotaPorTurmaQueryHandler(IServicoEol  servicoEol, IRepositorioNotaTipoValor repositorioNotaTipoValor, IRepositorioCiclo repositorioCiclo)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public async Task<TipoNota> Handle(ObterTipoNotaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            //TODO: TIPO DE TURMA NÃO EXISTE NO SGP, É NECESSÁRIO SEMPRE CONSULTAR O EOL.....
            var turmaEOL = await servicoEol
                .ObterDadosTurmaPorCodigo(request.Turma.CodigoTurma);

            // Para turma tipo 2 o padrão é nota.
            if (turmaEOL.TipoTurma == TipoTurma.EdFisica)
                return TipoNota.Nota;

            var anoCicloModalidade = string.Empty;
            if (request.Turma != null)
                anoCicloModalidade = request.Turma.Ano == AnoCiclo.Alfabetizacao.Name() ? AnoCiclo.Alfabetizacao.Description() : request.Turma.Ano;            

            var ciclo = repositorioCiclo
                .ObterCicloPorAnoModalidade(anoCicloModalidade, request.Turma.ModalidadeCodigo);

            if (ciclo == null)
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");

            return repositorioNotaTipoValor
                .ObterPorCicloIdDataAvalicacao(ciclo.Id, request.DataReferencia)?.TipoNota ?? TipoNota.Nota;            
        }
    }
}
