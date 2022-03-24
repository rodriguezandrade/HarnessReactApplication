import React from 'react'
import { Table, Badge, Tag, Statistic, Row, Col, Space } from "antd";
import { ExperimentOutlined } from '@ant-design/icons';

export const RootTable = ({ table }) => {
    let entireAudioTable;
    let baseReportData = [];
    if (table.entireAudio && table.data !== undefined) {
        entireAudioTable = {
            columns: [
                { title: 'User Number', dataIndex: 'key', key: 'key' },
                { title: 'User Name', dataIndex: 'userDetail', key: 'userDetail' }
                // ,
                // {
                //     title: 'Audio Chunking Detail Time Order(Minutes:Seconds:Milliseconds)',
                //     dataIndex: 'chunks',
                //     key: 'chunks',
                //     width: '50%',
                //     render: chunks => (
                //         <>
                //             {chunks.map((chunk, index) => {
                //                 return (
                //                     <div key={index}>
                //                         <br />
                //                          Chunk Number: <b>{chunk.chunkNumber},</b> ChunkRangeTaken: <b>{chunk.chunkRangeTaken}</b> , ChunkTimeCallStart: {chunk.chunkTimeCallStart}, ChunkTimeCallEnd: {chunk.chunkTimeCallEnd}, ChunkTimeCallDuration: <b>{chunk.chunkTimeCallDuration}</b>, ResponseChunkSize:  <b>{chunk.responseChunkSize} </b>
                //                     </div>
                //                 );
                //             })}
                //         </>
                //     ),
                // },
                // {
                //     title: 'Api Call Status',
                //     dataIndex: 'apiCallStatus',
                //     key: 'apiCallStatus',
                //     width: '10%',
                //     render: apiCallStatus => (
                //         <>
                //             <Tag color={apiCallStatus ? "green" : "volcano"}  >
                //                 {apiCallStatus ? "Success" : "Failed"}
                //             </Tag>
                //         </>
                //     ),
                // }
            ]
        }
    }

    return (
        <>
            {table.data?.length > 0 && table.data.map((tableDetail, index) =>
            (
                !table.entireAudio ?
                    /// using range bytes
                    <div className="ant-col ant-col-24 ant-col-offset-1" id={`table_${index}`} key={index}>
                        <Row gutter={[48, 16]} align="middle" justify="space-around">
                            <Col span={4} >
                                <Space align="center">
                                    <Statistic title="Test Harness Summary" value={index + 1} prefix={<ExperimentOutlined />} />
                                </Space>
                            </Col>
                            <Col span={4}>
                                <Row align="middle" justify="center" >
                                    <Tag color="#87d068"> Tested Users </Tag>
                                </Row>

                                <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                    <Badge overflowCount={1000000} size="small" count={tableDetail.summaryReport.testedUsers} />
                                </Row>

                            </Col>
                            <Col span={4}>
                                <Row align="middle" justify="center" >
                                    <Tag color="#87d068"> Percentage Call Duration </Tag>
                                </Row>
                                <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                    <Badge size="small" count={tableDetail.summaryReport.averageCallTimeDuration} />
                                </Row>
                            </Col>
                            <Col span={4}>
                                <Row align="middle" justify="center" >
                                    <Tag color="#87d068"> Api Call Status </Tag>
                                </Row>
                                <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                    <Badge size="small" count={tableDetail.summaryReport.apiCallStatus} />
                                </Row>
                            </Col>
                            <Col span={4}>
                                <Row align="middle" justify="center" >
                                    <Tag color="#87d068"> Used Audio</Tag>
                                </Row>
                                <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                    <Badge className="site-badge-count-109" size="small" count={tableDetail.summaryReport.usedAudio} />
                                </Row>
                            </Col>
                            <Col span={4}>
                                <Row align="middle" justify="center" >
                                    <Tag color="#87d068" > Server </Tag>
                                </Row>
                                <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                    <Badge className="site-badge-count-109" size="small" count={tableDetail.summaryReport.server} />
                                </Row>
                            </Col>
                        </Row>,

                        <Table
                            loading={table.isLoading}
                            pagination='bottomCenter'
                            tableLayout='fixed'
                            key={tableDetail.key}
                            columns={table.columns}
                            dataSource={tableDetail.baseReport}
                            scroll={{ y: 340, x: 500 }}
                        />
                    </div> :

                    ///use entire audio
                    (
                        baseReportData = [],
                        tableDetail.baseReport.map((user, index) => {
                            let userData = {
                                key: index + 1,
                                userDetail: user.userDetail,
                                channelDetail: user.channelDetail.map((channel, index) => {
                                    return ({
                                        key: index,
                                        name: channel.name,
                                        apiCallStatus: channel.apiCallStatus.toString(),
                                        chunks: channel.chunks.map((chunck) => (
                                            chunck
                                        ))
                                    })
                                })
                            }

                            baseReportData.push(userData);
                        }),
                        <div className="ant-col ant-col-24 ant-col-offset-1" id={`table_${index}`} key={index}>
                            <Row gutter={[48, 16]} align="middle" justify="space-around">
                                <Col span={4} >
                                    <Space align="center">
                                        <Statistic title="Test Harness Summary" value={index + 1} prefix={<ExperimentOutlined />} />
                                    </Space>
                                </Col>
                                <Col span={4}>
                                    <Row align="middle" justify="center" >
                                        <Tag color="#87d068"> Tested Users </Tag>
                                    </Row>

                                    <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                        <Badge overflowCount={1000000} size="small" count={tableDetail.summaryReport.testedUsers} />
                                    </Row>
                                </Col>
                                <Col span={4}>
                                    <Row align="middle" justify="center" >
                                        <Tag color="#87d068"> Api Call Status Summary by Channel </Tag>
                                    </Row>
                                    <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                        <Badge size="small" count={tableDetail.summaryReport.apiCallStatus} />
                                    </Row>
                                </Col>
                                {/* <Col span={4}>
                                    <Row align="middle" justify="center" >
                                        <Tag color="#87d068"> Used Audio</Tag>
                                    </Row>
                                    <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                        <Badge className="site-badge-count-109" size="small" count={tableDetail.baseReport.map((detail, index) => {

                                            detail.channelDetail.map((channel, index) => {
                                                return (
                                                   channel.chunks.map((chunck, index) => { return (
                                                        <a>{chunck.name}</a>
                                                    )})
                                                )
                                            })
                                        })} />
                                    </Row>
                                </Col> */}
                                <Col span={4}>
                                    <Row align="middle" justify="center" >
                                        <Tag color="#87d068" > Server </Tag>
                                    </Row>
                                    <Row align="middle" justify="center" style={{ marginTop: 5 }} >
                                        <Badge className="site-badge-count-109" size="small" count={tableDetail.summaryReport.server} />
                                    </Row>
                                </Col>
                            </Row>

                            <Table
                                loading={table.isLoading}
                                pagination='bottomCenter'
                                tableLayout='fixed'
                                key={entireAudioTable.key}
                                columns={entireAudioTable.columns}
                                dataSource={baseReportData}
                                scroll={{ y: 340, x: 500 }}
                                expandable={{
                                    expandedRowRender: data => <div style={{ margin: 0 }}>{data.channelDetail.map((channel, index) =>
                                    (
                                        <div key={index}>
                                            Channel No. <b>{channel.key + 1}</b> Used Audio: <b>{channel.name} </b> Call Successful: <b>{channel.apiCallStatus}</b>
                                            {channel.chunks.map(((chunk, index) =>
                                            (
                                                <div key={index}>
                                                    <br />
                                                    Chunk Number: <b>{chunk.chunkNumber},</b> ChunkRangeTaken: <b>{chunk.chunkRangeTaken}Mb</b> , ChunkTimeCallStart: {chunk.chunkTimeCallStart}, ChunkTimeCallEnd: {chunk.chunkTimeCallEnd}, ChunkTimeCallDuration: <b>{chunk.chunkTimeCallDuration}</b>, ResponseChunkSize:  <b>{chunk.responseChunkSize}Mb </b>
                                                </div>
                                            ))
                                            )}
                                            <br />
                                        </div>
                                    ))}</div>
                                }}
                            />
                        </div>)
            ))}
        </>
    )
}
// : <div key={tableDetail.key}></div>