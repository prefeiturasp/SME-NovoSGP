import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';

const RegistrosIndividuais = props => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [exibir, setExibir] = useState(true);

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="registros-individuais-collapse"
        onClick={onClickExpandir}
        titulo="Registros individuais"
        indice="registros-individuais"
        show={exibir}
        alt="registros-individuais"
      >
        Registros individuais
      </CardCollapse>
    </div>
  );
};

RegistrosIndividuais.propTypes = {
  semestreSelecionado: PropTypes.string,
};

RegistrosIndividuais.defaultProps = {
  semestreSelecionado: '',
};

export default RegistrosIndividuais;
