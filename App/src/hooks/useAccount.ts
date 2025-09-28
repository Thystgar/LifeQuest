import React from 'react';

import { useAuth } from '@/hooks/useAuth';
import { useApi } from '@/hooks/useApi';

import { Account } from '@/api';

import { useAccountContext } from '@/contexts/AccountContext';

export function useAccount() {
    const { account, setAccount } = useAccountContext();
    const { isUserAuthenticated } = useAuth();
    const { fetchAccount } = useApi();

    const loadAccount = async () => {
        try {
            const accountData = await fetchAccount();
            setAccount(accountData);
            console.log('Account data:', accountData);
        } catch (error) {
            console.error('Failed to fetch account data:', error);
        }
    };

    React.useEffect(() => {
        if (!isUserAuthenticated) {
            setAccount(null);
            return;
        }
        else {
            loadAccount();
        }
    }, [isUserAuthenticated]);

    return { 
        account,
        onPointChange: async () => await loadAccount(),
        isMemberOfGroup: account?.groupId != null && account?.groupId !== '',
        onGroupJoin: async () => await loadAccount()
    };
}