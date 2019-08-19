import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import './configuracao/ReactotronConfig';
import { useSelector } from 'react-redux';
import Rotas from './rotas/rotas';
import history from './servicos/history';
import Alert from './componentes/alert';

import GlobalStyle from './estilos/global';
import Navbar from './componentes-sgp/navbar';
import Sidebar from './componentes-sgp/sidebar';
import { store } from './redux';

const notificacoes = { alertas: [] };
function App() {
  return (
    <Provider store={store}>
      <Router history={history}>
        <GlobalStyle />
        <Navbar />
        {notificacoes.alertas.map(alerta => (
          <Alert alerta={alerta} key={alerta.id}>
            {alerta.mensagem}
          </Alert>
        ))}
        <div className="container-fluid h-100">
          <div className="row h-100">
            <Sidebar />
            <main
              role="main"
              className="col-md-9 ml-sm-auto col-lg-10 bg-white pt-2"
            >
              <Rotas />
            </main>
          </div>
        </div>
      </Router>
    </Provider>
  );
}

export default App;
