import React from 'react';
import Card from '~/componentes/card';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import { Corpo } from './pagina-com-erro.css';

const PaginaComErro = () => {
  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  return (
    <Card>
      <Corpo className="col-md-12">
        <i className="far fa-frown not-found"></i>
        {/* <span className="not-found">404</span> */}
        <span className="msg-principal">Ocorreu um erro!</span>
        <span>
          A página que você tentou acessar não está disponível no momento.
        </span>
        <Button
          label="Voltar"
          icon="arrow-left"
          color={Colors.Azul}
          border
          className="mr-2"
          onClick={onClickVoltar}
        />
      </Corpo>
    </Card>
  );
};

export default PaginaComErro;
