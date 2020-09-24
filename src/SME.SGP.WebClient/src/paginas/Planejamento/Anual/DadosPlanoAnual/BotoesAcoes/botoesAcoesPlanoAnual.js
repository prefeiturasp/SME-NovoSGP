import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import {
  limparDadosPlanoAnual,
  setComponenteCurricularPlanoAnual,
} from '~/redux/modulos/anual/actions';
import { confirmar } from '~/servicos';
import history from '~/servicos/history';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import servicoSalvarPlanoAnual from '../../servicoSalvarPlanoAnual';

const BotoesAcoesPlanoAnual = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const planoAnualEmEdicao = useSelector(
    store => store.planoAnual.planoAnualEmEdicao
  );

  const componenteCurricular = useSelector(
    store => store.planoAnual.componenteCurricular
  );

  const onSalvar = async () => {
    const salvou = await servicoSalvarPlanoAnual.validarSalvarPlanoAnual();
    return salvou;
  };

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async () => {
    if (planoAnualEmEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        const salvou = await onSalvar();
        if (salvou) {
          history.push(URL_HOME);
        }
      } else {
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const onCancelar = async () => {
    if (planoAnualEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        // TODO Esta forlando a recarregar tela, ajustar!!!
        dispatch(limparDadosPlanoAnual());
        const componente = { ...componenteCurricular };
        dispatch(setComponenteCurricularPlanoAnual(componente));
      }
    }
  };

  return (
    <>
      <Button
        id="btn-voltar-plano-anual"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-3"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar-plano-anual"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-3"
        onClick={onCancelar}
        disabled={
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ||
          !planoAnualEmEdicao
        }
      />
      <Button
        id="btn-salvar-plano-anual"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        onClick={onSalvar}
        disabled={
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ||
          !planoAnualEmEdicao
        }
      />
    </>
  );
};

export default BotoesAcoesPlanoAnual;
