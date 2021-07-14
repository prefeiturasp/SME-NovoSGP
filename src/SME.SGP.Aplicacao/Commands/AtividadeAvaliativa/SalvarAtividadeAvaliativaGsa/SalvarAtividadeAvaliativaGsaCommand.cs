using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeAvaliativaGsaCommand : IRequest
    {
        public SalvarAtividadeAvaliativaGsaCommand(long aulaId, string usuarioRf, string turmaCodigo, long componenteCurricularId, string titulo, string descricao, DateTime dataCriacao, DateTime? dataAlteracao, long atividadeClassroomId)
        {
            AulaId = aulaId;
            UsuarioRf = usuarioRf;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
            Titulo = titulo;
            Descricao = descricao;
            DataCriacao = dataCriacao;
            DataAlteracao = dataAlteracao;
            AtividadeClassroomId = atividadeClassroomId;
        }

        public long AulaId { get; }
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
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informado para importação da atividade do GSA");

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
