export default class FiltroPlanoAnualExpandidoDto {
  constructor(
    anoLetivo,
    componenteCurricularEolId,
    escolaId,
    modalidadePlanoAnual,
    turmaId
  ) {
    this.anoLetivo = anoLetivo;
    this.componenteCurricularEolId = componenteCurricularEolId;
    this.escolaId = escolaId;
    this.modalidadePlanoAnual = modalidadePlanoAnual;
    this.turmaId = turmaId;
  }
}
