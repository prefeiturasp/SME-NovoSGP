import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import { Cabecalho } from '~/componentes-sgp';
import Card from '~/componentes/card';
import BotoesAcoesPlanoAEE from './Componentes/botoesAcoesPlanoAEE';
import LoaderPlano from './Componentes/LoaderPlano/loaderPlano';
import TabCadastroPasso from './Componentes/TabCadastroPlano/tabCadastroPlano';
import { setLimparDadosQuestionarioDinamico } from '~/redux/modulos/questionarioDinamico/actions';
import {
  setDesabilitarCamposPlanoAEE,
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
import {
  erros,
  setBreadcrumbManual,
  verificaSomenteConsulta,
} from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import { setDadosObjectCardEstudante } from '~/redux/modulos/objectCardEstudante/actions';
import { RotasDto } from '~/dtos';
import MarcadorSituacaoPlanoAEE from './Componentes/MarcadorSituacaoPlanoAEE/marcadorSituacaoPlanoAEE';

const PlanoAEECadastro = ({ match }) => {
  const dispatch = useDispatch();

  const limparDadosPlano = useCallback(() => {
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

  const obterPlanoPorId = useCallback(async () => {
    const planoId = match?.params?.id ? match?.params?.id : 0;
    dispatch(setPlanoAEELimparDados());
    dispatch(setExibirLoaderPlanoAEE(true));
    const resultado = await ServicoPlanoAEE.obterPlanoPorId(planoId)
      .catch(e => erros(e))
      .finally(() => dispatch(setExibirLoaderPlanoAEE(false)));

    if (resultado?.data) {
      if (resultado?.data?.aluno) {
        const { aluno } = resultado?.data;

        const dadosObjectCard = {
          nome: aluno.nome,
          dataNascimento: aluno.dataNascimento,
          situacao: aluno.situacao,
          dataSituacao: aluno.dataSituacao,
          nomeResponsavel: aluno.nomeResponsavel,
          tipoResponsavel: aluno.tipoResponsavel,
          celularResponsavel: aluno.celularResponsavel,
          dataAtualizacaoContato: aluno.dataAtualizacaoContato,
          codigoEOL: aluno.codigoAluno,
          turma: aluno.turmaEscola,
          numeroChamada: aluno.numeroAlunoChamada,
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
