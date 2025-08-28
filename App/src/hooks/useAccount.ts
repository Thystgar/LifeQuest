import React from 'react';

import { useAuth } from '@/hooks/useAuth';
import { useApi } from '@/hooks/useApi';

import { Account } from '@/api';

export function useAccount() {
    const [account, setAccount] = React.useState<Account | null>(null);
    const { isUserAuthenticated } = useAuth();
    const { fetchAccount } = useApi();

    React.useEffect(() => {
        if (!isUserAuthenticated) {
            setAccount(null);
            return;
        }
        else {
            const loadAccount = async () => {
                try {
                    const accountData = await fetchAccount();
                    setAccount(accountData);
                    console.log('Account data:', accountData);
                } catch (error) {
                    console.error('Failed to fetch account data:', error);
                }
            };
            loadAccount();
        }
    }, [isUserAuthenticated]);

    return { account };
}