import React, { useCallback, useEffect, useState } from 'react';
import PropTypes from 'prop-types';

import { useDispatch, useSelector } from 'react-redux';
import { CardCollapse, Editor } from '~/componentes';

import { erros, verificaSomenteConsulta } from '~/servicos';
import { RotasDto } from '~/dtos';

import SecaoDevolutivaCoordenacao from '../SecaoDevolutivaCoordenacao/secaoDevolutivaCoordenacao';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import {
  setExibirLoaderPlanoAEE,
  setDadosDevolutiva,
} from '~/redux/modulos/planoAEE/actions';
import SecaoDevolutivaResponsavel from '../SecaoDevolutivaResponsavel/secaoDevolutivaResponsavel';
import SecaoDevolutivaPaai from '../SecaoDevolutivaPaai/secaoDevolutivaPaai';

const SecaoDevolutivasPlano = ({ match }) => {
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);
  const dadosDevolutiva = useSelector(store => store.planoAEE.dadosDevolutiva);
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];
  const somenteConsulta = useSelector(store => store.navegacao.somenteConsulta);

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
          match={match}
        />
        {dadosDevolutiva?.podeAtribuirResponsavel && (
          <SecaoDevolutivaResponsavel />
        )}

        {!dadosDevolutiva?.podeEditarParecerPAAI && <SecaoDevolutivaPaai />}
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
