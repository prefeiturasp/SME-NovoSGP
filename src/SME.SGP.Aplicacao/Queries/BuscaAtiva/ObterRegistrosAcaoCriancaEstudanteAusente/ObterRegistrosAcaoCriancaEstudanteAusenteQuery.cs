using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAcaoCriancaEstudanteAusenteQuery : IRequest<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>>
    {
        public ObterRegistrosAcaoCriancaEstudanteAusenteQuery(string codigoAluno, long turmaId)
        {
            CodigoAluno = codigoAluno;
            TurmaId = turmaId;
        }
        public string CodigoAluno { get; set; }
        public long TurmaId { get; set; }
        
    }

    public class ObterRegistrosAcaoCriancaEstudanteAusenteQueryValidator : AbstractValidator<ObterRegistrosAcaoCriancaEstudanteAusenteQuery>
    {
        public ObterRegistrosAcaoCriancaEstudanteAusenteQueryValidator()
        {
            RuleFor(c => c.CodigoAluno).NotEmpty().WithMessage("O código do aluno deve ser informado para pesquisa de Registros de Ação");
            RuleFor(c => c.TurmaId).NotEmpty().WithMessage("O identificador da turma deve ser informado para pesquisa de Registros de Ação");
        }
    }
}
