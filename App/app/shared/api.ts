import { Reward, Quest, Account } from './types';

const BASE_URI = 'http://10.0.2.2:8080';

export const fetchQuests = async (): Promise<Quest[]> => {
    try {
        const response = await fetch(`${BASE_URI}/quests`);
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
        const response = await fetch(`${BASE_URI}/quests/finish?id=${id}`, {
            method: 'POST',
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error completing quest:', error);
        throw error;
    }
};

export const reactivateQuest = async (id: string): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/quests/reactivate?id=${id}`, {
            method: 'POST',
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error reactivating quest:', error);
        throw error;
    }
};

export const fetchRewards = async (): Promise<Reward[]> => {
    try {
        const response = await fetch(`${BASE_URI}/rewards`);
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
        const response = await fetch(`${BASE_URI}/rewards/redeem?id=${id}`, {
            method: 'POST',
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error redeeming reward:', error);
        throw error;
    }
};

export const reactivateReward = async (id: string): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/rewards/reactivate?id=${id}`, {
            method: 'POST',
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error reactivating reward:', error);
        throw error;
    }
};

export const addReward = async (name: string, points: string): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/rewards`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Name: name,
                Points: points,
            }),
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

export const updateReward = async (id: string, name: string, points: string): Promise<void> => {
    try {
        const response = await fetch(`${BASE_URI}/rewards`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Id: id,
                Name: name,
                Points: points,
            }),
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error('Error updating reward:', error);
        throw error;
    }
};

export const fetchAccount = async (): Promise<Account> => {
    try {
        const response = await fetch(`${BASE_URI}/account`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching account information:', error);
        throw error;
    }
};