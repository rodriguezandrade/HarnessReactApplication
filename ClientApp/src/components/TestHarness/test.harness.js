import React, { useState } from 'react'
import { Form, Switch, Input, Space, Button, Card, Typography, Row, Col, Select, Tooltip, DatePicker, InputNumber, Slider, Tag, Spin } from 'antd';
import { RootTable } from '../UI/Table/table';
import { NotificationInfo } from '../UI/Notification/notification';
import { SetupHarness } from './setup.harness';
import { NotificationType } from '../../enums/notificationType';
import { useFetchServers } from '../../hooks/useFetchServers';
import { PdfGenerator } from '../UI/PDF/PdfGenerator';
import { SoundOutlined } from '@ant-design/icons';
import moment from 'moment';

export const TestHarness = () => {
  const { Option } = Select;
  const { Text, Title } = Typography;
  const [entireAudio, setEntireAudio] = useState(true);

  const headers = {
    'Accept': 'application/json',
    'Content-Type': 'application/json;charset=UTF-8'
  };

  let tableInfo = {
    columns: [
      {
        title: 'User Name',
        dataIndex: 'userDetail',
        key: 'userDetail',
        render: (text => <a>{text}</a>),
      },
      {
        title: 'Time Api Call Start',
        dataIndex: 'timeApiCallStart',
        key: 'timeApiCallStart',
      },
      {
        title: 'Time Api Call End',
        dataIndex: 'timeApiCallEnd',
        key: 'timeApiCallEnd',
      },
      {
        title: 'Api Call Status',
        dataIndex: 'apiCallStatus',
        key: 'apiCallStatus',
        render: apiCallStatus => (
          <>
            <Tag color={apiCallStatus ? "green" : "volcano"}  >
              {apiCallStatus ? "Success" : "Failed"}
            </Tag>
          </>
        ),
      },
      {
        title: 'Api Call Duration',
        dataIndex: 'timeApiCallDurationDto',
        key: 'timeApiCallDurationDto',
      },
      ,
      {
        title: 'Response Audio Size',
        dataIndex: 'bodySize',
        key: 'bodySize',
      },
      {
        title: 'Range Megabytes taken',
        dataIndex: 'rangueTaken',
        key: 'rangueTaken',
      }
    ],
    data: [],
    entireAudio: entireAudio,
    isLoading: false,
  }

  let audiosInitialValue = [{
    key: null,
    hasItem: null,
    isLoading: false,
    audioName: null,
    sessionName: null,
    duration: null,
    size: null,
  }];

  let selectData = {
    AmountUser: 1,
    ServerName: '',
    AudioName: '',
    SessionName: '',
    FromDate: '',
    ToDate: '',
    FromRange: 0,
    ToRange: 0,
    DelayUser: 0.5,
    HarnesTestAmount: 1,
    EntireAudio: false
  }

  const [table, setTable] = useState(tableInfo);
  const [data, setData] = useState(selectData);
  const [audios, setAudios] = useState(audiosInitialValue);
  const [audioInformation, setAudioInformation] = useState({});
  const [{ Success, Error, Information }] = NotificationType();
  const [executingTestHarness, setExecutingTestHarness] = useState(false);
  const [isLoadingAudio, setLoadingAudio] = useState(false);
  const [genericNotification] = NotificationInfo();
  const [servers, loadServers] = useFetchServers();

  const [dates, setDates] = useState([]);
  const [hackValue, setHackValue] = useState();
  const [value, setValue] = useState();

  const onClickRangeDatePicker = () => {
    genericNotification({ type: Information, message: 'Loading audios, please wait...' });
    setLoadingAudio(() => (true));
    setAudios(() => (audiosInitialValue))
    fetch(`/api/harness/audios/?FromDate=${data.FromDate}&ToDate=${data.ToDate}&ServerName=${data.ServerName}`, { method: 'POST' })
      .then((response) => response.json())
      .then((result) => {
        var { errors } = result;
        if (result.hasOwnProperty('errors')) {
          setLoadingAudio(() => (false));
          genericNotification({ type: Error, message: errors.Error[0] });
        } else if (result.length > 0) {
          let audiosArr = [];

          audiosArr = result.map((result, index) => ({
            key: index,
            isLoading: false,
            hasItem: result.length > 0 ? false : true,
            sessionName: result.sessionName,
            duration: result.duration,
            audioName: result.audioName,
            size: result.size,
          }));
 
          setAudios(() => (audiosArr));
          setLoadingAudio(() => (false));

        } else if (result.length === 0) {
          setAudios(() => (audiosInitialValue));
          setLoadingAudio(() => (false));
          genericNotification({ type: Information, message: 'Please select a date range with registered cases' });
        }
      }).catch((error) => {
        setLoadingAudio(() => (false));
        genericNotification({ type: Error, message: error[0] });
      });
  }

  const onChangeSwitch = (checked) => {
    // setEntireAudio(checked);
    setData(() => ({ ...data, EntireAudio: true }))
    // setTable(() => ({ ...tableInfo, entireAudio: true }));
  }

  const onChangeRangeDatePicker = (dates, dateStrings) => {
    if (dateStrings[0] !== '' && dateStrings[1] !== '' && data.ServerName !== '') {
      setData(() => ({ ...data, FromDate: dateStrings[0], ToDate: dateStrings[1] }))
    } else {
      setData(() => ({ ...data, FromDate: '', ToDate: '' }))
    }
  }

  const onchangeAudio = (name) => {
    const audioInformations = JSON.parse(JSON.stringify(audios));
    var audioSelected = audioInformations.filter(x => x.sessionName === name);

    setAudioInformation({
      ...audioInformation,
      sessionName: audioSelected[0].sessionName,
      audioName: audioSelected[0].audioName,
      size: audioSelected[0].size.toFixed(2),
      duration: audioSelected[0].duration
    });
    setData(() => ({ ...data, SessionName: audioSelected[0].sessionName, AudioName: audioSelected[0].audioName, ToRange: audioSelected[0].size }));
  }

  const onChangeServer = (serverName) => {
    setData(() => ({ ...data, ServerName: serverName }))
  }
  const onClickServer = () => {
    loadServers({ loadSetup: true });
  }

  const onChangeAmountUser = (number) => setData(() => ({
    ...data,
    AmountUser: number
  }))

  const onChangeDelayUser = (number) => setData(() => ({
    ...data,
    DelayUser: number
  }))

  const onChangeHarnessTestAmount = (number) => setData(() => ({
    ...data,
    HarnesTestAmount: number
  }))

  const isEmpty = (val) => {
    return (val === undefined || val == null || val.length <= 0) ? true : false;
  }

  const onFinish = () => {
    if (isEmpty(data.AmountUser) || isEmpty(data.ServerName) || isEmpty(data.AudioName) || isEmpty(data.FromRange) || isEmpty(data.ToRange) ||
      isEmpty(data.FromDate) || isEmpty(data.ToDate) || isEmpty(data.DelayUser) || isEmpty(data.HarnesTestAmount)) {
      genericNotification({ type: Error, message: 'Please check the information in the form' });
    } else {
      genericNotification({ type: Information, message: 'Starting Test Harness...' });

      const request = {
        AmountUser: data.AmountUser,
        ServerName: data.ServerName,
        AudioName: data.AudioName,
        FromDate: data.FromDate,
        ToDate: data.ToDate,
        FromRange: !data.EntireAudio ? parseFloat(data.FromRange) : 0,
        SessionName: data.SessionName,
        ToRange: !data.EntireAudio ? parseFloat(data.ToRange) : 0,
        UserDelay: data.DelayUser,
        TestHarnessAmount: data.HarnesTestAmount,
        EntireAudio: true
        // EntireAudio: data.EntireAudio
      }
      setExecutingTestHarness(true);
      setTable(() => ({ ...table, isLoading: true }));
      const requestOptions = { method: "POST", body: JSON.stringify(request), credentials: 'include', mode: 'cors', headers: headers };
      fetch(`/api/harness/start`, requestOptions)
        .then((response) => response.json())
        .then((response) => { 
          if (response !== undefined || response !== null || response.length >= 0) {
            setTable({
              ...table,
              data: response.map((result, index) => ({
                key: index,
                ...result,
                baseReport: result.baseReport.map((baseRep, index) => ({
                  ...baseRep,
                  key: index,
                }))
              }))
            });

            genericNotification({ type: Success, message: 'Test Harness Executed Successfully' });
            setExecutingTestHarness(false);
          } else {
            setExecutingTestHarness(false);
            setTable(() => ({ ...table, isLoading: false }));
            genericNotification({ type: Error, message: 'An error was ocurred, please review the API conexion' });
          }
        }).catch(() => {
          setExecutingTestHarness(false);
          setTable(() => ({ ...table, isLoading: false }));
          genericNotification({ type: Error, message: 'An error was ocurred, please review the API conexion' });
        })
    }
  };

  const onchangeSlider = (value, descriminator) => {
    /// using slider
    if (descriminator === undefined) {
      //  fill min/max bytes 
      setData(() => ({
        ...data,
        FromRange: value[0],
        ToRange: value[1]
      }));
    }
    /// using the inputs numbers
    else {
      // fill min bytes 
      if (descriminator === "min") {
        setData(() => ({
          ...data,
          FromRange: value
        }));
      }
      //  fill max bytes
      if (descriminator === "max") {
        setData(() => ({
          ...data,
          ToRange: value
        }));
      }
    }
  }

  const generatePdf = () => {
    if (table.data.length > 0) {
      var columns = table.columns;
      PdfGenerator({ body: table.data, header: columns.filter(x => x.title).map(x => x.title), entireAudio: true });
    }
  }

  const disabledDate = current => {
    if (!dates || dates.length === 0) {
      return false;
    }
    const tooLate = dates[0] && current.diff(dates[0], 'days') > 31;
    const tooEarly = dates[1] && dates[1].diff(current, 'days') > 31;
    return tooEarly || tooLate;
  };

  const onOpenChange = open => {
    if (open) {
      setAudioInformation({});
      setHackValue([]);
      setDates([]);
    } else {
      setHackValue(undefined);
    }
  };

  return (
    <>
      <Title style={{ marginLeft: 179 }} level={2}>Test Harness Tool</Title>
      <Spin spinning={executingTestHarness} tip="Executing Test Harness...">
        <SetupHarness> </SetupHarness>
        <Row justify="start">
          <Col flex="0 1 100px">
            <Form name="form" layout="inline" onFinish={onFinish} >
              <Col style={{ marginTop: 6 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item>
                    <Tooltip title="Amount of test harness">
                      <Text>Number Of Test Iterations &nbsp;&nbsp;&nbsp;</Text>
                    </Tooltip>
                    <InputNumber
                      onChange={(number) => onChangeHarnessTestAmount(number)}
                      style={{ width: 321 }}
                      min={1} max={10000}
                      defaultValue={1}
                      value={data.HarnesTestAmount}
                    />
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 6 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item>
                    <Tooltip title="Request delay between users">
                      <Text>Delay Request Between Users &nbsp;&nbsp;&nbsp;</Text>
                    </Tooltip>
                    <Input
                      className="site-input-split"
                      style={{
                        width: 49,
                        borderLeft: 0,
                        borderRight: 0,
                        pointerEvents: 'none',
                      }}
                      placeholder="Sec"
                      disabled
                    />
                    <InputNumber
                      className="site-input-right"
                      onChange={(number) => onChangeDelayUser(number)}
                      style={{ width: 250, textAlign: 'center' }}
                      min={0.5} max={10}
                      defaultValue={0.5}
                      value={data.DelayUser}
                    />
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 6 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item>
                    <Tooltip title="Amount of users to test">
                      <Text>Amount of Users &nbsp;&nbsp;&nbsp;</Text>
                    </Tooltip>
                    <InputNumber
                      onChange={(number) => onChangeAmountUser(number)}
                      style={{ width: 377 }}
                      min={1} max={10000}
                      defaultValue={1}
                      value={data.AmountUser}
                    />
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 12 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item >
                    <Select
                      name="server"
                      type="server"
                      showSearch
                      style={{ width: 495, overflow: 'auto' }}
                      optionFilterProp="children"
                      defaultValue={['Select Server']}
                      onChange={onChangeServer}
                      onClick={onClickServer}
                      filterOption={(input, option) =>
                        option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                      }
                    >
                      {
                        servers.data.length > 0 ? servers.data.map((server, index) => (<Option key={index} value={server}>{server}</Option>)) :
                          <Option><center><Spin size="small" tip="Loading server list, please wait..." /></center></Option>
                      }
                    </Select>
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 12 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item>
                    <Tooltip title="Select Range Dates">
                      <Text>Case Search Date Range&nbsp;&nbsp;&nbsp;&nbsp;</Text>
                    </Tooltip>
                    <DatePicker.RangePicker
                      disabledDate={disabledDate}
                      disabled={data.ServerName === ''}
                      name="dateRange"
                      type="dateRange"
                      onCalendarChange={val => setDates(val)}
                      onChange={onChangeRangeDatePicker}
                      onOpenChange={onOpenChange}
                      style={{ width: 330 }} />
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 12 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item>
                    <center>
                      <Button
                        disabled={data.FromDate === '' && data.ToDate === ''}
                        type="primary"
                        icon={<SoundOutlined />}
                        loading={isLoadingAudio}
                        onClick={onClickRangeDatePicker}
                      >
                        Search Cases Ranges!
                      </Button>
                    </center>
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 12 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item>
                    <Select
                      name="audio"
                      type="audio"
                      disabled={data.FromDate == '' || data.ToDate == ''}
                      showSearch
                      style={{ width: 495, overflow: 'auto' }}
                      optionFilterProp="children"
                      defaultValue={['Select Case']}
                      onChange={onchangeAudio}
                      filterOption={(input, option) =>
                        option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                      }
                    >
                      {
                        audios[0].audioName !== null ? audios.map((audio, index) => (audio.hasItem !== null ?
                          <Option key={audio.key} value={audio.sessionName}>{audio.sessionName}</Option> : audio.hasItem === null ?
                            <Option><center><Spin size="small" tip="Loading audios, please wait..." /></center></Option> : audio.hasItem === false ?
                              null : null)) : null
                      }
                    </Select>
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 0 }} span={24} offset={2}>
                <Space align="center">
                  <Form.Item>
                    <Card title={`Case Details: ${audioInformation.sessionName === undefined ? '' : audioInformation.sessionName}`} style={{ width: 500 }}>
                      <p>Duration: {audioInformation.duration} Secs</p>
                      <p>Size: {audioInformation.size} ~ MB</p>
                      <Switch
                        disabled={true}
                        defaultChecked
                        checkedChildren="Simulate Transcriptionist Player App Requests"
                        unCheckedChildren="Use Range Selected"
                        // disabled={audioInformation.sessionName === undefined}
                        onChange={onChangeSwitch}
                      />

                      {
                        entireAudio == false &&
                        <div>
                          <Slider
                            disabled={audioInformation.sessionName === undefined}
                            onChange={onchangeSlider}
                            min={0}
                            max={audioInformation.size === undefined ? data.ToRange : audioInformation.size}
                            range="true"
                            value={[data.FromRange, data.ToRange]}
                          />
                          <Input.Group compact>
                            <Text>Range Megabytes &nbsp;&nbsp;</Text>
                            <Input
                              className="site-input-split"
                              style={{
                                width: 50,
                                borderLeft: 0,
                                borderRight: 0,
                                pointerEvents: 'none',
                              }}
                              placeholder="Min"
                              disabled
                            />
                            <InputNumber
                              style={{ width: 100, textAlign: 'center' }}
                              placeholder="Min:"
                              disabled={audioInformation.sessionName === undefined}
                              min={0}
                              value={data.FromRange}
                              max={audioInformation.size === undefined ? data.ToRange : audioInformation.size}
                              onChange={(number) => { onchangeSlider(number, "min") }} />
                            <Input
                              className="site-input-split"
                              style={{
                                width: 50,
                                borderLeft: 0,
                                borderRight: 0,
                                pointerEvents: 'none',
                              }}
                              placeholder="Max"
                              disabled
                            />

                            <InputNumber
                              className="site-input-right"
                              style={{ width: 100, textAlign: 'center', }}
                              value={data.ToRange === 0 ? 0 : data.ToRange}
                              disabled={audioInformation.sessionName == undefined}
                              min={0}
                              max={audioInformation.size === undefined ? data.ToRange : audioInformation.size}
                              placeholder={`Max:${audioInformation.size === undefined ? '' : `: ${audioInformation.size}`}`}
                              onChange={(number) => { onchangeSlider(number, "max") }}

                            />
                          </Input.Group> </div>
                      }
                    </Card>
                  </Form.Item>
                </Space>
              </Col>

              <Col style={{ marginTop: 12 }} span={24} offset={5}>
                <Space align="center">
                  <Form.Item shouldUpdate>
                    {() => (
                      <Button
                        style={{ background: "#6AB931", borderColor: "#6AB931", marginLeft: 6 }}
                        type="primary"
                        htmlType="submit"
                        size='large'
                        disabled={isLoadingAudio === true || audioInformation.sessionName == undefined || isEmpty(data.AudioName) || isEmpty(data.SessionName)}
                        shape="round"
                      >
                        Start Test Harness
                      </Button>
                    )}
                  </Form.Item>
                </Space>

                <Space align="center">
                  <Form.Item  >
                    <Button
                      style={{ background: "#2fabe1", borderColor: "#2fabe1", marginLeft: 12 }}
                      type="primary"
                      onClick={generatePdf}
                      size='large'
                      shape="round"
                      disabled={table.data.length <= 0}
                    >
                      Generate Pdf Report
                    </Button>
                  </Form.Item>
                </Space>
              </Col>

            </Form>
          </Col>

          <Col flex="1 1 200px" span={24} offset={1}>
            <RootTable table={table}></RootTable>
          </Col>
        </Row>
      </Spin>
    </>
  )
}
