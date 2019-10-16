export default class DisciplinaDTO {
  constructor(codigo, nome, possuiObjetivos, regencia) {
    this.codigo = codigo;
    this.nome = nome;
    this.possuiObjetivos = possuiObjetivos;
    this.regencia = regencia;
    this.selecionada = false;
  }
}
