import React from 'react';

import { useAuth } from '@/hooks/useAuth';
import { useApi } from '@/hooks/useApi';

import { Account } from '@/api';

import { useAccountContext } from '@/contexts/AccountContext';

export function useAccount() {
    const { account, setAccount, isLoading, setIsLoading } = useAccountContext();
    const { isUserAuthenticated } = useAuth();
    const { fetchAccount } = useApi();
    const loadingRef = React.useRef(false);

    const loadAccount = async () => {
        if (loadingRef.current) return;
        try {
            loadingRef.current = true;
            setIsLoading(true);
            const accountData = await fetchAccount();
            setAccount(accountData);
            console.log('Account data:', accountData);
        } catch (error) {
            console.error('Failed to fetch account data:', error);
        } finally {
            loadingRef.current = false;
            setIsLoading(false);
        }
    };

    React.useEffect(() => {
        if (!isUserAuthenticated) {
            setAccount(null);
            setIsLoading(false);
            return;
        }
        else {
            setIsLoading(true);
            loadAccount();
        }
    }, [isUserAuthenticated]);

    return {
        account,
        isAccountLoading: isLoading,
        onPointChange: async () => await loadAccount(),
        isMemberOfGroup: account?.groupId != null && account?.groupId !== '',
        onGroupJoin: async () => await loadAccount(),
        termsAccepted: account?.termsAccepted || false
    };
}