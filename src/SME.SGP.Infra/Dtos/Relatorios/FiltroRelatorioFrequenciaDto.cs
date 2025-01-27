using FluentValidation;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioFrequenciaDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public TipoRelatorioFaltasFrequencia TipoRelatorio { get; set; }
        public IEnumerable<string> AnosEscolares { get; set; }
        public bool TurmasPrograma { get; set; }
        public List<string> CodigosTurma { get; set; }
        public IEnumerable<string> ComponentesCurriculares { get; set; }
        public List<int> Bimestres { get; set; }
        public CondicoesRelatorioFaltasFrequencia Condicao { get; set; }
        public int QuantidadeAusencia { get; set; }
        public TipoQuantidadeAusencia TipoQuantidadeAusencia { get; set; }
        public TipoFormatoRelatorio TipoFormatoRelatorio { get; set; }
        public string NomeUsuario { get; set; }
        public string CodigoRf { get; set; }        
    }


    public class FiltroRelatorioFaltasFrequenciaDtoValidator : AbstractValidator<FiltroRelatorioFrequenciaDto>
    {
        public FiltroRelatorioFaltasFrequenciaDtoValidator()
        {
            RuleFor(c => c.TipoRelatorio)
                .IsInEnum()
                .WithMessage("O tipo de relatório de faltas ou frequência deve ser informado.");

            RuleFor(c => c.Modalidade)
                .IsInEnum()
                .WithMessage("A modalidade deve ser informada.");

            RuleFor(c => c.Semestre)
                .NotEmpty()
                .WithMessage("Quando a modalidade é EJA o Semestre deve ser informado.")
                .When(c => c.Modalidade == Modalidade.EJA);

            RuleFor(c => c.Bimestres)
                .NotEmpty()
                .WithMessage("Os bimestres devem ser informados.")
                .When(c => c.Modalidade != Modalidade.EJA);            

            RuleFor(c => c.TipoFormatoRelatorio)
                .NotEmpty()
                .WithMessage("O formato deve ser informado.");
        }
    }

}
