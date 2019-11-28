/**
 * @description Verifica se o objeto está inteiro vazio ou nulo
 * @param {Object} obj Objeto a ser validado
 */
const validaSeObjetoEhNuloOuVazio = obj => {
  return Object.values(obj).every(x => x === null || x === '');
};

const valorNuloOuVazio = valor => {
  return valor === null || valor === '';
};

const stringNulaOuEmBranco = valor => {
  return valor? valor.trim() === '': true;
};

export { validaSeObjetoEhNuloOuVazio, valorNuloOuVazio, stringNulaOuEmBranco };
