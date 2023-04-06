using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class AlterarCompensacaoAusenciaAlunoEAulaCommand  : IRequest<bool>
    {
        public AlterarCompensacaoAusenciaAlunoEAulaCommand(IEnumerable<long> registroFrequenciaAlunoIds, int qtdeFaltasCompensadas)
        {
            RegistroFrequenciaAlunoIds = registroFrequenciaAlunoIds;
            QtdeFaltasCompensadas = qtdeFaltasCompensadas;
        }

        public IEnumerable<long> RegistroFrequenciaAlunoIds { get; set; }
        public int QtdeFaltasCompensadas { get; set; }
    }
        
    public class AlterarCompensacaoAusenciaAlunoEAulaCommandValidator: AbstractValidator<AlterarCompensacaoAusenciaAlunoEAulaCommand>
    {
        public AlterarCompensacaoAusenciaAlunoEAulaCommandValidator()
        {
            RuleFor(a => a.RegistroFrequenciaAlunoIds)
                .NotEmpty()
                .WithMessage("Os Ids de registro frequencia aluno devem ser informados para a alteração de compensação de ausência aluno e aula.");

            RuleFor(a => a.QtdeFaltasCompensadas)
                .NotEmpty()
                .WithMessage("A quantidade de compenações devem ser informadas para a alteração de compensação de ausência aluno e aula.");
        }
    }
}
