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
  setExibirLoaderPlanoAEE,
  setPlanoAEEDados,
  setPlanoAEELimparDados,
} from '~/redux/modulos/planoAEE/actions';
import CollapseLocalizarEstudante from '~/componentes-sgp/CollapseLocalizarEstudante/collapseLocalizarEstudante';
import ObjectCardEstudantePlanoAEE from './Componentes/ObjectCardEstudantePlanoAEE/objectCardEstudantePlanoAEE';
import SituacaoEncaminhamentoAEE from './Componentes/SituacaoEncaminhamentoAEE/situacaoEncaminhamentoAEE';
import BotaoVerSituacaoEncaminhamentoAEE from './Componentes/BotaoVerSituacaoEncaminhamentoAEE/botaoVerSituacaoEncaminhamentoAEE';
import {
  setDadosCollapseLocalizarEstudante,
  setLimparDadosLocalizarEstudante,
} from '~/redux/modulos/collapseLocalizarEstudante/actions';
import { erros, setBreadcrumbManual } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import { setDadosObjectCardEstudante } from '~/redux/modulos/objectCardEstudante/actions';
import { RotasDto } from '~/dtos';

const PlanoAEECadastro = ({ match }) => {
  const dispatch = useDispatch();

  const limparDadosPlano = useCallback(() => {
    dispatch(setPlanoAEELimparDados());
    dispatch(setLimparDadosQuestionarioDinamico());
  }, [dispatch]);

  const validarSePermiteProximoPasso = async codigoEstudante => {
    return ServicoPlanoAEE.existePlanoAEEEstudante(codigoEstudante);
  };

  useEffect(() => {
    const planoId = match?.params?.id;
    if (planoId) {
      setBreadcrumbManual(
        match.url,
        'Editar Plano',
        `${RotasDto.RELATORIO_AEE_PLANO}`
      );
    }
  }, [match]);

  // TODO PERMISSAO
  // useEffect(() => {
  //   const encaminhamentoId = match?.params?.id || 0;

  //   const soConsulta = verificaSomenteConsulta(permissoesTela);
  //   const desabilitar =
  //     encaminhamentoId > 0
  //       ? soConsulta || !permissoesTela.podeAlterar
  //       : soConsulta || !permissoesTela.podeIncluir;
  //   dispatch(setDesabilitarCamposEncaminhamentoAEE(desabilitar));
  // }, [match, permissoesTela, dispatch]);

  const obterPlanoPorId = useCallback(async () => {
    const planoId = match?.params?.id ? match?.params?.id : 0;

    dispatch(setExibirLoaderPlanoAEE(true));
    const resultado = await ServicoPlanoAEE.obterPlanoPorId(planoId)
      .catch(e => erros(e))
      .finally(() => dispatch(setExibirLoaderPlanoAEE(false)));

    if (resultado?.data) {
      if (resultado?.data?.aluno) {
        const { aluno } = resultado?.data;

        const dadosObjectCard = {
          nome: aluno.nome,
          numeroChamada: aluno.numeroAlunoChamada,
          dataNascimento: aluno.dataNascimento,
          codigoEOL: aluno.codigoAluno,
          situacao: aluno.situacao,
          dataSituacao: aluno.dataSituacao,
        };
        dispatch(setDadosObjectCardEstudante(dadosObjectCard));
      }
      if (resultado?.data?.turma) {
        const { aluno, turma } = resultado?.data;
        const dadosCollapseLocalizarEstudante = {
          anoLetivo: turma.anoLetivo,
          codigoAluno: aluno.codigoAluno,
          codigoTurma: turma.codigo,
          turmaId: turma.id,
        };

        dispatch(
          setDadosCollapseLocalizarEstudante(dadosCollapseLocalizarEstudante)
        );
      }

      dispatch(setPlanoAEEDados(resultado?.data));
    }
  }, [match, dispatch]);

  useEffect(() => {
    obterPlanoPorId();
  }, [obterPlanoPorId]);

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
