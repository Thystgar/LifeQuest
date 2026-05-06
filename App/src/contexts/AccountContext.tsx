import React, { createContext, useState } from "react";
import type { Account } from '../api';

type AccountContextType = {
  account: Account | null;
  setAccount: React.Dispatch<React.SetStateAction<Account | null>>;
  isLoading: boolean;
  setIsLoading: React.Dispatch<React.SetStateAction<boolean>>;
};

const AccountContext = createContext<AccountContextType | null>(null);

export const AccountProvider = ({ children }: { children: React.ReactNode }) => {
  const [account, setAccount] = useState<Account | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  return (
    <AccountContext.Provider value={{ account, setAccount, isLoading, setIsLoading }}>
      {children}
    </AccountContext.Provider>
  );
};

export const useAccountContext = (): AccountContextType => {
  const context = React.useContext(AccountContext);
  if (!context) {
    throw new Error('useAccount must be used within an AccountProvider');
  }
  return context;
};


