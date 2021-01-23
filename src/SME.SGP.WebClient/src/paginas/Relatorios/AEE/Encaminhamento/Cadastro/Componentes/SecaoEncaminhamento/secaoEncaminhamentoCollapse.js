import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import AuditoriaEncaminhamento from '../AuditoriaEncaminhamento/auditoriaEncaminhamento';
import ModalErrosEncaminhamento from '../ModalErrosEncaminhamento/modalErrosEncaminhamento';
import DadosSecaoEncaminhamento from './DadosSecaoEncaminhamento/dadosSecaoEncaminhamento';
import ObjectCardEncaminhamento from './objectCardEncaminhamento';

const SecaoEncaminhamentoCollapse = props => {
  const { match } = props;

  const dadosSecaoLocalizarEstudante = useSelector(
    store => store.encaminhamentoAEE.dadosSecaoLocalizarEstudante
  );

  return (
    <CardCollapse
      key="secao-encaminhamento-collapse-key"
      titulo="Encaminhamento"
      indice="secao-encaminhamento-collapse-indice"
      alt="secao-encaminhamento-alt"
    >
      {dadosSecaoLocalizarEstudante?.codigoAluno ? (
        <>
          <ObjectCardEncaminhamento />
          <DadosSecaoEncaminhamento match={match} />
          <AuditoriaEncaminhamento />
          <ModalErrosEncaminhamento />
        </>
      ) : (
        ''
      )}
    </CardCollapse>
  );
};

SecaoEncaminhamentoCollapse.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

SecaoEncaminhamentoCollapse.defaultProps = {
  match: {},
};

export default SecaoEncaminhamentoCollapse;
