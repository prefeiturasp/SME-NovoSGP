import React, { useCallback, useEffect } from 'react';
import PropTypes from 'prop-types';

import { useDispatch, useSelector } from 'react-redux';
import { CardCollapse } from '~/componentes';

import { erros, verificaSomenteConsulta } from '~/servicos';
import { RotasDto, situacaoPlanoAEE } from '~/dtos';

import SecaoDevolutivaCoordenacao from '../SecaoDevolutivaCoordenacao/secaoDevolutivaCoordenacao';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import {
  setExibirLoaderPlanoAEE,
  setDadosDevolutiva,
  setAtualizarDados,
} from '~/redux/modulos/planoAEE/actions';
import SecaoDevolutivaResponsavel from '../SecaoDevolutivaResponsavel/secaoDevolutivaResponsavel';
import SecaoDevolutivaPaai from '../SecaoDevolutivaPaai/secaoDevolutivaPaai';

const SecaoDevolutivasPlano = ({ match }) => {
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);
  const dadosDevolutiva = useSelector(store => store.planoAEE.dadosDevolutiva);
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];
  const atualizarDados = useSelector(store => store.planoAEE.atualizarDados);

  const dispatch = useDispatch();

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);
  }, [permissoesTela]);

  const obterDevolutiva = useCallback(async () => {
    dispatch(setExibirLoaderPlanoAEE(true));
    const resultado = await ServicoPlanoAEE.obterDevolutiva(match?.params?.id)
      .catch(e => erros(e))
      .finally(() => dispatch(setExibirLoaderPlanoAEE(false)));

    if (resultado?.data) {
      dispatch(setDadosDevolutiva(resultado?.data));
    }
  }, [dispatch, match]);

  useEffect(() => {
    obterDevolutiva();
  }, [obterDevolutiva]);

  useEffect(() => {
    if (atualizarDados) {
      obterDevolutiva();
    }
    dispatch(setAtualizarDados(false));
  }, [atualizarDados, dispatch, obterDevolutiva]);

  return (
    <>
      <CardCollapse
        key="secao-devolutivas-plano-collapse-key"
        titulo="Devolutivas"
        show
        indice="secao-devolutivas-plano-collapse-indice"
        alt="secao-informacoes-plano-alt"
      >
        <SecaoDevolutivaCoordenacao
          desabilitarDevolutivaCordenacao={
            !dadosDevolutiva?.podeEditarParecerCoordenacao
          }
        />
        {dadosDevolutiva?.podeAtribuirResponsavel ? (
          <SecaoDevolutivaResponsavel />
        ) : (
          ''
        )}

        {(dadosDevolutiva?.podeEditarParecerPAAI ||
          planoAEEDados?.situacao === situacaoPlanoAEE.DevolutivaPAAI ||
          planoAEEDados?.situacao === situacaoPlanoAEE.Encerrado ||
          planoAEEDados?.situacao ===
            situacaoPlanoAEE.EncerradoAutomaticamento) && (
          <SecaoDevolutivaPaai />
        )}
      </CardCollapse>
    </>
  );
};

SecaoDevolutivasPlano.defaultProps = {
  match: {},
};

SecaoDevolutivasPlano.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default SecaoDevolutivasPlano;
