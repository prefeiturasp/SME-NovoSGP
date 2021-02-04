import React, { useCallback, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import { Cabecalho } from '~/componentes-sgp';
import Card from '~/componentes/card';
import BotoesAcoesPlanoAEE from './Componentes/botoesAcoesPlanoAEE';
import LoaderPlano from './Componentes/LoaderPlano/loaderPlano';
import TabCadastroPasso from './Componentes/TabCadastroPlano/tabCadastroPlano';
import { setLimparDadosQuestionarioDinamico } from '~/redux/modulos/questionarioDinamico/actions';
import {
  setPlanoAEEDados,
  setPlanoAEELimparDados,
} from '~/redux/modulos/planoAEE/actions';
import CollapseLocalizarEstudante from '~/componentes-sgp/CollapseLocalizarEstudante/collapseLocalizarEstudante';
import ObjectCardEstudantePlanoAEE from './Componentes/ObjectCardEstudantePlanoAEE/objectCardEstudantePlanoAEE';
import SituacaoEncaminhamentoAEE from './Componentes/SituacaoEncaminhamentoAEE/situacaoEncaminhamentoAEE';
import BotaoVerSituacaoEncaminhamentoAEE from './Componentes/BotaoVerSituacaoEncaminhamentoAEE/botaoVerSituacaoEncaminhamentoAEE';
import { setLimparDadosLocalizarEstudante } from '~/redux/modulos/collapseLocalizarEstudante/actions';

const PlanoAEECadastro = ({ match }) => {
  const dispatch = useDispatch();

  const limparDadosPlano = useCallback(() => {
    dispatch(setPlanoAEELimparDados());
    dispatch(setLimparDadosQuestionarioDinamico());
  }, [dispatch]);

  const validarSePermiteProximoPasso = codigoEstudante => {
    dispatch(
      setPlanoAEEDados({
        encaminhamento: {
          situação: 'Aguardando validação CP',
          encaminhamentoId: 1,
        },
        secao: {
          id: 1,
          nome: 'Informações do Plano',
          questionarioId: 1,
        },
        planosAnteriores: [
          {
            id: 2,
            nome: 'Informações do Plano - v1',
            questionarioId: 1,
          },
          {
            id: 3,
            nome: 'Informações do Plano - v2',
            questionarioId: 1,
          },
        ],
      })
    );
    return true;
    // return ServicoEncaminhamentoAEE.podeCadastrarEncaminhamentoEstudante(
    //   codigoEstudante
    // );
  };

  useEffect(() => {
    return () => {
      limparDadosPlano();
      dispatch(setLimparDadosLocalizarEstudante());
    };
  }, [dispatch, limparDadosPlano]);

  return (
    <LoaderPlano>
      <Cabecalho pagina="Plano AEE" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <BotoesAcoesPlanoAEE match={match} />
            </div>
            {match?.params?.id ? (
              ''
            ) : (
              <div className="col-md-12 mb-2">
                <CollapseLocalizarEstudante
                  changeDre={limparDadosPlano}
                  changeUe={limparDadosPlano}
                  changeTurma={limparDadosPlano}
                  changeLocalizadorEstudante={limparDadosPlano}
                  clickCancelar={limparDadosPlano}
                  validarSePermiteProximoPasso={validarSePermiteProximoPasso}
                />
              </div>
            )}
            <div className="col-md-12 mb-2">
              <ObjectCardEstudantePlanoAEE />
            </div>
            <div className="col-md-12 mt-2 mb-2">
              <SituacaoEncaminhamentoAEE />
            </div>
            <div className="col-md-12 mb-4">
              <BotaoVerSituacaoEncaminhamentoAEE />
            </div>
            <div className="col-md-12 mt-2 mb-2">
              <TabCadastroPasso match={match} />
            </div>
          </div>
        </div>
      </Card>
    </LoaderPlano>
  );
};

PlanoAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

PlanoAEECadastro.defaultProps = {
  match: {},
};

export default PlanoAEECadastro;
