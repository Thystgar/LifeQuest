import React from 'react';
import type { Account } from '../api';

const AccountContext = React.createContext<Account | null>(null);

export default AccountContext;
