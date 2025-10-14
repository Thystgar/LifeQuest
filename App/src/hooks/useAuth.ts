import React, { useEffect } from 'react';

import * as AuthSession from 'expo-auth-session';
import * as WebBrowser from 'expo-web-browser';
import Constants from 'expo-constants';

import { useAuthContext } from '@/contexts/AuthContext';

export function useAuth() {
    const { auth, setAuth } = useAuthContext();

    //     useEffect(() => {
    //         const logger = setInterval(() => {
    //             console.log('Current access token:', accessToken);
    //         }, 1000);
    //         return () => clearInterval(logger); // Cleanup on unmount
    //     }, [accessToken]);

    //     useEffect(() => {
    //   console.log('useAuth mounted');
    // }, []);


    const config = {
        issuer: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/v2.0',
        clientId: '8c14ca47-92da-4673-a63b-1cf4f9c40653',
        redirectUrl: 'com.lifequestapp://auth',  // react-native
        //redirectUrl: 'exp://localhost:8081', // web
        scopes: ['openid', 'profile', 'email', 'offline_access', 'api://5a4e418f-36f4-4546-9983-0f76427232d6/User'],
        serviceConfiguration: {
            authorizationEndpoint: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/authorize',
            tokenEndpoint: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/token',
            revocationEndpoint: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/logout',
        },
    };

    WebBrowser.maybeCompleteAuthSession();

    const discovery = {
        authorizationEndpoint: config.serviceConfiguration.authorizationEndpoint,
        tokenEndpoint: config.serviceConfiguration.tokenEndpoint,
        revocationEndpoint: config.serviceConfiguration.revocationEndpoint,
    };

    const redirectUri = Constants.executionEnvironment == 'storeClient'
        ? AuthSession.makeRedirectUri({ scheme: 'exp' }) // expo go
        : AuthSession.makeRedirectUri({ native: config.redirectUrl }); // react-native

    const authRequestConfig = {
        clientId: config.clientId,
        scopes: config.scopes,
        redirectUri,
        responseType: 'code',
    };

    const [request, response, promptAsync] = AuthSession.useAuthRequest(authRequestConfig, discovery);
    const handleTokenExchange = async (authCodeResult: AuthSession.AuthSessionResult) => {
        if (authCodeResult?.type === 'success' && authCodeResult?.params?.code && request?.codeVerifier) {
            const body = {
                code: authCodeResult.params.code,
                grant_type: "authorization_code",
                client_id: config.clientId,
                redirect_uri: redirectUri,
                code_verifier: request?.codeVerifier,
            };

            const accessTokenResponse = await fetch("https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/token", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                },
                body: new URLSearchParams(body).toString(),
            });
            console.log("Access Token Response:", accessTokenResponse);
            const json = await accessTokenResponse.json();

            if (json.access_token) {
                setAuth(json.access_token);
                console.log("Access Token set 1:", auth);
            } else {
                setAuth(null);
                console.log("Access Token set 2:", null);
            }
        }
        else {
            console.log("No auth code or code verifier present");
        }
    }

    const signIn = async () => {
        try {
            const result = await promptAsync();
            await handleTokenExchange(result);
            console.log("Authentication successful");
        } catch (error) {
            console.error('Authentication error:', error);
        }
    };

    const signOut = async () => {
        setAuth(null);
        console.log("Access Token set 3:", null);
    };

    return {
        signIn,
        signOut,
        auth,
        isUserAuthenticated: !!auth,
    };
}



