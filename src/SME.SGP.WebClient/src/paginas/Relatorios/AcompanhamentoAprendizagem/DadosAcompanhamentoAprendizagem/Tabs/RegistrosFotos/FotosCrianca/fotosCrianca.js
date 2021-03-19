import PropTypes from 'prop-types';
import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import FotosCriancaDados from './fotosCriancaDados';

const FotosCrianca = props => {
  const { semestreSelecionado } = props;

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="fotos-crianca-collapse"
        titulo="Fotos da criança"
        indice="fotos-crianca"
        alt="fotos-crianca"
      >
        <span className="font-weight-bold">
          Carregue até 3 fotos da criança
        </span>
        <FotosCriancaDados semestreSelecionado={semestreSelecionado} />
      </CardCollapse>
    </div>
  );
};

FotosCrianca.propTypes = {
  semestreSelecionado: PropTypes.string,
};

FotosCrianca.defaultProps = {
  semestreSelecionado: '',
};

export default FotosCrianca;
