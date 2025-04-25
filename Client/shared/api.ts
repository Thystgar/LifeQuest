import { Reward, Quest, Account } from './types';

const BASE_URI = 'http://20.166.162.61:8080';

export const fetchQuests = async (): Promise<Quest[]> => {
    try {
        const response = await fetch(`${BASE_URI}/quest`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching quests:', error);
        throw error;
    }
};

export const completeQuest = async (id: string): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/quest/${id}/complete`, {
            method: 'PUT',
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error completing quest:', error);
        throw error;
    }
};

export const fetchRewards = async (): Promise<Reward[]> => {
    try {
        const response = await fetch(`${BASE_URI}/reward`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching rewards:', error);
        throw error;
    }
};

export const redeemReward = async (id: string): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/reward/${id}/redeem`, {
            method: 'PUT',
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error redeeming reward:', error);
        throw error;
    }
};

export const addReward = async (reward: Reward): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/reward`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
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

export const deleteReward = async (id: string): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/rewards?id=${id}`, {
            method: 'DELETE',
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error deleting reward:', error);
        throw error;
    }
};

export const fetchAccount = async (id: string): Promise<Account> => {
    try {
        const response = await fetch(`${BASE_URI}/account/${id}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching account information:', error);
        throw error;
    }
};