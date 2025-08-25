import React from 'react';

// Custom authorization context class
export class Authorization {
	code: string;
	constructor(code: string) {
		this.code = code;
	}
}

const AuthContext = React.createContext<Authorization | null>(null);

export default AuthContext;
