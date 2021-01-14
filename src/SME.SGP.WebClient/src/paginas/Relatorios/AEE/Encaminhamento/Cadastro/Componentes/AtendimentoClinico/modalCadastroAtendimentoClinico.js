import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import {
  CampoData,
  CampoTexto,
  Colors,
  ModalConteudoHtml,
  SelectComponent,
} from '~/componentes';
import Button from '~/componentes/button';
import {
  setExibirModalCadastroAtendimentoClinicoAEE,
  setDadosAtendimentoClinicoAEE,
} from '~/redux/modulos/encaminhamentoAEE/actions';

const ModalCadastroAtendimentoClinico = () => {
  const dispatch = useDispatch();

  const exibirModalCadastroAtendimentoClinicoAEE = useSelector(
    store => store.encaminhamentoAEE.exibirModalCadastroAtendimentoClinicoAEE
  );

  const [diaSemana, setDiaSemana] = useState('dom');
  const [atendimentoAtividade, setAtendimentoAtividade] = useState('');
  const [localRealizacao, setLocalRealizacao] = useState('');
  const [horarioInicio, setHorarioInicio] = useState();
  const [horarioTermino, setHorarioTermino] = useState();

  const listaDiasSemana = [
    {
      valor: 'dom',
      desc: 'Domingo',
    },
    {
      valor: 'seg',
      desc: 'Segunda',
    },
    {
      valor: 'ter',
      vadesclor: 'Terça',
    },
    {
      valor: 'qua',
      desc: 'Quarta',
    },
    {
      valor: 'qui',
      desc: 'Quinta',
    },
    {
      valor: 'sex',
      desc: 'Sexta',
    },
    {
      valor: 'sab',
      desc: 'Sábado',
    },
  ];

  const onChangeDiaSemana = valor => {
    setDiaSemana(valor);
  };

  const onChangeAtendimentoAtividade = e => {
    setAtendimentoAtividade(e.target.value);
  };

  const onChangeLocalRealizacao = e => {
    setLocalRealizacao(e.target.value);
  };

  const onChangeHorarioInicio = valor => {
    setHorarioInicio(valor);
  };

  const onChangeHorarioTermino = valor => {
    setHorarioTermino(valor);
  };

  const onClose = () => {
    dispatch(setExibirModalCadastroAtendimentoClinicoAEE(false));
  };

  const onSalvar = () => {
    dispatch(
      setDadosAtendimentoClinicoAEE([
        {
          diaSemana,
          atendimentoAtividade,
          localRealizacao,
          horarioInicio,
          horarioTermino,
        },
      ])
    );
    onClose();
  };

  return (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="detalhamento-atendimento-clinico"
      visivel={exibirModalCadastroAtendimentoClinicoAEE}
      titulo="Detalhamento de atendimento clínico"
      onClose={onClose}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <div className="col-md-12 mb-2">
        <SelectComponent
          label="Dias da Semana"
          lista={listaDiasSemana}
          valueOption="valor"
          valueText="desc"
          valueSelect={diaSemana}
          onChange={onChangeDiaSemana}
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoTexto
          onChange={onChangeAtendimentoAtividade}
          label="Atendimento/Atividade"
          value={atendimentoAtividade}
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoTexto
          onChange={onChangeLocalRealizacao}
          label="Local de realização"
          value={localRealizacao}
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoData
          onChange={onChangeHorarioInicio}
          label="Horário de início"
          value={horarioInicio}
          placeholder="09:00"
          formatoData="HH:mm"
          somenteHora
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoData
          onChange={onChangeHorarioTermino}
          value={horarioTermino}
          label="Horário término"
          placeholder="09:30"
          formatoData="HH:mm"
          somenteHora
        />
      </div>

      <div className="col-md-12 mt-2 p-0 d-flex justify-content-end">
        <Button
          key="btn-voltar"
          id="btn-voltar"
          label="Cancelar"
          color={Colors.Azul}
          border
          onClick={onClose}
          className="mt-2 mr-2"
        />
        <Button
          key="btn-salvar"
          id="btn-salvar"
          label="Adicionar Registro"
          color={Colors.Roxo}
          border
          onClick={onSalvar}
          className="mt-2"
        />
      </div>
    </ModalConteudoHtml>
  );
};

export default ModalCadastroAtendimentoClinico;
