import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import AuditoriaEncaminhamento from '../AuditoriaEncaminhamento/auditoriaEncaminhamento';
import ModalEncerramentoEncaminhamentoAEE from '../ModalEncerramentoEncaminhamentoAEE/modalEncerramentoEncaminhamentoAEE';
import ModalErrosEncaminhamento from '../ModalErrosEncaminhamento/modalErrosEncaminhamento';
import MotivoEncerramento from '../MotivoEncerramento/MotivoEncerramento';
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
          <MotivoEncerramento />
          <DadosSecaoEncaminhamento match={match} />
          <AuditoriaEncaminhamento />
          <ModalErrosEncaminhamento />
          <ModalEncerramentoEncaminhamentoAEE match={match} />
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
