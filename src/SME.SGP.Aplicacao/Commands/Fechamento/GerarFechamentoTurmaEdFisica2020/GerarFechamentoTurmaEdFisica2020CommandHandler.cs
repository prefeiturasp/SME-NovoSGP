using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarFechamentoTurmaEdFisica2020CommandHandler : IRequestHandler<GerarFechamentoTurmaEdFisica2020Command, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        
        public GerarFechamentoTurmaEdFisica2020CommandHandler(IMediator mediator, IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                                              IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                              IRepositorioFechamentoAluno repositorioFechamentoAluno,
                                                              IRepositorioFechamentoNota repositorioFechamentoNota)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }
        public async Task<bool> Handle(GerarFechamentoTurmaEdFisica2020Command request, CancellationToken cancellationToken)
        {
            var verificaFechamento = await mediator.Send(new ObterFechamentoTurmaPorTurmaIdQuery(request.TurmaId));

            long fechamentoTurmaId = 0;
            long fechamentoTurmaDisciplinaId = 0;

            if (verificaFechamento == null)
            {
                var fechamentoTurma = new FechamentoTurma()
                {
                    TurmaId = request.TurmaId
                };

                fechamentoTurmaId = await repositorioFechamentoTurma.SalvarAsync(fechamentoTurma);
            }
            else
                fechamentoTurmaId = verificaFechamento.Id;

            var verificaFechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdQuery(request.TurmaId));
            

            if (!verificaFechamentoTurmaDisciplina.Any(f=> f.DisciplinaId == MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA))   
            {
                var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
                {
                    DisciplinaId = MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA,
                    Situacao = SituacaoFechamento.ProcessadoComSucesso,
                    FechamentoTurmaId = fechamentoTurmaId
                };

                fechamentoTurmaDisciplinaId = await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);
           
                foreach (var aluno in request.CodigoAlunos)
                {
                    var fechamentoAluno = new FechamentoAluno()
                    {
                        FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId,
                        AlunoCodigo = aluno.ToString()
                    };

                    long fechamentoAlunoId = await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);

                    var fechamentoAlunoNota = new FechamentoNota()
                    {
                        FechamentoAlunoId = fechamentoAlunoId,
                        DisciplinaId = MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA,
                        Nota = 5
                    };

                    try
                    {
                        await repositorioFechamentoNota.SalvarAsync(fechamentoAlunoNota);
                    }
                    catch (Exception ex)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao salvar fechamento de aluno - Turma Ed. Física 2020 - Aluno: {aluno} / Turma: {request.TurmaId}", LogNivel.Critico, LogContexto.Fechamento));
                        return false;
                    }

                }
            }
            return true;
        }
    }
}
