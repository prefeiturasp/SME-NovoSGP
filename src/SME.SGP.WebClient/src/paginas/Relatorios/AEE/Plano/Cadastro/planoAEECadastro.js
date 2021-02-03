import React, { useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import { Cabecalho } from '~/componentes-sgp';
import Card from '~/componentes/card';
import BotoesAcoesPlanoAEE from './Componentes/botoesAcoesPlanoAEE';
import LoaderPlano from './Componentes/LoaderPlano/loaderPlano';
import TabCadastroPasso from './Componentes/TabCadastroPlano/tabCadastroPlano';
import { setLimparDadosQuestionarioDinamico } from '~/redux/modulos/questionarioDinamico/actions';
import { setPlanoAEELimparDados } from '~/redux/modulos/planoAEE/actions';
import CollapseLocalizarEstudante from '~/componentes-sgp/CollapseLocalizarEstudante/collapseLocalizarEstudante';
import ObjectCardEstudante from '~/componentes-sgp/ObjectCardEstudante/objectCardEstudante';

const PlanoAEECadastro = ({ match }) => {
  const dispatch = useDispatch();

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const limparDadosEncaminhamento = useCallback(() => {
    dispatch(setPlanoAEELimparDados());
    dispatch(setLimparDadosQuestionarioDinamico());
  }, [dispatch]);

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
                  changeDre={limparDadosEncaminhamento}
                  changeUe={limparDadosEncaminhamento}
                  changeTurma={limparDadosEncaminhamento}
                  changeLocalizadorEstudante={limparDadosEncaminhamento}
                  clickCancelar={limparDadosEncaminhamento}
                />
              </div>
            )}
            <div className="col-md-12 mb-2">
              {dadosCollapseLocalizarEstudante?.codigoAluno ? (
                <>
                  <ObjectCardEstudante
                    codigoAluno={dadosCollapseLocalizarEstudante?.codigoAluno}
                    anoLetivo={dadosCollapseLocalizarEstudante?.anoLetivo}
                    exibirBotaoImprimir={false}
                    exibirFrequencia={false}
                  />
                </>
              ) : (
                ''
              )}
              <TabCadastroPasso match={match} />
              {/* <SecaoEncaminhamentoCollapse match={match} /> */}
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
