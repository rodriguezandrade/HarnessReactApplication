import React, { useEffect } from 'react';
import { Collapse, Input, Row, Form, Col, Button, Space, Tooltip, Typography, Divider } from 'antd';
import { ApiOutlined } from '@ant-design/icons';
import { NotificationInfo } from '../UI/Notification/notification';
import { NotificationType } from '../../enums/notificationType';
import { useForm } from '../../hooks/useForm';

export const SetupHarness = () => {
    const { Panel } = Collapse;
    const { Text } = Typography;
    const headers = {
        'Accept': 'application/json',
        'Content-Type': 'application/json;charset=UTF-8'
    };

    const [{ Success, Error }] = NotificationType();
    const [genericNotification] = NotificationInfo();

    let harnessSetup = {
        AccessPointPortalApiUrl: '',
        TranscriberHubApiUrl: '',
        Email: '',
        Password: ''
    }
    const [testHarnessSetup, handleInputChange, setValues] = useForm(harnessSetup);

    useEffect(() => {
        fetch(`/api/harness/load`, { method: 'GET' })
            .then((response) => response.json())
            .then((result) => {
                if (result != null) {
                    setValues({
                        ...testHarnessSetup,
                        AccessPointPortalApiUrl: (result.accessPointPortalApiUrl.replace('https://', '')).replace('/', ''),
                        TranscriberHubApiUrl: (result.transcriberHubApiUrl.replace('https://', '')).replace('/', ''),
                        Email: result.adminUserLogin.email,
                        Password: result.adminUserLogin.password
                    });
                }
            }).catch(() => {
                genericNotification({ type: Error, message: 'An error was ocurred, please review the WebPortalAPI conexion' });
            });
    }, []);

    const { AccessPointPortalApiUrl, TranscriberHubApiUrl, Email, Password } = testHarnessSetup;

    const isEmpty = (val) => {
        return (val === undefined || val == null || val.length <= 0) ? true : false;
    }

    const onFinish = () => {

        if (isEmpty(AccessPointPortalApiUrl) || isEmpty(TranscriberHubApiUrl) || isEmpty(Email) || isEmpty(Password)) {
            genericNotification({ type: Error, message: 'Please review the setup harness section, cannot be empty' });
        } else {
            const request = {
                AccessPointPortalApiUrl: `https://${AccessPointPortalApiUrl}/`,
                TranscriberHubApiUrl: `https://${TranscriberHubApiUrl}/`,
                AdminUserLogin: {
                    Email: Email,
                    Password: Password
                }
            }

            const requestOptions = { method: "POST", body: JSON.stringify(request), credentials: 'include', mode: 'cors', headers: headers };
            fetch(`/api/harness/setup`, requestOptions)
                .then((response) => response.json())
                .then((response) => {
                    if (response !== undefined || response !== null || response.length >= 0) {
                        genericNotification({ type: Success, message: 'Setup set Successfully' });

                    } else {
                        genericNotification({
                            type: Error, message: 'An error was ocurred, please review the setup harness section'
                        });
                    }
                })
                .catch(() => {
                    genericNotification({ type: Error, message: 'An error was ocurred, please review the setup harness section' });
                })
        }
    }

    return (

        <Row justify="start">
            <Col flex="0 1 540px">
                <Collapse
                    expandIcon={({ isActive }) => <ApiOutlined style={{ color: '#f5222d', width: 500 }} rotate={isActive ? 90 : 0} />}
                    defaultActiveKey={['0']}
                    ghost
                >
                    <Panel header="Test Harness Setting" key="1" flex="0 1 522px">
                        <Row justify="start">
                            <Col flex="0 1 522px">
                                <Form name="form" layout="inline" onFinish={onFinish} >
                                    <Form.Item >
                                        <Tooltip title="Access Point Portal Web API endpoint">
                                            <Text>Access Point Portal Web API &nbsp;&nbsp;&nbsp;</Text>
                                        </Tooltip>
                                        <Input
                                            name='AccessPointPortalApiUrl'
                                            prefix="https://" suffix="/"
                                            className="site-input-right"
                                            onChange={handleInputChange}
                                            style={{ width: 334, marginTop: 6 }}
                                            value={AccessPointPortalApiUrl}
                                            placeholder="domain:port"
                                        />
                                    </Form.Item>

                                    <Form.Item>
                                        <Tooltip title="Enter the Transcriber Hub API">
                                            <Text>Enter the Transcriber Hub API &nbsp;&nbsp;&nbsp;</Text>
                                        </Tooltip>
                                        <Input
                                            name="TranscriberHubApiUrl"
                                            prefix="https://" suffix="/"
                                            className="site-input-right"
                                            onChange={handleInputChange}
                                            style={{ width: 326, marginTop: 6 }}
                                            value={TranscriberHubApiUrl}
                                            placeholder="domain:port"
                                        />
                                    </Form.Item>

                                    <Form.Item>
                                        <Tooltip title="Enter the admin email">
                                            <Text>Enter the Admin Email &nbsp;&nbsp;&nbsp;</Text>
                                        </Tooltip>
                                        <Input
                                            name="Email"
                                            type="email"
                                            className="site-input-right"
                                            onChange={handleInputChange}
                                            style={{ width: 370, marginTop: 6 }}
                                            value={Email}
                                            placeholder="Enter the Admin Email"
                                        />
                                    </Form.Item>
                                    <Form.Item>
                                        <Tooltip title="Enter the admin password">
                                            <Text>Enter the Admin Password &nbsp;&nbsp;&nbsp;</Text>
                                        </Tooltip>
                                        <Input
                                            name="Password"
                                            type="password"
                                            className="site-input-right"
                                            onChange={handleInputChange}
                                            style={{ width: 345, marginTop: 6 }}
                                            value={Password}
                                            placeholder="Enter the Admin Password"
                                        />
                                    </Form.Item>

                                    <Col style={{ marginTop: 4 }} span={24} offset={9}>
                                        <Space align="center">
                                            <Form.Item shouldUpdate>
                                                {() => (
                                                    <Button
                                                        style={{ background: "#2fabe1", borderColor: "#2fabe1", marginLeft: 12, marginTop: 5 }}
                                                        type="primary"
                                                        htmlType="submit"
                                                        size='large'
                                                        shape="round"
                                                    >
                                                        Set Configuration
                                                    </Button>
                                                )}
                                            </Form.Item>
                                        </Space>

                                    </Col>
                                </Form>
                            </Col>
                        </Row>
                        <Divider />
                    </Panel>
                </Collapse>
            </Col>
        </Row>
    )
}
