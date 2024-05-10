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

const downloadBlob = (data, fileName) => {
  const a = document.createElement('a');
  document.body.appendChild(a);
  a.style = 'display: none';

  const blob = new Blob([data]);
  const url = window.URL.createObjectURL(blob);
  a.href = url;
  a.download = fileName;
  a.click();
  window.URL.revokeObjectURL(url);

  document.body.removeChild(a);
};

const removerTudoQueNaoEhDigito = v => {
  return String(v).replace(/\D/g, '');
};

const maskTelefone = v => {
  v = String(v);
  v = removerTudoQueNaoEhDigito(v);
  v = v.replace(/^(\d{2})(\d)/g, '($1) $2'); // Coloca parênteses em volta dos dois primeiros dígitos
  v = v.replace(/(\d)(\d{4})$/, '$1-$2'); // Coloca hífen entre o quarto e o quinto dígitos
  return v;
};

const ordenarListaMaiorParaMenor = (conteudoParaOrdenar, nomeCampo) => {
  const ordenar = (a, b) => {
    return b[nomeCampo] - a[nomeCampo];
  };
  const dadosOrdenados = conteudoParaOrdenar.sort(ordenar);
  return dadosOrdenados;
};

const clonarObjeto = objeto => {
  return JSON.parse(JSON.stringify(objeto));
};

const removerArrayAninhados = arr =>
  arr.reduce(
    (acc, val) =>
      Array.isArray(val)
        ? acc.concat(removerArrayAninhados(val))
        : acc.concat(val),
    []
  );

const permiteInserirFormato = (arquivo, tiposArquivosPermitidos) => {
  if (tiposArquivosPermitidos?.trim()) {
    const listaPermitidos = tiposArquivosPermitidos
      .split(',')
      .map(tipo => tipo?.trim()?.toLowerCase());

    const tamanhoNome = arquivo?.name?.length;

    const permiteTipo = listaPermitidos.find(tipo => {
      const nomeTipoAtual = arquivo.name.substring(
        tamanhoNome,
        tamanhoNome - tipo.length
      );

      if (nomeTipoAtual) {
        return tipo?.toLowerCase() === nomeTipoAtual?.toLowerCase();
      }

      return false;
    });

    return !!permiteTipo;
  }
  return true;
};

const getBase64DataURL = (file, type) => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    const fileBlob = new Blob([file], { type });
    reader.readAsDataURL(fileBlob);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
  });
};

const obterTamanhoImagemPorArquivo = file => {
  return new Promise(resolve => {
    const reader = new FileReader();

    reader.onload = () => {
      const img = new Image();
      img.src = reader.result;
      img.onload = () =>
        resolve({
          width: img.naturalWidth,
          height: img.naturalHeight,
        });
    };

    reader.readAsDataURL(file);
  });
};
const obterTodosMeses = () => {
  const meses = [
    {
      numeroMes: '1',
      nome: 'Janeiro',
    },
    {
      numeroMes: '2',
      nome: 'Fevereiro',
    },
    {
      numeroMes: '3',
      nome: 'Março',
    },
    {
      numeroMes: '4',
      nome: 'Abril',
    },
    {
      numeroMes: '5',
      nome: 'Maio',
    },
    {
      numeroMes: '6',
      nome: 'Junho',
    },
    {
      numeroMes: '7',
      nome: 'Julho',
    },
    {
      numeroMes: '8',
      nome: 'Agosto',
    },
    {
      numeroMes: '9',
      nome: 'Setembro',
    },
    {
      numeroMes: '10',
      nome: 'Outubro',
    },
    {
      numeroMes: '11',
      nome: 'Novembro',
    },
    {
      numeroMes: '12',
      nome: 'Dezembro',
    },
  ];

  return meses;
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
  downloadBlob,
  maskTelefone,
  ordenarListaMaiorParaMenor,
  removerArrayAninhados,
  clonarObjeto,
  permiteInserirFormato,
  getBase64DataURL,
  obterTamanhoImagemPorArquivo,
  obterTodosMeses,
};
