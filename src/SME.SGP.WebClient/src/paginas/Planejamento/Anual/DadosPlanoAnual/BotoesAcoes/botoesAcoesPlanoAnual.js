import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import {
  limparDadosPlanoAnual,
  setComponenteCurricularPlanoAnual,
  setExibirModalCopiarConteudo,
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

  const listaTurmasParaCopiar = useSelector(
    store => store.planoAnual.listaTurmasParaCopiar
  );

  const planejamentoAnualId = useSelector(
    store => store.planoAnual.planejamentoAnualId
  );

  const planoAnualSomenteConsulta = useSelector(
    store => store.planoAnual.planoAnualSomenteConsulta
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
    if (planoAnualEmEdicao && !planoAnualSomenteConsulta) {
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
    if (planoAnualEmEdicao && !planoAnualSomenteConsulta) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        dispatch(limparDadosPlanoAnual());
        const componente = { ...componenteCurricular };
        dispatch(setComponenteCurricularPlanoAnual(componente));
      }
    }
  };

  const abrirCopiarConteudo = async () => {
    dispatch(setExibirModalCopiarConteudo(true));
  };

  return (
    <>
      <Button
        id="btn-copiar-conteudo-plano-anual"
        label="Copiar Conteúdo"
        icon="share-square"
        color={Colors.Azul}
        className="mr-3"
        border
        onClick={abrirCopiarConteudo}
        disabled={
          !planejamentoAnualId ||
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ||
          planoAnualSomenteConsulta ||
          planoAnualEmEdicao ||
          !listaTurmasParaCopiar ||
          listaTurmasParaCopiar.length === 0
        }
      />
      
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
          planoAnualSomenteConsulta ||
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
          planoAnualSomenteConsulta ||
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ||
          !planoAnualEmEdicao
        }
      />
    </>
  );
};

export default BotoesAcoesPlanoAnual;
