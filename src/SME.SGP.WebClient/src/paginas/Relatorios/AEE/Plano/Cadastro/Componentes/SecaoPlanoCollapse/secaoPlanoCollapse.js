import PropTypes from 'prop-types';
import React from 'react';
import * as moment from 'moment';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import SecaoVersaoPlanoCollapse from '../SecaoVersaoPlano/secaoVersaoPlanoCollapse';
import MontarDadosPorSecao from './DadosSecaoPlano/montarDadosPorSecao';
import ModalErrosPlano from '../ModalErrosPlano/modalErrosPlano';

const SecaoPlanoCollapse = props => {
  const { match } = props;

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  return (
    <>
      <ModalErrosPlano />
      {planoAEEDados?.questoes?.length ? (
        <CardCollapse
          key="secao-informacoes-plano-collapse-key"
          titulo={
            planoAEEDados?.versoes === null
              ? 'Informações do Plano'
              : `Informações do Plano - v${
                  planoAEEDados?.ultimaVersao?.numero
                } (${moment(planoAEEDados?.ultimaVersao?.criadoEm).format(
                  'DD/MM/YYYY'
                )})`
          }
          show
          indice="secao-informacoes-plano-collapse-indice"
          alt="secao-informacoes-plano-alt"
        >
          <MontarDadosPorSecao
            dados={{ questionarioId: 0 }}
            dadosQuestionarioAtual={planoAEEDados?.questoes}
            match={match}
          />
        </CardCollapse>
      ) : (
        ''
      )}
      {planoAEEDados?.versoes?.length ? (
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
