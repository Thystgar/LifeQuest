import React from 'react';

import { useAuth } from '@/hooks/useAuth';
import { useApi } from '@/hooks/useApi';

import { Account } from '@/api';

import { useAccountContext } from '@/contexts/AccountContext';

export function useAccount() {
    const { account, setAccount } = useAccountContext();
    const { isUserAuthenticated } = useAuth();
    const { fetchAccount } = useApi();
    const loadingRef = React.useRef(false);

    const loadAccount = async () => {
        if (loadingRef.current) return;
        try {
            loadingRef.current = true;
            const accountData = await fetchAccount();
            setAccount(accountData);
            console.log('Account data:', accountData);
        } catch (error) {
            console.error('Failed to fetch account data:', error);
        } finally {
            loadingRef.current = false;
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