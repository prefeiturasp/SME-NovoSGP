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
        <div className="container-fluid h-100">
          <div className="row h-100">
            <Sidebar />
            <main role="main" className="col-lg-10 col-md-10 col-sm-12 col-xs-12">
              <div className="card-body m-r-0 m-l-0 p-l-0 p-r-0 m-t-0">
                <Rotas />
              </div>
            </main>
          </div>
        </div>
      </Router>
    </Provider>
  );
}

export default App;
