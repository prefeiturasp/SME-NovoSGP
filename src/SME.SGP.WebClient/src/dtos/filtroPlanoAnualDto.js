export default class filtroPlanoAnualDto {
  constructor(
    anoLetivo,
    bimestre,
    escolaId,
    turmaId,
    ComponenteCurricularEolId
  ) {
    this.anoLetivo = anoLetivo;
    this.bimestre = bimestre;
    this.escolaId = escolaId;
    this.turmaId = turmaId;
    this.ComponenteCurricularEolId = ComponenteCurricularEolId;
  }
}
