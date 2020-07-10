import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import CampoNumero from '~/componentes/campoNumero';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoFaltasFrequencia from '~/servicos/Paginas/Relatorios/FaltasFrequencia/ServicoFaltasFrequencia';
import FiltroHelper from '~componentes-sgp/filtro/helper';

const FaltasFrequencia = () => {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaSemestre, setListaSemestre] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaAnosEscolares, setListaAnosEscolares] = useState([]);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [listaBimestre, setListaBimestre] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);
  const [modalidadeId, setModalidadeId] = useState(undefined);
  const [semestre, setSemestre] = useState(undefined);
  const [anosEscolares, setAnosEscolares] = useState(undefined);
  const [componenteCurricular, setComponenteCurricular] = useState(undefined);
  const [bimestre, setBimestre] = useState(undefined);
  const [valorComparacao, setValorComparacao] = useState(undefined);

  const [listaTipoRelatorio] = useState([
    { valor: 'frequencia', desc: 'Frequência' },
    { valor: 'faltas', desc: 'Faltas' },
    { valor: 'ambos', desc: 'Ambos' },
  ]);
  const [tipoRelatorio, setTipoRelatorio] = useState(undefined);

  const [listaCondicao] = useState([
    { valor: 'igual', desc: 'Igual' },
    { valor: 'maior', desc: 'Maior ' },
    { valor: 'menor', desc: 'Menor' },
  ]);
  const [condicao, setCondicao] = useState(undefined);

  const listaFormatos = [
    { valor: '1', desc: 'PDF' },
    { valor: '4', desc: 'EXCEL' },
  ];
  const [formato, setFormato] = useState('1');

  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [desabilitarBtnGerar, setDesabilitarBtnGerar] = useState(true);

  const obterAnosLetivos = useCallback(async () => {
    const anosLetivo = await AbrangenciaServico.buscarTodosAnosLetivos().catch(
      e => erros(e)
    );
    if (anosLetivo && anosLetivo.data) {
      const a = [];
      anosLetivo.data.forEach(ano => {
        a.push({ desc: ano, valor: ano });
      });
      setAnoLetivo(a[0].valor);
      setListaAnosLetivo(a);
    } else {
      setListaAnosLetivo([]);
    }
  }, []);

  const obterModalidades = async (ue, ano) => {
    if (ue && ano) {
      // TODO - Add endpoint novo com a opção todas!
      const { data } = await api.get(`/v1/ues/${ue}/modalidades?ano=${ano}`);
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: String(item.id),
        }));

        if (lista && lista.length && lista.length === 1) {
          setModalidadeId(lista[0].valor);
        }
        setListaModalidades(lista);
      }
    }
  };

  const obterUes = useCallback(async dre => {
    if (dre) {
      // TODO - Add endpoint novo com a opção todas!
      const { data } = await AbrangenciaServico.buscarUes(dre);
      if (data) {
        const lista = data
          .map(item => ({
            desc: `${tipoEscolaDTO[item.tipoEscola]} ${item.nome}`,
            valor: String(item.codigo),
          }))
          .sort(FiltroHelper.ordenarLista('desc'));

        if (lista && lista.length && lista.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, []);

  const onChangeDre = dre => {
    setDreId(dre);

    setListaUes([]);
    setUeId(undefined);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
  };

  const obterDres = async () => {
    // TODO - Add endpoint novo com a opção todas!
    const { data } = await AbrangenciaServico.buscarDres();
    if (data && data.length) {
      const lista = data
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
    }
  };

  const obterSemestres = async (
    modalidadeSelecionada,
    anoLetivoSelecionado
  ) => {
    const retorno = await api.get(
      `v1/abrangencias/false/semestres?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
        0}`
    );
    if (retorno && retorno.data) {
      const lista = retorno.data.map(periodo => {
        return { desc: periodo, valor: periodo };
      });

      if (lista && lista.length && lista.length === 1) {
        setSemestre(lista[0].valor);
      }
      setListaSemestre(lista);
    }
  };

  useEffect(() => {
    if (anoLetivo && ueId) {
      obterModalidades(ueId, anoLetivo);
    } else {
      setModalidadeId(undefined);
      setListaModalidades([]);
    }
  }, [anoLetivo, ueId]);

  useEffect(() => {
    if (dreId) {
      obterUes(dreId);
    } else {
      setUeId(undefined);
      setListaUes([]);
    }
  }, [dreId, obterUes]);

  const obterAnosEscolares = useCallback(async () => {
    // TODO - Add endpoint novo com a opção todas!
    setTimeout(() => {
      setListaAnosEscolares([
        { desc: '1', valor: '1' },
        { desc: '3', valor: '2' },
        { desc: '3', valor: '3' },
        { desc: '4', valor: '4' },
      ]);
    }, 2000);
  }, []);

  useEffect(() => {
    if (modalidadeId && ueId) {
      obterAnosEscolares(modalidadeId, ueId);
    } else {
      setAnosEscolares(undefined);
      setListaAnosEscolares([]);
    }
  }, [modalidadeId, ueId, obterAnosEscolares]);

  const obterComponenteCurricular = () => {
    // TODO - Add endpoint novo com a opção todas!
    setTimeout(() => {
      setListaComponenteCurricular([
        { desc: 'Matemática', valor: '1' },
        { desc: 'Geografia', valor: '2' },
        { desc: 'Inglês', valor: '3' },
        { desc: 'Arte', valor: '4' },
        { desc: 'Todos', valor: 'todos' },
      ]);
    }, 2000);
  };

  useEffect(() => {
    setListaComponenteCurricular([]);
    setComponenteCurricular(undefined);
    obterComponenteCurricular();
  }, [anosEscolares]);

  const obterBimestres = () => {
    // TODO - Add endpoint novo com a opção todas!
    setTimeout(() => {
      setListaBimestre([
        { desc: '1º', valor: '1' },
        { desc: '2º', valor: '2' },
        { desc: '3º', valor: '3' },
        { desc: '4º', valor: '4' },
        { desc: 'Final', valor: 'final' },
        { desc: 'Todos', valor: 'todos' },
      ]);
    }, 2000);
  };

  useEffect(() => {
    setListaBimestre([]);
    setBimestre(undefined);
    obterBimestres();
  }, [componenteCurricular]);

  useEffect(() => {
    if (modalidadeId && anoLetivo) {
      if (modalidadeId == modalidade.EJA) {
        obterSemestres(modalidadeId, anoLetivo);
      } else {
        setSemestre(undefined);
        setListaSemestre([]);
      }
    } else {
      setSemestre(undefined);
      setListaSemestre([]);
    }
  }, [modalidadeId, anoLetivo]);

  useEffect(() => {
    const desabilitar =
      !anoLetivo ||
      !dreId ||
      !ueId ||
      !modalidadeId ||
      !anosEscolares ||
      !formato;

    if (modalidadeId == modalidade.EJA) {
      setDesabilitarBtnGerar(!semestre || desabilitar);
    } else {
      setDesabilitarBtnGerar(desabilitar);
    }
  }, [anoLetivo, dreId, ueId, modalidadeId, anosEscolares, formato, semestre]);

  useEffect(() => {
    obterAnosLetivos();
    obterDres();
  }, [obterAnosLetivos]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {
    setAnoLetivo(undefined);
    setDreId(undefined);
    setListaAnosLetivo([]);
    setListaDres([]);

    obterAnosLetivos();
    obterDres();

    setFormato('PDF');
  };

  const onClickGerar = async () => {
    setCarregandoGeral(true);
    const params = {
      anoLetivo,
      dreId,
      ueId,
      modalidadeId,
      semestre,
      anosEscolares,
      componenteCurricular,
      bimestre,
      tipoRelatorio,
      condicao,
      valorComparacao,
      formato,
    };
    const retorno = await ServicoFaltasFrequencia.gerar(params).catch(e =>
      erros(e)
    );
    if (retorno && retorno.status === 200) {
      sucesso(
        'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
      );
      setDesabilitarBtnGerar(true);
    }
    setCarregandoGeral(false);
  };

  const onChangeUe = ue => {
    setUeId(ue);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
  };

  const onChangeModalidade = novaModalidade => {
    setModalidadeId(novaModalidade);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
  };

  const onChangeAnos = valor => setAnosEscolares(valor);

  const onChangeSemestre = valor => setSemestre(valor);
  const onChangeComponenteCurricular = valor => setComponenteCurricular(valor);
  const onChangeBimestre = valor => setBimestre(valor);
  const onChangeTipoRelatorio = valor => setTipoRelatorio(valor);
  const onChangeCondicao = valor => setCondicao(valor);
  const onChangeComparacao = valor => setValorComparacao(valor);
  const onChangeFormato = valor => setFormato(valor);

  return (
    <>
      <Cabecalho pagina="Frequência e faltas" />
      <Loader loading={carregandoGeral}>
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  id="btn-voltar-frequencia-faltas"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
                <Button
                  id="btn-cancelar-frequencia-faltas"
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-3"
                  onClick={() => onClickCancelar()}
                />
                <Button
                  id="btn-gerar-frequencia-faltas"
                  icon="print"
                  label="Gerar"
                  color={Colors.Azul}
                  border
                  bold
                  className="mr-2"
                  onClick={() => onClickGerar()}
                  disabled={desabilitarBtnGerar}
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
                <SelectComponent
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo && listaAnosLetivo.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Selecione o ano"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-9 col-xl-5 mb-2">
                <SelectComponent
                  label="DRE"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaDres && listaDres.length === 1}
                  onChange={onChangeDre}
                  valueSelect={dreId}
                  placeholder="Diretoria Regional de Educação (DRE)"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-9 col-xl-5 mb-2">
                <SelectComponent
                  label="UE"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaUes && listaUes.length === 1}
                  onChange={onChangeUe}
                  valueSelect={ueId}
                  placeholder="Unidade Escolar (UE)"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  label="Modalidade"
                  lista={listaModalidades}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaModalidades && listaModalidades.length === 1}
                  onChange={onChangeModalidade}
                  valueSelect={modalidadeId}
                  placeholder="Selecione uma modalidade"
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-2 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaSemestre}
                  valueOption="valor"
                  valueText="desc"
                  label="Semestre"
                  disabled={
                    !modalidadeId ||
                    modalidadeId == modalidade.FUNDAMENTAL ||
                    (listaSemestre && listaSemestre.length === 1)
                  }
                  valueSelect={semestre}
                  onChange={onChangeSemestre}
                  placeholder="Selecione o semestre"
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-2 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaAnosEscolares}
                  valueOption="valor"
                  valueText="desc"
                  label="Ano"
                  disabled={
                    listaAnosEscolares && listaAnosEscolares.length === 1
                  }
                  valueSelect={anosEscolares}
                  onChange={onChangeAnos}
                  placeholder="Selecione o ano"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-5 col-xl-5 mb-2">
                <SelectComponent
                  lista={listaComponenteCurricular}
                  valueOption="valor"
                  valueText="desc"
                  label="Componente Curricular"
                  disabled={
                    listaComponenteCurricular &&
                    listaComponenteCurricular.length === 1
                  }
                  valueSelect={componenteCurricular}
                  onChange={onChangeComponenteCurricular}
                  placeholder="Selecione o componente curricular"
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-3 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaBimestre}
                  valueOption="valor"
                  valueText="desc"
                  label="Bimestre"
                  disabled={listaBimestre && listaBimestre.length === 1}
                  valueSelect={bimestre}
                  onChange={onChangeBimestre}
                  placeholder="Selecione o bimestre"
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  lista={listaTipoRelatorio}
                  valueOption="valor"
                  valueText="desc"
                  label="Tipo de relatório"
                  disabled={
                    listaTipoRelatorio && listaTipoRelatorio.length === 1
                  }
                  valueSelect={tipoRelatorio}
                  onChange={onChangeTipoRelatorio}
                  placeholder="Selecione o tipo"
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-3 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaCondicao}
                  valueOption="valor"
                  valueText="desc"
                  label="Condição"
                  disabled={listaCondicao && listaCondicao.length === 1}
                  valueSelect={condicao}
                  onChange={onChangeCondicao}
                  placeholder="Selecione a condição"
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-3 col-xl-2 mb-2">
                <CampoNumero
                  onChange={onChangeComparacao}
                  value={valorComparacao}
                  min={0}
                  label="Valor"
                  className="w-100"
                  placeholder="Digite o valor"
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  label="Formato"
                  lista={listaFormatos}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={formato}
                  onChange={onChangeFormato}
                  disabled
                  placeholder="Selecione o formato"
                />
              </div>
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default FaltasFrequencia;
