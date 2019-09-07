import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Rotas from '../rotas/rotas';
import Alert from '~/componentes/alert';

const Conteudo = () => {
  const MenuStore = useSelector(store => store.menu);
  const [collapsed, setCollapsed] = useState(false);
  const notificacoes = useSelector(state => state.notificacoes);

  useEffect(() => {
    setCollapsed(MenuStore.collapsed);
  }, [MenuStore.collapsed]);

  return (
    <main
      role="main"
      className={
        collapsed
          ? 'col-lg-10 col-md-10 col-sm-10 col-xs-12 col-xl-11'
          : 'col-sm-8 col-md-9 col-lg-9 col-xl-10'
      }
    >
      <div className="card-body m-r-0 m-l-0 p-l-0 p-r-0 m-t-0">
        <div className="col-md-12">
          {notificacoes.alertas.map(alerta => (
            <Alert alerta={alerta} key={alerta.id} />
          ))}
        </div>
        <Rotas />
      </div>
    </main>
  );
};

export default Conteudo;
