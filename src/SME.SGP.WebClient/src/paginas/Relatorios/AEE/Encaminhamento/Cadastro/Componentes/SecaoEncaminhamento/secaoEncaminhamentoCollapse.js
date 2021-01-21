import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
// import { useDispatch } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import DadosSecaoEncaminhamento from './DadosSecaoEncaminhamento/dadosSecaoEncaminhamento';
import ObjectCardEncaminhamento from './objectCardEncaminhamento';

const SecaoEncaminhamentoCollapse = props => {
  const { match } = props;
  // const dispatch = useDispatch();

  const onClickCardCollapse = () => {
    // dispatch(setClicouNoBimestre({ bimestre }));
  };

  const dadosSecaoLocalizarEstudante = useSelector(
    store => store.encaminhamentoAEE.dadosSecaoLocalizarEstudante
  );

  return (
    <CardCollapse
      key="secao-encaminhamento-collapse-key"
      titulo="Encaminhamento"
      indice="secao-encaminhamento-collapse-indice"
      alt="secao-encaminhamento-alt"
      onClick={onClickCardCollapse}
    >
      {dadosSecaoLocalizarEstudante?.codigoAluno ? (
        <>
          <ObjectCardEncaminhamento />
          <DadosSecaoEncaminhamento match={match} />
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
