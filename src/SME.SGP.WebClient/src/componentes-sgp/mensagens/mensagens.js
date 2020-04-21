import { Row } from 'antd';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import Alert from '~/componentes/alert';
import Grid from '~/componentes/grid';

const Mensagens = () => {

  const somenteConsulta = useSelector(store => store.navegacao.somenteConsulta);
  const alertas = useSelector(state => state.alertas);
  return (
    <div className="card-body m-r-0 m-l-0 p-l-0 p-r-0 m-t-0">
      {alertas.alertas.map(alerta => (
        <Row key={shortid.generate()}>
          <Grid cols={12}>
            <Alert alerta={alerta} key={alerta.id} closable />
          </Grid>
        </Row>
      ))}
      <Row key={shortid.generate()} hidden={!somenteConsulta}>
        <Grid cols={12}>
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'AlertaPrincipal',
              mensagem: 'Você tem apenas permissão de consulta nesta tela.',
              estiloTitulo: { fontSize: '18px' },
            }}
          />
        </Grid>
      </Row>
    </div>
  );
};

export default Mensagens;
