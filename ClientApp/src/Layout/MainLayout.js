import React from 'react';
import { Layout, Menu, Breadcrumb, Row, Col } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import ViqLogo from '../assets/viqLogo.png';
import { TestHarness } from '../components/TestHarness/test.harness';

export const MainLayout = () => {
    const { SubMenu } = Menu;
    const { Header, Content, Footer, Sider } = Layout;

    return (
        <Layout>
            <Header className="header">
                <Row>
                    <Col span={1}>
                        <img style={{ height: '6vh', width: '6vh' }} src={ViqLogo} alt="logo" />
                    </Col>

                    <Col span={12}>
                        <h5 style={{ marginTop: 20, color: 'white' }}>VIQ Tools</h5>
                    </Col>
                </Row>

            </Header>
            <Content style={{ padding: '0 20px' }}>
                <Breadcrumb style={{ margin: '16px 0' }}>
                    <Breadcrumb.Item>Test Harness Transcripcionist Player</Breadcrumb.Item>
                    <Breadcrumb.Item>Main</Breadcrumb.Item>
                </Breadcrumb>
                <Layout className="site-layout-background" style={{ padding: '24px 0' }}>
                    <Sider className="site-layout-background" width={310}>
                        <Menu
                            mode="inline"
                            defaultSelectedKeys={['1']}
                            defaultOpenKeys={['sub1']}
                            style={{ height: '100%' }}
                        >
                            <SubMenu key="sub1" icon={<UserOutlined />} title="Test Harness Transcripcionist Player">
                                <Menu.Item key="1">Main</Menu.Item> 
                            </SubMenu> 
                        </Menu>
                    </Sider>
                    <Content style={{ padding: '0 24px', minHeight: 280 }}>
                        <TestHarness></TestHarness>
                    </Content>
                </Layout>
            </Content>
            <Footer style={{ textAlign: 'center' }}>© Copyright VIQ Solutions Inc. {new Date().getFullYear()}. All Rights Reserved.</Footer>
        </Layout>
    )
}