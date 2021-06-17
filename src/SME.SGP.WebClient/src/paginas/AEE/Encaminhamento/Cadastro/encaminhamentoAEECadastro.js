import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Card } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import CollapseLocalizarEstudante from '~/componentes-sgp/CollapseLocalizarEstudante/collapseLocalizarEstudante';
import { RotasDto } from '~/dtos';
import { setLimparDadosAtribuicaoResponsavel } from '~/redux/modulos/collapseAtribuicaoResponsavel/actions';
import { setLimparDadosLocalizarEstudante } from '~/redux/modulos/collapseLocalizarEstudante/actions';
import {
  setDesabilitarCamposEncaminhamentoAEE,
  setLimparDadosEncaminhamento,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import { setLimparDadosQuestionarioDinamico } from '~/redux/modulos/questionarioDinamico/actions';
import { setBreadcrumbManual, verificaSomenteConsulta } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import BotoesAcoesEncaminhamentoAEE from './Componentes/botoesAcoesEncaminhamentoAEE';
import LoaderEncaminhamento from './Componentes/LoaderEncaminhamento/loaderEncaminhamento';
import MontarDadosSecoes from './Componentes/MontarDadosSecoes/montarDadosSecoes';

const EncaminhamentoAEECadastro = ({ match }) => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_ENCAMINHAMENTO];

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);
  }, [permissoesTela]);

  useEffect(() => {
    const encaminhamentoId = match?.params?.id || 0;

    const soConsulta = verificaSomenteConsulta(permissoesTela);
    const desabilitar =
      encaminhamentoId > 0
        ? soConsulta || !permissoesTela.podeAlterar
        : soConsulta || !permissoesTela.podeIncluir;
    dispatch(setDesabilitarCamposEncaminhamentoAEE(desabilitar));
  }, [match, permissoesTela, dispatch]);

  const obterEncaminhamentoPorId = useCallback(async () => {
    const encaminhamentoId = match?.params?.id;

    ServicoEncaminhamentoAEE.obterEncaminhamentoPorId(encaminhamentoId);
  }, [match]);

  useEffect(() => {
    const encaminhamentoId = match?.params?.id;
    if (encaminhamentoId) {
      obterEncaminhamentoPorId();
    }
  }, [match, obterEncaminhamentoPorId, dispatch]);

  const limparDadosEncaminhamento = useCallback(() => {
    dispatch(setLimparDadosEncaminhamento());
    dispatch(setLimparDadosQuestionarioDinamico());
  }, [dispatch]);

  useEffect(() => {
    return () => {
      limparDadosEncaminhamento();
      dispatch(setLimparDadosLocalizarEstudante());
      dispatch(setLimparDadosAtribuicaoResponsavel());
    };
  }, [dispatch, limparDadosEncaminhamento]);

  useEffect(() => {
    const encaminhamentoId = match?.params?.id;
    if (encaminhamentoId) {
      setBreadcrumbManual(
        match.url,
        'Editar',
        `${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}`
      );
    }
  }, [match]);

  const validarSePermiteProximoPasso = codigoEstudante => {
    return ServicoEncaminhamentoAEE.podeCadastrarEncaminhamentoEstudante(
      codigoEstudante
    );
  };

  return (
    <LoaderEncaminhamento>
      <Cabecalho pagina="Encaminhamento AEE" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end mb-3">
              <BotoesAcoesEncaminhamentoAEE match={match} />
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
                  validarSePermiteProximoPasso={validarSePermiteProximoPasso}
                />
              </div>
            )}
            <MontarDadosSecoes match={match} />
          </div>
        </div>
      </Card>
    </LoaderEncaminhamento>
  );
};

EncaminhamentoAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

EncaminhamentoAEECadastro.defaultProps = {
  match: {},
};

export default EncaminhamentoAEECadastro;
