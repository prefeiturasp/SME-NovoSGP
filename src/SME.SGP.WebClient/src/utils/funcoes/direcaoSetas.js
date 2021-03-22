const converterAcaoTecla = keyCode => {
  switch (keyCode) {
    case 38:
      return -1;
    case 40:
      return 1;
    case 48:
      return 0;
    case 96:
      return 0;
    default:
      return false;
  }
};

const acharItem = (dados, alunoEscolhido, numero, nomeItem) => {
  if (dados?.length) {
    return dados
      ?.map((valor, index, elementos) => {
        if (valor[nomeItem] === alunoEscolhido[nomeItem]) {
          return elementos[index + numero];
        }
        return '';
      })
      .filter(item => item && item[nomeItem]);
  }
  return '';
};

const esperarMiliSegundos = milisegundos => {
  return new Promise(resolve => setTimeout(resolve, milisegundos));
};

const tratarString = item =>
  item
    ?.toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]|[^a-zA-Zs]|\s/g, '');

const moverCursor = async (
  itemEscolhido,
  indexElemento = 0,
  regencia = false
) => {
  const elemento = document.getElementsByName(itemEscolhido);
  let elementoCursor = elemento[indexElemento];
  if (regencia) {
    await esperarMiliSegundos(600);
    const [elementoDiv] = elemento;
    const [elementoInput] = elementoDiv.getElementsByTagName('input');
    elementoCursor = elementoInput;
  }
  if (elementoCursor) {
    elementoCursor.focus();
    elementoCursor.select();
  }
};

export {
  converterAcaoTecla,
  acharItem,
  esperarMiliSegundos,
  tratarString,
  moverCursor,
};
