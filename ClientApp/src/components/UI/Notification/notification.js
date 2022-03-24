import { SmileOutlined, StopOutlined, InfoOutlined } from "@ant-design/icons";
import { NotificationType } from '../../../enums/notificationType';
import { notification } from 'antd';
import React from 'react';

export const NotificationInfo = (initialState = {}) => {
    const [{ Success, Information }] = NotificationType();

    const genericNotification = ({ type, message, description }) => { 

        var icon = type === Success ? <SmileOutlined style={{ color: '#389e0d' }} /> :
            type === Information ? <InfoOutlined style={{ color: '#1890ff' }} /> :
                <StopOutlined  style={{ color: '#F3401A' }} />;

        notification.open({
            message: message === undefined ? '' : message,
            description: description === undefined ? '' : description,
            icon: icon
        }); 
    };

    return [genericNotification];
}
