import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import FotosCriancaDados from './fotosCriancaDados';

const FotosCrianca = props => {
  const { semestreSelecionado } = props;

  const [exibir, setExibir] = useState(false);

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="fotos-crianca-collapse"
        onClick={onClickExpandir}
        titulo="Fotos da criança"
        indice="fotos-crianca"
        show={exibir}
        alt="fotos-crianca"
      >
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
