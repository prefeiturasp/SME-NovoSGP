import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import ObjectCardEstudante from '~/componentes-sgp/ObjectCardEstudante/objectCardEstudante';
import CardCollapse from '~/componentes/cardCollapse';
import situacaoAEE from '~/dtos/situacaoAEE';
import ModalDevolverAEE from '../../ModalDevolverAEE/modalDevolverAEE';
import ModalEncerramentoEncaminhamentoAEE from '../../ModalEncerramentoEncaminhamentoAEE/modalEncerramentoEncaminhamentoAEE';
import ModalErrosEncaminhamento from '../../ModalErrosEncaminhamento/modalErrosEncaminhamento';
import MotivoEncerramento from '../../MotivoEncerramento/MotivoEncerramento';
import DadosSecaoEncaminhamento from './dadosSecaoEncaminhamento';

const SecaoEncaminhamentoCollapse = props => {
  const { match } = props;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  const desabilitarCamposEncaminhamentoAEE = useSelector(
    store => store.encaminhamentoAEE.desabilitarCamposEncaminhamentoAEE
  );

  return (
    <CardCollapse
      key="secao-encaminhamento-collapse-key"
      titulo="Encaminhamento"
      indice="secao-encaminhamento-collapse-indice"
      alt="secao-encaminhamento-alt"
      show={
        dadosEncaminhamento?.situacao === situacaoAEE.AtribuicaoResponsavel ||
        dadosEncaminhamento?.situacao === situacaoAEE.Analise
          ? false
          : !!dadosCollapseLocalizarEstudante?.codigoAluno
      }
    >
      {dadosCollapseLocalizarEstudante?.codigoAluno ? (
        <>
          <ObjectCardEstudante
            codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
            anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
            exibirBotaoImprimir={false}
            exibirFrequencia={false}
            permiteAlterarImagem={!desabilitarCamposEncaminhamentoAEE}
          />
          <MotivoEncerramento />
          <DadosSecaoEncaminhamento match={match} />
          <ModalErrosEncaminhamento />
          <ModalEncerramentoEncaminhamentoAEE match={match} />
          <ModalDevolverAEE match={match} />
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
