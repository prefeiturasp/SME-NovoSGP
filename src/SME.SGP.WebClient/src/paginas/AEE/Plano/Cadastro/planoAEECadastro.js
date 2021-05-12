import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Cabecalho } from '~/componentes-sgp';
import CollapseLocalizarEstudante from '~/componentes-sgp/CollapseLocalizarEstudante/collapseLocalizarEstudante';
import Card from '~/componentes/card';
import { RotasDto } from '~/dtos';
import { setLimparDadosLocalizarEstudante } from '~/redux/modulos/collapseLocalizarEstudante/actions';
import {
  setDesabilitarCamposPlanoAEE,
  setPlanoAEELimparDados,
} from '~/redux/modulos/planoAEE/actions';
import { setLimparDadosQuestionarioDinamico } from '~/redux/modulos/questionarioDinamico/actions';
import { setBreadcrumbManual, verificaSomenteConsulta } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import BotaoVerSituacaoEncaminhamentoAEE from './Componentes/BotaoVerSituacaoEncaminhamentoAEE/botaoVerSituacaoEncaminhamentoAEE';
import BotoesAcoesPlanoAEE from './Componentes/botoesAcoesPlanoAEE';
import LoaderPlano from './Componentes/LoaderPlano/loaderPlano';
import MarcadorSituacaoPlanoAEE from './Componentes/MarcadorSituacaoPlanoAEE/marcadorSituacaoPlanoAEE';
import ObjectCardEstudantePlanoAEE from './Componentes/ObjectCardEstudantePlanoAEE/objectCardEstudantePlanoAEE';
import ObservacoesPlanoAEE from './Componentes/ObservacoesPlanoAEE/observacoesPlanoAEE';
import SituacaoEncaminhamentoAEE from './Componentes/SituacaoEncaminhamentoAEE/situacaoEncaminhamentoAEE';
import TabCadastroPlano from './Componentes/TabCadastroPlano/tabCadastroPlano';

const PlanoAEECadastro = ({ match }) => {
  const dispatch = useDispatch();

  const limparDadosPlano = useCallback(() => {
    dispatch(setLimparDadosQuestionarioDinamico());
    dispatch(setPlanoAEELimparDados());
  }, [dispatch]);

  const validarSePermiteProximoPasso = async codigoEstudante => {
    return ServicoPlanoAEE.existePlanoAEEEstudante(codigoEstudante);
  };

  useEffect(() => {
    const planoId = match?.params?.id;
    if (planoId) {
      setBreadcrumbManual(
        match.url,
        'Editar',
        `${RotasDto.RELATORIO_AEE_PLANO}`
      );
    }
  }, [match]);

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];

  useEffect(() => {
    const planoId = match?.params?.id || 0;

    const soConsulta = verificaSomenteConsulta(permissoesTela);
    const desabilitar =
      planoId > 0
        ? soConsulta || !permissoesTela.podeAlterar
        : soConsulta || !permissoesTela.podeIncluir;
    dispatch(setDesabilitarCamposPlanoAEE(desabilitar));
  }, [match, permissoesTela, dispatch]);

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
            <div className="col-md-12 mb-2 d-flex justify-content-end">
              <MarcadorSituacaoPlanoAEE />
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
            <div className="col-md-4 mb-4">
              <BotaoVerSituacaoEncaminhamentoAEE />
            </div>
            <div className="col-md-12 mt-2 mb-2">
              <TabCadastroPlano match={match} />
            </div>
            <div className="col-sm-12">
              <ObservacoesPlanoAEE />
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
