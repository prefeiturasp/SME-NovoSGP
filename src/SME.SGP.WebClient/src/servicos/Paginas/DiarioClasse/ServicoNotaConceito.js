class ServicoNotaConceito {
  obterTodosConceitos = () => {
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({
          data: [
            {
              id: 1,
              nome: 'P',
            },
            {
              id: 2,
              nome: 'S',
            },
            {
              id: 3,
              nome: 'NS',
            },
          ],
        });
      }, 1000);
    });
  };
}
export default new ServicoNotaConceito();
