import ExcecaoUsuario from './ExcecaoUsuario';

export default class RFNãoEncontradoExcecao extends ExcecaoUsuario {
  constructor() {
    super('RF não encontrado', 1, 'Registro funcional não foi encontrado!');
  }
}
