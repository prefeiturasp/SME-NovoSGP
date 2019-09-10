import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import './configuracao/ReactotronConfig';
import history from './servicos/history';
import GlobalStyle from './estilos/global';
import Navbar from './componentes-sgp/navbar';
import Sider from './componentes-sgp/sider';
import { store } from './redux';
import Conteudo from './componentes-sgp/conteudo';
import { activeRoute } from './redux/modulos/navegacao/actions'

function App() {

  history.listen((location) => {
    store.dispatch(activeRoute(location.pathname));
    localStorage.setItem('rota-atual', location.pathname);
  });

  return (
    <Provider store={store}>
      <Router history={history}>
        <GlobalStyle />
        <Navbar />
        <div className="container-fluid h-100">
          <Sider />
          <Conteudo />
        </div>
      </Router>
    </Provider>
  );
}

export default App;
