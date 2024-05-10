using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeAvaliativaGsaCommand : IRequest
    {
        public SalvarAtividadeAvaliativaGsaCommand(DateTime dataAula, AtividadeGsaDto atividadeGsa)
        {
            DataAula = dataAula;
            UsuarioRf = atividadeGsa.UsuarioRf;
            TurmaCodigo = atividadeGsa.TurmaId;
            ComponenteCurricularId = atividadeGsa.ComponenteCurricularId;
            Titulo = atividadeGsa.Titulo;
            Descricao = atividadeGsa.Descricao;
            DataCriacao = atividadeGsa.DataCriacao;
            DataAlteracao = atividadeGsa.DataAlteracao;
            AtividadeClassroomId = atividadeGsa.AtividadeClassroomId;
        }

        public DateTime DataAula { get; }
        public string UsuarioRf { get; }
        public string TurmaCodigo { get; }
        public long ComponenteCurricularId { get; }
        public string Titulo { get; }
        public string Descricao { get; }
        public DateTime DataCriacao { get; }
        public DateTime? DataAlteracao { get; }
        public long AtividadeClassroomId { get; }
    }

    public class SalvarAtividadeAvaliativaGsaCommandValidator : AbstractValidator<SalvarAtividadeAvaliativaGsaCommand>
    {
        public SalvarAtividadeAvaliativaGsaCommandValidator()
        {
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para importação da atividade do GSA");

            RuleFor(a => a.UsuarioRf)
                .NotEmpty()
                .WithMessage("O RF do usuário deve ser informado para importação da atividade do GSA");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para importação da atividade do GSA");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular da turma deve ser informado para importação da atividade do GSA");

            RuleFor(a => a.Titulo)
                .NotEmpty()
                .WithMessage("O título deve ser informado para importação da atividade do GSA");

            RuleFor(a => a.Descricao)
                .NotEmpty()
                .WithMessage("A descrição deve ser informada para importação da atividade do GSA");

            RuleFor(a => a.DataCriacao)
                .NotEmpty()
                .WithMessage("A data de criação deve ser informada para importação da atividade do GSA");

            RuleFor(a => a.AtividadeClassroomId)
                .NotEmpty()
                .WithMessage("O identificador da atividade no GSA deve ser informada para importação da atividade do GSA");
        }
    }
}
