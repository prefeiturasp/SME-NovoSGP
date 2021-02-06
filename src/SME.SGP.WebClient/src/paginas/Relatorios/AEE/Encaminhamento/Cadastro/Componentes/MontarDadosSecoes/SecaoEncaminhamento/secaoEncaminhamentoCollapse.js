import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import ObjectCardEstudante from '~/componentes-sgp/ObjectCardEstudante/objectCardEstudante';
import CardCollapse from '~/componentes/cardCollapse';
import AuditoriaEncaminhamento from '../../AuditoriaEncaminhamento/auditoriaEncaminhamento';
import ModalEncerramentoEncaminhamentoAEE from '../../ModalEncerramentoEncaminhamentoAEE/modalEncerramentoEncaminhamentoAEE';
import ModalErrosEncaminhamento from '../../ModalErrosEncaminhamento/modalErrosEncaminhamento';
import MotivoEncerramento from '../../MotivoEncerramento/MotivoEncerramento';
import DadosSecaoEncaminhamento from './DadosSecaoEncaminhamento/dadosSecaoEncaminhamento';

const SecaoEncaminhamentoCollapse = props => {
  const { match } = props;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  return (
    <CardCollapse
      key="secao-encaminhamento-collapse-key"
      titulo="Encaminhamento"
      indice="secao-encaminhamento-collapse-indice"
      alt="secao-encaminhamento-alt"
    >
      {dadosCollapseLocalizarEstudante?.codigoAluno ? (
        <>
          <ObjectCardEstudante
            codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
            anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
            exibirBotaoImprimir={false}
            exibirFrequencia={false}
          />
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
