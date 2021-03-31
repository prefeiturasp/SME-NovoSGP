class ServicoImagemEstudante {
  obterImagemEstudante = () => {
    return new Promise(resolve => {
      const params = {
        data: {
          codigo: '3163e746-edc8-4bc5-8016-43100b863faf',
          download: {
            item1:
              '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',
            item2: 'image/jpeg',
            item3: 'Criança.jpg',
            nome: 'Criança.jpg',
          },
        },
        status: 200,
      };

      setTimeout(() => {
        resolve(params);
      }, 3000);
    });
  };

  uploadImagemEstudante = codigoAluno => {
    console.log(`Upload estudante ${codigoAluno}`);
    return new Promise(resolve => {
      const params = {
        data: {
          codigo: '3163e746-edc8-4bc5-8016-43100b863faf',
          download: {
            item1:
              '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',
            item2: 'image/jpeg',
            item3: 'Criança.jpg',
            nome: 'Criança.jpg',
          },
        },
        status: 200,
      };

      setTimeout(() => {
        resolve(params);
      }, 1000);
    });
  };

  excluirImagemEstudante = codigoAluno => {
    console.log(`Remover foto estudante ${codigoAluno}`);
    return new Promise(resolve => {
      const params = {
        data: true,
        status: 200,
      };

      setTimeout(() => {
        resolve(params);
      }, 1000);
    });
  };
}

export default new ServicoImagemEstudante();
