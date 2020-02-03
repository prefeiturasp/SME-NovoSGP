namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamento
    {
        void RealizarFechamento(string codigoTurma, string disciplinaId, long periodoEscolarId);
    }
}