/**
 * @description Verifica se o objeto está inteiro vazio ou nulo
 * @param {Object} obj Objeto a ser validado
 */
const validaSeObjetoEhNuloOuVazio = obj => {
  return Object.values(obj).every(x => x === null || x === '');
};

/**
 * @description Verifica se o objeto está inteiro preenchido (todas as propriedades)
 * @param {Object} obj Objeto a ser validado
 */
const objetoEstaTodoPreenchido = obj => {
  return !Object.values(obj).some(x => x === null || x === '');
};

const valorNuloOuVazio = valor => {
  return valor === null || valor === '' || valor === undefined;
};

const stringNulaOuEmBranco = valor => {
  return valor ? valor.trim() === '' : true;
};

export {
  validaSeObjetoEhNuloOuVazio,
  valorNuloOuVazio,
  stringNulaOuEmBranco,
  objetoEstaTodoPreenchido,
};
