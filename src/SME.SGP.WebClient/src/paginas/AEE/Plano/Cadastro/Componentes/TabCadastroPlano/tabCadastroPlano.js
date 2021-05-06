import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setDadosCollapseLocalizarEstudante } from '~/redux/modulos/collapseLocalizarEstudante/actions';
import { setDadosObjectCardEstudante } from '~/redux/modulos/objectCardEstudante/actions';
import {
  setAtualizarDados,
  setExibirLoaderPlanoAEE,
  setPlanoAEEDados,
  setPlanoAEELimparDados,
} from '~/redux/modulos/planoAEE/actions';
import { erros } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import MontarDadosTabs from './montarDadosTabs';

const TabCadastroPlano = props => {
  const { match } = props;

  const dispatch = useDispatch();

  const atualizarDados = useSelector(store => store.planoAEE.atualizarDados);

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const obterPlanoPorId = useCallback(async () => {
    const planoId = match?.params?.id ? match?.params?.id : 0;

    let turmaCodigo = 0;

    if (!planoId) {
      turmaCodigo = dadosCollapseLocalizarEstudante?.codigoTurma;
    }

    dispatch(setExibirLoaderPlanoAEE(true));
    const resultado = await ServicoPlanoAEE.obterPlanoPorId(
      planoId,
      turmaCodigo
    )
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
          ehAtendidoAEE: aluno?.ehAtendidoAEE,
          numeroChamada: aluno.numeroAlunoChamada,
        };
        dispatch(setDadosObjectCardEstudante(dadosObjectCard));
      }
      dispatch(setPlanoAEEDados(resultado?.data));

      if (resultado?.data?.turma) {
        const { aluno, turma } = resultado?.data;
        const dadosLocalizarEstudante = {
          anoLetivo: turma.anoLetivo,
          codigoAluno: aluno.codigoAluno,
          codigoTurma: turma.codigo,
          turmaId: turma.id,
        };

        dispatch(setDadosCollapseLocalizarEstudante(dadosLocalizarEstudante));
      }
    } else {
      dispatch(setPlanoAEELimparDados());
    }
  }, [match, dispatch, dadosCollapseLocalizarEstudante]);

  useEffect(() => {
    if (atualizarDados) {
      obterPlanoPorId();
    }
    dispatch(setAtualizarDados(false));
  }, [atualizarDados, dispatch, obterPlanoPorId]);

  useEffect(() => {
    const planoId = match?.params?.id ? match?.params?.id : 0;
    if (planoId && !dadosCollapseLocalizarEstudante?.codigoAluno) {
      obterPlanoPorId();
    } else if (!planoId && dadosCollapseLocalizarEstudante?.codigoAluno) {
      obterPlanoPorId();
    }
  }, [obterPlanoPorId, match, dadosCollapseLocalizarEstudante]);

  return <MontarDadosTabs match={match} />;
};

TabCadastroPlano.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

TabCadastroPlano.defaultProps = {
  match: {},
};

export default TabCadastroPlano;
