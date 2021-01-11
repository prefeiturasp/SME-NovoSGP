import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Loader, SelectComponent } from '~/componentes';
import { FiltroHelper } from '~/componentes-sgp';
import LocalizadorEstudante from '~/componentes/LocalizadorEstudante';
import { AbrangenciaServico, erros } from '~/servicos';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';

const SecaoLocalizarEstudanteDados = () => {
  // const dispatch = useDispatch();

  const codigosAlunosSelecionados = useSelector(
    state => state.localizadorEstudante.codigosAluno
  );

  const anoAtual = '2020';
  // const [anoAtual] = useState(window.moment().format('YYYY'));

  const listaAnosLetivo = [
    {
      desc: anoAtual,
      valor: anoAtual,
    },
  ];

  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);

  const [dreId, setDreId] = useState();
  const [ueId, setUeId] = useState();
  const [turmaId, setTurmaId] = useState();

  const [
    alunoLocalizadorSelecionado,
    setAlunoLocalizadorSelecionado,
  ] = useState();

  const [carregandoTurmas, setCarregandoTurmas] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);

  useEffect(() => {
    if (codigosAlunosSelecionados?.length > 0) {
      setTurmaId();
    }
  }, [codigosAlunosSelecionados]);

  const [carregandoUes, setCarregandoUes] = useState(false);

  const obterUes = useCallback(async () => {
    if (dreId) {
      setCarregandoUes(true);
      const resposta = await AbrangenciaServico.buscarUes(
        dreId,
        `v1/abrangencias/false/dres/${dreId}/ues?anoLetivo=${anoAtual}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoUes(false));

      if (resposta?.data) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista?.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, [dreId, anoAtual]);

  useEffect(() => {
    if (dreId) {
      obterUes();
    } else {
      setUeId();
      setListaUes([]);
    }
  }, [dreId, obterUes]);

  const onChangeDre = dre => {
    setDreId(dre);

    setListaUes([]);
    setUeId();

    setListaTurmas([]);
    setTurmaId();
  };

  const obterDres = useCallback(async () => {
    if (anoAtual) {
      setCarregandoDres(true);
      const resposta = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/false/dres?anoLetivo=${anoAtual}`
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoDres(false));

      if (resposta?.data?.length) {
        const lista = resposta.data
          .map(item => ({
            desc: item.nome,
            valor: String(item.codigo),
            abrev: item.abreviacao,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setDreId(lista[0].valor);
        }
      } else {
        setListaDres([]);
        setDreId(undefined);
      }
    }
  }, [anoAtual]);

  useEffect(() => {
    if (anoAtual) {
      obterDres();
    }
  }, [anoAtual, obterDres]);

  const obterTurmas = useCallback(async () => {
    if (ueId) {
      setCarregandoTurmas(true);
      const resposta = await AbrangenciaServico.buscarTurmas(
        ueId,
        0,
        '',
        anoAtual,
        false
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoTurmas(false));

      if (resposta?.data) {
        setListaTurmas(resposta.data);

        if (resposta?.length === 1) {
          setTurmaId(resposta[0].valor);
        }
      }
    }
  }, [anoAtual, ueId]);

  useEffect(() => {
    if (ueId) {
      obterTurmas();
    } else {
      setTurmaId();
      setListaTurmas([]);
    }
  }, [ueId, obterTurmas]);

  const onChangeUe = ue => {
    setUeId(ue);

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeTurma = valor => {
    setTurmaId(valor);
    setAlunoLocalizadorSelecionado();
  };

  const onChangeLocalizadorEstudante = aluno => {
    if (aluno?.alunoCodigo && aluno?.alunoNome) {
      setAlunoLocalizadorSelecionado(aluno);
    } else {
      setAlunoLocalizadorSelecionado();
    }
  };

  const onClickProximoPasso = async () => {
    // TODO
    console.log('onClickProximoPasso');
  };

  const onClickCancelar = async () => {
    // TODO
    console.log('onClickCancelar');
  };

  return (
    <div className="row">
      <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
        <SelectComponent
          id="ano-letivo"
          label="Ano Letivo"
          lista={listaAnosLetivo}
          valueOption="valor"
          valueText="desc"
          disabled
          valueSelect={anoAtual}
          placeholder="Ano letivo"
        />
      </div>
      <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
        <Loader loading={carregandoDres} tip="">
          <SelectComponent
            id="dre"
            label="Diretoria Regional de Educação (DRE)"
            lista={listaDres}
            valueOption="valor"
            valueText="desc"
            disabled={!anoAtual || listaDres?.length === 1}
            onChange={onChangeDre}
            valueSelect={dreId}
            placeholder="Diretoria Regional De Educação (DRE)"
          />
        </Loader>
      </div>
      <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
        <Loader loading={carregandoUes} tip="">
          <SelectComponent
            id="ue"
            label="Unidade Escolar (UE)"
            lista={listaUes}
            valueOption="valor"
            valueText="desc"
            disabled={!dreId || listaUes?.length === 1}
            onChange={onChangeUe}
            valueSelect={ueId}
            placeholder="Unidade Escolar (UE)"
          />
        </Loader>
      </div>
      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-3 mb-2">
        <Loader loading={carregandoTurmas} tip="">
          <SelectComponent
            id="turma"
            lista={listaTurmas}
            valueOption="codigo"
            valueText="modalidadeTurmaNome"
            label="Turma"
            disabled={listaTurmas?.length === 1}
            valueSelect={turmaId}
            onChange={onChangeTurma}
            placeholder="Turma"
          />
        </Loader>
      </div>
      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-9 mb-2">
        <div className="row">
          <LocalizadorEstudante
            id="estudante"
            showLabel
            ueId={ueId}
            onChange={onChangeLocalizadorEstudante}
            anoLetivo={anoAtual}
            desabilitado={!dreId || !ueId}
            codigoTurma={turmaId}
          />
        </div>
      </div>
      <div className="col-md-12 d-flex justify-content-end pb-4 mt-2">
        <Button
          id="btn-cancelar"
          label="Cancelar"
          color={Colors.Roxo}
          border
          className="mr-3"
          onClick={onClickCancelar}
        />
        <Button
          id="btn-proximo-passo"
          label="Próximo passo"
          color={Colors.Roxo}
          border
          bold
          onClick={onClickProximoPasso}
        />
      </div>
    </div>
  );
};

export default SecaoLocalizarEstudanteDados;
