import { useState } from 'react'
import { NotificationInfo } from '../components/UI/Notification/notification';

export const useFetchServers = () => {
    const [genericNotification] = NotificationInfo();
    const [servers, setServers] = useState({
        data: [],
        loading: true
    });

    const loadServers = ({ loadSetup = false }) => {
        if (loadSetup) {
            fetch(`/api/harness/servers`, { method: 'GET' })
                .then((response) => response.json())
                .then((result) => {

                    setServers(() => ({ ...servers, data: result, loading: false }));
                }).catch(() => {
                    genericNotification({ type: Error, message: 'An error was ocurred, please review the TranscriberHub Api Service conexion.' });
                });
        }
    }

    return [servers, loadServers];
}
