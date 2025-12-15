using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAvaliacoesBimestraisAlunoQueryHandler : IRequestHandler<ObterAvaliacoesBimestraisAlunoQuery, AvaliacoesBimestraisAlunoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAvaliacoesBimestraisAluno repositorio;

        public ObterAvaliacoesBimestraisAlunoQueryHandler(IMediator mediator, IRepositorioAvaliacoesBimestraisAluno repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AvaliacoesBimestraisAlunoDto> Handle(ObterAvaliacoesBimestraisAlunoQuery request, CancellationToken cancellationToken)
        {
            // Obter dados do aluno
            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(request.CodigoAluno, request.AnoLetivo));
            if (aluno == null)
                return null;

            // Obter turmas do aluno no ano letivo
            var turmasAluno = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(request.CodigoAluno, request.AnoLetivo, false, false));
            if (!turmasAluno.Any())
                return null;

            var avaliacoesBimestrais = new List<AvaliacaoBimestreDto>();
            AvaliacaoFinalDto avaliacaoFinal = null;
            bool possuiConselhoClasse = false;

            foreach (var turmaAluno in turmasAluno)
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaAluno.CodigoTurma.ToString()));
                if (turma == null) continue;

                // Verificar se é modalidade EF, Médio ou EJA regular
                if (!ValidarModalidadePermitida(turma.ModalidadeCodigo))
                    continue;

                // Verificar se possui conselho de classe gravado
                var conselhoClasse = await mediator.Send(new ObterConselhoClassePorTurmaEPeriodoQuery(turma.Id, null));

                if (possuiConselhoClasse)
                {
                    // Definir quantidade de períodos baseado na modalidade
                    var quantidadePeriodos = turma.ModalidadeCodigo == Modalidade.EJA ? 3 : 4; // EJA tem 3 trimestres, demais 4 bimestres
                    
                    // Obter avaliações por bimestre/trimestre
                    for (int periodo = 1; periodo <= quantidadePeriodos; periodo++)
                    {
                        var indicadoresPeriodo = await repositorio.ObterIndicadoresPorBimestre(request.CodigoAluno, turma.Id, turma.CodigoTurma, periodo, request.AnoLetivo);

                        avaliacoesBimestrais.Add(new AvaliacaoBimestreDto
                        {
                            Bimestre = periodo.ToString(),
                            Indicadores = indicadoresPeriodo
                        });
                    }

                    // Obter avaliação final (se houver)
                    var indicadoresFinais = await repositorio.ObterIndicadoresAvaliacaoFinal(request.CodigoAluno, turma.Id, turma.CodigoTurma, request.AnoLetivo);

                    if (indicadoresFinais.Any())
                    {
                        avaliacaoFinal = new AvaliacaoFinalDto
                        {
                            Indicadores = indicadoresFinais
                        };
                    }
                }

                break; // Processar apenas a primeira turma encontrada no critério
            }

            return new AvaliacoesBimestraisAlunoDto
            {
                CodigoAluno = request.CodigoAluno,
                NomeAluno = aluno.Nome,
                AvaliacoesBimestrais = avaliacoesBimestrais,
                AvaliacaoFinal = avaliacaoFinal,
                PossuiConselhoClasse = possuiConselhoClasse
            };
        }

        private bool ValidarModalidadePermitida(Modalidade modalidade)
        {
            return modalidade == Modalidade.Fundamental || 
                   modalidade == Modalidade.Medio || 
                   modalidade == Modalidade.EJA;
        }
    }
}