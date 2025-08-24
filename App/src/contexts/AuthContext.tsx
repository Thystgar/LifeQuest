import React from 'react';
import type { AuthorizeResult } from 'react-native-app-auth';

const AuthContext = React.createContext<AuthorizeResult | null>(null);

export default AuthContext;
