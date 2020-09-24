using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesCJComponentesIdsPorTurmaRFQuery : IRequest<long[]>
    {
        public ObterAtribuicoesCJComponentesIdsPorTurmaRFQuery(long turmaCodigo, string usuarioRF)
        {
            TurmaCodigo = turmaCodigo;
            UsuarioRF = usuarioRF;
        }

        public long TurmaCodigo { get; set; }
        public string UsuarioRF { get; set; }
    }
}
