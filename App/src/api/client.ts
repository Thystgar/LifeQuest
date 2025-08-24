
import { Account, Quest, Reward } from '.';

const BASE_URI = 'http://10.0.2.2:8080';

// Helper to add Authorization header if accessToken is provided
const getAuthHeader = (accessToken?: string) =>
  accessToken ? { 'Authorization': `Bearer ${accessToken}` } : undefined;

export const fetchQuests = async (accessToken?: string): Promise<Quest[]> => {
    try {
        console.log('Fetching quests with access token:', accessToken);
        const headers: Record<string, string> = getAuthHeader(accessToken) || {};
        const response = await fetch(`${BASE_URI}/quest`, {
            headers,
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching quests:', error);
        throw error;
    }
};

export const completeQuest = async (id: string, accessToken?: string): Promise<void> => {
    try {
        const headers: Record<string, string> = getAuthHeader(accessToken) || {};
        const response = await fetch(`${BASE_URI}/quest/${id}/complete`, {
            method: 'PUT',
            headers,
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error completing quest:', error);
        throw error;
    }
};

export const addQuest = async (quest: Quest, accessToken?: string): Promise<void> => {
    try {
        const headers: Record<string, string> = {
            'Content-Type': 'application/json',
            ...(getAuthHeader(accessToken) || {}),
        };
        const response = await fetch(`${BASE_URI}/quest`, {
            method: 'POST',
            headers,
            body: JSON.stringify(quest),
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error adding quest:', error);
        throw error;
    }
};

export const fetchRewards = async (accessToken?: string): Promise<Reward[]> => {
    try {
        const headers: Record<string, string> = getAuthHeader(accessToken) || {};
        const response = await fetch(`${BASE_URI}/reward`, {
            headers,
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching rewards:', error);
        throw error;
    }
};

export const redeemReward = async (id: string, accessToken?: string): Promise<void> => {
    try {
        const headers: Record<string, string> = getAuthHeader(accessToken) || {};
        const response = await fetch(`${BASE_URI}/reward/${id}/redeem`, {
            method: 'PUT',
            headers,
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error redeeming reward:', error);
        throw error;
    }
};

export const addReward = async (reward: Reward, accessToken?: string): Promise<void> => {
    try {
        const headers: Record<string, string> = {
            'Content-Type': 'application/json',
            ...(getAuthHeader(accessToken) || {}),
        };
        const response = await fetch(`${BASE_URI}/reward`, {
            method: 'POST',
            headers,
            body: JSON.stringify(reward),
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error adding reward:', error);
        throw error;
    }
};

export const deleteReward = async (id: string, accessToken?: string): Promise<void> => {
    try {
        const headers: Record<string, string> = getAuthHeader(accessToken) || {};
        const response = await fetch(`${BASE_URI}/rewards?id=${id}`, {
            method: 'DELETE',
            headers,
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error deleting reward:', error);
        throw error;
    }
};

export const fetchAccount = async (accessToken?: string): Promise<Account> => {
    try {
        console.log('Fetching account with access token:', accessToken);
        const headers: Record<string, string> = getAuthHeader(accessToken) || {};
        const response = await fetch(`${BASE_URI}/account`, {
            headers,
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching account information:', error);
        throw error;
    }
};