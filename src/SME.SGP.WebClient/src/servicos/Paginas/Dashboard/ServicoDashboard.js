// import api from '~/servicos/api';

class ServicoDashboard {
  obterDadosDashboard = () => {
    const dados = [
      {
        descricao: 'Carta Intenções',
        rota: '/planejamento/carta-intencoes',
        turmaObrigatoria: true,
        usuarioTemPermissao: true,
        icone: 'fa-file-alt',
        pack: 'fas',
      },
      {
        descricao: 'Calendário do professor',
        rota: '/calendario-escolar/calendario-professor',
        turmaObrigatoria: true,
        usuarioTemPermissao: true,
        icone: 'fa-columns',
        pack: 'fas',
      },
    ];
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ data: dados });
      }, 2000);
    });
  };
}

export default new ServicoDashboard();
