import PropTypes from 'prop-types';
import React from 'react';
import * as moment from 'moment';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import SecaoVersaoPlanoCollapse from '../SecaoVersaoPlano/secaoVersaoPlanoCollapse';
import MontarDadosPorSecao from './DadosSecaoPlano/montarDadosPorSecao';

const SecaoPlanoCollapse = props => {
  const { match } = props;

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  return (
    <>
      {planoAEEDados?.questoes?.length ? (
        <CardCollapse
          key="secao-informacoes-plano-collapse-key"
          titulo={
            planoAEEDados?.versoes === null
              ? 'Informações do Plano'
              : `Informações do Plano - v${
                  planoAEEDados?.versoes?.[0]?.numero
                } (${moment(planoAEEDados?.versoes?.[0]?.criadoEm).format(
                  'DD/MM/YYYY'
                )})`
          }
          indice="secao-informacoes-plano--collapse-indice"
          alt="secao-informacoes-plano--alt"
        >
          <MontarDadosPorSecao
            dados={{ id: 0, questionarioId: planoAEEDados?.questionarioId }}
            dadosQuestionarioAtual={planoAEEDados?.questoes}
            match={match}
          />
        </CardCollapse>
      ) : (
        ''
      )}
      {planoAEEDados?.versoes?.length > 1 ? (
        <SecaoVersaoPlanoCollapse
          questionarioId={planoAEEDados?.questionarioId}
          planoId={planoAEEDados?.id}
          versoes={planoAEEDados?.versoes}
        />
      ) : (
        ''
      )}
    </>
  );
};

SecaoPlanoCollapse.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

SecaoPlanoCollapse.defaultProps = {
  match: {},
};

export default SecaoPlanoCollapse;
