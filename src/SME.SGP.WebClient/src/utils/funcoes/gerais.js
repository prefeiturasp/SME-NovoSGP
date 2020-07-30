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

const ordenarPor = (lista, propriedade) => {
  return lista.sort((a, b) => {
    if (a[propriedade] > b[propriedade]) return 1;

    if (a[propriedade] < b[propriedade]) return -1;

    return 0;
  });
};
const ordenarDescPor = (lista, propriedade) => {
  return lista.sort((a, b) => {
    if (a[propriedade] < b[propriedade]) return 1;

    if (a[propriedade] > b[propriedade]) return -1;

    return 0;
  });
};

const stringNulaOuEmBranco = valor => {
  return valor ? valor.trim() === '' : true;
};

const removerCaracteresEspeciais = especialChar => {
  especialChar = especialChar.replace('/[áàãâä]/ui', 'a');
  especialChar = especialChar.replace('/[éèêë]/ui', 'e');
  especialChar = especialChar.replace('/[íìîï]/ui', 'i');
  especialChar = especialChar.replace('/[óòõôö]/ui', 'o');
  especialChar = especialChar.replace('/[úùûü]/ui', 'u');
  especialChar = especialChar.replace('/[ç]/ui', 'c');
  especialChar = especialChar.replace('/[^a-z0-9]/i', '_');
  especialChar = especialChar.replace('/_+/', '_'); //
  return especialChar;
};

const removerNumeros = numChar => {
  numChar = numChar.replace(/\d+/g, '');
  return numChar;
};

export {
  validaSeObjetoEhNuloOuVazio,
  valorNuloOuVazio,
  stringNulaOuEmBranco,
  objetoEstaTodoPreenchido,
  removerCaracteresEspeciais,
  ordenarPor,
  ordenarDescPor,
  removerNumeros,
};
